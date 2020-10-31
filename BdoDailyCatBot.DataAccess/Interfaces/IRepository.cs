using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BdoDailyCatBot.DataAccess.Interfaces
{
    public interface IRepository<T> where T : class 
    {
        IEnumerable<T> GetAll();
        bool Add(T item);
    }
}
