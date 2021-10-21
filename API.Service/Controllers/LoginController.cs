using API.Security;
using API.Security.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace API.Service.Controllers
{
    /// <summary>
    /// login controller class for authenticate users
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }

        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }

        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(Models.Credentials.JWT login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var user = ConfigurationManager.AppSettings["JWTUser"];
            var password = ConfigurationManager.AppSettings["JWTPassword"];

            bool isCredentialValid = (login.Username == user && login.Password == password);
            if (isCredentialValid)
            {
                var token = TokenGenerator.GenerateTokenJwt(login.Username);
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("ldap/authenticate")]
        public IHttpActionResult AuthenticateLDAP(Models.Credentials.Plain login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            try
            {


                Identity userAuthenticated = SigninManager.SignIn(login.Username, login.Password);

                if (userAuthenticated != null)
                    return Ok(TokenGenerator.GenerateTokenJwt(userAuthenticated.Email));
                else
                    return Unauthorized();

            }
            catch (Exception ex)
            {
                Exception e = ex.InnerException ?? ex;

                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"{e.Message}{System.Environment.NewLine}{e.Source}{System.Environment.NewLine}{e.StackTrace}{System.Environment.NewLine}"),
                    ReasonPhrase = "Error autenticacion"
                };
                throw new HttpResponseException(resp);
            }
        }
    }

}
