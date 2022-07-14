using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTModelFunctionsTest
{
    [TestClass]
    public class RevealController_Tests
    {
        [TestMethod]
        public void RevealAlreadyAnsweredQuestion_ReturnsQuestionAnswer()
        {
            var controller = new RevealController();
            var response = controller.Post(new RevealQuestionInfo { History = "", QuestionId = 2400, StudentId = "bce20431-5af2-4837-812f-5a2c5b65ce53" });
            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(200, okResponse.StatusCode);

            var resultValue = okResponse.Value as RevealResultInfo;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(resultValue.Answer, "4");
        }

        [TestMethod]
        public void RevealAnswerWithInvalidQuestion_ReturnsNullAnswer()
        {
            var controller = new RevealController();
            var response = controller.Post(new RevealQuestionInfo { History = "", QuestionId = -1, StudentId = "bce20431-5af2-4837-812f-5a2c5b65ce53" });
            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(200, okResponse.StatusCode);

            var resultValue = okResponse.Value as RevealResultInfo;
            Assert.IsNotNull(resultValue);
            Assert.IsNull(resultValue.Answer);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void RevealAnswerWithInvalidStudentHash_ThrowsNullStudentIdException()
        {
            var controller = new RevealController();
            var response = controller.Post(new RevealQuestionInfo { History = "", QuestionId = 2400, StudentId = "invalidshash" });

        }
    }
}
