using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BdoDailyCatBot.DataAccess.Interfaces
{
    public interface IFilesRepository
    {
        Task<bool> Add<T>(T channels, Entities.FileTypes FileTypes);
        Task<List<T>> GetAll<T>(Entities.FileTypes FileTypes);
    }
}
