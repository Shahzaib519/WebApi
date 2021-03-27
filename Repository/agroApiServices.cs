using MongoDB.Bson;
using MongoDB.Driver;
using ograzeeApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                                Name = userName,
                                Email = email,
                                Mobile = mobileNo,
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
        void SendPasswordResetEmail(string email) {
            ///////
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
        public dynamic AddNewSale(string email, string image, string cat, string quant, string uom, string address, string demand)
        {
            if (Repository.MongoHelper.ConnectToDataMongoService())
            {
                var filter = Builders<UserDataField>.Filter.Eq("Email", email);
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

        // updateSale
        void UpdateSale(string email/*SaleDataModel sale*/) { 
            //////////
        }

        // updateSale
        void DeleteSale(string email/*SaleDataModel sale pickid*/){
            ////////
        }

        /////////////////////////////////////////////////
        /////////////////////////////////////////////////
        /////////////////////////////////////////////////
        /////////////////////////////////////////////////
    }
}
