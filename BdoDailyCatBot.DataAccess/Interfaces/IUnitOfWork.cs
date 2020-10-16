using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.DataAccess.Interfaces
{
    interface IUnitOfWork : IDisposable
    {
        IRepository<Users> Users { get; }
        IRepository<Captains> Captains { get; }
        void Save();
    }
}
