
namespace RESTModelFunctionsTest;

[TestClass]
public class CourseCompletionController_Tests
{
    [TestMethod]
    public void GetCourseCompletionSummaryOfActiveCourseId_ReturnsListOfCourses()
    {
        CourseCompletionController courseCompletionController = new CourseCompletionController();
        var result = courseCompletionController.Post(new CourseCompletionStudentInfo { Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", CourseInstanceId = 116 });
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var resultValue = okResult.Value as List<CourseCompletionSummary>;
        Assert.IsNotNull(resultValue);
        Assert.IsTrue(resultValue?.Count > 0);
    }

    [TestMethod]
    [DataRow(117)]
    [DataRow(-1)]
    public void GetCourseCompletionSummaryOfInactiveAndInvalidCourse_ReturnsEmptyListOfSummaries(int courseInstanceId)
    {
        CourseCompletionController courseCompletionController = new CourseCompletionController();
        var result = courseCompletionController.Post(new CourseCompletionStudentInfo { Hash = "123jklkauaxjaff", CourseInstanceId = courseInstanceId });
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var resultValue = okResult.Value as List<CourseCompletionSummary>;
        Assert.IsNotNull(resultValue);
        Assert.IsTrue(resultValue?.Count == 0);
    }

    [TestMethod]
    public void GetSummaryOfEnrolledCourseWithCompletedAssignments_ReturnsCompletedAssignment()
    {
        CourseCompletionController courseCompletionController = new CourseCompletionController();
        var result = courseCompletionController.Post(new CourseCompletionStudentInfo { Hash = "fa0846a6-e736-43e2-9f0d-429eb3563c83", CourseInstanceId = 104 });
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var resultValue = okResult.Value as List<CourseCompletionSummary>;
        Assert.IsNotNull(resultValue);
        Assert.IsTrue(resultValue?.Count > 0);
        var completedActivityResult = resultValue?.Where(a => a.ActivityId == 1731).Select(a => a.Completed).FirstOrDefault();
        Assert.AreEqual(completedActivityResult, 1);
    }

    [TestMethod]
    public void GetSummaryOfEnrolledCourseWithCompletedItems_ReturnsValidSummary()
    {
        CourseCompletionController courseCompletionController = new CourseCompletionController();
        var result = courseCompletionController.Post(new CourseCompletionStudentInfo { Hash = "fa0846a6-e736-43e2-9f0d-429eb3563c83", CourseInstanceId = 104 });
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var resultValue = okResult.Value as List<CourseCompletionSummary>;
        Assert.IsNotNull(resultValue);
        Assert.IsTrue(resultValue?.Count > 0);
        var completedActivityResult = resultValue?.Where(a => a.ActivityId == 1731).FirstOrDefault();
        Assert.AreEqual(completedActivityResult.Title, "Video Notes");
        Assert.AreEqual(completedActivityResult.Items, 1);
        Assert.AreEqual(completedActivityResult.ItemsCompleted, 1);
        Assert.AreEqual(completedActivityResult.ActivityId, 1731);
        Assert.AreEqual(completedActivityResult.CourseObjective, "Understand the concepts of Data Analysis and Design");
        Assert.AreEqual(completedActivityResult.ActivityType, "Material");
        Assert.AreEqual(completedActivityResult.ModuleObjective, "385");
        Assert.AreEqual(completedActivityResult.ModuleObjectiveId, 385);
    }

    [TestMethod]
    [DataRow(15)]
    [DataRow(16)]
    [DataRow(17)]
    public void GetSummaryOfCourseNotEnrolled_ReturnsAllAssignmentsIncomplete(int courseInstanceId )
    {
        CourseCompletionController courseCompletionController = new CourseCompletionController();
        var result = courseCompletionController.Post(new CourseCompletionStudentInfo { Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", CourseInstanceId = 100 });
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var resultValue = okResult.Value as List<CourseCompletionSummary>;
        
        Assert.IsFalse(resultValue.Any(a => a.Completed == 1));
        Assert.IsFalse(resultValue.Any(a => a.Completed == 1));
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void GetSummaryOfCourseByInvalidUser_ThorwsException()
    {
        CourseCompletionController courseCompletionController = new CourseCompletionController();
        var result = courseCompletionController.Post(new CourseCompletionStudentInfo { Hash = "invalidhash", CourseInstanceId = 100 });
    }

}
