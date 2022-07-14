using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTModelFunctionsTest
{
    [TestClass]
    public class ProfileInfoController_Tests
    {
        [TestMethod]
        public void GetStudentProfileInfoWithStudentIdTrue_ReturnsStudentId()
        {
            var controller = new ProfileInfoController();
            var response = controller.Post(new ProfileStudentInfo { Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", StudentId = true });

            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(200, okResponse.StatusCode);

            var resultValue = okResponse.Value as ProfileResultInfo;
            Assert.AreEqual(resultValue.StudentId, 708);
        }

        [TestMethod]
        public void GetStudentProfileWithStudentIdFalse_ReturnsCompleteProfile()
        {
            var controller = new ProfileInfoController();
            var response = controller.Post(new ProfileStudentInfo { Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", StudentId = false });

            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(200, okResponse.StatusCode);

            var resultValue = okResponse.Value as ProfileResultInfo;
            Assert.AreEqual(resultValue.UserName, "unittestaccount");
            Assert.AreEqual(resultValue.Password, "123456");
            Assert.AreEqual(resultValue.FullName, "Unit Test Account");
            Assert.AreEqual(resultValue.Email, "unittestaccount@gmail.com");
            Assert.IsNotNull(resultValue.Photo);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetStudentProfileWithInvalidHash_ThrowsException()
        {
            var controller = new ProfileInfoController();
            var response = controller.Post(new ProfileStudentInfo { Hash = "InvalidHash", StudentId = false });
        }
    }
}
