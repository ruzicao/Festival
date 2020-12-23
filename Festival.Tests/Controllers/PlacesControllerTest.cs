using System;
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
    public class PlacesControllerTest
    {
        [TestMethod]
        public void GetReturnsEventsWithSameId()
        {
            var mockRepository = new Mock<IEventRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(new Event { Id = 1, Name="Dogadjaj", Price=200.00M, Year=2016, PlaceId=1 });

            var controller = new EventsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Get(1);
            var contentResult = actionResult as OkNegotiatedContentResult<Event>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Id);
        }

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
        public void DeleteReturnsOk()
        {
            // Arrange
            var mockRepository = new Mock<IEventRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(new Event { Id = 1, Name = "Dogadjaj", Price = 200.00M, Year = 2016, PlaceId = 1 });
            var controller = new EventsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }


        [TestMethod]
        public void PostMethodSetsLocationHeader()
        {
            // Arrange
            var mockRepository = new Mock<IEventRepository>();
            var controller = new EventsController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.Post(new Event { Id = 1, Name = "Dogadjaj", Price = 200.00M, Year = 2016, PlaceId = 1 });
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Event>;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual(1, createdResult.RouteValues["id"]);
        }

    }
}
