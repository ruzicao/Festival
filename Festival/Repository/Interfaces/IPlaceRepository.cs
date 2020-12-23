using Festival.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Festival.Repository.Interfaces
{
    public interface IPlaceRepository
    {
        IQueryable<Place> GetAll();
        Place GetById(int id);
        IQueryable<Place> GetByCode(int kod);
    }
}
