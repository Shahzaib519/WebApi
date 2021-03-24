using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ograzeeApi.Models
{
    public class SaleData
    {

        [BsonElement]
        public string Category { set; get; }
        public string Quantity { set; get; }
        public string Uom { set; get; }

        public string Address { set; get; }
        public string Demand { set; get; }
        public string SaleImageURL { set; get; }
    }
}
