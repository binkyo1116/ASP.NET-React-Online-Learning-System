using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTModelFunctionsTest
{
    [TestClass]
    public class ForgetPasswordController_Tests
    {
        [TestMethod]
        [DataRow("invalid@email.com", "The email address that you have, doesn't match with any registered email.")]
        [DataRow("unittestaccountwithoutcourses@gmail.com", "The email address is duplicated")]
        public void ForgetPasswordWithInvalidEmail_ReturnsInvalidEmail(string email, string errorMessage)
        {
            var controller = new ForgetPasswordController();
            var response = controller.Post(new SCredential { registerEmail = email });
            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(okResponse.StatusCode, 200);

            var resultValue = okResponse.Value as ForgetPasswordResultInfo;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(resultValue.error, errorMessage);
            Assert.AreEqual(resultValue.success, "");
        }

        [TestMethod]
        public void ForgetPasswordValidEmail_ReturnsNewPasswordToEmail()
        {
            var controller = new ForgetPasswordController();
            var response = controller.Post(new SCredential { registerEmail = "unittestaccount@gmail.com" });
            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(okResponse.StatusCode, 200);

            var resultValue = okResponse.Value as ForgetPasswordResultInfo;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(resultValue.error, "");
            Assert.AreEqual(resultValue.success, "Your new Password has been sent to your register email successfully.");
        }
    }
}
