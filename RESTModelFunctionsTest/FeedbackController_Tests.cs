using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTModelFunctionsTest
{
    [TestClass]
    public class FeedbackController_Tests
    {
        [TestMethod]
        public void GetFeedbackList_ReturnsFeedbacks()
        {
            var controller = new FeedbackController();
            var result = controller.Post(new FeedbackIncomingInfo { Method = "GetList", CourseInstanceId = 115, StudentHash = "bce20431-5af2-4837-812f-5a2c5b65ce53", OnlyMine = true });
            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.StatusCode, 200);

            var resultValue = okResult.Value as FeedbackResultInfo;
            Assert.IsNotNull(resultValue);
            Assert.IsNotNull(resultValue.CourseName);
            Assert.IsTrue(resultValue.FeedbackList.Count > 0);
        }

        [TestMethod]
        public void FeedbackControllerWithoutMethod_ReturnsEmptyResponse()
        {
            var controller = new FeedbackController();
            var result = controller.Post(new FeedbackIncomingInfo { Method = "", StudentHash = "bce20431-5af2-4837-812f-5a2c5b65ce53" });
            var okResult = result.Result as OkObjectResult;

            Assert.AreEqual(okResult.Value, "");
        }

        [TestMethod]
        public void GetCourses_ReturnsCourses()
        {
            var controller = new FeedbackController();
            var result = controller.Post(new FeedbackIncomingInfo { Method = "GetCourses", StudentHash = "bce20431-5af2-4837-812f-5a2c5b65ce53" });
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.StatusCode, 200);

            var resultValue = okResult.Value as List<StudentCourse>;
            Assert.IsNotNull(resultValue);
            Assert.IsTrue(resultValue.Count > 0);
        }
    }
}
