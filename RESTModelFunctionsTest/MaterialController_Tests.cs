using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTModelFunctionsTest
{
    [TestClass]
    public class MaterialController_Tests
    {
        [TestMethod]
        public void GetMaterialWitValidInput_ReturnsMaterial()
        {
            var controller = new MaterialController();
            var response = controller.Post(new QuestionSetInfo { QuestionSetId = 558, StudentHash = "bce20431-5af2-4837-812f-5a2c5b65ce53" });
            var okResponse = response.Result as OkObjectResult;

            Assert.IsNotNull(okResponse);
            Assert.AreEqual(okResponse.StatusCode, 200);

            var resultValue = okResponse.Value as MaterialResultInfo;

            Assert.IsNotNull(resultValue);
            Assert.IsNotNull(resultValue.Title);
            Assert.IsTrue(resultValue.QuizQuestions.Count > 0);
            Assert.IsTrue(resultValue.Materails.Count > 0);

        }

        [TestMethod]
        [DataRow(-1, "bce20431-5af2-4837-812f-5a2c5b65ce53" )]
        public void GetMaterialWitInvalidActivityId_ReturnsActivityLists(int activityId, string hash)
        {
            var controller = new MaterialController();
            var response = controller.Post(new QuestionSetInfo { QuestionSetId = activityId, StudentHash = hash });
            var okResponse = response.Result as OkObjectResult;

            Assert.IsNotNull(okResponse);
            Assert.AreEqual(okResponse.StatusCode, 200);

            var resultValue = okResponse.Value as MaterialResultInfo;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(resultValue.Title, string.Empty);
            Assert.IsTrue(resultValue.Materails.Count == 0);
            Assert.IsTrue(resultValue.QuizQuestionHints.Count == 0);
            Assert.IsTrue(resultValue.QuizQuestions.Count == 0);

        }

        [TestMethod]
        [DataRow(558, "invalidhash")]
        public void GetMaterialWitInvalidStudentHash_ReturnsNoAnswer(int activityId, string hash)
        {
            var controller = new MaterialController();
            var response = controller.Post(new QuestionSetInfo { QuestionSetId = activityId, StudentHash = hash });
            var okResponse = response.Result as OkObjectResult;

            Assert.IsNotNull(okResponse);
            Assert.AreEqual(okResponse.StatusCode, 200);

            var resultValue = okResponse.Value as MaterialResultInfo;

            Assert.IsNotNull(resultValue);

            Assert.IsTrue(resultValue.QuizQuestions.Count > 0);
            Assert.IsFalse(resultValue.QuizQuestions.Any(a => a.Answer != ""));
        }
    }
}
