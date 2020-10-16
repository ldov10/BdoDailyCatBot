using BdoDailyCatBot.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.DataAccess.Repositories
{
    class CaptainsRepository : IRepository<Captains>
    {
        private DbContext db;

        public CaptainsRepository(DbContext context)
        {
            this.db = context;
        }
    }
}
