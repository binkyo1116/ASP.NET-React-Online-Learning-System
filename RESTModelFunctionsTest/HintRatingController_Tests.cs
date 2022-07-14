using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTModelFunctionsTest
{
    [TestClass]
    public class HintRatingController_Tests
    {
        [TestMethod]
        [DataRow(-2)]
        [DataRow(2)]
        public void HintRatingWithInvalidRating_ReturnsError(int rating)
        {
            var controller = new HintRatingController();
            var response = controller.Post(new HintInfo { Rating = rating });
            var okResponse = response.Result as OkObjectResult;
            Assert.IsNotNull(okResponse);
            Assert.AreEqual(okResponse.StatusCode, 200);

            var resultValue = (string)okResponse.Value;
            Assert.AreEqual(resultValue, "Error when sending rating");
        }
    }
}
