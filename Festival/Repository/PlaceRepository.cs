using Festival.Models;
using Festival.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Festival.Repository
{
    public class PlaceRepository:IDisposable, IPlaceRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IQueryable<Place> GetAll()
        {
            return db.Places;
        }
           public Place GetById(int id) 
            { 
                return db.Places.FirstOrDefault(g => g.Id == id);
            }

   
        public IQueryable<Place> GetByCode(int kod)
        {
            return db.Places.Where(p=>p.ZipCode<kod).OrderBy(z =>z.ZipCode);
        }
    }
}