using MongoDB.Bson;
using MongoDB.Driver;
using ograzeeApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;

namespace ograzeeApi.Repository
{
    public class agroApiServices
    {
       
        /////////////////////////////////////////////////
        /////////////////////////////////////////////////
        /////////////< Login, Signup >///////////////////
        /////////////////////////////////////////////////
        /////////////////////////////////////////////////

        // sign-in
        public int SignIn(string email, string password) {
            /////
            if(Repository.MongoHelper.ConnectToMongoService())
            {
                var builder = Builders<registration>.Filter;
                var filter = builder.And(builder.Eq("Email", email), builder.Eq("Password", password));
                var result = MongoHelper.clients_collection().Find(filter).ToList();
                if(result.Count==1) // Successfully login
                {
                    return 1;
                }
                else
                {
                    return result.Count;
                }
            }
            else
            {
                return -1;
            }

        }


        //// sign-out
        //void SignOut() {
        //    //////
        //}

        private static Random random= new Random();
        private object GenerateRandomID(int v)
        {
            string strArray = "abcdefghijklmnopqrstuvwxyz123456789";
            return new string(Enumerable.
                Repeat(strArray, v)
                .Select(s => s[random.Next(s.Length)]).ToArray()
                );
        }

        // sign-up
        // create new user in USers
        public int SignUp(string email, string password, string userName, string mobileNo) {
            if (Repository.MongoHelper.ConnectToMongoService() && Repository.MongoHelper.ConnectToDataMongoService())
            {
                var filter = Builders<registration>.Filter.Eq("Email", email);
                var result = Repository.MongoHelper.clients_collection().Find(filter).ToList();
                if (result.Count ==1)
                {
                    return 1;
                }
                else
                {
                    Object id = GenerateRandomID(24);
                    Repository.MongoHelper.clients_collection().InsertOneAsync(
                            new registration
                            {
                                _id = id,
                                Email = email,
                                Password = password,
                                Name = userName,
                                Mobile = mobileNo
                            }
                        );
                    Repository.MongoHelper.GetProfileCollection(email).InsertOneAsync(
                            new UserDataField
                            {
                                Id=id,
                                CustomerID = "AGRO" + mobileNo,
                                Name = userName,
                                Email = email,
                                Mobile = mobileNo,
                                EmailVerified = "no",
                                Address = "",
                                ProfileImage = "",
                                Sales = new List<SaleData>(),
                                Sale = 0,
                                Withdrawalable = 0,
                                SalesCount = 0
                            }
                        );
                    return 0;
                }
            }
            else
            {
                return -1;
            }
        }

