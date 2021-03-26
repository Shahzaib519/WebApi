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
            if(Repository.MongoHelper.ConnectToMongoService("SUsers"))
            {
                var builder = Builders<registration>.Filter;
                var filter = builder.And(builder.Eq("Email", email), builder.Eq("Password", password));
                var result = Repository.MongoHelper.clients_collection().Find(filter).ToList();
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


        // sign-out
        void SignOut() {
            //////
        }

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
            if (Repository.MongoHelper.ConnectToMongoService("SUsers"))
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
        void GetDashboardData(string email) {
            /////////
        }

        /////////////////////////////////////////////////
        /////////////////////////////////////////////////
        /////////////////< Profile >/////////////////////
        /////////////////////////////////////////////////
        /////////////////////////////////////////////////

        // getProfile
        void GetProfile(string email) {
            //////
        }

        // updateProfile
        void UpdateProfile(string email /*ProfileDataModel updatedProfile*/) {
            ///////
        }


        /////////////////////////////////////////////////
        /////////////////////////////////////////////////
        /////////////////< Account >/////////////////////
        /////////////////////////////////////////////////
        /////////////////////////////////////////////////

        // getAccount
        //     CurrentBalance, Sale(Demand)Amount, WithdrawalableAmount
        void GetAccount(string email) {
            ////
        }

        // updateCurrentBalance
        void UpdateCurrentBalance(string email, string addBalance, bool addSub) {
            /////
        }

        // updateSaleAmount
        void UpdateSaleAmount(string email, string addSaleAmount, bool addSub) {
            /////
        }

        // updateWithdrawalableAmount
        void UpdateWithdrawalableAmount(string email, string withdarwAmount, bool addSub) {
            /////
        }

        /////////////////////////////////////////////////
        /////////////////////////////////////////////////
        ////////////////////< Sale >/////////////////////
        /////////////////////////////////////////////////
        /////////////////////////////////////////////////

        // addNewSale
        void AddNewSale(string email /*SaleDataModel sale*/)
        {
            //////
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
