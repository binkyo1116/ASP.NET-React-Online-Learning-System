namespace RESTModelFunctionsTest;

[TestClass]
public class CodeErrorsHintsController_Tests
{
    [TestMethod]
    public void GetCourses_ReturnsCourses()
    {
        CodeErrorsHintsController codeErrorsController = new CodeErrorsHintsController();
        var result = codeErrorsController.GetCourses();

        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var resultValue = okResult.Value as List<VmCourse>;
        Assert.IsTrue(resultValue?.Count > 0);
        
    }
}
