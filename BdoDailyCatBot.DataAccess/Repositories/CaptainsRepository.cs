using BdoDailyCatBot.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using BdoDailyCatBot.DataAccess.Entities;
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
            var state = db.Entry(captain).State = EntityState.Modified;
        }

        public IEnumerable<Captains> GetAll()
        {
            return db.Captains;
        }

        public async Task Add(Captains captain)
        {
            await db.Captains.AddAsync(captain);
        }
    }
}
