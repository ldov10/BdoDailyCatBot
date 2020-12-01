using BdoDailyCatBot.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BdoDailyCatBot.DataAccess.Interfaces
{
    public interface IFilesRepository
    {
        Task<bool> Add<T>(T item, Entities.FileTypes FileTypes) where T: ToFile;
        Task<List<T>> GetAll<T>(Entities.FileTypes FileTypes) where T: ToFile;
        //Task Update<T>(T item) where T : ToFile;
    }
}
