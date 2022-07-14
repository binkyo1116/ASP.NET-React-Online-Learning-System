namespace RESTModelFunctionsTest;

[TestClass]
public class CourseAnnouncementController_Tests
{
    [TestMethod]
    public void GetCourseAnnouncements_ReturnsAnnouncementsWithStudentDetails()
    {
        CourseAnnouncementController courseAnnouncementController = new CourseAnnouncementController();
        var result = courseAnnouncementController.Post(new CourseAnnouncementStudentInfo { CourseInstanceId = 115 });
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var resultValue = okResult.Value as List<AnnouncementInfo>;
        Assert.IsNotNull(resultValue);
        Assert.IsTrue(resultValue.Count > 0);

        Assert.IsNotNull(resultValue?.FirstOrDefault()?.Name);
        Assert.IsNotNull(resultValue?.FirstOrDefault()?.Photo);
        Assert.IsNotNull(resultValue?.FirstOrDefault()?.Title);
        Assert.IsNotNull(resultValue?.FirstOrDefault()?.Description);

    }

    [TestMethod]
    public void GetCourseAnnouncementsWithInvalidCourseId_ReturnsZeroAnnouncements()
    {
        CourseAnnouncementController courseAnnouncementController = new CourseAnnouncementController();
        var result = courseAnnouncementController.Post(new CourseAnnouncementStudentInfo { CourseInstanceId = -1 });
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var resultValue = okResult.Value as List<AnnouncementInfo>;
        Assert.IsNotNull(resultValue);
        Assert.IsTrue(resultValue.Count == 0);

    }
}
