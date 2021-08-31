using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudProject.Data
{
    public interface IUserRepo<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        void Login(string Name, string Password);
        Task<T> SaveAsync(T entity);
    }
}
