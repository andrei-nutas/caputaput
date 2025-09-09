using TAF_iSAMS.Test.API.Services.School;

namespace TAF_iSAMS.Tests.API.School
{
    [TestFixture]
    [Category("School")]
    [Category("Forms")]
    public class FormTests : TestBase
    {
        private FormService _formService;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _formService = new FormService(ApiConfig, OAuthHandler);
        }

        [Test]
        [Description("Test getting all forms")]
        public async Task CanGetAllForms()
        {
            // Act
            var forms = await _formService.GetAllFormsAsync();

            // Assert
            Assert.That(forms, Is.Not.Null);

            // Log the results
            TestContext.WriteLine($"Retrieved {forms.Count} forms");
            foreach (var form in forms.Take(5))
            {
                TestContext.WriteLine($"- {form.Name} ({form.Code})");
            }
            if (forms.Count > 5)
            {
                TestContext.WriteLine($"- ... and {forms.Count - 5} more");
            }
        }
    }
}