using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTModelFunctionsTest
{
    // Update the tests after module progress controller is completed. 
    [TestClass]
    public class ModuleProgressController_Tests
    {
        [TestMethod]
        public void ModuleProgressController_ReturnsOkResponse()
        {
            var controller = new ModuleProgressController();
            var response = controller.Post(new ModuleProgressStudentInfo { CourseId = 115, CourseObjectiveId = 273, Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53" });

            var okResponse = response.Result as OkObjectResult;
            Assert.IsNull(okResponse);
        }
    }
}
