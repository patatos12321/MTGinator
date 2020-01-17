using System.Collections.Generic;

namespace MTGinator.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();

        T GetById(int id);

        void Save(T document);

        void Save(IEnumerable<T> document);

        void Delete(int id);
    }
}
