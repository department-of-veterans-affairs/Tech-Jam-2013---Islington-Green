using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Api;

namespace IG.Web.Tests
{
    [TestClass]
    public class AppointmentControllerTests
    {
        [TestMethod]
        public void Get_ReturnsExpectedValue()
        {
            AppointmentController controller = new AppointmentController();
            var result = controller.Get(1);
        }
    }
}
