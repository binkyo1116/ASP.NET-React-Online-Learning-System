using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTModelFunctionsTest
{
    [TestClass]
    public class PollResponseController_Tests
    {
        [TestMethod]
        public void PollResponseWithoutMethod_ReturnsNullResponse()
        {
            var controller = new PollResponseController();
            var response = controller.Post(new PollResponseStudentInfo { Method = "" });

            var okResponse = response.Result as OkObjectResult;
            Assert.IsNull(okResponse);
        }

        [TestMethod]
        public void GetPollResponseWithValidInput_ReturnsValidPolls()
        {
            var controller = new PollResponseController();
            var response = controller.Post(new PollResponseStudentInfo { Method = "Get", CourseInstanceId = 115, Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", ModuleObjectiveId = 498, PollGroupId = 38 });

            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(200, okResponse.StatusCode);

            var resultValue = okResponse.Value as List<PollResponseResultInfo>;
            Assert.IsNotNull(resultValue.Count > 0);
            var pollQuestion = resultValue.Where(a => a.PollQuestionId == 172).FirstOrDefault();
            Assert.IsNotNull(pollQuestion);
            Assert.AreEqual(pollQuestion.PollQuestion, "Should we write tests for exceptions?");
        }

        [TestMethod]
        public void GetPollResponseWithInvalidInput_ReturnsZeroPolls()
        {
            var controller = new PollResponseController();
            var response = controller.Post(new PollResponseStudentInfo { Method = "Get", CourseInstanceId = -1, Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", ModuleObjectiveId = -1, PollGroupId = -1 });

            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(200, okResponse.StatusCode);

            var resultValue = okResponse.Value as List<PollResponseResultInfo>;
            Assert.IsNotNull(resultValue.Count == 0);

        }

        [TestMethod]
        public void AddPollResponseAgain_ReturnsPostSaved()
        {
            var controller = new PollResponseController();
            var si = new PollResponseStudentInfo { Method = "Add", CourseInstanceId = 115, Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", ModuleObjectiveId = 498, PollGroupId = 38 };
            si.StudentResponses = new List<studentResponse>
            {
                new studentResponse{ OptionId = 506,  PollQuestionId = 172 },
            };
            var response = controller.Post(si);

            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(200, okResponse.StatusCode);

            var resultValue = okResponse.Value as PollResponseResultInfo;
            Assert.IsNotNull(resultValue.Result == "Your post was saved");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddPollResponseWithInvalidInput_ThrowsException()
        {
            var controller = new PollResponseController();
            var si = new PollResponseStudentInfo { Method = "Add", CourseInstanceId = -1, Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", ModuleObjectiveId = -1, PollGroupId = -1 };

            var response = controller.Post(si);

        }
    }
}
