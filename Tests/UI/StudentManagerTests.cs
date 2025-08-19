using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAF_iSAMS.Pages.Extensions;
using TAF_iSAMS.Pages.Modules.StudentManager;

namespace TAF_iSAMS.Tests.UI
{
    
        [TestFixture]
        // Optionally you can run tests within this fixture in parallel:
        [Parallelizable(ParallelScope.Children)]
        public class StudentManagerTests : BaseTest
        {
            [Test]
            [Category("AutoLogin")]
            [Category("Student Manager")]
            public async Task Test_Add_New_Student()
            {

                // 1) We are now logged in automatically because of [Category("AutoLogin")]
                //    thanks to logic in BaseTest's [SetUp].

                // 2) Click "View All Modules"
                await homePage.ClickViewAllModulesAsync();

                // 3) In the search bar write "Student Manager" and click on the module.
                var studentManagerPage = await homePage.NavigateToModuleAsync<StudentManagerFunctions>();

                // 4) In the top iFrame, click on "Add Staff"
                //    We'll use your ModuleNavigationBar class to do that:
                await studentManagerPage.NavigateToTabAsync("Add Pupil");

                // 5) Enter student details
                await studentManagerPage.EnterPupilDetails();

                // 6) Click Next button
                await studentManagerPage.ExecuteNextButton();

                // 7) Test asssertion
                var newPupilSuccessMessageVisible = await studentManagerPage.Stage1CompletedVisible();
                Assert.That(newPupilSuccessMessageVisible, Is.True, "New pupil created successfully");

                // 8) Clean up data - delete the newly created pupil to ensure the test is re-usable
                studentManagerPage.DeleteTestPupil();


            }


            [Test]
            [Category("AutoLogin")]
            [Category("Student Resources")]
            public async Task Test_Search_Current_Student()
            {

                // 1) We are now logged in automatically because of [Category("AutoLogin")]
                //    thanks to logic in BaseTest's [SetUp].

                // 2) Click "View All Modules"
                await homePage.ClickViewAllModulesAsync();

                // 3) In the search bar write "Student Manager" and click on the module.
                var studentManagerPage = await homePage.NavigateToModuleAsync<StudentManagerFunctions>();

                // 4) In the top iFrame, click on "Current Pupils"
                //    We'll use your ModuleNavigationBar class to do that:
                await studentManagerPage.NavigateToTabAsync("Current Pupils");

                // 5) Add a current pupil directly to the database to ensure the pupil is available for searching in the test
                studentManagerPage.DeleteSearchCurrentPupil();
                studentManagerPage.AddCurrentTestPupil();

                // 6) Enter the pupil surname and forename within the search page on the Current Pupils tab
                await studentManagerPage.EnterCurrentStudentSearchDetails();

                // 7) Click on the pupil in the search results
                await studentManagerPage.ClickPupilNameOnPage();

                // 8) Assertion
                var pupilRecordOpened = await studentManagerPage.PupilEnrolmentTabVisible();
                Assert.That(pupilRecordOpened, Is.True, "Enrolment tab found");

                // 9) Data clean up - delete the pupil created specifically for this to make the test re-usable
                studentManagerPage.DeleteSearchCurrentPupil();

            }


            [Test]
            [Category("AutoLogin")]
            [Category("Student Resources")]
            public async Task Test_Search_Applicant_Student()
            {

                // 1) We are now logged in automatically because of [Category("AutoLogin")]
                //    thanks to logic in BaseTest's [SetUp].

                // 2) Click "View All Modules"
                await homePage.ClickViewAllModulesAsync();

                // 3) In the search bar write "Student Manager" and click on the module.
                var studentManagerPage = await homePage.NavigateToModuleAsync<StudentManagerFunctions>();

                // 4) In the top iFrame, click on "Applicants" tab
                //    We'll use your ModuleNavigationBar class to do that:
                await studentManagerPage.NavigateToTabAsync("Applicants");

                // 5) Add an applicant directly to the database to ensure the applicant is available for searching in the test
                studentManagerPage.DeleteSearchApplicant();
                studentManagerPage.AddSearchApplicant();

                // 6) Enter the applicant surname and forename within the search page on the Applicants tab
                await studentManagerPage.EnterApplicantStudentSearchDetails();

                // 7) Click on the applicant in the search results
                await studentManagerPage.ClickApplicantNameOnPage();

                // 8) Assertion
                var pupilRecordOpened = await studentManagerPage.FamilyTabVisible();
                Assert.That(pupilRecordOpened, Is.True, "Family tab found");

                // 9) Data clean up - delete the applicant created specifically for this to make the test re-usable
                studentManagerPage.DeleteSearchApplicant();


            }


