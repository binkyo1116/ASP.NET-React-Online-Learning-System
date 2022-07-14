using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTModelFunctionsTest
{
    [TestClass]
    public class PasswordUpdateController_Tests
    {
        [TestMethod]
        [DataRow("123456", "708", "OK")]
        [DataRow("invalid-password", "708", "")]
        public void UpdatePasswordWithValidAndInvalidCurrentPassword_ReturnsOkAndEmptyResponse(string currentPassword, string studentId, string expectedResponse)
        {
            var controller = new PasswordUpdateController();
            var response = controller.Post(new PasswordUpdateStudentInfo { CurrentPassword = currentPassword, NewPassword = "123456", StudentId = studentId });

            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(okResponse.StatusCode, 200);

            var resultValue = okResponse.Value as PasswordUpdateResultInfo;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(resultValue.Result, expectedResponse);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdatePasswordWithInvalidStudentId_ThrowsException()
        {
            var controller = new PasswordUpdateController();
            var response = controller.Post(new PasswordUpdateStudentInfo { CurrentPassword = "invalid-password", NewPassword = "123456", StudentId = "-1" });
        }
    }
}