        // sendPasswordResetEmail
        public int SendPasswordResetEmail(string email, string code="7e7x")
        {
            try {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("AgroZee", "agrozeegglobal@gmail.com"));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = "Password Reset";
                message.Body = new TextPart("plain")
                {
                    Text = "Proceed with this " + code + " through app"
                };
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("agrozeegglobal@gmail.com", "Global7869");
                    client.Send(message);
                    client.Disconnect(true);
                }
                return 0;
            }
            catch (Exception)
            {
                return -1;
            }
            
        }

        /////////////////////////////////////////////////
        /////////////////////////////////////////////////
        /////////////////< Dashboard >///////////////////
        /////////////////////////////////////////////////
        /////////////////////////////////////////////////

        // getDashboardData
        // Name  ==> from Profile
        // CurrentBalance, Sale(Demand)Amount, WithdrawalableAmount ==> from Account of the required User
        public UserDataField GetDashboardData(string email) {
            if (Repository.MongoHelper.ConnectToDataMongoService())
            {
                var filter = Builders<UserDataField>.Filter.Eq("Email", email);
                //var filter = builder.And(builder.Eq("Email", email), builder.Eq("Password", password));
                //var projection = Builders<UserDataField>.Projection.Include("Name").Include("Sale").Include("Withdrawalable").Exclude("_id");
                List<UserDataField> result = MongoHelper.GetProfileCollection(email).Find(filter).ToList();
                if(result.Count==1)
                { 
                    return result[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        //ValidReceiver(emailto, customerIDto)=="no"
        public UserDataField ValidReceiver(string email, string customerID)
        {
            if (Repository.MongoHelper.ConnectToDataMongoService())
            {
                var builder = Builders<UserDataField>.Filter;
                var filter = builder.And(builder.Eq("Email", email), builder.Eq("CustomerID", customerID));
                //var projection = Builders<UserDataField>.Projection.Include("Name").Include("Sale").Include("Withdrawalable").Exclude("_id");
                List<UserDataField> result = MongoHelper.GetProfileCollection(email).Find(filter).ToList();
                if (result.Count == 1)
                {
                    return result[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }



        /////////////////////////////////////////////////
        /////////////////////////////////////////////////
        /////////////////< Profile >/////////////////////
        /////////////////////////////////////////////////
        /////////////////////////////////////////////////

        public dynamic UpdateAddressMobile(string email, string address, string mobile)
        {
            if (MongoHelper.ConnectToDataMongoService())
            {
                var filter = Builders<UserDataField>.Filter.Eq("Email", email);
                var update = Builders<UserDataField>.Update.Set("Address", address).Set("Mobile", mobile).CurrentDate("lastModified");
                var result = MongoHelper.GetProfileCollection(email).UpdateOne(filter, update);
                return result;
            }
            else
            {
                return null;
            }
        }

        public dynamic UpdatePassword(string email, string oldp, string newp)
        {
            if (MongoHelper.ConnectToMongoService())
            {
                var builder = Builders<registration>.Filter;
                var filter = builder.And(builder.Eq("Email", email), builder.Eq("Password", oldp));
                var update = Builders<registration>.Update.Set("Password", newp).CurrentDate("lastModified");
                var result = MongoHelper.clients_collection().UpdateOne(filter, update);
                return result;
            }
            else
            {
                return null;
            }
        }

        public dynamic UpdateImage(string email, string image)
        {
            if (MongoHelper.ConnectToDataMongoService())
            {
                var filter = Builders<UserDataField>.Filter.Eq("Email", email);
                var update = Builders<UserDataField>.Update.Set("ProfileImage", image).CurrentDate("lastModified");
                var result = MongoHelper.GetProfileCollection(email).UpdateOne(filter, update);
                return result;
            }
            else
            {
                return null;
            }
        }

        /////////////////////////////////////////////////
        /////////////////////////////////////////////////
        /////////////////< Account >/////////////////////
        /////////////////////////////////////////////////
        /////////////////////////////////////////////////


        public dynamic updateSalesCount(string email, int count)
        {
            if (MongoHelper.ConnectToDataMongoService())
            {
                var filter = Builders<UserDataField>.Filter.Eq("Email", email);
                var update = Builders<UserDataField>.Update.Set("SalesCount", count).CurrentDate("lastModified");
                var result = MongoHelper.GetProfileCollection(email).UpdateOne(filter, update);
                return result;
            }
            else
            {
                return null;
            }
        }


        public dynamic updateSaleAmount(string email, double saleAmount)
        {
            if (MongoHelper.ConnectToDataMongoService())
            {
                var filter = Builders<UserDataField>.Filter.Eq("Email", email);
                var update = Builders<UserDataField>.Update.Set("Sale", saleAmount).CurrentDate("lastModified");
                var result = MongoHelper.GetProfileCollection(email).UpdateOne(filter, update);
                return result;
            }
            else
            {
                return null;
            }
        }

        
        // updateWithdrawalableAmount
        public dynamic UpdateWithdrawalableAmount(string email, double withdarwAmount)
        {
            if (MongoHelper.ConnectToDataMongoService())
            {
                var filter = Builders<UserDataField>.Filter.Eq("Email", email);
                var update = Builders<UserDataField>.Update.Set("Withdrawalable", withdarwAmount).CurrentDate("lastModified");
                var result = MongoHelper.GetProfileCollection(email).UpdateOne(filter, update);
                return result;
            }
            else
            {
                return null;
            }
        }

        /////////////////////////////////////////////////
        /////////////////////////////////////////////////
        ////////////////////< Sale >/////////////////////
        /////////////////////////////////////////////////
        /////////////////////////////////////////////////

        // addNewSale
        public dynamic AddNewSale(string email, string image, string cat, double quant, string address, double demand, string date)
        {
            if (Repository.MongoHelper.ConnectToSaleMongoService())
            {
                try
                {
                    Object id = GenerateRandomID(24);
                    Repository.MongoHelper.GetSaleCollection(email).InsertOneAsync(
                            new SaleData
                            {
                                _id = id,
                                Category = cat,
                                Quantity = quant,
                                Address = address,
                                Demand = demand,
                                SaleImageURL = image,
                                PublishDate = date
                            }
                        );
                    return 0;
                }
                catch(Exception e)
                {
                    return null;
                }
            }
            else
            {
                return -1;

            }
        }
        // updateSale
        public dynamic UpdateSaleRequest(string email, string id, string image, string cat, double quant, string address, double demand, string date) {
            if (MongoHelper.ConnectToSaleMongoService())
            {
               var filter = Builders<SaleData>.Filter.Eq("_id", id);
                var update = Builders<SaleData>.Update
                    .Set("Category", cat)
                    .Set("Quantity", quant)
                    .Set("Address", address)
                    .Set("Demand", demand)
                    .Set("SaleImageURL", image)
                    .Set("PublishDate", date)

                    .CurrentDate("lastModified");
                var result = MongoHelper.GetSaleCollection(email).UpdateOne(filter, update);
                return result;
            }
            else
            {
                return null;
            }
        }

        // DeleteSale
        public dynamic DeleteSale(string email, string id){
            if (MongoHelper.ConnectToSaleMongoService())
            {
                var filter = Builders<SaleData>.Filter.Eq("_id", id);
                var result = MongoHelper.GetSaleCollection(email).DeleteOne(filter);

                return result;
            }
            else
            {
                return null;
            }
        }

        /////////////////////////////////////////////////
        /////////////////////////////////////////////////
        /////////////////////////////////////////////////
        /////////////////////////////////////////////////
    }
}
