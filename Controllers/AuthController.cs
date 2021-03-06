﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using makerspace_tools_api.Helpers;

namespace DiscourseSso.Controllers
{
  [Route("api/[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;
        private readonly TokenHelpers _helpers;

        public AuthController(
            IDistributedCache cache, 
            IConfiguration config, 
            ILogger<AuthController> logger,
            TokenHelpers helpers)
        {
            _cache = cache;
            _config = config;
            _logger = logger;
            _helpers = helpers;
        }

        // public async Task<IActionResult> Login(string address)
        public async Task<IActionResult> Login(string returnUrl)
        {
            // generate & store nonce in cache
            string nonce = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "").Replace("+", "");
            await _cache.SetStringAsync(
                nonce,
                "_",
                new DistributedCacheEntryOptions()
                {
                    SlidingExpiration = new TimeSpan(12, 0, 0) // nonce lasts only 12 hours
                }).ConfigureAwait(false);

            // create payload, base64 encode & url encode
            string payload = $"nonce={nonce}&return_sso_url={returnUrl}";

            string base64Payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));
            string urlEncodedPayload = Uri.EscapeUriString(base64Payload);

            // generating HMAC-SHA256 from base64-encoded payload using sso secret as signiture
            string hexSigniture;
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.ASCII.GetBytes(_config["DiscourseSso:SsoSecret"])))
            {
                byte[] sha256 = hmac.ComputeHash(Encoding.ASCII.GetBytes(base64Payload));
                hexSigniture = BitConverter.ToString(sha256).Replace("-", "").ToLower();
            }

            // send auth request to Discourse
            string redirectTo = $"{_config["DiscourseSso:DiscourseRootUrl"]}/session/sso_provider?sso={urlEncodedPayload}&sig={hexSigniture}";
            return Redirect(redirectTo);
        }

        public async Task<IActionResult> GetToken(string sig, string sso)
        {
            // generate HMAC-SHA256 from sso using sso secret as key
            byte[] ssoSha256;
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.ASCII.GetBytes(_config["DiscourseSso:SsoSecret"])))
                ssoSha256 = hmac.ComputeHash(Encoding.ASCII.GetBytes(sso));

            // convert sig from HEX to bytes
            byte[] sigBytes = _helpers.HexStringToByteArray(sig);

            // making sure above two aree equal
            if (Encoding.ASCII.GetString(ssoSha256) != Encoding.ASCII.GetString(sigBytes))
            {
                _logger.LogDebug($"ssoSha256 != sigBytes");
                return BadRequest("Somehting went wrong.");
            }

            // base64-decoding & url-decoding sso
            string base64Sso = Uri.UnescapeDataString(Encoding.UTF8.GetString(Convert.FromBase64String(sso)));

            // convering sso query string to dictionary
            var userInfo = base64Sso.Replace("?", "").Split('&').ToDictionary(x => x.Split('=')[0], x => x.Split('=')[1]);

            // verifiying nonce in sso has been added before, remove if true
            if (await _cache.GetStringAsync(userInfo["nonce"]).ConfigureAwait(false) == null)
                return BadRequest("Nonce not previously registered or has expired.");
            else
                await _cache.RemoveAsync(userInfo["nonce"]).ConfigureAwait(false);

            // don't include these in JWT
            userInfo.Remove("nonce");
            userInfo.Remove("return_sso_url");

            // creating JWT with info from sso as claims
            string jwt = _helpers.CreateJwt(userInfo["external_id"], userInfo);

            return Ok(jwt);
        }
    }
}
