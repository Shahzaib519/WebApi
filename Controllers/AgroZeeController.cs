using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ograzeeApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ograzeeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgroZeeController : ControllerBase
    {
        agroApiServices api = new agroApiServices();

        // GET: api/<agroController>
        [HttpGet]
        [Route("login")]
        public string login(string email, string password)
        {
            int res = api.SignIn(email, password);
            
            if(res==1)     // successful
            {
                return "Sigin, True, 1";
            }
            else if (res == -1)     // nope
            {
                return "Sigin, False,-1";
            }
            else
            {
                return "Sigin, wrong Credentials," + res;
            }
            //return "value2" + password;
        }




        // GET: api/<agroController>
        [HttpGet]
        [Route("signup")]
        public string SignUp(string email, string password, string name, string mobile)
        {
            int res = api.SignUp(email, password, name, mobile);

            if (res == 1)     // successful
            {
                return "True, 1";
            }
            else if (res == -1)     // nope
            {
                return "False,-1";
            }
            else if (res == 0)     // nope
            {
                return "True, 0";
            }
            else
            {
                return "wrong Credentials," + res;
            }
            //return "value2" + password;
        }



        //
        //
        //
        // GET api/<agroController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<agroController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<agroController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<agroController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
