using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TemplateAuthenication.Controllers
{
  
    [Route("api/{controller}/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
       
        [HttpGet]
        public IActionResult Get()
        {
            User user1 = new User { Email = "asd@.asd", UserName = "Dejan", PhoneNumber = "123-123" };
            User user2 = new User { Email = "asd@.asd1", UserName = "Dejan1", PhoneNumber = "123-1233" };
            List<User> users = new List<User> { user1, user2 };
            return Ok(users);
        }
    }
}
