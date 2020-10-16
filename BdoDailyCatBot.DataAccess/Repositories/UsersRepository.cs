using BdoDailyCatBot.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.DataAccess.Repositories
{
    class UsersRepository : IRepository<Users>
    {
        private DbContext db;

        public UsersRepository(DbContext context)
        {
            this.db = context;
        }
    }
}
