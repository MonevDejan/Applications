using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
//using LazyCache;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Controllers
{
    [Route("api/{controller}/[action]")]
    public class AuthController : ControllerBase
    {
        private UserManager<User> _userManager;
        private readonly ILogger _logger;
        private IConfiguration _configuration;

        public AuthController(
            UserManager<User> userManager,
            IConfiguration configuration,
            ILogger<AuthController> logger
            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        public class Errors
        {
            public ModelStateDictionary AddErrorsToModelState(IdentityResult identityResult, ModelStateDictionary modelState)
            {
                foreach (var e in identityResult.Errors)
                {
                    modelState.TryAddModelError(e.Code, e.Description);
                }

                return modelState;
            }

            public ModelStateDictionary AddErrorToModelState(string code, string description, ModelStateDictionary modelState)
            {
                modelState.TryAddModelError(code, description);
                return modelState;
            }
        }


        // POST api/auth/login
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CredentialsViewModel credentials)
        {
            try
            {
                // User u = new User() { Email = "dm@gmail.com", UserName= "dm@gmail.com" };
                //var result = await _userManager.CreateAsync(u, "P@ssw0rd");

                Errors errs = new Errors();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // get the user to verifty
                var userToVerify = await _userManager.FindByNameAsync(credentials.UserName).ConfigureAwait(false);

                // check with email
                if (userToVerify == null)
                {
                    userToVerify = await _userManager.FindByEmailAsync(credentials.UserName).ConfigureAwait(false);
                }

                if (userToVerify == null)
                {
                    return BadRequest(errs.AddErrorToModelState("login_failure", "InvalidUsernameOrPassword", ModelState));
                }

                // check the credentials
                if (await _userManager.CheckPasswordAsync(userToVerify, credentials.Password).ConfigureAwait(false))
                {
                    var host = HttpContext.Request.Host.Host;

                    var claims = new[]
                    {
                        new Claim("name", credentials.UserName),
                        new Claim("id", userToVerify.Id),
                        new Claim("email",  userToVerify.Email),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: "beto2.com",
                        audience: "beto2.com",
                        claims: claims,
                        expires: DateTime.Now.AddHours(24),
                        signingCredentials: creds
                        );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token)
                    });

                }

                var claim1 = new[]
                {
                    new Claim("name", credentials.UserName),
                    new Claim("email", userToVerify.Email)
                };

                return BadRequest(errs.AddErrorToModelState("login_failure", "InvalidUsernameOrPassword", ModelState));
            }
            catch (DataException ex)
            {
                _logger.LogError(ex, "AuthController/Post");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AuthController/Post");
                throw;
            }
        }

    }
}