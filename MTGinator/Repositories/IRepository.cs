using System.Collections.Generic;

namespace MTGinator.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();

        void Save(T document);

        void Save(IEnumerable<T> document);

        void Delete(int id);
    }
}
