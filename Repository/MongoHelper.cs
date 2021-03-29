﻿using MongoDB.Driver;
using ograzeeApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ograzeeApi.Repository
{
    public class MongoHelper
    {
        // different clients
        public static IMongoClient Userclient { get; set; }
        public static IMongoClient Dataclient { get; set; }
        public static IMongoClient Saleclient { get; set; }

        // different database
        public static IMongoDatabase Userdatabase { get; set; }
        public static IMongoDatabase Datadatabase { get; set; }
        public static IMongoDatabase Saledatabase { get; set; }


        public static string mongoConnection = "mongodb+srv://agro123:agro123@cluster0.iwszy.mongodb.net/";

        public static IMongoCollection<registration> clients;
        public static IMongoCollection<UserDataField> profile;

        internal static bool ConnectToMongoService(string mongoDb="SUsers") // SUsers, SData  
        {
            try
            {
                if (Userclient == null)
                {
                    Userclient = new MongoClient(mongoConnection);
                    Userdatabase = Userclient.GetDatabase(mongoDb);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal static bool ConnectToDataMongoService(string mongoDb="SData") // SUsers, SData  
        {
            try
            {
                if (Dataclient == null)
                {
                    Dataclient = new MongoClient(mongoConnection);
                    Datadatabase = Dataclient.GetDatabase(mongoDb);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal static bool ConnectToSaleMongoService(string mongoDb = "Sale") // SUsers, SData  
        {
            try
            {
                if (Saleclient == null)
                {
                    Saleclient = new MongoClient(mongoConnection);
                    Saledatabase = Saleclient.GetDatabase(mongoDb);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static IMongoCollection<registration> clients_collection()
        {
            return Userdatabase.GetCollection<registration>("Clients");
        }

        public static IMongoCollection<UserDataField> GetProfileCollection(string collection) //email
        {
            return Datadatabase.GetCollection<UserDataField>(collection);
        }

        public static IMongoCollection<SaleData> GetSaleCollection(string collection) //email
        {
            return Saledatabase.GetCollection<SaleData>(collection);
        }

    }
}
