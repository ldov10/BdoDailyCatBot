using BdoDailyCatBot.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using BdoDailyCatBot.DataAccess.Entities;
using System.Text;
using System.Threading.Tasks;

namespace BdoDailyCatBot.DataAccess.Repositories
{
    class UsersRepository : IRepository<Users>
    {
        private DbContext db;

        public UsersRepository(DbContext context)
        {
            this.db = context;
        }

        public IEnumerable<Users> GetAll()
        {
            return db.Users;
        }

        public void Update(Users user)
        {
            db.Entry(user).State = EntityState.Modified;
        }

        public async Task Add(Users user)
        {
            await db.Users.AddAsync(user);
        }
    }
}
