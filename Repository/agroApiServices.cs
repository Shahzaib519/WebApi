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
        public bool SignIn(string email, string password) {
            /////
            if(Repository.MongoHelper.ConnectToMongoService("SUsers"))
            {
                
                return true;
            }
            else
            {
                return false;
            }

        }


        // sign-out
        void SignOut() {
            //////
        }

        // sign-up
        void SignUp(string email, string password, string userName, string mobileNo) {
            //////
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