            [Test]
            [Category("AutoLogin")]
            [Category("Student Resources")]
            public async Task Test_Search_Former_Student()
            {

                // 1) We are now logged in automatically because of [Category("AutoLogin")]
                //    thanks to logic in BaseTest's [SetUp].

                // 2) Click "View All Modules"
                await homePage.ClickViewAllModulesAsync();

                // 3) In the search bar write "Student Manager" and click on the module.
                var studentManagerPage = await homePage.NavigateToModuleAsync<StudentManagerFunctions>();

                // 4) In the top iFrame, click on "Former Pupils" tab
                //    We'll use your ModuleNavigationBar class to do that:
                await studentManagerPage.NavigateToTabAsync("Former Pupils");

                // 5) Add a former pupil directly to the database to ensure the former pupil is available for searching in the test
                studentManagerPage.DeleteFormerPupil();
                studentManagerPage.AddFormerPupil();

                // 6) Enter the former pupil surname and forename within the search page on the Former Pupils tab
                await studentManagerPage.EnterFormerPupilSearchDetails();

                // 7) Click on the pupil in the search results
                await studentManagerPage.ClickFormerPupilNameOnPage();

                // 8) Assertion
                var pupilRecordOpened = await studentManagerPage.FormerPupilFamilyTabVisible();
                Assert.That(pupilRecordOpened, Is.True, "Family tab found");

                // 9) Data clean up - delete the former pupil created specifically for this to make the test re-usable
                studentManagerPage.DeleteFormerPupil();

            }



        [Test]
        [Category("AutoLogin")]
        [Category("Student Resources")]
        public async Task Test_Edit_Current_Student()
        {

            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();

            // 3) In the search bar write "Student Manager" and click on the module.
            var studentManagerPage = await homePage.NavigateToModuleAsync<StudentManagerFunctions>();

            // 4) In the top iFrame, click on "Current Pupils"
            //    We'll use your ModuleNavigationBar class to do that:
            await studentManagerPage.NavigateToTabAsync("Current Pupils");

            // 5) Add a current pupil directly to the database to ensure the pupil is available for searching and editing in the test
            studentManagerPage.DeleteSearchCurrentPupil();
            studentManagerPage.AddCurrentTestPupil();

            // 6) Enter the pupil surname and forename within the search page on the Current Pupils tab
            await studentManagerPage.EnterCurrentStudentSearchDetails();

            // 7) Click on the pupil in the search results
            await studentManagerPage.ClickPupilNameOnPage();

            // 8) Amend the value in the "Middle Names" field on the pupil
            await studentManagerPage.UpdateStudentMiddleNameField();

            // 9) Click on the "Update Data" button
            await studentManagerPage.ClickUpdateDataButton();

            // 10) Assertion
            var pupilRecordUpdated = await studentManagerPage.PupilUpdatedMessageVisible();
            Assert.That(pupilRecordUpdated, Is.True, "Pupil updated message found");

            // 11) Data clean up - delete the pupil created specifically for this to make the test re-usable
            studentManagerPage.DeleteSearchCurrentPupil();


        }


