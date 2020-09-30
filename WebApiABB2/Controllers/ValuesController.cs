using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using WebApiABB2.Models;

namespace WebApiABB2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly IConfiguration _config;

        public ValuesController(IConfiguration config)
        {
            _config = config;
        }



        //// GET api/values
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Credentials user)
        {

            string responseMessage = "";
            string tokenUrl = _config.GetValue<string>("AzureAd:tokenUrl");
            var req = new HttpRequestMessage(HttpMethod.Post, tokenUrl);

            RequestContent(user, req);

            using (var client = new HttpClient())
            {
                var res = await client.SendAsync(req);
                responseMessage = await res.Content.ReadAsStringAsync();
                HttpResponseMessage response;
                if (res.IsSuccessStatusCode)
                {
                    return (ActionResult)new OkObjectResult(responseMessage);
                }
                return (ActionResult)new BadRequestObjectResult(responseMessage);

            }
        }

        private void RequestContent(Credentials user, HttpRequestMessage req)
        {
            req.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = _config.GetValue<string>("AzureAd:grant_type"),
                ["resource"] = _config.GetValue<string>("AzureAd:resource"),
                ["client_id"] = _config.GetValue<string>("AzureAd:client_id"),
                ["tenant_id"] = _config.GetValue<string>("AzureAd:tenant_id"),
                ["scope"] = _config.GetValue<string>("AzureAd:scope"),
                ["client_secret"] = _config.GetValue<string>("AzureAd:client_secret"),
                ["username"] = user.username,
                ["password"] = user.password
            });
        }



        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}


