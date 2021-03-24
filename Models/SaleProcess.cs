using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ograzeeApi.Models
{
    public class SaleProcess
    {
        public string Category { set; get; }
        public string Quantity { set; get; }
        public string Uom { set; get; }

        public string Address { set; get; }
        public string Demand { set; get; }
        public string Accepted { set; get; }
        public string FinalAmount { set; get; }

        public string SaleImageURL { set; get; }
    }
}
