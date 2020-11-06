using BdoDailyCatBot.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Users>> GetAll()
        {
            return new List<Users>();
        }

        public async Task<bool> Add(Users user)
        {
            if ((await db.Users.Where(p => p.IdDiscord == user.IdDiscord).ToListAsync()).Count > 0)
            {
                return false;
            }

            try
            {
                await db.Users.AddAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // TODO: ...
            }

            return false;
        }
    }
}
