using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Api;

namespace IG.Web.Tests
{
    [TestClass]
    public class CustomerInformationControllerTests
    {
        [TestMethod]
        public void Get_ReturnsExpected()
        {
            CustomerInformationController controller = new CustomerInformationController();
            var result = controller.Get(1);
        }
    }
}
