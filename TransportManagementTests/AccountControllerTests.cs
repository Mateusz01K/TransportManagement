using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TransportManagementTests.Account
{
    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]
        public void RegisterUser_Return_View()
        {
            var controller = new AccountController();
            var result = controller.RegisterUser() as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Register", result.ViewName);
        }
    }
}
