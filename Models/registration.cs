﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ograzeeApi.Models
{
    public class registration
    {
        [BsonId]
        public string _id { get; set; }


        [BsonElement] 
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public dynamic lastModified { get; set; }

    }
}
