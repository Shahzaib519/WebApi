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
        public object Id { get; set; }

        [BsonElement]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string ProfileImage { get; set; }

        public int SalesCount { get; set; }
        public double Sale { get; set; }
        public double Withdrawalable { get; set; }
        public dynamic lastModified { get; set; }
        public List<SaleData> Sales { get; set; }
    
    }
}
