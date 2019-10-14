using System.Security.Claims;
using System.Threading.Tasks;
using ShiftPlanner.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ShiftPlanner.Domain;
using ShiftPlanner.Web.Models;
using ShiftPlanner.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using LazyCache;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShiftPlanner.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ShiftPlanner.Web.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private UserManager<User> _userManager;
        private readonly ILogger _logger;
        //private readonly JwtIssuerOptions _jwtOptions;
        private IConfiguration _configuration;
        //private readonly IAppCache _cache;
        private ShiftPlannerContext _context;
        IServiceProvider _serviceProvider;
        private readonly CatalogDbContext _catalogdb;


        public AuthController(
            UserManager<User> userManager,
            //IOptions<JwtIssuerOptions> jwtOptions,
            IConfiguration configuration,
            ILogger<AuthController> logger,
            //IAppCache cache,
            IHttpContextAccessor httpcontext,
            //ShiftPlannerContext context,
            IServiceProvider serviceProvider,
            CatalogDbContext catalogdb
            )
        {
            _userManager = userManager;
            //_jwtOptions = jwtOptions.Value;
            _configuration = configuration;
            _logger = logger;
            _catalogdb = catalogdb;
            //_cache = cache;

            _serviceProvider = serviceProvider;

            // Tenants
            var host = httpcontext.HttpContext.Request.Host.Host;
            var optionsBuilder = new DbContextOptionsBuilder<ShiftPlannerContext>();
            optionsBuilder.UseSqlServer(Startup.tenants[host]);
            _context = new ShiftPlannerContext(optionsBuilder.Options);
            _context.UserId = CurrentUserIDfromContext(httpcontext.HttpContext);

            // override UserManager Context
            IPasswordHasher<User> hasher = new PasswordHasher<User>();
            var validator = new UserValidator<User>();
            var validators = new List<UserValidator<User>> { validator };
            ILogger<UserManager<User>> loggerU = null;
            _userManager = new UserManager<User>(new UserStore<User>(_context), null, hasher, validators, 
            null, null, null, null, loggerU);


        }

        // POST api/auth/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]CredentialsViewModel credentials)
        {
            try
            {
                Errors errs = new Errors();


                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);

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
                    var tenant = _catalogdb.Tenants.FirstOrDefault(x => x.DomainHost == host);


                    var claims = new[]
                    {
                    new Claim("name", credentials.UserName),
                    new Claim("id", userToVerify.Id),
                    new Claim("rol",  userToVerify.Role == Domain.Enum.Role.Admin ? "Admin" : "Standard"),
                    new Claim("photo", tenant.storageprefix + "/images/" + userToVerify.Photo),
                    new Claim("lastname",  userToVerify.LastName),
                    new Claim("firstname",  userToVerify.FirstName),
                    new Claim( userToVerify.Role == Domain.Enum.Role.Admin  ? "Admin" : "Standard", ""),
                    new Claim("email",  userToVerify.Email),

                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: "yourdomain.com",
                        audience: "yourdomain.com",
                        claims: claims,
                        expires: DateTime.Now.AddHours(24),
                        signingCredentials: creds
                        );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token)
                    });

                }

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