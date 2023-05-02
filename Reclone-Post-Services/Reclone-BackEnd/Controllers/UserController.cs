using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Reclone_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("UserMicroservice");
        }

        //establish connect with the user service project

        [HttpGet("getUserServiceResponse")]
        public async Task<string> Get()
        {
            var response = await _httpClient.GetAsync("/api/post/sendUserServiceResponse");
            return await response.Content.ReadAsStringAsync();
        }

        // return response to user service project

        [HttpGet("sendPostServiceResponse")]

        public string SendResponse()
        {
            return "Hello from the Post controller!!";
        }
    }
}
