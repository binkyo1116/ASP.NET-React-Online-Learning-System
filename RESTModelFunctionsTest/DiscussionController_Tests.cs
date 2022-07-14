using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTModelFunctionsTest
{
    [TestClass]
    public class DiscussionController_Tests
    {
        [TestMethod]
        public void GetDiscussionsWithValidInput_ReturnDiscussions()
        {
            DiscussionController controller = new DiscussionController();
            var si = new DiscussionStudentInfo { Method = "Get", Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", CourseInstanceId = 115, DiscussionBoardId = 22, ModuleObjectiveId = 498 };
            var result = controller.Post(si);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.StatusCode, 200);

            var ResultValue = okResult.Value as DiscussionResultInfo;
            Assert.IsNotNull(ResultValue);
            Assert.IsTrue(ResultValue.Posts.Count > 0);
            Assert.IsNotNull(ResultValue.BoardTitle);
        }

        [TestMethod]
        public void AddAndDeleteDiscussion_ReturnsPostDeleted()
        {
            var unitTestDiscussionTitle = "Discussion Created Through Unit Test";
            var unitTestDiscussionDescription = "This unit test created by unit test. Should have been deleted by unit test after creating.";
            DiscussionController controller = new DiscussionController();
            var si = new DiscussionStudentInfo { Method = "Add", Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", CourseInstanceId = 115, DiscussionBoardId = 22, NewPostTitle = unitTestDiscussionTitle , NewPostDescription = unitTestDiscussionDescription, ModuleObjectiveId = 498 };
            var result = controller.Post(si);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.StatusCode, 200);

            var ResultValue = okResult.Value as DiscussionResultInfo;
            Assert.AreEqual(ResultValue.Result, "Your post was saved");

            //Get Created Post to Delete it 
            si.Method = "Get";
            result = controller.Post(si);
            okResult = result.Result as OkObjectResult;
            ResultValue = okResult.Value as DiscussionResultInfo;

            var createdDiscussion = ResultValue.Posts.Where(a => a.Title == unitTestDiscussionTitle && a.Description == unitTestDiscussionDescription).FirstOrDefault();
            Assert.IsNotNull(createdDiscussion);

            //Delete the created Discussion
            si.Method = "Delete";
            si.DiscussionPostId = createdDiscussion.Id;
            result = controller.Post(si);
            okResult = result.Result as OkObjectResult;
            ResultValue = okResult.Value as DiscussionResultInfo;
            Assert.AreEqual(ResultValue.Result, "Your post was deleted");

        }

        [TestMethod]
        public void DeleteDiscussionWithOtherStudentId_ReturnsNotAllowed()
        {
            DiscussionController controller = new DiscussionController();
            var si = new DiscussionStudentInfo { Method = "Delete", Hash = "123jklkauaxjaff", DiscussionPostId = 57, CourseInstanceId = 115, DiscussionBoardId = 22, ModuleObjectiveId = 498 };
            var result = controller.Post(si);
            var okResult = result.Result as OkObjectResult;
            var ResultValue = okResult.Value as DiscussionResultInfo;
            
            Assert.AreEqual(ResultValue.Result, "You have no rights to delete this post");
        }

        [TestMethod]
        public void DeleteDiscussionWithInvalidStudentId_ReturnsPostNotFound()
        {
            DiscussionController controller = new DiscussionController();
            var si = new DiscussionStudentInfo { Method = "Delete", Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", DiscussionPostId = -1, CourseInstanceId = 115, DiscussionBoardId = 22, ModuleObjectiveId = 498 };
            var result = controller.Post(si);
            var okResult = result.Result as OkObjectResult;
            var ResultValue = okResult.Value as DiscussionResultInfo;

            Assert.AreEqual(ResultValue.Result, "Post is not found");
        }

        [TestMethod]
        public void UpdateDiscussionWithValidInput_ReturnsPostUpdated()
        {
            var controller = new DiscussionController();
            var si = new DiscussionStudentInfo { Method = "Update", Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", DiscussionPostId = 57, CourseInstanceId = 115, DiscussionBoardId = 22, ModuleObjectiveId = 498 };
            si.NewPostTitle = "Discussion Created Through Unit Test For Get method";
            si.NewPostDescription = "This unit test created by unit test for get and update method.";
            var result = controller.Post(si);
            var okResult = result.Result as OkObjectResult;
            var ResultValue = okResult.Value as DiscussionResultInfo;

            Assert.AreEqual(ResultValue.Result, "Your post was updated");
        }

        [TestMethod]
        public void UpdateDiscussionOfOtherStudent_ReturnsNotAllowed()
        {
            var controller = new DiscussionController();
            var si = new DiscussionStudentInfo { Method = "Update", Hash = "123jklkauaxjaff", DiscussionPostId = 57, CourseInstanceId = 115, DiscussionBoardId = 22, ModuleObjectiveId = 498 };
            si.NewPostTitle = "Discussion Created Through Unit Test For Get method";
            si.NewPostDescription = "This unit test created by unit test for get and update method.";
            var result = controller.Post(si);
            var okResult = result.Result as OkObjectResult;
            var ResultValue = okResult.Value as DiscussionResultInfo;

            Assert.AreEqual(ResultValue.Result, "You have no rights to update this post");
        }

        [TestMethod]
        public void UpdateDiscussionWithInvalidDiscussionId_ReturnsPostNotFound()
        {
            var controller = new DiscussionController();
            var si = new DiscussionStudentInfo { Method = "Update", Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", DiscussionPostId = -1, CourseInstanceId = 115, DiscussionBoardId = 22, ModuleObjectiveId = 498 };
            si.NewPostTitle = "Discussion Created Through Unit Test For Get method";
            si.NewPostDescription = "This unit test created by unit test for get and update method.";
            var result = controller.Post(si);
            var okResult = result.Result as OkObjectResult;
            var ResultValue = okResult.Value as DiscussionResultInfo;

            Assert.AreEqual(ResultValue.Result, "Post is not found");
        }

        [TestMethod]
        public void DiscussionWithoutMethod_ReturnsEmtpyResponse()
        {
            var controller = new DiscussionController();
            var si = new DiscussionStudentInfo { Method = "", Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", DiscussionPostId = -1, CourseInstanceId = 115, DiscussionBoardId = 22, ModuleObjectiveId = 498 };
            var result = controller.Post(si);
            var okResult = result.Result as OkObjectResult;

            Assert.IsNull(okResult);        
        }
    }
}
