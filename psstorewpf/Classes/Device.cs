using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psstorewpf.Classes
{
    public class Device
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }
        public string name { set; get; }
        public Decimal pricePerHour { set; get; }
        public Decimal Multi_Price { set; get; }
        static DataAccess da = new DataAccess();
        public Device(string name, decimal normal_price,decimal multi_price)
        {
            this.name = name;
            pricePerHour = normal_price;
            Multi_Price = multi_price;
        }
        public void insert()
        {
            da.insert("devices", this);
        }
        public static List<T> getAll<T>()
        {
            return da.getAll<T>("devices");
        }
    }
}
