using System;
using System.Web.Mvc;
using CandidateCodeTest.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CandidateCodeTest.Tests.Controllers
{
    [TestClass]
    public class CustomerControllerTest
    {
        public void Index()
        {
            // Arrange
            CustomerController controller = new CustomerController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
