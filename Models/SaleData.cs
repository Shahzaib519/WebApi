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
        public object _id { get; set; }

        [BsonElement]
        public string Category { set; get; }
        public double Quantity { set; get; }
        //public string Uom { set; get; }

        public string Address { set; get; }
        public double Demand { set; get; }
        public string SaleImageURL { set; get; }
        public string PublishDate { set; get; }
    }
}
