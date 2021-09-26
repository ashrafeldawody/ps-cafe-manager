using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace psstorewpf.Classes
{
    public class CafeInvoice
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string device_name { get; set; } = "";
        public DateTime datetime { get; set; }
        public string formattedDateTime => datetime.ToString("dd/MM/yyyy hh:mm tt");
        public BsonArray items { get; set; }
        public string readableItems => formattedItems();
        public decimal cost { get; set; }


        public CafeInvoice()
        {

        }
        public string formattedItems()
        {
            StringBuilder sb = new StringBuilder();
            foreach(BsonDocument item in items)
            {
                sb.AppendFormat("{0}({1}×{2}) = {3}", item.GetValue("name").ToString(), item.GetValue("quantity").ToString(), item.GetValue("price").ToString(), item.GetValue("total_price").ToString());
                sb.AppendLine("");
            }
            return sb.ToString();
        }
    }
}
