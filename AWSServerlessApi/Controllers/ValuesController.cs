using AWSServerlessApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AWSServerlessApi.Controllers;

[Route("api/[controller]")]
public class ValuesController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public ValuesController(IConfiguration config)
    {
            this._configuration = config;
    }
    [AllowAnonymous]
    [HttpGet]
    [Route("Ping")]
    public IEnumerable<string> Ping()
    {
        return ["Ping from JoinController is successful..."];
    }

    // GET api/values
    [HttpGet]
    public async Task<IEnumerable<string>> Get()
    {
        //var setting1 = _configuration["MySettings:Setting1"];
        //var setting2 = _configuration["MySettings:Setting2"];

        //return [ setting1, setting2 ];

        var config = new AppConfig();
        config.ApplicationId = "drmyok5";
        config.EnvironmentId = "nq82t4j";
        config.ConfigProfileId = "fass4wi";
        var data = await config.GetConfigDetails();
        return [data["setting1"].ToString(), data["setting2"].ToString()];
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/values
    [HttpPost]
    public void Post([FromBody]string value)
    {
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}