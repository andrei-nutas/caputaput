using TAF_iSAMS.Test.API.Auth;
using TAF_iSAMS.Test.API.Config;
using TAF_iSAMS.Test.API.Models.School;

namespace TAF_iSAMS.Test.API.Services.School
{
    /// <summary>
    /// Service for interacting with form endpoints
    /// </summary>
    public class FormService : BaseApiService
    {
        public FormService(ApiConfig apiConfig, OAuthHandler oAuthHandler)
            : base(apiConfig, oAuthHandler)
        {
        }

        /// <summary>
        /// Gets all forms
        /// </summary>
        /// <returns>A list of forms</returns>
        public async Task<List<Form>> GetAllFormsAsync()
        {
            // Get the wrapped response and return just the forms
            var response = await GetAsync<FormResponse>("school/forms");
            return response.Forms;
        }
    }
}