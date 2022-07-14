using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTModelFunctionsTest
{
    [TestClass]
    public class CourseObjectiveController_Tests
    {
        [TestMethod]
        public void GetCourseObjectiveWithValidHashAndCourseInstance_ReturnsCourseObjectives()
        {
            CourseObjectiveController controller = new CourseObjectiveController();
            var response = controller.Post(new CourseObjectiveStudentInfo { Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", CourseInstanceId = 115, Method = "GetCourseObjective" });
            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(okResponse.StatusCode, 200);

            var responseValue = okResponse.Value as CourseObjectiveCourseInfo;
            Assert.IsNotNull(responseValue);
            Assert.IsTrue(responseValue.CourseObjectiveList.Count > 0);
            Assert.IsTrue(responseValue.Name != null);
        }

        [TestMethod]
        public void GetCourseObjectivesOfCourseWithModules_ReturnsCourseObjectivesWithModules()
        {
            CourseObjectiveController controller = new CourseObjectiveController();
            var response = controller.Post(new CourseObjectiveStudentInfo { Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", CourseInstanceId = 115, Method = "GetCourseObjective" });
            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(okResponse.StatusCode, 200);

            var responseValue = okResponse.Value as CourseObjectiveCourseInfo;
            Assert.IsNotNull(responseValue);
            Assert.IsTrue(responseValue.CourseObjectiveList.Any(a => a.Modules.Count > 0));
        }

        [TestMethod]
        public void GetGradesWithValidHashAndCourseInstance_ReturnsCourseObjectivesInfoList()
        {
            CourseObjectiveController controller = new CourseObjectiveController();
            var response = controller.Post(new CourseObjectiveStudentInfo { Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", CourseInstanceId = 115, Method = "LoadGrades" });
            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(okResponse.StatusCode, 200);

            var responseValue = okResponse.Value as List<CourseObjectiveInfo>;
            Assert.IsNotNull(responseValue);
            Assert.IsTrue(responseValue.Count > 0);

            Assert.IsTrue(responseValue.Any(a => a.Modules.Count > 0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [DataRow("bce20431-5af2-4837-812f-5a2c5b65ce53", -1)]
        [DataRow("invalidhash", 115)]
        public void GetCourseObjectiveWithInvalidHashOrCourseInstance_ThrowsException(string hash, int courseInstanceId)
        {
            CourseObjectiveController controller = new CourseObjectiveController();
            var response = controller.Post(new CourseObjectiveStudentInfo { Hash = hash, CourseInstanceId = courseInstanceId, Method = "GetCourseObjective" });

        }

        [TestMethod]
        public void GetCourseObjectiveWithoutMethod_ReturnsEmptyResponse()
        {
            CourseObjectiveController controller = new CourseObjectiveController();
            var response = controller.Post(new CourseObjectiveStudentInfo { Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", CourseInstanceId = 115, Method = "" });
            var okResponse = response.Result as OkObjectResult;
            Assert.IsNull(okResponse);

        }
    }
}
