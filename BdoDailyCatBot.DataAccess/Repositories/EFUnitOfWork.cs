﻿using BdoDailyCatBot.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BdoDailyCatBot.DataAccess.Repositories
{
    class EFUnitOfWork : IUnitOfWork
    {
        private DbContext db;
        private UsersRepository usersRepository;
        private CaptainsRepository captainsRepository;

        public EFUnitOfWork()
        {
            db = new DbContext();
        }

        public IRepository<Users> Users
        {
            get
            {
                if (usersRepository != null)
                {
                    usersRepository = new UsersRepository(db);
                }

                return usersRepository;
            }
        }

        public IRepository<Captains> Captains
        {
            get
            {
                if (captainsRepository != null)
                {
                    captainsRepository = new CaptainsRepository(db);
                }

                return captainsRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);   
        }
    }
}
