using BdoDailyCatBot.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BdoDailyCatBot.DataAccess.Repositories
{
    class CaptainsRepository : IRepository<Captains>
    {
        private DbContext db;

        public CaptainsRepository(DbContext context)
        {
            this.db = context;
        }

        public void Update(Captains captain)
        {
            db.Entry(captain).State = EntityState.Modified;
        }

        public IEnumerable<Captains> GetAll()
        {
            return new List<Captains>();
        }

        public async Task<bool> Add(Captains captain)
        {
            return false;
        }
    }
}
