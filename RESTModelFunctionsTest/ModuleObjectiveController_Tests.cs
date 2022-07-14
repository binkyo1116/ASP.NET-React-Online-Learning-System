using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTModelFunctionsTest
{
    [TestClass]
    public class ModuleObjectiveController_Tests
    {
        [TestMethod]
        public void GetModuleObjectiveWithValidHashAndModuleId_ReturnsValidResults()
        {
            ModuleObjectiveController moduleObjectiveController = new ModuleObjectiveController();
            var response = moduleObjectiveController.Post(new ModuleObjectiveStudentInfo { Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", CourseInstanceId = 115, ModuleId = 506 });
            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(okResponse.StatusCode, 200);

            var resultValue = okResponse.Value as ModuleInfo;
            Assert.IsNotNull(resultValue);
            Assert.IsNotNull(resultValue.Description);
            Assert.AreEqual(resultValue.StudentId, 708);
            Assert.IsTrue(resultValue.ModuleObjectives.Count > 0);
            Assert.IsTrue(resultValue.ModuleObjectives.Any(a => a.Quizzes.Count > 0));
            Assert.IsTrue(resultValue.ModuleObjectives.Any(a => a.Polls.Count > 0));
            Assert.IsTrue(resultValue.ModuleObjectives.Any(a => a.Materials.Count > 0));
            Assert.IsTrue(resultValue.ModuleObjectives.Any(a => a.Discussions.Count > 0));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        [DataRow("invalidHash", 506)]
        public void GetModuleObjectiveWithInvalidModuleId_ThrowsInvalidOperationException(string hash, int moduleId)
        {
            ModuleObjectiveController moduleObjectiveController = new ModuleObjectiveController();
            var response = moduleObjectiveController.Post(new ModuleObjectiveStudentInfo { Hash = hash, CourseInstanceId = 115, ModuleId = moduleId });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [DataRow("bce20431-5af2-4837-812f-5a2c5b65ce53", -1)]
        public void GetModuleObjectiveWithInvalidHash_ThrowsArgumentOutOfRangeException(string hash, int moduleId)
        {
            ModuleObjectiveController moduleObjectiveController = new ModuleObjectiveController();
            var response = moduleObjectiveController.Post(new ModuleObjectiveStudentInfo { Hash = hash, CourseInstanceId = 115, ModuleId = moduleId });
        }
    }
}
