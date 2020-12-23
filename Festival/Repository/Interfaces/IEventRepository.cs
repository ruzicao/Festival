using Festival.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Festival.Repository.Interfaces
{
    public interface IEventRepository
    {
        IQueryable<Event> GetAll();
        Event GetById(int id);
        void Add(Event ev);
        void Update(Event ev);
        void Delete(Event ev);

        IQueryable<Event> GetByYear(int start, int kraj);
        IQueryable<EventStatisticsDTO> GetStatistics();

    }
}
