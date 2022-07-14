namespace RESTModelFunctionsTest;

    [TestClass]
    public class BreadcrumbController_Tests
    {
        [TestMethod]
        public void ValidCourseInstanceId_ReturnsCourseName()
        {
            BreadcrumbController breadcrumbController = new BreadcrumbController();
            var incomingInfo = new IncomingInfo { CourseInstanceId = 11 };
            var result = breadcrumbController.Post(incomingInfo);
            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var resultValue = okResult.Value as BreadcrumbResultInfo;

            Assert.IsNotNull(resultValue?.CourseName);
        }

        [TestMethod]
        public void ValidModuleId_ReturnsModuleName()
        {
            BreadcrumbController breadcrumbController = new BreadcrumbController();
            var incomingInfo = new IncomingInfo { ModuleId = 1 };
            var result = breadcrumbController.Post(incomingInfo);
            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var resultValue = okResult.Value as BreadcrumbResultInfo;

            Assert.IsNotNull(resultValue?.ModuleName);
        }

        [TestMethod]
        public void ValidPollGroupId_ReturnsPollName()
        {
            BreadcrumbController breadcrumbController = new BreadcrumbController();
            var incomingInfo = new IncomingInfo { PollGroupId = 2 };
            var result = breadcrumbController.Post(incomingInfo);
            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var resultValue = okResult.Value as BreadcrumbResultInfo;

            Assert.IsNotNull(resultValue?.PollName);
        }

        [TestMethod]
        public void ValidQuestionSetId_ReturnsActivityName()
        {
            BreadcrumbController breadcrumbController = new BreadcrumbController();
            var incomingInfo = new IncomingInfo { QuestionSetId = 1 };
            var result = breadcrumbController.Post(incomingInfo);
            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var resultValue = okResult.Value as BreadcrumbResultInfo;

            Assert.IsNotNull(resultValue?.ActivityName);
        }

        [TestMethod]
        public void ValidCodingProblemId_ReturnsAssessmentName()
        {
            BreadcrumbController breadcrumbController = new BreadcrumbController();
            var incomingInfo = new IncomingInfo { CodingProblemId = 1 };
            var result = breadcrumbController.Post(incomingInfo);
            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var resultValue = okResult.Value as BreadcrumbResultInfo;

            Assert.IsNotNull(resultValue?.AssessmentName);
        }


    }

