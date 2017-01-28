using System.Web.Mvc;
using Crouny.DAL.EntityModel;
using Crouny.DAL.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crouny.Web.Controllers.Web;

namespace Crouny.Web.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            var entities = new CrounyEntities();
            // Arrange
            HomeController controller = new HomeController(new DeviceRepository(entities), new RuleRepository(entities));

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
