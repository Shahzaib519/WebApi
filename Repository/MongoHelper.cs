using MongoDB.Driver;
using ograzeeApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ograzeeApi.Repository
{
    public class MongoHelper
    {
        public static IMongoClient client { get; set; }
        public static IMongoDatabase database { get; set; }

        public static string mongoConnection = "mongodb+srv://agro123:agro123@cluster0.iwszy.mongodb.net/";

        public static IMongoCollection<registration> clients;
        public static IMongoCollection<UserDataField> profile;

        internal static bool ConnectToMongoService(string mongoDb) // SUsers, SData  
        {
            try
            {
                client = new MongoClient(mongoConnection);
                database = client.GetDatabase(mongoDb);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static IMongoCollection<registration> clients_collection(string collection) //email
        {
            return database.GetCollection<registration>("Clients");
        }

        public static IMongoCollection<UserDataField> GetProfileCollection(string collection) //email
        {
            return database.GetCollection<UserDataField>(collection);
        }

    }
}
