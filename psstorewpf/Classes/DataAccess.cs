using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Diagnostics;
namespace psstorewpf.Classes
{
    public class DataAccess
    {
        MongoClient dbClient;
        private IMongoDatabase database;
        public DataAccess()
        {
            dbClient = new MongoClient("mongodb://localhost:27017");
            database = dbClient.GetDatabase("ps4store");
        }
        public List<T> getAll<T>(string table)
        {
            IMongoCollection<T> collection = database.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }
        public List<T> getDateTimeRange<T>(string table, string field, DateTime fromDate, DateTime toDate)
        {
            var dateQuery = new BsonDocument
            {
                {field , new BsonDocument {
                    { "$gt" , fromDate},
                    { "$lt" , toDate}
                }}
            };
            IMongoCollection<T> collection = database.GetCollection<T>(table);
            return collection.Find(dateQuery).ToList();
        }
        public T LoadRecordByID<T>(string table, string id)
        {
            var collection = database.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
            return collection.Find(filter).First();
        }
        public void insert<T>(string table,T record)
        {
            var collection = database.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        public void UpdateRecord<T>(string table, string id, T record)
        {
            var collection = database.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
            var result = collection.ReplaceOne(filter, record);
        }
        public void decrementQty<T>(string itemName,int qty)
        {
            var collection = database.GetCollection<T>("items");
            var filter = Builders<T>.Filter.Eq("name", itemName);
            var update = Builders<T>.Update.Inc("qty", -qty);
            var result = collection.FindOneAndUpdate(filter, update);
        }
        public void DeleteRecord<T>(string table,string field,string val)
        {
            var coll = database.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq(field, val);
            coll.DeleteOne(filter);
        }
    }
}
