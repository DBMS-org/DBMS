using Application.DTOs.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected IActionResult Ok<T>(T data)
        {
            var response = new ApiResponse<T>(data, true, "Request successful.", (int)HttpStatusCode.OK);
            return new ObjectResult(response) { StatusCode = response.StatusCode };
        }

        protected new IActionResult Ok()
        {
            var response = new ApiResponse(true, "Request successful.", (int)HttpStatusCode.OK);
            return new ObjectResult(response) { StatusCode = response.StatusCode };
        }

        protected IActionResult Created<T>(T data, string location)
        {
            var response = new ApiResponse<T>(data, true, "Resource created successfully.", (int)HttpStatusCode.Created);
            return Created(location, response);
        }

        protected IActionResult CreatedAtAction<T>(string actionName, object routeValues, T data)
        {
            var response = new ApiResponse<T>(data, true, "Resource created successfully.", (int)HttpStatusCode.Created);
            return base.CreatedAtAction(actionName, routeValues, response);
        }

        protected IActionResult BadRequest(string message = "Bad request.")
        {
            var response = new ApiResponse(false, message, (int)HttpStatusCode.BadRequest);
            return new ObjectResult(response) { StatusCode = response.StatusCode };
        }

        protected IActionResult NotFound(string message = "Resource not found.")
        {
            var response = new ApiResponse(false, message, (int)HttpStatusCode.NotFound);
            return new ObjectResult(response) { StatusCode = response.StatusCode };
        }

        protected IActionResult Conflict(string message = "A conflict occurred.")
        {
            var response = new ApiResponse(false, message, (int)HttpStatusCode.Conflict);
            return new ObjectResult(response) { StatusCode = response.StatusCode };
        }

        protected IActionResult Unauthorized(string message = "Unauthorized access.")
        {
            var response = new ApiResponse(false, message, (int)HttpStatusCode.Unauthorized);
            return new ObjectResult(response) { StatusCode = response.StatusCode };
        }

        protected IActionResult Forbidden(string message = "Forbidden access.")
        {
            var response = new ApiResponse(false, message, (int)HttpStatusCode.Forbidden);
            return new ObjectResult(response) { StatusCode = response.StatusCode };
        }

        protected IActionResult InternalServerError(string message = "An internal server error occurred.")
        {
            var response = new ApiResponse(false, message, (int)HttpStatusCode.InternalServerError);
            return new ObjectResult(response) { StatusCode = response.StatusCode };
        }
    }
} 