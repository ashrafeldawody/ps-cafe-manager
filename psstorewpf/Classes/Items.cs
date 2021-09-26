using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace psstorewpf.Classes
{
    public class Items
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public int qty { get; set; }
        public decimal totalPrice => (qty>0)? price*qty: 0;
        DataAccess da = new DataAccess();
        public Items(string name, decimal price,int qty)
        {
            this.name = name;
            this.price = price;
            this.qty = qty;
        }
    }
}
