using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ograzeeApi.Models
{
    public class SysData
    {
        [BsonId]
        public object _id { get; set; }
        
        [BsonElement]
        public string Key { get; set; }
        public int ClientsCount { get; set; }
        public int SalesCount { get; set; }
        public int TransactionsCount { get; set; }
        public dynamic lastModified { get; set; }
        //

    }
}
