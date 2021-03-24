using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ograzeeApi.Models
{
    public class login
    {
        [BsonElement]
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
