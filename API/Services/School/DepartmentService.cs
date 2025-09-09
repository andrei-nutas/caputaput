using System.Text.Json.Serialization;
using TAF_iSAMS.Test.API.Auth;
using TAF_iSAMS.Test.API.Config;
using TAF_iSAMS.Test.API.Models.School;

namespace TAF_iSAMS.Test.API.Services.School
{
    /// <summary>
    /// Service for interacting with department endpoints
    /// </summary>
    public class DepartmentService : BaseApiService
    {
        public DepartmentService(ApiConfig apiConfig, OAuthHandler oAuthHandler)
            : base(apiConfig, oAuthHandler)
        {
        }

        /// <summary>
        /// Gets all teaching departments
        /// </summary>
        /// <param name="expandSubjects">Whether to include subjects in the response</param>
        /// <returns>A list of departments</returns>
        public async Task<List<Department>> GetTeachingDepartmentsAsync(bool expandSubjects = false)
        {
            var endpoint = expandSubjects
                ? "school/departments/teaching?expand=subjects"
                : "school/departments/teaching";

            var response = await GetAsync<DepartmentResponse>(endpoint);
            return response.Departments;
        }

        /// <summary>
        /// Gets all non-teaching departments
        /// </summary>
        /// <returns>A list of departments</returns>
        public async Task<List<Department>> GetNonTeachingDepartmentsAsync()
        {
            var response = await GetAsync<DepartmentResponse>("school/departments/nonteaching");
            return response.Departments;
        }

        /// <summary>
        /// Gets a teaching department by ID
        /// </summary>
        /// <param name="departmentId">The ID of the department</param>
        /// <returns>The department</returns>
        public async Task<Department> GetTeachingDepartmentAsync(string departmentId)
        {
            // Individual department endpoint might not use a wrapper
            return await GetAsync<Department>($"school/departments/teaching/{departmentId}");
        }

        /// <summary>
        /// Gets a non-teaching department by ID
        /// </summary>
        /// <param name="departmentId">The ID of the department</param>
        /// <returns>The department</returns>
        public async Task<Department> GetNonTeachingDepartmentAsync(string departmentId)
        {
            // Individual department endpoint might not use a wrapper
            return await GetAsync<Department>($"school/departments/nonteaching/{departmentId}");
        }

        /// <summary>
        /// Gets the subjects associated with a department
        /// </summary>
        /// <param name="departmentId">The ID of the department</param>
        /// <returns>A list of subjects</returns>
        public async Task<List<Subject>> GetDepartmentSubjectsAsync(string departmentId)
        {
            // This endpoint might also use a wrapper
            try
            {
                return await GetAsync<List<Subject>>($"school/departments/teaching/{departmentId}/subjects");
            }
            catch (System.Text.Json.JsonException)
            {
                // If direct conversion fails, try with a wrapper
                var response = await GetAsync<SubjectsResponse>($"school/departments/teaching/{departmentId}/subjects");
                return response.Subjects;
            }
        }

        /// <summary>
        /// Associates a subject with a department
        /// </summary>
        /// <param name="departmentId">The ID of the department</param>
        /// <param name="subjectId">The ID of the subject</param>
        /// <returns>True if the operation was successful</returns>
        public async Task<bool> AssociateSubjectWithDepartmentAsync(string departmentId, string subjectId)
        {
            using var client = await CreateHttpClientAsync();

            var response = await client.PostAsync(
                $"{ApiConfig.BaseUrl}/school/departments/teaching/{departmentId}/subjects/{subjectId}",
                null);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Disassociates a subject from a department
        /// </summary>
        /// <param name="departmentId">The ID of the department</param>
        /// <param name="subjectId">The ID of the subject</param>
        /// <returns>True if the operation was successful</returns>
        public async Task<bool> DisassociateSubjectFromDepartmentAsync(string departmentId, string subjectId)
        {
            return await DeleteAsync($"school/departments/teaching/{departmentId}/subjects/{subjectId}");
        }

        /// <summary>
        /// Gets the employees associated with a teaching department
        /// </summary>
        /// <param name="departmentId">The ID of the department</param>
        /// <returns>A list of employees</returns>
        public async Task<List<object>> GetTeachingDepartmentEmployeesAsync(string departmentId)
        {
            // This endpoint might also use a wrapper
            try
            {
                return await GetAsync<List<object>>($"school/departments/teaching/{departmentId}/employees");
            }
            catch (System.Text.Json.JsonException)
            {
                // If direct conversion fails, try with a wrapper
                var response = await GetAsync<EmployeesResponse>($"school/departments/teaching/{departmentId}/employees");
                return response.Employees;
            }
        }

        /// <summary>
        /// Gets the employees associated with a non-teaching department
        /// </summary>
        /// <param name="departmentId">The ID of the department</param>
        /// <returns>A list of employees</returns>
        public async Task<List<object>> GetNonTeachingDepartmentEmployeesAsync(string departmentId)
        {
            // This endpoint might also use a wrapper
            try
            {
                return await GetAsync<List<object>>($"school/departments/nonteaching/{departmentId}/employees");
            }
            catch (System.Text.Json.JsonException)
            {
                // If direct conversion fails, try with a wrapper
                var response = await GetAsync<EmployeesResponse>($"school/departments/nonteaching/{departmentId}/employees");
                return response.Employees;
            }
        }
    }

    /// <summary>
    /// Represents the response from the subjects endpoint
    /// </summary>
    public class SubjectsResponse
    {
        [JsonPropertyName("subjects")]
        public List<Subject> Subjects { get; set; }
    }

    /// <summary>
    /// Represents the response from the employees endpoint
    /// </summary>
    public class EmployeesResponse
    {
        [JsonPropertyName("employees")]
        public List<object> Employees { get; set; }
    }
}