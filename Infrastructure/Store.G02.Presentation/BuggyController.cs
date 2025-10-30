using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Presentation
{
    public class BuggyController : ControllerBase
    {
        [HttpGet("notfound")]
        public IActionResult GetNotFound() 
        {
            return NotFound();
        }

        [HttpGet("servererror")]
        public IActionResult GetServerError() 
        {
            throw new Exception();
            return Ok();
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest();
        }

        [HttpGet("unauthorize")]
        public IActionResult GetAnAuthorized() 
        {
            return Unauthorized();
        }
    }
}
