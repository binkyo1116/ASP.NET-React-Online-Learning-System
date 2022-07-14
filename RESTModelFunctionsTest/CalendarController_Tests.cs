namespace RESTModelFunctionsTest;

    [TestClass]
    public class CalendarController_Tests
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CalenderInfoWithInvalidStudentHash_ReturnsException()
        {
            CalendarController calendarController = new CalendarController();
            var calendarIncomingInfo = new CalendarIncomingInfo { StudentHash = "invalidstudenthash" };
            var result = calendarController.Post(calendarIncomingInfo);
        }

        [TestMethod]
        public void CalenderInfoWithValidStudentHash_ReturnsOkResponse()
        {
            CalendarController calendarController = new CalendarController();
            var calendarIncomingInfo = new CalendarIncomingInfo { StudentHash = "fa0846a6-e736-43e2-9f0d-429eb3563c83" };
            var result = calendarController.Post(calendarIncomingInfo);
            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

        }
    }

