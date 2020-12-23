using Festival.Models;
using Festival.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Festival.Repository
{
    public class EventRepository:IDisposable, IEventRepository
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

       public IQueryable<Event> GetAll()
        {

            IQueryable<Event> list = db.Events.Include(p=>p.Place).OrderByDescending(p => p.Price);
            return list;
        }
       public Event GetById(int id)
        {
            return db.Events.Include(p => p.Place).FirstOrDefault(g => g.Id == id);
        }

        public void Add(Event ev) 
        {
            db.Events.Add(ev);
            db.SaveChanges();
        }
        public void Update(Event ev)
        {
            db.Entry(ev).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        public void Delete(Event ev) 
        {
            db.Events.Remove(ev);
            db.SaveChanges();
        }


        public IQueryable<Event> GetByYear(int start, int kraj)
        {
            IQueryable<Event> result = db.Events.Include(g => g.Place).Where(g => g.Year >= start && g.Year <= kraj).OrderBy(y=>y.Year);
            return result;
        }

        public IQueryable<EventStatisticsDTO> GetStatistics()
        {
            IQueryable<Event> events = GetAll();
            var rezultat = events.GroupBy(
            g => g.Place,
            g => g.Price,
            (place, sumprice) => new EventStatisticsDTO()
            {
                Id = place.Id,
                Location = place.Location,
                SumPrice = sumprice.Sum()
            }).OrderByDescending(r => r.SumPrice);
            return rezultat.AsQueryable();
        }
    }
}