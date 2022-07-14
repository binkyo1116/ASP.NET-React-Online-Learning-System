namespace RESTModelFunctionsTest;

[TestClass]
public class LoginController_Tests
{

    [TestMethod]
    public void LoginWithInvalidPassword_ReturnsPasswordIncorrect()
    {
        //send valid username but wrong password
        Credentials credentials = new Credentials
        {
            username = "unittestaccount",
            password = "invalidpassword"
        };
        LoginController loginController = new LoginController();
        var ResultTest = loginController.Post(credentials);
        Assert.AreEqual("Password was incorrect", ResultTest.error);

    }

    [TestMethod]
    public void LoginWithInvalidUsername_RetrunsCannotFindUser()
    {
        //send valid username but wrong password
        Credentials credentials = new Credentials
        {
            username = "invaliduser",
            password = "invalidpassword"
        };
        LoginController loginController = new LoginController();
        var ResultTest = loginController.Post(credentials);
        Assert.AreEqual("Could not find a login with that username", ResultTest.error);

    }

    [TestMethod]
    public void LoginWithCorrectCredentials_ReturnsNoError()
    {
        Credentials credentials = new Credentials
        {
            username = "unittestaccount",
            password = "123456"
        };
        LoginController loginController = new LoginController();
        var ResultTest = loginController.Post(credentials);

        Assert.AreEqual("", ResultTest.error);
    }

    [TestMethod]
    public void LoginWithValidCredential_ButNotRegisteredInCourse_ReturnsNotRegisteredError()
    {
        Credentials credentials = new Credentials
        {
            username = "unittestaccountwithoutcourses",
            password = "123456"
        };
        LoginController loginController = new LoginController();
        var ResultTest = loginController.Post(credentials);

        Assert.AreEqual("Student not registered in a course", ResultTest.error);
    }

    [TestMethod]
    public void LoginWithAdminCredential_ReturnsIsAdminTrue()
    {
        Credentials credentials = new Credentials
        {
            username = "s1",
            password = "p1"
        };
        LoginController loginController = new LoginController();
        var ResultTest = loginController.Post(credentials);

        Assert.AreEqual(true, ResultTest.IsAdmin);
    }

}