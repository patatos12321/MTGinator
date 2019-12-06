using LiteDB;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace MTGinator.Repositories
{
    public abstract class AbstractRepository<T> : IRepository<T> where T : IDocument
    {
        protected LiteDatabase _database;

        public AbstractRepository(IConfiguration config)
        {
            _database = new LiteDatabase(config["DatabasePath"]);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return GetLiteCollection().FindAll();
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

        protected abstract LiteCollection<T> GetLiteCollection();
    }
}
