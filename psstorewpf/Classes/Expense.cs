using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace psstorewpf.Classes
{
    public class Expense
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime datetime { get; set; }
        public string formattedDateTime => datetime.ToString("dd/MM/yyyy hh:mm tt");

        public string type { get; set; }
        public string reason { get; set; }
        public string amount { get; set; }
        public Expense(string _type,string _reason,string _amount)
        {
            datetime = DateTime.Now;
            type = _type;
            reason = _reason;
            amount = _amount;
        }
    }

}
