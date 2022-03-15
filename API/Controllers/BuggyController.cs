using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }

        // this is to view some errors that are common and that we need to handle

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret(){
            return "secret Text";
        }


        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound(){
            var thing = _context.Users.Find(-1);
            if(thing == null) return NotFound();

            return Ok(thing);
        }


        [HttpGet("server-error")]
        public ActionResult<string> GetServerError(){

            var thing = _context.Users.Find(-1);

            // this wil return null reference exception cause thing is null and cant toString null
            var thingToReturn = thing.ToString();

            return thingToReturn;

        }


        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest(){
            return BadRequest("This was not a good request");
        }
    }
}