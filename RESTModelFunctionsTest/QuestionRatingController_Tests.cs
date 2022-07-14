using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTModelFunctionsTest
{
    [TestClass]
    public class QuestionRatingController_Tests
    {
        [TestMethod]
        public void UpdateQuestionWithValidRating_ReturnsEmtpyString()
        {
            var controller = new QuestionRatingController();
            var response = controller.Post(new QuestionRatingQuestionInfo { QuestionId = 2399, Rating = 1, StudentId = "bce20431-5af2-4837-812f-5a2c5b65ce53" });
            var OkResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(OkResponse);
            Assert.AreEqual(200, OkResponse.StatusCode);

            var resultValue = OkResponse.Value as string;
            Assert.AreEqual(resultValue, string.Empty);
        }

        [TestMethod]
        [DataRow(-2)]
        [DataRow(2)]
        public void UpdateQuestionWithInvalidRating_ReturnsErrorMessage(int rating)
        {
            var controller = new QuestionRatingController();
            var response = controller.Post(new QuestionRatingQuestionInfo { QuestionId = 2399, Rating = rating, StudentId = "bce20431-5af2-4837-812f-5a2c5b65ce53" });
            var OkResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(OkResponse);
            Assert.AreEqual(200, OkResponse.StatusCode);

            var resultValue = OkResponse.Value as string;
            Assert.AreEqual(resultValue, "Error when sending rating");
        }
    }
}
