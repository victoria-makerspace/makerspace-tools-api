using Jose;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace makerspace_tools_api.Helpers
{
  public class TokenHelpers
  {
    private IConfiguration _config;
    public TokenHelpers(IConfiguration config)
    {
      _config = config;
    }

    public string CreateJwt(string sub, Dictionary<string, string> claims = null)
    {
      var payload = new Dictionary<string, object>
            {
                { "sub", sub },
                { "iss", _config["Jwt:Issuer"]},
                { "iat", ToUnixTime(DateTime.Now) },
                { "exp", _config["Jwt:Expiry"]?.Length == 0 ? ToUnixTime(DateTime.Now.AddDays(30)) : int.Parse(_config["Jwt:Expiry"]) },
                { "aud", _config["Jwt:Audience"] }
            };

      if (claims != null)
      {
        foreach (var kvPair in claims)
          payload.Add(kvPair.Key, kvPair.Value);
      }

      string token = JWT.Encode(payload,
          Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]),
          JwsAlgorithm.HS256);

      return token;
    }

    private long ToUnixTime(DateTime dateTime)
    {
      return (int)dateTime
          .ToUniversalTime()
          .Subtract(new DateTime(1970, 1, 1))
          .TotalSeconds;
    }

    public byte[] HexStringToByteArray(string hex)
    {
      byte[] bytes = new byte[hex.Length / 2];
      for (int i = 0; i < hex.Length; i += 2)
        bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
      return bytes;
    }
  }
}
