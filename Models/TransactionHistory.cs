using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ograzeeApi.Models
{
    public class TransactionHistory
    {
        [BsonElement]
        public object Id { get; set; }

        [BsonElement]
        public string Date { get; set; }
        public double Amount { get; set; }
        public string Type { get; set; }
        public string ToName{ get; set; }
        public string ToEmail { get; set; }
        public string ToCustomerID{ get; set; }
    }
}
