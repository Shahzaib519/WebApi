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


        private static Random random= new Random();
        private string GenerateRandomID(int v)
        {
            string strArray = "abcdefghijklmnopqrstuvwxyz123456789";
            return new string(Enumerable.
                Repeat(strArray, v)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // sign-up
        // create new user in USers
        public long SignUp(string email, string password, string userName, string mobileNo) {
            if (Repository.MongoHelper.ConnectToMongoService() && Repository.MongoHelper.ConnectToDataMongoService())
            {
                var filter = Builders<registration>.Filter.Eq("Email", email);
                var result = Repository.MongoHelper.clients_collection().Find(filter).ToList();
                if (result.Count ==1)
                {
                    return -2;
                }
                else
                {
                    int currentCount = GetClientsCount();
                    string id = currentCount + GenerateRandomID(24);
                    // Create Credentials for new user
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
                    // Setup profile for new user
                    Repository.MongoHelper.GetProfileCollection(email).InsertOneAsync(
                            new UserDataField
                            {
                                Id = id,
                                CustomerID = "AGRO" + mobileNo,
                                Name = userName,
                                Email = email,
                                Mobile = mobileNo,
                                EmailVerified = "no",
                                Address = "",
                                ProfileImage = "",
                                Sale = 0,
                                Withdrawalable = 0,
                                SalesCount = 0
                            }
                        );
                    // increment client count
                    return IncrementClientCount();
                }
            }
            else
            {
                return -3;
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
                    Text = "Proceed with this " + code + " through app. "
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

        public UpdateResult UpdateAddressMobile(string email, string address, string mobile)
        {
            if (MongoHelper.ConnectToDataMongoService())
            {
                var filter = Builders<UserDataField>.Filter.Eq("Email", email);
                var update = Builders<UserDataField>.Update.Set("Address", address).Set("Mobile", mobile).CurrentDate("lastModified");
                UpdateResult result = MongoHelper.GetProfileCollection(email).UpdateOne(filter, update);
                return result;
            }
            else
            {
                return null;
            }
        }

        public UpdateResult UpdatePassword(string email, string oldp, string newp)
        {
            if (MongoHelper.ConnectToMongoService())
            {
                var builder = Builders<registration>.Filter;
                var filter = builder.And(builder.Eq("Email", email), builder.Eq("Password", oldp));
                var update = Builders<registration>.Update.Set("Password", newp).CurrentDate("lastModified");
                UpdateResult result = MongoHelper.clients_collection().UpdateOne(filter, update);
                return result;
            }
            else
            {
                return null;
            }
        }

        public UpdateResult UpdateImage(string email, string image)
        {
            if (MongoHelper.ConnectToDataMongoService())
            {
                var filter = Builders<UserDataField>.Filter.Eq("Email", email);
                var update = Builders<UserDataField>.Update.Set("ProfileImage", image).CurrentDate("lastModified");
                UpdateResult result = MongoHelper.GetProfileCollection(email).UpdateOne(filter, update);
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

        long IncrementClientCount()
        {
            // get and then increament
            int current = GetClientsCount();
            if(current == -1 )
            {
                return -1;
            }
            return updateClientsCount(current + 1);
        }

        long IncrementSaleCount()
        {
            // get and then increament
            int current = GetSalesCount();
            if (current == -1)
            {
                return -1;
            }
            return updateSalesCount(current + 1);
        }


        //////////////////////////////////////
        ///////////// Get Counts /////////////
        //////////////////////////////////////
        public int GetSalesCount()
        {
            if (Repository.MongoHelper.ConnectToSysMongoService())
            {
                var filter = Builders<SysData>.Filter.Eq("Key", "0123456789");
                //var filter = builder.And(builder.Eq("Email", email), builder.Eq("Password", password));
                //var projection = Builders<UserDataField>.Projection.Include("Name").Include("Sale").Include("Withdrawalable").Exclude("_id");
                List<SysData> result = MongoHelper.GetSysCollection().Find(filter).ToList();
                if (result.Count == 1)
                {
                    return result[0].SalesCount;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return -1;
            }
        }

        public int GetClientsCount()
        {
            if (Repository.MongoHelper.ConnectToSysMongoService())
            {
                var filter = Builders<SysData>.Filter.Eq("Key", "0123456789");
                //var filter = builder.And(builder.Eq("Email", email), builder.Eq("Password", password));
                //var projection = Builders<UserDataField>.Projection.Include("Name").Include("Sale").Include("Withdrawalable").Exclude("_id");
                List<SysData> result = MongoHelper.GetSysCollection().Find(filter).ToList();
                if (result.Count == 1)
                {
                    return result[0].ClientsCount;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return -1;
            }
        }

        public int GetTransactionCount()
        {
            if (Repository.MongoHelper.ConnectToSysMongoService())
            {
                var filter = Builders<SysData>.Filter.Eq("Key", "0123456789");
                //var filter = builder.And(builder.Eq("Email", email), builder.Eq("Password", password));
                //var projection = Builders<UserDataField>.Projection.Include("Name").Include("Sale").Include("Withdrawalable").Exclude("_id");
                List<SysData> result = MongoHelper.GetSysCollection().Find(filter).ToList();
                if (result.Count == 1)
                {
                    return result[0].TransactionsCount;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return -1;
            }
        }


        //////////////////////////////////////
        //////////// Update Counts ///////////
        //////////////////////////////////////
        public long updateSalesCount(int count)
        {
            if (MongoHelper.ConnectToSysMongoService())
            {
                var filter = Builders<SysData>.Filter.Empty;
                var update = Builders<SysData>.Update.Set("SalesCount", count).CurrentDate("lastModified");
                UpdateResult result = MongoHelper.GetSysCollection().UpdateOne(filter, update);
                return result.ModifiedCount;
            }
            else
            {
                return -1;
            }
        }

        public long updateTransactionsCount(int count)
        {
            if (MongoHelper.ConnectToSysMongoService())
            {
                var filter = Builders<SysData>.Filter.Empty;
                var update = Builders<SysData>.Update.Set("TransactionsCount", count).CurrentDate("lastModified");
                UpdateResult result = MongoHelper.GetSysCollection().UpdateOne(filter, update);
                return result.ModifiedCount;
            }
            else
            {
                return -1;
            }
        }

        public long updateClientsCount(int count)
        {
            if (MongoHelper.ConnectToSysMongoService())
            {
                var filter = Builders<SysData>.Filter.Empty;
                var update = Builders<SysData>.Update.Set("ClientsCount", count).CurrentDate("lastModified");
                UpdateResult result = MongoHelper.GetSysCollection().UpdateOne(filter, update);
                return result.ModifiedCount;
            }
            else
            {
                return -1;
            }
        }


        //////////////////////////////////////
        ///////// Update Sale Amount /////////
        //////////////////////////////////////
        public UpdateResult updateSaleAmount(string email, double saleAmount)
        {
            if (MongoHelper.ConnectToDataMongoService())
            {
                var filter = Builders<UserDataField>.Filter.Eq("Email", email);
                var update = Builders<UserDataField>.Update.Set("Sale", saleAmount).CurrentDate("lastModified");
                UpdateResult result = MongoHelper.GetProfileCollection(email).UpdateOne(filter, update);
                return result;
            }
            else
            {
                return null;
            }
        }


        ///////////////////////////////////////////////////
        //////////// updateWithdrawalableAmount ///////////
        ///////////////////////////////////////////////////
        public UpdateResult UpdateWithdrawalableAmount(string email, double withdarwAmount)
        {
            if (MongoHelper.ConnectToDataMongoService())
            {
                var filter = Builders<UserDataField>.Filter.Eq("Email", email);
                var update = Builders<UserDataField>.Update.Set("Withdrawalable", withdarwAmount).CurrentDate("lastModified");
                UpdateResult result = MongoHelper.GetProfileCollection(email).UpdateOne(filter, update);
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
        public long AddNewSale(string email, string image, string cat, double quant, string address, double demand, string date)
        {
            if (MongoHelper.ConnectToSaleMongoService())
            {
                try
                {
                    int number = GetSalesCount();
                    string id = number.ToString()+GenerateRandomID(24);
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
                   return IncrementSaleCount();
                }
                catch(Exception e)
                {
                    return -2;
                }
            }
            else
            {
                return -3;

            }
        }
        
        
        // updateSale
        public UpdateResult UpdateSaleRequest(string email, string id, string image, string cat, double quant, string address, double demand, string date) {
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
                UpdateResult result = MongoHelper.GetSaleCollection(email).UpdateOne(filter, update);
                return result;
            }
            else
            {
                return null;
            }
        }

        // DeleteSale
        public DeleteResult DeleteSale(string email, string id){
            if (MongoHelper.ConnectToSaleMongoService())
            {
                var filter = Builders<SaleData>.Filter.Eq("_id", id);
                DeleteResult result = MongoHelper.GetSaleCollection(email).DeleteOne(filter);

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
