using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext context;

        public BuggyController(StoreContext context)
        {
            this.context = context;
        }

        [HttpGet("testauth")]
        [Authorize]
        public ActionResult<string> GetSecretText()
        {
            return "Secret stuff";
        } 

        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var product = context.Products.Find(42);

            if (product == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(product);
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var product = context.Products.Find(42);

            var productToReturn = product.ToString();

            return Ok(product);
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }


    }
}