        [Test]
        [Category("AutoLogin")]
        [Category("Student Resources")]
        public async Task Test_Edit_Applicant()
        {

            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();

            // 3) In the search bar write "Student Manager" and click on the module.
            var studentManagerPage = await homePage.NavigateToModuleAsync<StudentManagerFunctions>();

            // 4) In the top iFrame, click on "Applicants"
            //    We'll use your ModuleNavigationBar class to do that:
            await studentManagerPage.NavigateToTabAsync("Applicants");

            // 5) Add an applicant directly to the database to ensure the applicant is available for searching in the test
            studentManagerPage.DeleteSearchApplicant();
            studentManagerPage.AddSearchApplicant();

            // 6) Enter the applicant surname and forename within the search page on the Applicants tab
            await studentManagerPage.EnterApplicantStudentSearchDetails();

            // 7) Click on the applicant in the search results
            await studentManagerPage.ClickApplicantNameOnPage();

            // 8) Amend the value in the "Middle Names" field on the pupil
            await studentManagerPage.UpdateStudentMiddleNameField();

            // 9) Click on the "Update Data" button
            await studentManagerPage.ClickUpdateDataButton();

            // 10) Assertion
            var pupilRecordUpdated = await studentManagerPage.PupilUpdatedMessageVisible();
            Assert.That(pupilRecordUpdated, Is.True, "Pupil updated message found");

            // 11) Data clean up - delete the pupil created specifically for this to make the test re-usable
            studentManagerPage.DeleteSearchApplicant();

        }


        [Test]
        [Category("AutoLogin")]
        [Category("Student Resources")]
        public async Task Test_Edit_Former_Student()
        {

            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();

            // 3) In the search bar write "Student Manager" and click on the module.
            var studentManagerPage = await homePage.NavigateToModuleAsync<StudentManagerFunctions>();

            // 4) In the top iFrame, click on "Former Pupils"
            //    We'll use your ModuleNavigationBar class to do that:
            await studentManagerPage.NavigateToTabAsync("Former Pupils");

            // 5) Add a former pupil directly to the database to ensure the former pupil is available for searching in the test
            studentManagerPage.DeleteFormerPupil();
            studentManagerPage.AddFormerPupil();

            // 6) Enter the former pupil surname and forename within the search page on the Former Pupils tab
            await studentManagerPage.EnterFormerPupilSearchDetails();

            // 7) Click on the pupil in the search results
            await studentManagerPage.ClickFormerPupilNameOnPage();

            // 8) Amend the value in the "Middle Names" field on the pupil
            await studentManagerPage.UpdateStudentMiddleNameField();

            // 9) Click on the "Update Data" button
            await studentManagerPage.ClickUpdateDataButton();

            // 10) Assertion
            var pupilRecordUpdated = await studentManagerPage.PupilUpdatedMessageVisible();
            Assert.That(pupilRecordUpdated, Is.True, "Pupil updated message found");

            // 11) Data clean up - delete the pupil created specifically for this to make the test re-usable
            studentManagerPage.DeleteFormerPupil();

        }


        [Test]
        [Category("AutoLogin")]
        [Category("Student Resources")]
        public async Task Edit_Multiple_Pupils()
        {

            // 1) We are now logged in automatically because of [Category("AutoLogin")]
            //    thanks to logic in BaseTest's [SetUp].

            // 2) Click "View All Modules"
            await homePage.ClickViewAllModulesAsync();

            // 3) In the search bar write "Student Manager" and click on the module.
            var studentManagerPage = await homePage.NavigateToModuleAsync<StudentManagerFunctions>();

            // 4) In the top iFrame, click on "Current Pupils"
            //    We'll use your ModuleNavigationBar class to do that:
            await studentManagerPage.NavigateToTabAsync("Current Pupils");

            // 5) Add two pupils directly to the database to ensure the pupils are available for searching in the test
            studentManagerPage.DeleteSearchCurrentPupil();
            studentManagerPage.AddCurrentTestPupil();
            studentManagerPage.AddCurrentTestPupil();

            // 6) Enter the pupil surname and forename within the search page on the Current Pupils tab
            await studentManagerPage.EnterCurrentStudentSearchDetails();

            // 7) Tick the 'Select all' check box to highlight both staff members in the search results
            await studentManagerPage.TickAllSearchResults();

            //THE REMAINDER OF THIS TEST IS BLOCKED UNTIL STORY 161262 HAS BEEN RESOLVED
            //161262 RELATES TO AN ISSUE USING THE PINK DROP DOWN TO PERFORM A MULTIPLE EDIT

            // 8) Data clean up
            studentManagerPage.DeleteSearchCurrentPupil();

        }


    }

}
