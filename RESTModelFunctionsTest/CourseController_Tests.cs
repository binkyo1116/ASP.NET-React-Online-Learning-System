using Microsoft.AspNetCore.Http;

namespace RESTModelFunctionsTest;

[TestClass]
public class CourseController_Tests
{
    #region GetCourses
    [TestMethod]
    public void GetCourses_WithValidHashOfStudentEnrolled_ReturnsCourses()
    {
        CourseController courseController = new CourseController();
        var studentInfo = new CourseStudentInfo { Method = "Get", Hash = "bce20431-5af2-4837-812f-5a2c5b65ce53", IsAdmin = "false" };
        var result = courseController.Post(studentInfo);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var resultValue = okResult.Value as CourseResult;

        Assert.IsTrue(resultValue?.CourseList.Count > 0);
    }

    [TestMethod]
    [DataRow("123jklkauaxjaff")]
    [DataRow("invalidhash")]
    public void GetCourses_WithInvalidHashAndValidHashOfStudentNotEnrolled_ReturnsZeroCourses(string studentHash)
    {
        CourseController courseController = new CourseController();
        var studentInfo = new CourseStudentInfo { Method = "Get", Hash = studentHash, IsAdmin = "false" };
        var result = courseController.Post(studentInfo);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var resultValue = okResult.Value as CourseResult;

        Assert.IsTrue(resultValue?.CourseList.Count == 0);
    }


    [TestMethod]
    public void GetCourses_WithValidHashOfAdminUser_ReturnsCoursesAndStudents()
    {
        CourseController courseController = new CourseController();
        var studentInfo = new CourseStudentInfo { Method = "Get", Hash = "fa0846a6-e736-43e2-9f0d-429eb3563c83", AdminHash = "fa0846a6-e736-43e2-9f0d-429eb3563c83", IsAdmin = "true" };
        var result = courseController.Post(studentInfo);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var resultValue = okResult.Value as CourseResult;

        Assert.IsTrue(resultValue?.CourseList.Count > 0);
        Assert.IsTrue(resultValue?.StudentList.Count > 0);
    }
    #endregion

    #region NavigateStudents

    [TestMethod]
    public void NavigateStuduent_WithInvalidStudentId_ReturnsStudentNotFound()
    {
        CourseController courseController = new CourseController();
        var studentInfo = new CourseStudentInfo { Method = "NavigateStudent", AdminHash = "fa0846a6-e736-43e2-9f0d-429eb3563c83", SelectedStudentId = 0, IsAdmin = "true" };
        var result = courseController.Post(studentInfo);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var resultValue = okResult.Value as StudentResultInfo;

        Assert.AreEqual(resultValue?.error, "Student not found");
    }

    [TestMethod]
    public void NavigateStuduent_WithStudentIdNotEnrolled_ReturnsStudentNotEnrolled()
    {
        CourseController courseController = new CourseController();
        var studentInfo = new CourseStudentInfo { Method = "NavigateStudent", AdminHash = "fa0846a6-e736-43e2-9f0d-429eb3563c83", SelectedStudentId = 709, IsAdmin = "true" };
        var result = courseController.Post(studentInfo);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var resultValue = okResult.Value as StudentResultInfo;

        Assert.AreEqual(resultValue?.error, "Student not registered in a course");
    }

    [TestMethod]
    public void NavigateStuduent_RegisteredInCourseAndHasProfilePic_ReturnsStudentProfilePic()
    {
        CourseController courseController = new CourseController();
        var studentInfo = new CourseStudentInfo { Method = "NavigateStudent", AdminHash = "fa0846a6-e736-43e2-9f0d-429eb3563c83", SelectedStudentId = 708, IsAdmin = "true" };
        var result = courseController.Post(studentInfo);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var resultValue = okResult.Value as StudentResultInfo;

        Assert.IsFalse(string.IsNullOrWhiteSpace(resultValue?.Picture));
    }

    [TestMethod]
    [DataRow("fa0846a6-e736-43e2-9f0d-429eb3563c83", "1", false)]
    [DataRow("fa0846a6-e736-43e2-9f0d-429eb3563c83", "", true)]
    [DataRow("", "1", true)]
    public void NavigateStuduent_WithOutAdminOrAdminHashOrStudentId_ReturnsEmptyStudentName(string adminHash, string studentId, bool isAdmin)
    {
        CourseController courseController = new CourseController();
        var studentInfo = new CourseStudentInfo { Method = "NavigateStudent", AdminHash = adminHash, SelectedStudentId = string.IsNullOrEmpty(studentId)?0:Convert.ToInt32(studentId), IsAdmin = isAdmin.ToString() };
        var result = courseController.Post(studentInfo);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var resultValue = okResult.Value as StudentResultInfo;

        Assert.IsNull(resultValue?.StudentName);
    }

    [TestMethod]
    [DataRow("Get", "invalidhash", "invalidadminhash")]
    [DataRow("NavigateStudent", "invalidhash", "invalidadminhash")]
    [ExpectedException(typeof(InvalidOperationException))]
    public void InvalidHashOrAdminHash_ThrowsException(string method, string hash, string adminHash)
    {
        CourseController courseController = new CourseController();
        var studentInfo = new CourseStudentInfo { Method = method, AdminHash = adminHash, IsAdmin = "true" };
        var result = courseController.Post(studentInfo);
    }
    #endregion

    #region Grades
    [TestMethod]
    public void GetGrades_ValidAdminHash_ReturnsCoursesWithTotalGrades()
    {
        CourseController courseController = new CourseController();
        var studentInfo = new CourseStudentInfo { Method = "Grades", Hash = "fa0846a6-e736-43e2-9f0d-429eb3563c83", AdminHash = "fa0846a6-e736-43e2-9f0d-429eb3563c83", IsAdmin = "true" };
        var result = courseController.Post(studentInfo);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var resultValue = okResult.Value as CourseResult;

        Assert.IsTrue(resultValue?.CourseList.Count > 0);
        Assert.IsTrue(resultValue?.CourseList.Any(a => a.TotalGrade != ""));
    }

    [TestMethod]
    public void GetGrades_ValidInvalidStudentHash_ReturnsCoursesWithTotalGrades()
    {
        CourseController courseController = new CourseController();
        var studentInfo = new CourseStudentInfo { Method = "Grades", Hash = "invalidhash", IsAdmin = "true" };
        var result = courseController.Post(studentInfo);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var resultValue = okResult.Value as CourseResult;

        Assert.IsFalse(resultValue?.CourseList.Count > 0);
        Assert.IsFalse(resultValue?.CourseList.Any(a => a.TotalGrade != ""));
    }
    #endregion

    [TestMethod]
    public void GetCoursesWithoutSendingMethod_ReturnsEmptyResponse()
    {
        CourseController courseController = new CourseController();
        var studentInfo = new CourseStudentInfo { Method = "", Hash = "invalidhash", IsAdmin = "true" };
        var result = courseController.Post(studentInfo);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNull(okResult);

    }
}
