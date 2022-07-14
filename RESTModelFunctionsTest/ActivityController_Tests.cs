namespace RESTModelFunctionsTest;

    [TestClass]
    public class ActivityController_Tests
    {
        [TestMethod]
        public void StudentActivityTypeAssessment_ReturnsCodingProblem()
        {
            ActivityController activityController = new ActivityController();
            var studentInfo = new StudentInfo { Type = "Assessment", ActivityId = 1 };
            var result = activityController.Post(studentInfo);
            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);            
            
            var resultValue = okResult.Value as ActivityResultInfo;

            Assert.IsTrue(resultValue?.CodingProblemId > 0);
            Assert.AreEqual(resultValue?.Type, "code");
        }

        [TestMethod]
        public void StudentActivityTypeQuiz_ReturnsQuiz()
        {
            ActivityController activityController = new ActivityController();
            var studentInfo = new StudentInfo { Type = "Quiz", ActivityId = 1 };
            var result = activityController.Post(studentInfo);
            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var resultValue = okResult.Value as ActivityResultInfo;

            Assert.IsTrue(resultValue?.Id > 0);
            Assert.AreEqual(resultValue?.Type, "quiz");
        }

        [TestMethod]
        public void StudentActivityTypeMaterial_ReturnsMaterial()
        {
            ActivityController activityController = new ActivityController();
            var studentInfo = new StudentInfo { Type = "Material", ActivityId = 1 };
            var result = activityController.Post(studentInfo);
            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var resultValue = okResult.Value as ActivityResultInfo;

            Assert.IsTrue(resultValue?.Description != null);
        }

    }
