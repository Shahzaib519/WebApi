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
        [Route("canwithdraw")]
        public dynamic CanWidthdraw(string email, double amount)
        {
            UserDataField sender = api.GetDashboardData(email);

            if (sender == null)     // unsuccessful
            {
                return "Network Connection, -1";
            }
            else
            {
                return sender.Withdrawalable < amount ? "no" : "yes"; 
            }
        }


        [HttpGet]
        [Route("transferamount")]
        public dynamic TransferAmount(string emailfrom, string emailto, string customerIDto, double amount)
        {
            //1- get withdrawalable amount of sender
            //2-  substract it from current withdrawalable
            //3-     add it to the receiver's withdrawalable; having customerID && emailto

            if(CanWidthdraw(emailfrom, amount)=="no")
            {
                return "Not possibile to withdraw!";
            }

            if(api.ValidReceiver(emailto,customerIDto)==null)
            {
                return "Invalid Receiver infomation";
            }


            // step#1-to-2 => updateWithdrawal
            // step# => update Receiver's withdrawal

            UserDataField sender = api.GetDashboardData(emailfrom);
            UserDataField receiver = api.GetDashboardData(emailto);

            if (sender == null)     // unsuccessful
            {
                return "Network Connection, -1";
            }
            if(receiver == null)
            {
                return "Network connection, -1";
            }

            double SenderUpdatedWithdrawalableAmount = sender.Withdrawalable - amount;    // 70
            var res1 = api.UpdateWithdrawalableAmount(emailfrom, SenderUpdatedWithdrawalableAmount); //step#1,2


            double ReceiverUpdatedWithdrawalableAmount = receiver.Withdrawalable + amount;
            var res2 = api.UpdateWithdrawalableAmount(emailto, ReceiverUpdatedWithdrawalableAmount); // get the oldWithdrawal then add this amount and then update


            return "Successfully tranfered amount Rs. "+ amount + " from " + emailfrom + " To " + emailto + " having " + customerIDto ;
        }




        [HttpGet]
        [Route("newsale")]
        public dynamic NewSaleRequest(string email, string image, string cat, double quantity, double demandamount, string address, string date)
        {
            dynamic res = api.AddNewSale(email, image, cat, quantity, address, demandamount, date);

            if (res == null)     // successful
            {
                return "Already Exists, 1";
            }
            else if (res == -1)     // nope
            {
                return "Network Connection,-1";
            }
            else if (res == 0)     // nope
            {
                return "Successfully New Request Added!";
            }
            else
            {
                return res;
            }
        }



        [HttpGet]
        [Route("updsale")]
        public dynamic UpdateSale(string email, string id, string image, string cat, double quantity, double demandamount, string address, string date)
        {
            dynamic res = api.UpdateSaleRequest(email, id, image, cat, quantity, address, demandamount, date);

            if (res == null)     // successful
            {
                return "Already Exists, 1";
            }
            else
            {
                return res;
            }
        }

        [HttpGet]
        [Route("deletesale")]
        public dynamic DeleteSale(string email, string id)
        {
            dynamic res = api.DeleteSale(email, id);

            if (res == null)     // successful
            {
                return "Already Exists, 1";
            }
            else
            {
                return res;
            }
        }



    }
}
