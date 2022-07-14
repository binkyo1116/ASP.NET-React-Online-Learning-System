using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTModelFunctionsTest
{
    [TestClass]
    public class QuestionController_Tests
    {
        [TestMethod]
        public void GetQuestionWithValidIdAndStudentHash_ReturnsQuestionDetails()
        {
            var controller = new QuestionController();
            var response = controller.Post(new QuestionCInfo { QuestionId = 2399, StudentId = "bce20431-5af2-4837-812f-5a2c5b65ce53" });

            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(okResponse.StatusCode, 200);

            var resultValue = okResponse.Value as QuestionResultInfo;
            Assert.IsNotNull(resultValue);

            Assert.AreEqual(resultValue.Answer.ToLower(), "infinite");
            Assert.AreEqual(resultValue.ExpectedAnswer.ToLower(), "infinite");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        [DataRow(-1, "bce20431-5af2-4837-812f-5a2c5b65ce53")]
        [DataRow(2399, "invalid-hash")]
        public void GetQuestionWithInvalidQuestionIdOrStudentHash_ThrowsException(int questionId, string studentHash)
        {
            var controller = new QuestionController();
            var response = controller.Post(new QuestionCInfo { QuestionId = questionId, StudentId = studentHash });
        }
    }
}
