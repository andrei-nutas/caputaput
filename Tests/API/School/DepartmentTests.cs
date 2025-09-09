using TAF_iSAMS.Test.API.Services.School;

namespace TAF_iSAMS.Tests.API.School
{
    [TestFixture]
    [Category("School")]
    [Category("Departments")]
    public class DepartmentTests : TestBase
    {
        private DepartmentService _departmentService;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _departmentService = new DepartmentService(ApiConfig, OAuthHandler);
        }

        [Test]
        [Description("Test getting all teaching departments")]
        public async Task CanGetAllTeachingDepartments()
        {
            // Act
            var departments = await _departmentService.GetTeachingDepartmentsAsync();

            // Assert
            Assert.That(departments, Is.Not.Null);
            Assert.That(departments.Count, Is.GreaterThan(0), "Should return at least one teaching department");

            // Log the results
            TestContext.WriteLine($"Retrieved {departments.Count} teaching departments");
            foreach (var dept in departments.Take(3))
            {
                TestContext.WriteLine($"- {dept.Name} ({dept.Code})");
            }
            if (departments.Count > 3)
            {
                TestContext.WriteLine($"- ... and {departments.Count - 3} more");
            }
        }

        [Test]
        [Description("Test getting all teaching departments with subjects")]
        public async Task CanGetAllTeachingDepartmentsWithSubjects()
        {
            // Act
            var departments = await _departmentService.GetTeachingDepartmentsAsync(expandSubjects: true);

            // Assert
            Assert.That(departments, Is.Not.Null);
            Assert.That(departments.Count, Is.GreaterThan(0), "Should return at least one teaching department");

            // Check if any departments have subjects
            var deptsWithSubjects = departments.Count(d => d.Subjects != null && d.Subjects.Count > 0);
            TestContext.WriteLine($"Retrieved {departments.Count} teaching departments, {deptsWithSubjects} have subjects");
        }

        [Test]
        [Description("Test getting all non-teaching departments")]
        public async Task CanGetAllNonTeachingDepartments()
        {
            // Act
            var departments = await _departmentService.GetNonTeachingDepartmentsAsync();

            // Assert
            Assert.That(departments, Is.Not.Null);

            // Log the results
            TestContext.WriteLine($"Retrieved {departments.Count} non-teaching departments");
            foreach (var dept in departments.Take(3))
            {
                TestContext.WriteLine($"- {dept.Name} ({dept.Code})");
            }
            if (departments.Count > 3)
            {
                TestContext.WriteLine($"- ... and {departments.Count - 3} more");
            }
        }

        [Test]
        [Description("Test getting a teaching department by ID")]
        public async Task CanGetTeachingDepartmentById()
        {
            // Arrange
            var departmentId = TestData.ExistingDepartmentId;

            // Act
            var department = await _departmentService.GetTeachingDepartmentAsync(departmentId);

            // Assert
            Assert.That(department, Is.Not.Null);
            Assert.That(department.Id, Is.EqualTo(departmentId));

            // Log the results
            TestContext.WriteLine($"Retrieved department: {department.Name} ({department.Code})");
        }

        [Test]
        [Description("Test getting a non-teaching department by ID")]
        public async Task CanGetNonTeachingDepartmentById()
        {
            // Arrange
            var departmentId = TestData.NonTeachingDepartmentId;

            // Act
            var department = await _departmentService.GetNonTeachingDepartmentAsync(departmentId);

            // Assert
            Assert.That(department, Is.Not.Null);
            Assert.That(department.Id, Is.EqualTo(departmentId));

            // Log the results
            TestContext.WriteLine($"Retrieved non-teaching department: {department.Name} ({department.Code})");
        }

        [Test]
        [Description("Test getting a teaching department that does not exist")]
        public void CannotGetNonExistingTeachingDepartment()
        {
            // Arrange
            var departmentId = "-1"; // Non-existent ID

            // Act & Assert
            var ex = Assert.ThrowsAsync<HttpRequestException>(async () =>
                await _departmentService.GetTeachingDepartmentAsync(departmentId));

            Assert.That(ex.Message, Does.Contain("404"), "Expected a 404 Not Found response");
        }

        [Test]
        [Description("Test getting employees for a teaching department")]
        public async Task CanGetTeachingDepartmentEmployees()
        {
            // Arrange
            var departmentId = TestData.ExistingDepartmentId;

            // Act
            var employees = await _departmentService.GetTeachingDepartmentEmployeesAsync(departmentId);

            // Assert
            Assert.That(employees, Is.Not.Null);

            // Log the results
            TestContext.WriteLine($"Retrieved {employees.Count} employees for teaching department {departmentId}");
        }

        [Test]
        [Description("Test getting subjects for a teaching department")]
        public async Task CanGetDepartmentSubjects()
        {
            // Arrange
            var departmentId = TestData.ExistingDepartmentId;

            // Act
            var subjects = await _departmentService.GetDepartmentSubjectsAsync(departmentId);

            // Assert
            Assert.That(subjects, Is.Not.Null);

            // Log the results
            TestContext.WriteLine($"Retrieved {subjects.Count} subjects for department {departmentId}");
            foreach (var subject in subjects.Take(3))
            {
                TestContext.WriteLine($"- {subject.Name} ({subject.Code})");
            }
            if (subjects.Count > 3)
            {
                TestContext.WriteLine($"- ... and {subjects.Count - 3} more");
            }
        }

        [Test]
        [Description("Test associating and disassociating a subject with a department")]
        public async Task CanAssociateAndDisassociateSubjectWithDepartment()
        {
            // Arrange
            var departmentId = TestData.ExistingDepartmentId;
            var subjectId = TestData.ExistingSubjectId;

            // First, try to disassociate the subject (in case it's already associated)
            try
            {
                await _departmentService.DisassociateSubjectFromDepartmentAsync(departmentId, subjectId);
            }
            catch
            {
                // Ignore errors - subject might not be associated
            }

            // Act - Associate the subject
            var associateResult = await _departmentService.AssociateSubjectWithDepartmentAsync(departmentId, subjectId);

            // Assert
            Assert.That(associateResult, Is.True, "Subject should be associated with department successfully");

            // Verify the association
            var subjects = await _departmentService.GetDepartmentSubjectsAsync(departmentId);
            Assert.That(subjects.Any(s => s.Id == subjectId), Is.True, "Subject should be in the department's subjects");

            // Act - Disassociate the subject
            var disassociateResult = await _departmentService.DisassociateSubjectFromDepartmentAsync(departmentId, subjectId);

            // Assert
            Assert.That(disassociateResult, Is.True, "Subject should be disassociated from department successfully");

            // Verify the disassociation
            subjects = await _departmentService.GetDepartmentSubjectsAsync(departmentId);
            Assert.That(subjects.Any(s => s.Id == subjectId), Is.False, "Subject should not be in the department's subjects");
        }
    }
}