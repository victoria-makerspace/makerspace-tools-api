
using System.Linq;
using makerspace_tools_api.Models;
using Microsoft.AspNetCore.Http;

namespace makerspace_tools_api.Services
{
  public class MakerspaceUserService : IMakerspaceUserService
  {
    private const string NAMESPACE = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/";
    private const string USERNAME_CLAIM = "username";
    private const string FIRST_NAME_CLAIM = NAMESPACE + "givenname";
    private const string LAST_NAME_CLAIM = NAMESPACE + "surname";
    private const string EMAIL_CLAIM = NAMESPACE + "emailaddress";

    private readonly IHttpContextAccessor _httpContextAccessor;
    private MakerspaceUser _user;

    public MakerspaceUserService(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public MakerspaceUser User
    {
      get
      {
        return _user ??= new MakerspaceUser()
            {
            Username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == USERNAME_CLAIM)?.Value,
            FirstName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == FIRST_NAME_CLAIM)?.Value,
            LastName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == LAST_NAME_CLAIM)?.Value,
            Email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == EMAIL_CLAIM)?.Value,
            };
      }
    }
  }
}