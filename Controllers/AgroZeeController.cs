using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ograzeeApi.Models;
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
                return "Sigin successfully, 1";
            }
            else if (res == -1)     // nope
            {
                return "Network Connection, -1";
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
                return "Already Exists, 1";
            }
            else if (res == -1)     // nope
            {
                return "Network Connection,-1";
            }
            else if (res == 0)     // nope
            {
                return "Done with Signup, 0";
            }
            else
            {
                return "wrong Credentials," + res;
            }
            //return "value2" + password;
        }



        [HttpGet]
        [Route("dashboard")]
        public dynamic GetDashboardData(string email)
        {
            UserDataField res = api.GetDashboardData(email);

            if (res == null)     // unsuccessful
            {
                return "Network Connection,-1";
            }
            else
            {
                return res;
            }
        }



        [HttpGet]
        [Route("updateam")]
        public dynamic UpdateAddressMobile(string email, string address, string mobile)
        {
            dynamic res = api.UpdateAddressMobile(email, address, mobile);

            if (res == null)     // unsuccessful
            {
                return "Network Connection,-1";
            }
            else
            {
                return res;
            }
        }

        [HttpGet]
        [Route("updateps")]
        public dynamic UpdatePs(string email, string oldpassword, string newpassword)
        {
            dynamic res = api.UpdatePassword(email, oldpassword, newpassword);

            if (res == null)     // unsuccessful
            {
                return "Network Connection,-1";
            }
            else
            {
                return res;
            }
        }

        [HttpGet]
        [Route("updateimage")]
        public dynamic UpdateImage(string email, string image)
        {
            dynamic res = api.UpdateImage(email, image);

            if (res == null)     // unsuccessful
            {
                return "Network Connection,-1";
            }
            else
            {
                return res;
            }
        }

        [HttpGet]
        [Route("updatesaleamt")]
        public dynamic UpdateSaleAmount(string email, double amount)
        {
            dynamic res = api.updateSaleAmount(email, amount);

            if (res == null)     // unsuccessful
            {
                return "Network Connection,-1";
            }
            else
            {
                return res;
            }
        }

        [HttpGet]
        [Route("updatewithdraw")]
        public dynamic UpdateWithdrawalAmount(string email, double amount)
        {
            dynamic res = api.UpdateWithdrawalableAmount(email, amount);

            if (res == null)    // unsuccessful
            {
                return "Network Connection,-1";
            }
            else
            {
                return res;
            }
        }

        [HttpGet]
        [Route("updatesalecount")]
        public dynamic UpdateSaleCount(string email, int count)
        {
            dynamic res = api.updateSalesCount(email, count);

            if (res == null)    // unsuccessful
            {
                return "Network Connection,-1";
            }
            else
            {
                return res;
            }
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
