using Festival.Models;
using Festival.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Festival.Controllers
{
    public class PlacesController : ApiController
    {
        IPlaceRepository _repository { get; set; }

        public PlacesController(IPlaceRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Place> Get()
        {
            return _repository.GetAll();
        }

        public IHttpActionResult Get(int id)
        {
            var place = _repository.GetById(id);
            if (place == null)
            {
                return NotFound();
            }
            return Ok(place);
        }


        [Route("api/places")]
        public IQueryable<Place> GetByZipCode(int kod)
        {
            IQueryable<Place> result = _repository.GetByCode(kod);
            return result;
        }


    }
}
