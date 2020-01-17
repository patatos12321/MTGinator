﻿using LiteDB;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MTGinator.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : IDocument
    {
        protected readonly LiteDatabase _database;

        public GenericRepository(IConfiguration config)
        {
            _database = new LiteDatabase(config["DatabasePath"]);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return GetLiteCollection().FindAll();
        }

        public virtual T GetById(int id)
        {
            return GetLiteCollection().FindOne(d => d.Id == id);
        }

        public virtual void Save(T document)
        {
            var collection = GetLiteCollection();

            var dbDocument = collection.FindOne(d => d.Id == document.Id);
            if (dbDocument == null)
            {
                collection.Insert(document);
                return;
            }

            document.Id = dbDocument.Id;
            collection.Update(document);
        }

        public virtual void Save(IEnumerable<T> documents)
        {
            foreach (var document in documents)
            {
                Save(document);
            }
        }

        public virtual void Delete(int id)
        {
            GetLiteCollection().Delete(p => p.Id == id);
        }

        /// <summary>
        /// Fetch the CollectionNameAttribute to know the name of the collection.
        /// If there isn't any, the name of the collection will be the name of the class;
        /// </summary>
        /// <returns></returns>
        protected virtual LiteCollection<T> GetLiteCollection()
        {
            var typeInfo = typeof(T).GetTypeInfo();
            var attrs = typeInfo.GetCustomAttributes();
            var collectionNameAttribute = (CollectionNameAttribute)attrs.FirstOrDefault(a => a.GetType() == typeof(CollectionNameAttribute));

            if (collectionNameAttribute == null)
            {
                return _database.GetCollection<T>(typeof(T).Name);
            }

            return _database.GetCollection<T>(collectionNameAttribute.Name);
        }

        protected virtual LiteCollection<R> GetLiteCollectionForType<R>()
        {
            var typeInfo = typeof(R).GetTypeInfo();
            var attrs = typeInfo.GetCustomAttributes();
            var collectionNameAttribute = (CollectionNameAttribute)attrs.FirstOrDefault(a => a.GetType() == typeof(CollectionNameAttribute));

            if (collectionNameAttribute == null)
            {
                return _database.GetCollection<R>(typeof(T).Name);
            }

            return _database.GetCollection<R>(collectionNameAttribute.Name);
        }
    }
}
