using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTModelFunctionsTest
{
    [TestClass]
    public class CourseStatisticsController_Tests
    {
        [TestMethod]
        public void GetCourseStatisticsWithValidHashAndCourseInstance_ReturnsCourseDetails()
        {
            CourseStatisticsController controller = new CourseStatisticsController();
            var response = controller.Post(new  CourseStatisticsStudentInfo { Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", CourseInstanceId = 115 });
            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(okResponse.StatusCode, 200);

            var responseValue = okResponse.Value as CourseStatisticsResultInfo;
            Assert.IsNotNull(responseValue);
            Assert.IsTrue(responseValue.CourseName != null && responseValue.CourseName == "courseforunittest");
            Assert.IsTrue(responseValue.Assessment != null);
            Assert.IsTrue(responseValue.Quiz != null);
            Assert.IsTrue(responseValue.Material != null);
            Assert.IsTrue(responseValue.Midterm != null);
            Assert.IsTrue(responseValue.Final != null);
            Assert.IsTrue(responseValue.Poll != null );
            Assert.IsTrue(responseValue.Discussion != null);
            Assert.IsTrue(responseValue.Total != null);

        }

        [TestMethod]
        public void GetCourseStatisticsWithValidHashAndCourseInstance_ReturnsCompletion100ForCompletedItems()
        {
            CourseStatisticsController controller = new CourseStatisticsController();
            var response = controller.Post(new CourseStatisticsStudentInfo { Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", CourseInstanceId = 115 });
            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(okResponse.StatusCode, 200);

            var responseValue = okResponse.Value as CourseStatisticsResultInfo;
            Assert.IsNotNull(responseValue);
            Assert.IsTrue(responseValue.Poll.Completion == 100);

        }

        [TestMethod]
        public void GetCourseStatisticsWithValidHashAndCourseInstance_ReturnsCompletion0ForNonCompletedItems()
        {
            CourseStatisticsController controller = new CourseStatisticsController();
            var response = controller.Post(new CourseStatisticsStudentInfo { Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", CourseInstanceId = 115 });
            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(okResponse.StatusCode, 200);

            var responseValue = okResponse.Value as CourseStatisticsResultInfo;
            Assert.IsNotNull(responseValue);
            Assert.IsTrue(responseValue.Midterm.Completion == 0);

        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        [DataRow("bce20431-5af2-4837-812f-5a2c5b65ce53", -1)]
        [DataRow("invalidhash", 115)]
        public void GetCourseStatisticsWithInValidHashOrCourseInstance_Returns(string hash, int courstInstanceId)
        {
            CourseStatisticsController controller = new CourseStatisticsController();
            var response = controller.Post(new CourseStatisticsStudentInfo { Hash = hash , CourseInstanceId = courstInstanceId });
        }

        [TestMethod]
        public void GetCourseStatisticsWithValidHashAndCourseInstance_ReturnsCorrectTotal()
        {
            CourseStatisticsController controller = new CourseStatisticsController();
            var response = controller.Post(new CourseStatisticsStudentInfo { Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", CourseInstanceId = 115 });
            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(okResponse.StatusCode, 200);

            var responseValue = okResponse.Value as CourseStatisticsResultInfo;
            Assert.IsNotNull(responseValue);
            Assert.AreEqual(responseValue.Total.Weight, responseValue.Discussion.Weight + responseValue.Midterm.Weight + responseValue.Quiz.Weight + responseValue.Assessment.Weight + responseValue.Discussion.Weight + responseValue.Final.Weight + responseValue.Material.Weight + responseValue.Poll.Weight);
            Assert.AreEqual(responseValue.Total.WeightedGrade, 10);
        }

        [TestMethod]
        public void GetCourseStatisticsWithNoGradeScale_ReturnsZeroGPA()
        {
            CourseStatisticsController controller = new CourseStatisticsController();
            var response = controller.Post(new CourseStatisticsStudentInfo { Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", CourseInstanceId = 116 });
            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(okResponse.StatusCode, 200);

            var responseValue = okResponse.Value as CourseStatisticsResultInfo;
            Assert.AreEqual(responseValue.Total.GPA, 0);
            Assert.AreEqual(responseValue.Midterm.GPA, 0);
            Assert.AreEqual(responseValue.Assessment.GPA, 0);
            Assert.AreEqual(responseValue.Final.GPA, 0);
            Assert.AreEqual(responseValue.Poll.GPA, 0);
            Assert.AreEqual(responseValue.Discussion.GPA, 0);
        }
    }
}
