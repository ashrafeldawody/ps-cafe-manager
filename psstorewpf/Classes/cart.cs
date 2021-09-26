using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace psstorewpf.Classes
{
    public class Cart
    {
        public string name { get; set; }
        public decimal price { get; set; }
        public decimal total_price => price * qty;
        public decimal qty { get; set; } = 1;
    }

}
