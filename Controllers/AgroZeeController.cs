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



        private static Random random = new Random();
        private string GenerateRandomID(int v)
        {
            string strArray = "abcdefghijklmnopqrstuvwxyz123456789";
            return new string(Enumerable.
                Repeat(strArray, v)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }



        // GET: api/<agroController>
        [HttpGet]
        [Route("signup")]
        public string SignUp(string email, string password, string name, string mobile)
        {
            long res = api.SignUp(email, password, name, mobile);

            if (res == -2)     // successful
            {
                return "Already Exists, -2";
            }
            else if (res == -3)     // nope
            {
                return "Network Connection while connecting to server, -3";
            }
            else if (res == -1)     // nope
            {
                return "Problem while modifying Sys Data";
            }
            else if(res < 0)
            {
                return "Something wrong, " + res;
            }
            else
            {
                string code = GenerateRandomID(4);
                api.SendPasswordResetEmail(email, code);
                return "Done with Signup, with "+ code;
            }
            //return "value2" + password;
        }



        [HttpGet]
        [Route("verifyemail")]
        public dynamic SendPasswordResetEmail(string email)
        {
            string code = GenerateRandomID(4);
            int res = api.SendPasswordResetEmail(email, code);
            
            if (res == -1)     // unsuccessful
            {
                return "Network Connection or some problem. -1";
            }
            else
            {
                return res;
            }
        }


        //////////////////////////////////////
        ///////////// Dashboard //////////////
        //////////////////////////////////////

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
        [Route("getallsales")]
        public dynamic GetSales(string email)
        {
            dynamic res = api.GetSales(email);

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
        [Route("getalltransactionhistory")]
        public dynamic GetTransactions(string email)
        {
            dynamic res = api.GetTransactions(email);

            if (res == null)     // unsuccessful
            {
                return "Network Connection,-1";
            }
            else
            {
                return res;
            }
        }
        
        
        //[HttpGet]
        //[Route("verifyuser")]
        //public dynamic ValidReceiver(string email, string customerID)
        //{
        //    UserDataField res = api.ValidReceiver(email, customerID);

        //    if (res == null)     // unsuccessful
        //    {
        //        return "Network Connection,-1";
        //    }
        //    else
        //    {
        //        return res;
        //    }
        //}

        [HttpGet]
        [Route("getsalescount")]
        public dynamic GetSalesCount() 
        {
            return api.GetSalesCount();
        }

        [HttpGet]
        [Route("getclientscount")]
        public dynamic GetClientsCount()
        {
            return api.GetClientsCount();
        }

        [HttpGet]
        [Route("gettransactionscount")]
        public dynamic GetTransactionCount()
        {
            return api.GetTransactionCount();
        }



        //////////////////////////////////////
        /////////////// Update ///////////////
        //////////////////////////////////////

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



        //////////////////////////////////////
        ///////////// canWithdraw ////////////
        //////////////////////////////////////

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



        //////////////////////////////////////
        /////////////// Transfer /////////////
        //////////////////////////////////////        
        
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


        //////////////////////////////////////
        /////////////// Sale  ////////////////
        //////////////////////////////////////

        [HttpGet]
        [Route("newsale")]
        public dynamic NewSaleRequest(string email, string image, string cat, double quantity, double demandamount, string address, string date)
        {
            dynamic res = api.AddNewSale(email, image, cat, quantity, address, demandamount, date);

            if (res == -2)     // successful
            {
                return "Error while adding new Sale Request.";
            }
            else if (res == -3)     // nope
            {
                return "Network Connection while connecting to server, -3";
            }
            else if (res == -1)     // nope
            {
                return "Problem while modifying Sys Data";
            }
            else if (res < 0)
            {
                return "Something wrong, " + res;
            }
            else
            {
                return "Done with New Sale Request Sucessfully, 0="+res;
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
