﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    [Authorize]
    public class EventsController : ApiController
    {
        IEventRepository _repository { get; set; }

        public EventsController(IEventRepository repository)
        {
            _repository = repository;
        }
        [AllowAnonymous]
        public IQueryable<EventDTO> Get()
        {
            IQueryable<Event> events = _repository.GetAll();
            return events.ProjectTo<EventDTO>();
        }

     
        public IHttpActionResult Get(int id)
        {
            var ev = _repository.GetById(id);
            if (ev == null)
            {
                return NotFound();
            }
            return Ok(ev);
        }

         [Route("api/eventyear")]
        public IQueryable<Event> GetByYear(int start, int end)
        {
            return _repository.GetByYear(start, end);
        }

        public IHttpActionResult Post(Event ev)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.Add(ev);
            return CreatedAtRoute("DefaultApi", new { id = ev.Id }, ev);
        }

        public IHttpActionResult Put(int id, Event ev)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ev.Id)
            {
                return BadRequest();
            }

            try
            {
                _repository.Update(ev);
            }
            catch
            {
                throw;
            }

            return Ok(ev);
        }

        public IHttpActionResult Delete(int id)
        {
            var ev = _repository.GetById(id);
            if (ev == null)
            {
                return NotFound();
            }

            _repository.Delete(ev);
            return Ok();
        }
        
        [Route("api/statistics")]
        public IQueryable<EventStatisticsDTO> GetStatistics()
        {
            return _repository.GetStatistics();
        }
    }
}