using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTModelFunctionsTest
{
    [TestClass]
    public class ContactUsController_Tests
    {
        const string longEmailMessage = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer et imperdiet dolor. Donec consectetur id nibh id placerat. Aliquam mattis augue at lorem egestas mattis. Cras malesuada vitae erat vitae ultrices. Aliquam elit lectus, fermentum sit amet rhoncus eu, sagittis sit amet libero. Ut vitae tortor tristique, accumsan leo ut, congue nisi. Integer aliquam vitae nisl viverra malesuada. Etiam erat curae.";

        [TestMethod]
        public void PostContactUsFormWithoutSenderName_ReturnsErrorMessage()
        {
            ContactUsController contactUsController = new ContactUsController();
            var result = contactUsController.Post(new ContactUsUserCredential { Message = "Test message", SenderEmail = String.Empty, SenderName = String.Empty });
            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var resultValue = okResult.Value as ContactUsResultInfo;
            Assert.AreEqual(resultValue.success, String.Empty);
            Assert.AreEqual(resultValue.error, "Sorry! The Name field cannot be left blank.");
        }

        [TestMethod]
        [DataRow("", "Sorry! The Message field cannot be left blank")]
        [DataRow("@hello%", "Sorry! Message field do not support any special character.")]
        [DataRow(longEmailMessage, "Sorry! The message do not support more than 400 character.Total Character of your message is 410.")]

        public void PostContactUsFormWithInvalidMessage_ReturnsErrorMessage(string message, string expectedMessage)
        {
            ContactUsController contactUsController = new ContactUsController();
            var result = contactUsController.Post(new ContactUsUserCredential { Message = message, SenderEmail = "test@gmail.com", SenderName = "Hamza Anwar" });
            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var resultValue = okResult.Value as ContactUsResultInfo;
            Assert.AreEqual(resultValue.success, String.Empty);
            Assert.AreEqual(resultValue.error, expectedMessage);
        }

        [TestMethod]
        [DataRow("", "Sorry! The Email field cannot be left blank.")]
        [DataRow("invalidemail", "Sorry! Your Email address is not valid. Please provide a valid email address.")]
        public void PostContactUsFormWithInvalidEmail_ReturnsErrorMessage(string emailAddress, string expectedMessage)
        {
            ContactUsController contactUsController = new ContactUsController();
            var result = contactUsController.Post(new ContactUsUserCredential { Message = "Test message", SenderEmail = emailAddress, SenderName = "Hamza Anwar" });
            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var resultValue = okResult.Value as ContactUsResultInfo;
            Assert.AreEqual(resultValue.success, String.Empty);
            Assert.AreEqual(resultValue.error, expectedMessage);
        }

        [TestMethod]
        public void PostContactUsFormWithValidSenderNameAndEmailAndMessage_ReturnsSuccessMessage()
        {
            ContactUsController contactUsController = new ContactUsController();
            var result = contactUsController.Post(new ContactUsUserCredential { Message = "Test message", SenderName = "Muhammad Hamza", SenderEmail = "testemail@gmail.com" });
            var okResult = result.Result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var resultValue = okResult.Value as ContactUsResultInfo;
            Assert.AreNotEqual(resultValue.success, String.Empty);
            Assert.AreEqual(resultValue.error, String.Empty);
            Assert.AreEqual(resultValue.success, "Your Message has been sent successfully.");
        }
    }
}
