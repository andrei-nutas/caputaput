using System.Text.Json;

namespace TAF_iSAMS.Test.TestData.Providers
{
    /// <summary>
    /// Provides test data for school-related tests
    /// </summary>
    public class SchoolDataProvider
    {
        private const string SettingsFilePath = "appsettings.json";
        private static TestData _testData;

        /// <summary>
        /// Gets the test data
        /// </summary>
        /// <returns>The test data</returns>
        public static TestData GetTestData()
        {
            if (_testData != null)
            {
                return _testData;
            }

            try
            {
                // Parse configuration from settings file
                var json = File.ReadAllText(SettingsFilePath);
                var settings = JsonSerializer.Deserialize<TestSettings>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _testData = settings.TestData;

                return _testData;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load test data: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Test data for school-related tests
        /// </summary>
        public class TestData
        {
            public string ExistingDepartmentId { get; set; }
            public string NonTeachingDepartmentId { get; set; }
            public string ExistingSubjectId { get; set; }
            public string HouseId { get; set; }
        }

        /// <summary>
        /// Settings loaded from the configuration file
        /// </summary>
        private class TestSettings
        {
            public TestData TestData { get; set; }
        }
    }
}