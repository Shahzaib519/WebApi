using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ograzeeApi.Models
{
    public class UserDataField
    {
        [BsonElement]
        public string Id { get; set; }


        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string ProfileImage { get; set; }

        public string SaleCount { get; set; }
        public string Sale { get; set; }
        public string Withdrawalable { get; set; }
        public List<SaleData> Sales { get; set; }
    
    }
}
