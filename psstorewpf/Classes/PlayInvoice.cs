using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace psstorewpf.Classes
{
    public class PlayInvoice
    {
        [BsonId,BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required, MaxLength(30), RegularExpression("/^[A-Za-z]+$/")]
        public string device_name { get; set; }
        public Boolean isMulti { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int duration { get { return (int)(endTime - startTime).TotalMinutes; } }
        [BsonIgnore]
        public string type { get; set; }
        public decimal pricePerHour { get; set; }
        public decimal cost { get { return hoursDiff * pricePerHour; } }
        public string paid { get; set; }
        [BsonIgnore]
        public decimal hoursDiff => (decimal)(DateTime.Now - startTime).TotalHours;
        [BsonIgnore]
        public string formattedStartTime => startTime.AddHours(2).ToString("hh:mm:ss tt yyyy-MM-dd");
        [BsonIgnore]
        public string formattedEndTime => endTime.AddHours(2).ToString("hh:mm:ss tt yyyy-MM-dd");
        public PlayInvoice(string dev_name,Boolean isMulti)
        {
            this.device_name = dev_name;
            this.isMulti = isMulti;
        }

        public void insert()
        {
            DataAccess da = new DataAccess();

            da.insert("play_invoices", this);
        }
    }
}
