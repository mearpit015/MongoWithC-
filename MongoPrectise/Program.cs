using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoPrectise
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            MongoDBConnection dBConnection = new MongoDBConnection("AddressBook");
            // if no database is exist mongoDb automatically create with this name.
            /// Insert Record
            //Users users = new Users()
            //{
            //    Firstname = "Arpit",
            //    Lastname = "Srivastava",
            //    primaryAddress = new PrimaryAddress()
            //    {
            //        City = "Gorakhpur",
            //        Country = "India",
            //        State = "UP",
            //        ZipCode = "273001"

            //    }
            //};
            //dBConnection.Insertrecord("Users", users);

            /// for select records 

            //List<Users> recs = dBConnection.Loadrecords<Users>("Users");

            //foreach (Users item in recs)
            //{
            //    Console.WriteLine($"{item.Id}: {item.Firstname} {item.Lastname}");
            //    if (item.primaryAddress != null)
            //    {
            //        Console.WriteLine($"{item.primaryAddress.City}");
            //    }
            //    Console.WriteLine();
            //}

            /// Select record based on Id

            var rec = dBConnection.LoadRecordBasedOnId<Users>("Users", new Guid("8c69fa68-6bbb-4a9f-a4ba-899e52ae4440"));

            if(rec != null)
            {
                Console.WriteLine($"{rec.Id}: {rec.Firstname} {rec.Lastname}");
            }


            Console.ReadLine();
        }
    }

    internal class Users
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public PrimaryAddress primaryAddress { get; set; }
    }

    internal class PrimaryAddress
    {
        public string City { get; set; }

        public string ZipCode { get; set; }

        public string State { get; set; }

        public string Country { get; set; }
    }

    internal class MongoDBConnection
    {
        private IMongoDatabase db;
        public MongoDBConnection(string database)
        {
            MongoClient client = new MongoClient(); // for local
            db = client.GetDatabase(database);
        }

        public void Insertrecord<T>(string table, T record)
        {
            IMongoCollection<T> collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        public List<T> Loadrecords<T>(string table)
        {
            var Collection = db.GetCollection<T>(table);
            return Collection.Find(new BsonDocument()).ToList();
        }

        public T LoadRecordBasedOnId<T>(string table, Guid guid)
        {
            var Collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", guid);

            return Collection.Find(filter).First();
        }

    }
}
