using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Festival.Controllers;
using Festival.Models;
using Festival.Repository.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Festival.Tests.Controllers
{
    [TestClass]
    public class EventsControllerTest
    {
        [TestMethod]
        public void GetReturnsEventWithSameId()
        {
            // Arrange
            var mockRepository = new Mock<IEventRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(new Event { Id = 1, Name="Festival2020" });

            var controller = new EventsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Get(1);
            var contentResult = actionResult as OkNegotiatedContentResult<Event>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Id);
        }

        // --------------------------------------------------------------------------------------

        [TestMethod]
        public void GetReturnsNotFound()
        {
            // Arrange
            var mockRepository = new Mock<IEventRepository>();
            var controller = new EventsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Get(1);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteReturnsNotFound()
        {
            // Arrange 
            var mockRepository = new Mock<IEventRepository>();
            var controller = new EventsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        // --------------------------------------------------------------------------------------

        [TestMethod]
        public void DeleteReturnsOk()
        {
            // Arrange
            var mockRepository = new Mock<IEventRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(new Event { Id = 1, Name = "Summer" });
            var controller = new EventsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }

        // --------------------------------------------------------------------------------------

        [TestMethod]
        public void PutReturnsBadRequest()
        {
            // Arrange
            var mockRepository = new Mock<IEventRepository>();
            var controller = new EventsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Put(2, new Event { Id = 1, Name = "Summer" });

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        // -------------------------------------------------------------------------------------

        [TestMethod]
        public void PostMethodSetsLocationHeader()
        {
            // Arrange
            var mockRepository = new Mock<IEventRepository>();
            var controller = new EventsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Post(new Event { Id = 1, Name = "Summer" });
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Event>;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual(1, createdResult.RouteValues["id"]);
        }

        // ------------------------------------------------------------------------------------------

        [TestMethod]
        public void GetReturnsMultipleObjects()
        {
            // Arrange
            List<EventDTO> events = new List<EventDTO>();
            events.Add(new EventDTO { Id = 1, Name = "Novi Sad" });
            events.Add(new EventDTO { Id = 2, Name = "Beograd" });

            var mockRepository = new Mock<IEventRepository>();
            mockRepository.Setup(x => x.GetAll()).Returns(events.AsQueryable());
            var controller = new EventsController(mockRepository.Object);

            // Act
            IQueryable<Event> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(events.Count, result.ToList().Count);
            Assert.AreEqual(events.ElementAt(0), result.ElementAt(0));
            Assert.AreEqual(events.ElementAt(1), result.ElementAt(1));
        }
    }
}
