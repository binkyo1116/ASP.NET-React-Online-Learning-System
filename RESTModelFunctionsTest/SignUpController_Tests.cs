using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTModelFunctionsTest
{
    [TestClass]
    public class SignUpController_Tests
    {
        [TestMethod]
        [DataRow("unittestaccount", "uniqueemailforunittesting@test.com", "Sorry! User Name already exist. Please select another one.")]
        [DataRow("invalidusernameforunittesting12345", "unittestaccount@gmail.com", "Sorry! E-mail already exist. Please select another one.")]
        public void SignUpUsingExsitingUserNameOrEmail_ReturnsUserAlreadyExists(string username, string email, string expectedAnswer)
        {
            var controller = new SignUpController();
            var response = controller.Post(new SignUpStudentInfo { Email = email, Username = username});
            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(200, okResponse.StatusCode);

            var resultValue = okResponse.Value as SignUpResultInfo;
            Assert.IsNotNull(resultValue);
            Assert.IsFalse(resultValue.Success);
            Assert.AreEqual(resultValue.Message, expectedAnswer);
        }

        [TestMethod]
        public void SignUpWithoutFullNameOrPassword_ReturnsError()
        {
            var controller = new SignUpController();
            var response = controller.Post(new SignUpStudentInfo { Email = "validemail019203@test.com", Username = "validuniqueemailforunittesting0987" });
            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(200, okResponse.StatusCode);

            var resultValue = okResponse.Value as SignUpResultInfo;
            Assert.IsNotNull(resultValue);
            Assert.IsFalse(resultValue.Success);
            var ex = new NullReferenceException();
            Assert.AreEqual(resultValue.Message, ex.Message );
        }
    }
}
