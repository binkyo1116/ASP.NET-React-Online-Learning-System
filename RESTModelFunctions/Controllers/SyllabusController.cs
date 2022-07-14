using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
//using EFModel;
using LMS.Common.ViewModels;
using LMSLibrary;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SyllabusController : ApiController
    {
        public class SyllabusInfo
        {
            public int CourseInstanceId { get; set; }
        }
        public IHttpActionResult Post([FromBody] SyllabusInfo si)
        {
            //MaterialEntities model = new MaterialEntities();
            //CourseInstance ci = model.CourseInstances.Where(x => x.Id == si.CourseInstanceId && x.Active).FirstOrDefault();
            VmCourseInstance ci = GetCourseInstanceById(si.CourseInstanceId);

            //--------------------------------------------------------------------------------------
            ResultInfo syllabusInfo = new ResultInfo();
            if (ci == null)
            {
                System.Web.Http.Results.OkNegotiatedContentResult<ResultInfo> serializedEmptyResult = Ok(syllabusInfo);
                return serializedEmptyResult;
            }
            CourseQuarter quarter = new CourseQuarter();
            //string coursePreReq = "";
            string courseCoReq = "";

            List<CourseSession> sessions = new List<CourseSession>();
            List<CourseInstructor> instructors = new List<CourseInstructor>();
            //List<Prerequisite> prerequisites = new List<Prerequisite>();
            //List<Corequisite> corequisites = new List<Corequisite>();
            List<DescriptionInfo> outcomes = new List<DescriptionInfo>();
            List<DescriptionInfo> instructionMethods = new List<DescriptionInfo>();
            List<DescriptionInfo> technologies = new List<DescriptionInfo>();
            List<DescriptionInfo> textbooks = new List<DescriptionInfo>();
            List<DescriptionInfo> tools = new List<DescriptionInfo>();
            List<DescriptionInfo> supplies = new List<DescriptionInfo>();
            List<DescriptionInfo> materials = new List<DescriptionInfo>();

            List<GradeScale> gradeScales = new List<GradeScale>();
            List<GradeScaleWeight> gradeScaleWeights = new List<GradeScaleWeight>();
            List<DescriptionInfo> policies = new List<DescriptionInfo>();
            List<DescriptionInfo> communityStandards = new List<DescriptionInfo>();
            List<DescriptionInfo> campusPublicSafeties = new List<DescriptionInfo>();
            List<DescriptionInfo> supportServices = new List<DescriptionInfo>();
            List<DescriptionInfo> netiquette = new List<DescriptionInfo>();



            //------------------------------------------------------------------------------
            VmQuarter ciQuarter = GetCourseInstanceQuarter(ci.QuarterId);

            //if (ci.Quarter.Active)
            if (ciQuarter.Active)
            {
                quarter = GetCourseQuarter(ci.QuarterId);
                //ci.Quarter.WithdrawDate.ToString("MMMM dd, yyyy");
                //quarter = new CourseQuarter()
                //{
                //    SchoolName = ci.Quarter.School.Name,
                //    Calendar = ci.Quarter.School.AcademicCalendar,
                //    WithdrawDate = withdrawDate,
                //    SyllabusMessage = ci.Quarter.School.SyllabusMessage,
                //    Name = ci.Quarter.Name
                //};
            }
            //---------------------CourseSession-----------------------------------
            //sessions = (from i in ci.CourseInstanceSessions.Where(x => x.Active)
            //                select new CourseSession
            //                {
            //                    LectureDay = i.Session.LectureDay + " " + Convert12Hours(i.Session.StartTime, i.Session.EndTime),
            //                    Description = i.Session.Description,
            //                    Location = i.Session.Location
            //                }).ToList();
            sessions = GetCourseSession(ci.Id);
            //---------------------InstructorCourses--------------------------------
            List<VmInstructorCourse> instructorCourse = GetInstructorCourse(ci.Id);
            //foreach (InstructorCourse i in ci.InstructorCourses.Where(x => x.Active))
            foreach (VmInstructorCourse i in instructorCourse)
            {
                List<ContactInfo> conInfo = new List<ContactInfo>();
                conInfo = GetContactInfo(i.InstructorId);
                //foreach (ContactInformation c in i.Instructor.ContactInformations.Where(x => x.Active))
                //{
                //    string title = c.Preferred ? "Preferred Contact" : "Contact";
                //    ContactInfo contact = new ContactInfo()
                //    {
                //        Title = title,
                //        Contact = c.ContactInfo
                //    };
                //    conInfo.Add(contact);
                //}
                var instructorAvailableHours = GetInstructorAvailableHoursInfo(i.InstructorId);
                //var instructorAvailableHours = ci.InstructorAvailableHours.Select(iah =>
                //         new InstructorAvailableHoursInfo
                //         {
                //             StartTime = iah.StartTime,
                //             EndTime = iah.EndTime,
                //             DayOfWeek = iah.DayOfWeek
                //         }
                //         ).ToList();

                CourseInstructor cins = new CourseInstructor
                {
                    //Name = i.Instructor.InstructorName,
                    Name = i.InstructorName,
                    ContactInfo = conInfo,
                    InstructorAvailableHours = instructorAvailableHours
                };
                instructors.Add(cins);
            }


            //-----------------------------------CoursePrerequisites--------------------------
            /*int PreReqCount = 1;
            int PreReqtotalCount = ci.Course.CoursePrerequisites.Where(x => x.Active).Count();
            foreach (CoursePrerequisite i in ci.Course.CoursePrerequisites.Where(x => x.Active))
            {
                if (PreReqCount == PreReqtotalCount)
                {
                    coursePreReq += i.Course1.Name;
                }
                else if (PreReqtotalCount >= 3 && PreReqCount == (PreReqtotalCount - 1))
                {
                    coursePreReq += i.Course1.Name + " And ";
                }
                else
                {
                    coursePreReq += i.Course1.Name + " Or ";
                }
                PreReqCount++;
            }*/

            List<RequisiteInfo> coursePreReq = GetRequisiteInfo(ci.CourseId);
            //List<RequisiteInfo> coursePreReq = ci.Course.CourseCorequisites
            //    .Where(cc => cc.Type == "Prerequisite")
            //    .Select(cc => new RequisiteInfo
            //    {
            //        Department = cc.Course1.Department,
            //        Id = cc.Course1.Number.ToString(),
            //        GroupId = cc.GroupId
            //    }).ToList();

            //---------------------CourseCorequisites-------------------------------
            int CoReqCount = 1;
            //int CoReqtotalCount = ci.Course.CourseCorequisites.Where(x => x.Active && x.Type == "Corequisite").Count();
            List<VmCourseCorequisite> courseCorequisiteList = new List<VmCourseCorequisite>();
            courseCorequisiteList = CourseCorequisitesCount(ci.CourseId);
            int CoReqtotalCount = (int)courseCorequisiteList.Count();

            //foreach (CourseCorequisite i in ci.Course.CourseCorequisites.Where(x => x.Active && x.Type == "Corequisite"))
            foreach (VmCourseCorequisite i in courseCorequisiteList)
            {
                if (CoReqCount == CoReqtotalCount)
                {
                    //courseCoReq += model.Courses.Find(i.CorequisiteCourseId).Name;
                    courseCoReq += i.TypeCourseName;
                }
                else if (CoReqtotalCount >= 3 && CoReqCount == (CoReqtotalCount - 1))
                {
                    //courseCoReq += model.Courses.Find(i.CorequisiteCourseId).Name + " Or ";
                    courseCoReq += i.TypeCourseName + " Or ";
                }
                else
                {
                    //courseCoReq += model.Courses.Find(i.CorequisiteCourseId).Name + " Or ";
                    courseCoReq += i.TypeCourseName + " Or ";
                }
                CoReqCount++;
            }

            // Convert to Lambda expressions
            //outcomes = (from a in ci.Course.CourseObjectives select new DescriptionInfo { Description = a.Description }).ToList();
            outcomes = GetCourseObjectiveDescription(ci.CourseId);

            //instructionMethods = (from a in ci.CourseInstructionMethods.Where(x => x.Active) select new DescriptionInfo { Description = a.MethodsOfInstruction.Description }).ToList();
            instructionMethods = GetInstructionMethodDescription(ci.Id);

            //technologies = (from a in ci.Course.CourseTechnologyRequirements.Where(x => x.Active) select new DescriptionInfo { Description = a.TechnologyRequirement.Description }).ToList();
            technologies = GetTechnologieDescription(ci.CourseId);

            //textbooks = (from a in ci.Course.CourseTextbooks.Where(x => x.Active) select new DescriptionInfo { Description = a.Textbook.Description }).ToList();
            textbooks = GetTextBookDescription(ci.CourseId);

            //tools = (from a in ci.Course.CourseRequiredTools.Where(x => x.Active) select new DescriptionInfo { Description = a.RequiredTool.Description }).ToList();
            tools = GetToolsDescription(ci.CourseId);

            //supplies = (from a in ci.Course.CourseSupplies.Where(x => x.Active) select new DescriptionInfo { Description = a.Supply.Description }).ToList();
            supplies = GetSupplieDescription(ci.CourseId);

            //materials = (from a in ci.Course.CourseCourseMaterialRequirements.Where(x => x.Active) select new DescriptionInfo { Description = a.CourseMaterialRequirement.Description }).ToList();
            materials = GetMaterialDescription(ci.CourseId);

            //GradingPolicy gradingPolicy = ci.Quarter.School.GradingPolicies.FirstOrDefault();
            VmGradingPolicy gradingPolicy = GetGradingPolicy(ci.QuarterId);

            //GradeScaleGroup GScaleGroup = ci.Course.GradeScaleGroup;
            VmGradeScaleGroup GScaleGroup = GetGradeScaleGroup(ci.CourseId);// ci.Course.GradeScaleGroup;

            if (GScaleGroup != null)
            {
                //gradeScales = (from a in GScaleGroup.GradeScales select new GradeScale { GPA = a.GPA, Point = a.MaxNumberInPercent + "% - " + a.MinNumberInPercent + "%" }).ToList();
                gradeScales = GetGradeScales(GScaleGroup.Id);
            }

            //GradeWeight GWeight = ci.GradeWeights.FirstOrDefault();
            VmGradeWeight GWeight = GetGradeWeight(ci.Id);

            gradeScaleWeights = new List<GradeScaleWeight>();
            if (GWeight.ActivityWeight != 0)
            {
                gradeScaleWeights.Add(new GradeScaleWeight { Description = "Quizzes", Weight = GWeight.ActivityWeight });
            }
            if (GWeight.AssessmentWeight != 0)
            {
                gradeScaleWeights.Add(new GradeScaleWeight { Description = "Assignments", Weight = GWeight.AssessmentWeight });
            }
            if (GWeight.MaterialWeight != 0)
            {
                gradeScaleWeights.Add(new GradeScaleWeight { Description = "Material", Weight = GWeight.MaterialWeight });
            }
            if (GWeight.DiscussionWeight != 0)
            {
                gradeScaleWeights.Add(new GradeScaleWeight { Description = "Discussion", Weight = GWeight.DiscussionWeight });
            }
            if (GWeight.PollWeight != 0)
            {
                gradeScaleWeights.Add(new GradeScaleWeight { Description = "Poll", Weight = GWeight.PollWeight });
            }
            if (GWeight.MidtermWeight != 0)
            {
                gradeScaleWeights.Add(new GradeScaleWeight { Description = "Midterm", Weight = GWeight.MidtermWeight });
            }
            if (GWeight.FinalWeight != 0)
            {
                gradeScaleWeights.Add(new GradeScaleWeight { Description = "Final", Weight = GWeight.FinalWeight });
            }

            //policies = (from a in ci.Quarter.School.CoursePolicies.Where(x => x.Active)
            //            select new DescriptionInfo
            //            {
            //                Subtitle = a.Subtitle,
            //                Description = a.Description,
            //                Points = a.CoursePolicyPoints.Select(x => new Point { Description = x.Description }).ToList()
            //            }).ToList();

            policies = GetpoliciesDescription(ci.Id);

            //communityStandards = (from a in ci.Quarter.School.CommunityStandards.Where(x => x.Active) select new DescriptionInfo { Subtitle = a.Subtitle, Description = a.Description }).ToList();

            communityStandards = GetCommunityStandardDescription(ci.Id);

            //campusPublicSafeties = (from a in ci.Quarter.School.CampusPublicSafeties.Where(x => x.Active)
            //                        select new DescriptionInfo
            //                        {
            //                            Subtitle = a.Subtitle,
            //                            Description = a.Description,
            //                            Points = a.CampusPublicSafetyPoints.Select(x => new Point { Description = x.Description }).ToList()
            //                        }).ToList();

            campusPublicSafeties = GetCampusPublicSafetiesDescription(ci.Id);

            //supportServices = (from a in ci.Quarter.School.SupportServices.Where(x => x.Active) select new DescriptionInfo { Subtitle = a.Subtitle, Description = a.Description }).ToList();
            supportServices = GetSupportServicesDescription(ci.Id);

            //netiquette = (from a in ci.Quarter.School.Netiquettes.Where(x => x.Active)
            //              select new DescriptionInfo
            //              {
            //                  Subtitle = a.Title,
            //                  Description = a.Description,
            //                  Points = a.NetiquettePoints.Select(x => new Point { Description = x.Description }).ToList(),
            //                  Links = a.NetiquetteLinks.Where(y => y.Active).Select(x => new Link { Description = x.Description, Title = x.Title, AddressLink = x.Link }).ToList(),
            //              }).ToList();

            netiquette = GetNetiquetteDescription(ci.Id);

            //var nonAcademicDays = ci.Quarter.NonAcademicDays.Select(nad =>
            //               new NonAcamedicDaysInfo
            //               {
            //                   StartDate = nad.StartDate,
            //                   EndDate = nad.EndTime,
            //                   Description = nad.Description,
            //                   Type = nad.Type
            //               }).ToList();

            var nonAcademicDays = GetNonAcamedicDaysInfo(ci.QuarterId);

            //var studentSupportResources = ci.Quarter.School.StudentSupportResources.Select(ssr =>
            //               new StudentSupportResources
            //               {
            //                   Title = ssr.Title,
            //                   Link = ssr.Link
            //               }
            //).ToList();

            List<StudentSupportResources> studentSupportResources = GetStudentSupportResource(ci.QuarterId);


            //REVIEW
            //var tentativeAssignmentSchedule = model.TentativeAssignmentSchedule(si.CourseInstanceId);

            //List<TentativeAssignmentSchedule> TemporaryAssignmentScheduleList = new List<TentativeAssignmentSchedule>();
            //foreach (var item in tentativeAssignmentSchedule)
            //{
            //    TentativeAssignmentSchedule current = new TentativeAssignmentSchedule();
            //    current.Title = item.Week;
            //    current.Topic = item.Topic;
            //    current.QuizCount = item.Quizzes ?? 0;
            //    current.AssignmentCount = item.Assignments ?? 0;
            //    current.TypeOfTest = item.Test;
            //    current.Meeting = item.Meeting;
            //    current.DueDate = item.DueDate;

            //    TemporaryAssignmentScheduleList.Add(current);
            //}
            List<TentativeAssignmentSchedule> TemporaryAssignmentScheduleList = GetTentativeAssignmentSchedule(si.CourseInstanceId);

            /*
            List<NonAcamedicDaysInfo> nonAcamedicDays = new List<NonAcamedicDaysInfo>();
            foreach (var nad in ci.Quarter.NonAcademicDays)
            {
                nonAcamedicDays.Add(
                    new NonAcamedicDaysInfo
                    {
                        StartDate = nad.StartDate,
                        EndDate = nad.EndTime,
                        Description = nad.Description,
                        Type = nad.Type
                    });
            }
            */
            //-------------------------------------------Syllabus Data-------------------------------------------------------
            Course course = GetCourse(ci.Id);
            syllabusInfo = new ResultInfo()
            {
                CourseName = course.Name,//ci.Course.Name,
                Credits = course.Credits,//ci.Course.Credits,
                Sessions = sessions,
                Quarter = quarter,
                Instructors = instructors,
                CourseDescription = course.Description,//ci.Course.Description,
                Prerequisites = coursePreReq,
                Corequisites = courseCoReq,
                Outcomes = outcomes,
                InstructionMethods = instructionMethods,
                Technologies = technologies,
                Textbooks = textbooks,
                Tools = tools,
                Supplies = supplies,
                Materials = materials,
                GradingPolicy = gradingPolicy != null ? gradingPolicy.Description : "",
                GradeScales = gradeScales,
                GradeScaleWeights = gradeScaleWeights,
                Policies = policies,
                CommunityStandards = communityStandards,
                CampusPublicSafeties = campusPublicSafeties,
                SupportServices = supportServices,
                Netiquette = netiquette,
                NonAcademicDays = nonAcademicDays,
                StudentSupportResources = studentSupportResources,
                TentativeAssignmentSchedule = TemporaryAssignmentScheduleList
            };

            //------------------------------------------------------------------------------
            System.Web.Http.Results.OkNegotiatedContentResult<ResultInfo> serializedResult = Ok(syllabusInfo);
            return serializedResult;
        }

        #region Check
        private List<VmCourseCorequisite> CourseCorequisitesCount(int courseId)
        {
            //int CourseCorequisitesCount = ci.Course.CourseCorequisites.Where(x => x.Active && x.Type == "Corequisite").Count();

            string sqlQueryCourseCorequisites = $@"select cc.CourseId, cc.CorequisiteCourseId, cc.Active, cc.GroupId, cc.Type, cc.Id, c.Name, c2.Name as TypeCourseName from CourseCorequisite cc
                                                inner join Course c on cc.CourseId = c.Id 
                                                inner join Course c2 on cc.CorequisiteCourseId = c2.Id
                                                where cc.CourseId = {courseId} and cc.Active = 1 and cc.Type = 'Corequisite'";

            var courseCorequisitesData = SQLHelper.RunSqlQuery(sqlQueryCourseCorequisites);

            List<VmCourseCorequisite> list = new List<VmCourseCorequisite>();
            if (courseCorequisitesData.Count > 0)
            {
                foreach (var item in courseCorequisitesData)
                {
                    VmCourseCorequisite courseCorequisite = new VmCourseCorequisite
                    {
                        CourseId = (int)item[0],
                        CorequisiteCourseId = (int)item[1],
                        Active = (bool)item[2],
                        GroupId = (int)item[3],
                        Type = item[4].ToString(),
                        Id = (int)item[5],
                    };
                    list.Add(courseCorequisite);
                }
            }
            return list;
        }
        private List<DescriptionInfo> GetCourseObjectiveDescription(int courseId)
        {
            //outcomes = (from a in ci.Course.CourseObjectives select new DescriptionInfo { Description = a.Description }).ToList();
            string sqlQueryCourseObjective = $@"select c.Description from CourseObjective c
                                        inner join CourseCourseObjective cco on c.Id = cco.CourseObjectiveId
                                        where cco.CourseId = {courseId} and c.Active = 1";

            var descriptionInfoData = SQLHelper.RunSqlQuery(sqlQueryCourseObjective);

            List<DescriptionInfo> list = new List<DescriptionInfo>();
            if (descriptionInfoData.Count > 0)
            {
                foreach (var item in descriptionInfoData)
                {
                    DescriptionInfo courseCorequisite = new DescriptionInfo
                    {
                        Description = item[0].ToString()
                    };
                    list.Add(courseCorequisite);
                }
            }
            return list;
        }
        private List<DescriptionInfo> GetInstructionMethodDescription(int courseInstanceId)
        {
            //outcomes = (from a in ci.CourseInstructionMethods.Where(x => x.Active) select new DescriptionInfo { Description = a.MethodsOfInstruction.Description }).ToList();
            string sqlQueryInstructionMethod = $@"select m.Description from CourseInstructionMethod cm
                                                inner join MethodsOfInstruction m on m.Id = cm.MethodsOfInstructionId
                                                where cm.CourseInstanceId = {courseInstanceId} and m.Active = 1";

            var instructionMethodDescriptionData = SQLHelper.RunSqlQuery(sqlQueryInstructionMethod);

            List<DescriptionInfo> list = new List<DescriptionInfo>();
            if (instructionMethodDescriptionData.Count > 0)
            {
                foreach (var item in instructionMethodDescriptionData)
                {
                    DescriptionInfo model = new DescriptionInfo
                    {
                        Description = item[0].ToString()
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<DescriptionInfo> GetTechnologieDescription(int courseId)
        {
            //outcomes = technologies = (from a in ci.Course.CourseTechnologyRequirements.Where(x => x.Active) select new DescriptionInfo { Description = a.TechnologyRequirement.Description }).ToList();
            string sqlQueryTechnologie = $@"select t.Description from CourseTechnologyRequirement ct 
                                            inner join TechnologyRequirement t on t.Id = ct.TechnologyRequirementId
                                            where ct.CourseId = {courseId} and ct.Active = 1";

            var instructionMethodDescriptionData = SQLHelper.RunSqlQuery(sqlQueryTechnologie);

            List<DescriptionInfo> list = new List<DescriptionInfo>();
            if (instructionMethodDescriptionData.Count > 0)
            {
                foreach (var item in instructionMethodDescriptionData)
                {
                    DescriptionInfo model = new DescriptionInfo
                    {
                        Description = item[0].ToString()
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<DescriptionInfo> GetTextBookDescription(int courseId)
        {
            //textbooks = (from a in ci.Course.CourseTextbooks.Where(x => x.Active) select new DescriptionInfo { Description = a.Textbook.Description }).ToList();
            string sqlQueryTextBook = $@"select t.Description from CourseTextbook ct
                                        inner join Textbook t on t.TextbookId = ct.TextbookId
                                        where ct.CourseId = {courseId} and t.Active = 1";

            var textBookDescriptionData = SQLHelper.RunSqlQuery(sqlQueryTextBook);

            List<DescriptionInfo> list = new List<DescriptionInfo>();
            if (textBookDescriptionData.Count > 0)
            {
                foreach (var item in textBookDescriptionData)
                {
                    DescriptionInfo model = new DescriptionInfo
                    {
                        Description = item[0].ToString()
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<DescriptionInfo> GetToolsDescription(int courseId)
        {
            //tools = (from a in ci.Course.CourseRequiredTools.Where(x => x.Active) select new DescriptionInfo { Description = a.RequiredTool.Description }).ToList();
            string sqlQueryTextBook = $@"select r.Description from CourseRequiredTool ct
                                        inner join RequiredTool r on r.RequiredToolId = ct.RequiredToolId
                                        where ct.CourseId = {courseId} and r.Active = 1";

            var textBookDescriptionData = SQLHelper.RunSqlQuery(sqlQueryTextBook);

            List<DescriptionInfo> list = new List<DescriptionInfo>();
            if (textBookDescriptionData.Count > 0)
            {
                foreach (var item in textBookDescriptionData)
                {
                    DescriptionInfo model = new DescriptionInfo
                    {
                        Description = item[0].ToString()
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<DescriptionInfo> GetSupplieDescription(int courseId)
        {
            //supplies = (from a in ci.Course.CourseSupplies.Where(x => x.Active) select new DescriptionInfo { Description = a.Supply.Description }).ToList();
            string sqlQuerysupplie = $@"select s.Description from CourseSupplies cs
                                        inner join Supplies s on s.SupplieId = cs.SupplyId
                                        where cs.CourseId = {courseId} and s.Active = 1";

            var supplieData = SQLHelper.RunSqlQuery(sqlQuerysupplie);

            List<DescriptionInfo> list = new List<DescriptionInfo>();
            if (supplieData.Count > 0)
            {
                foreach (var item in supplieData)
                {
                    DescriptionInfo model = new DescriptionInfo
                    {
                        Description = item[0].ToString()
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<DescriptionInfo> GetMaterialDescription(int courseId)
        {
            //materials = (from a in ci.Course.CourseCourseMaterialRequirements.Where(x => x.Active) select new DescriptionInfo { Description = a.CourseMaterialRequirement.Description }).ToList();
            string sqlQueryMaterial = $@"select cmr.Description from CourseCourseMaterialRequirement cm
                                        inner join CourseMaterialRequirement cmr on cmr.Id =cm.CourseMaterialRequirementId
                                        where cm.CourseId = {courseId} and cmr.Active = 1";

            var materialData = SQLHelper.RunSqlQuery(sqlQueryMaterial);

            List<DescriptionInfo> list = new List<DescriptionInfo>();
            if (materialData.Count > 0)
            {
                foreach (var item in materialData)
                {
                    DescriptionInfo model = new DescriptionInfo
                    {
                        Description = item[0].ToString()
                    };
                    list.Add(model);
                }
            }
            return list;
        }

        //09-06
        private VmGradingPolicy GetGradingPolicy(int courseInstanceId)
        {
            //GradingPolicy gradingPolicy = ci.Quarter.School.GradingPolicies.FirstOrDefault();

            string sqlQueryGradingPolicy = $@"select g.Id, g.SchoolId, g.Description , g.Active from GradingPolicy g
                                            inner join school s on s.SchoolId = g.SchoolId
                                            inner join Quarter q on q.SchoolId = g.SchoolId
                                            inner join CourseInstance ci on ci.QuarterId = q.QuarterId
                                            where ci.Id = {courseInstanceId}";

            var gradingPolicyData = SQLHelper.RunSqlQuery(sqlQueryGradingPolicy);
            VmGradingPolicy gradingPolicyinfo = null;

            if (gradingPolicyData.Count > 0)
            {
                List<object> st = gradingPolicyData[0];
                gradingPolicyinfo = new VmGradingPolicy
                {
                    Id = (int)st[0],
                    SchoolId = (int)st[1],
                    Description = (string)st[2],
                    Active = (bool)st[3]
                };
            }

            return gradingPolicyinfo;
        }
        private VmGradeScaleGroup GetGradeScaleGroup(int courseId)
        {
            //GradeScaleGroup GScaleGroup = ci.Course.GradeScaleGroup;
            string sqlQueryGradeScaleGroup = $@"select gs.Id, gs.Title from Course c
                                                inner join GradeScaleGroup gs on gs.Id = c.GradeScaleGroupId
                                                where c.Id = {courseId}";

            var gradeScaleGroupData = SQLHelper.RunSqlQuery(sqlQueryGradeScaleGroup);
            VmGradeScaleGroup gradeScaleGroupinfo = null;

            if (gradeScaleGroupData.Count > 0)
            {
                List<object> st = gradeScaleGroupData[0];
                gradeScaleGroupinfo = new VmGradeScaleGroup
                {
                    Id = (int)st[0],
                    Title = (string)st[1]
                };
            }

            return gradeScaleGroupinfo;
        }
        private VmGradeWeight GetGradeWeight(int courseInstanceId)
        {
            //GradeWeight GWeight = ci.GradeWeights.FirstOrDefault();
            string sqlQueryGradeWeight = $@"select gw.Id, gw.CourseInstanceId, gw.ActivityWeight, gw.AssessmentWeight, gw.MaterialWeight,
                                            gw.DiscussionWeight, gw.PollWeight, gw.MidtermWeight, gw.FinalWeight from GradeWeight gw
                                            inner join CourseInstance ci on ci.Id = gw.CourseInstanceId
                                            where ci.Id = {courseInstanceId}";

            var gradeWeightData = SQLHelper.RunSqlQuery(sqlQueryGradeWeight);
            VmGradeWeight gradeWeightinfo = null;

            if (gradeWeightData.Count > 0)
            {
                List<object> st = gradeWeightData[0];
                gradeWeightinfo = new VmGradeWeight
                {
                    Id = (int)st[0],
                    CourseInstanceId = (int)st[1],
                    ActivityWeight = (int)st[2],
                    AssessmentWeight = (int)st[3],
                    MaterialWeight = (int)st[4],
                    DiscussionWeight = (int)st[5],
                    PollWeight = (int)st[6],
                    MidtermWeight = (int)st[7],
                    FinalWeight = (int)st[8]
                };
            }

            return gradeWeightinfo;
        }
        private List<GradeScale> GetGradeScales(int gradeScaleGroupId)
        {
            //GetGradeScales(GScaleGroup.Id); (from a in GScaleGroup.GradeScales select new GradeScale { GPA = a.GPA, Point = a.MaxNumberInPercent + "% - " + a.MinNumberInPercent + "%" }).ToList();
            string sqlQueryGradeScale = $@"select gs.GPA, gs.MaxNumberInPercent, gs.MinNumberInPercent from GradeScale gs
                                           inner join GradeScaleGroup gsg on gsg.Id = gs.GradeScaleGroupId
                                           where gsg.Id = {gradeScaleGroupId}";

            var gradeScaleData = SQLHelper.RunSqlQuery(sqlQueryGradeScale);

            List<GradeScale> list = new List<GradeScale>();
            if (gradeScaleData.Count > 0)
            {
                foreach (var item in gradeScaleData)
                {
                    GradeScale model = new GradeScale
                    {
                        GPA = Convert.ToDouble( item[0]),
                        Point = item[1].ToString() + "% - " + item[2].ToString() + "%"
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<DescriptionInfo> GetpoliciesDescription(int courseInstanceId)
        {
            //policies = (from a in ci.Quarter.School.CoursePolicies.Where(x => x.Active)
            //            select new DescriptionInfo
            //            {
            //                Subtitle = a.Subtitle,
            //                Description = a.Description,
            //                Points = a.CoursePolicyPoints.Select(x => new Point { Description = x.Description }).ToList()
            //            }).ToList();

            string sqlQueryPolicies = $@"select cp.subtitle, cp.Description, cp.Id from CoursePolicy cp
                                        inner join school s on s.SchoolId = cp.SchoolId
                                        inner join Quarter q on q.SchoolId = cp.SchoolId
                                        inner join CourseInstance ci on ci.QuarterId = q.QuarterId
                                        where ci.Id = {courseInstanceId}";

            var policiesData = SQLHelper.RunSqlQuery(sqlQueryPolicies);

            List<DescriptionInfo> list = new List<DescriptionInfo>();
            if (policiesData.Count > 0)
            {
                foreach (var item in policiesData)
                {
                    DescriptionInfo model = new DescriptionInfo
                    {
                        Subtitle = item[0].ToString(),
                        Description = item[1].ToString(),
                        Points = GetCoursePolicyPoints((int)item[2])//a.CoursePolicyPoints.Select(x => new Point { Description = x.Description }).ToList()
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<Point> GetCoursePolicyPoints(int coursePolicyId)
        {
            string sqlQueryPolicies = $@"select cpp.Description from CoursePolicyPoint cpp
                                        inner join CoursePolicy cp on cp.Id = cpp.CoursePolicyId
                                        where cp.Id = {coursePolicyId} and cp.Active = 1";

            var policiesData = SQLHelper.RunSqlQuery(sqlQueryPolicies);

            List<Point> list = new List<Point>();
            if (policiesData.Count > 0)
            {
                foreach (var item in policiesData)
                {
                    Point model = new Point
                    {
                        Description = item[0].ToString()
                        //Points = a.CoursePolicyPoints.Select(x => new Point { Description = x.Description }).ToList()
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<DescriptionInfo> GetCommunityStandardDescription(int courseInstanceId)
        {
            // communityStandards = (from a in ci.Quarter.School.CommunityStandards.Where(x => x.Active) select new DescriptionInfo { Subtitle = a.Subtitle, Description = a.Description }).ToList();

            string sqlQueryCommunityStandard = $@" select cs.subtitle, cs.Description from CommunityStandard cs
                                                  inner join school s on s.SchoolId = cs.SchoolId
                                                  inner join Quarter q on q.SchoolId = cs.SchoolId
                                                  inner join CourseInstance ci on ci.QuarterId = q.QuarterId
                                                  where ci.Id = {courseInstanceId} and cs.Active = 1;";

            var communityStandardData = SQLHelper.RunSqlQuery(sqlQueryCommunityStandard);

            List<DescriptionInfo> list = new List<DescriptionInfo>();
            if (communityStandardData.Count > 0)
            {
                foreach (var item in communityStandardData)
                {
                    DescriptionInfo model = new DescriptionInfo
                    {
                        Subtitle = item[0].ToString(),
                        Description = item[1].ToString()
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<DescriptionInfo> GetCampusPublicSafetiesDescription(int courseInstanceId)
        {
            //campusPublicSafeties = (from a in ci.Quarter.School.CampusPublicSafeties.Where(x => x.Active)
            //                        select new DescriptionInfo
            //                        {
            //                            Subtitle = a.Subtitle,
            //                            Description = a.Description,
            //                            Points = a.CampusPublicSafetyPoints.Select(x => new Point { Description = x.Description }).ToList()
            //                        }).ToList();

            string sqlQueryCampusPublicSafeties = $@"select cp.subtitle, cp.Description, cp.Id from CampusPublicSafety cp
                                                    inner join school s on s.SchoolId = cp.SchoolId
                                                    inner join Quarter q on q.SchoolId = cp.SchoolId
                                                    inner join CourseInstance ci on ci.QuarterId = q.QuarterId
                                                    where ci.Id = {courseInstanceId} and cp.Active = 1";

            var campusPublicSafetiesData = SQLHelper.RunSqlQuery(sqlQueryCampusPublicSafeties);

            List<DescriptionInfo> list = new List<DescriptionInfo>();
            if (campusPublicSafetiesData.Count > 0)
            {
                foreach (var item in campusPublicSafetiesData)
                {
                    DescriptionInfo model = new DescriptionInfo
                    {
                        Subtitle = item[0].ToString(),
                        Description = item[1].ToString(),
                        Points = GetCampusPublicSafetyPoints((int)item[2])//a.CoursePolicyPoints.Select(x => new Point { Description = x.Description }).ToList()
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<Point> GetCampusPublicSafetyPoints(int campusPublicSafetyId)
        {
            //a.CampusPublicSafetyPoints.Select(x => new Point { Description = x.Description }).ToList()
            string sqlQueryCampusPublicSafetyPoint = $@"select csp.Description from CampusPublicSafetyPoint csp
                                                        inner join CampusPublicSafety cp on cp.Id = csp.CampusPublicSafetyId
                                                        where cp.Id = {campusPublicSafetyId} and cp.Active = 1";

            var campusPublicSafetyPointData = SQLHelper.RunSqlQuery(sqlQueryCampusPublicSafetyPoint);

            List<Point> list = new List<Point>();
            if (campusPublicSafetyPointData.Count > 0)
            {
                foreach (var item in campusPublicSafetyPointData)
                {
                    Point model = new Point
                    {
                        Description = item[0].ToString()
                        //a.CampusPublicSafetyPoints.Select(x => new Point { Description = x.Description }).ToList()
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<DescriptionInfo> GetSupportServicesDescription(int courseInstanceId)
        {
            //supportServices = (from a in ci.Quarter.School.SupportServices.Where(x => x.Active) select new DescriptionInfo { Subtitle = a.Subtitle, Description = a.Description }).ToList();

            string sqlQuerySupportService = $@"select ss.subtitle, ss.Description from SupportServices ss
                                               inner join school s on s.SchoolId = ss.SchoolId
                                               inner join Quarter q on q.SchoolId = ss.SchoolId
                                               inner join CourseInstance ci on ci.QuarterId = q.QuarterId
                                               where ci.Id = {courseInstanceId} and ss.Active = 1";

            var supportServiceData = SQLHelper.RunSqlQuery(sqlQuerySupportService);

            List<DescriptionInfo> list = new List<DescriptionInfo>();
            if (supportServiceData.Count > 0)
            {
                foreach (var item in supportServiceData)
                {
                    DescriptionInfo model = new DescriptionInfo
                    {
                        Subtitle = item[0].ToString(),
                        Description = item[1].ToString()
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<DescriptionInfo> GetNetiquetteDescription(int courseInstanceId)
        {
            //netiquette = (from a in ci.Quarter.School.Netiquettes.Where(x => x.Active)
            //              select new DescriptionInfo
            //              {
            //                  Subtitle = a.Title,
            //                  Description = a.Description,
            //                  Points = a.NetiquettePoints.Select(x => new Point { Description = x.Description }).ToList(),
            //                  Links = a.NetiquetteLinks.Where(y => y.Active).Select(x => new Link { Description = x.Description, Title = x.Title, AddressLink = x.Link }).ToList(),
            //              }).ToList();

            string sqlQueryCampusPublicSafeties = $@"select cp.Title, cp.Description, cp.Id from Netiquette cp
                                                    inner join school s on s.SchoolId = cp.SchoolId
                                                    inner join Quarter q on q.SchoolId = cp.SchoolId
                                                    inner join CourseInstance ci on ci.QuarterId = q.QuarterId
                                                    where ci.Id = {courseInstanceId} and cp.Active = 1";

            var campusPublicSafetiesData = SQLHelper.RunSqlQuery(sqlQueryCampusPublicSafeties);

            List<DescriptionInfo> list = new List<DescriptionInfo>();
            if (campusPublicSafetiesData.Count > 0)
            {
                foreach (var item in campusPublicSafetiesData)
                {
                    DescriptionInfo model = new DescriptionInfo
                    {
                        Subtitle = item[0].ToString(),
                        Description = item[1].ToString(),
                        Points = GetNetiquettePoints((int)item[2]),//Points = a.NetiquettePoints.Select(x => new Point { Description = x.Description }).ToList(),
                        Links = GetNetiquetteLinks((int)item[2])
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<Point> GetNetiquettePoints(int netiquetteId)
        {
            //Points = a.NetiquettePoints.Select(x => new Point { Description = x.Description }).ToList(),
            string sqlQueryNetiquettePoint = $@"select np.Description from NetiquettePoint np
                                                        inner join Netiquette n on n.Id = np.NetiquetteId
                                                        where n.Id = {netiquetteId} and np.Active = 1";

            var netiquettePointData = SQLHelper.RunSqlQuery(sqlQueryNetiquettePoint);

            List<Point> list = new List<Point>();
            if (netiquettePointData.Count > 0)
            {
                foreach (var item in netiquettePointData)
                {
                    Point model = new Point
                    {
                        Description = item[0].ToString()
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<Link> GetNetiquetteLinks(int netiquetteId)
        {
            //a.NetiquetteLinks.Where(y => y.Active).Select(x => new Link { Description = x.Description, Title = x.Title, AddressLink = x.Link }).ToList(),
            string sqlQueryNetiquetteLink = $@"select nl.Description, nl.Title, nl.Link from NetiquetteLink nl
                                               inner join Netiquette n on n.Id = nl.NetiquetteId
                                               where n.Id = {netiquetteId} and nl.Active = 1";

            var netiquetteLinkData = SQLHelper.RunSqlQuery(sqlQueryNetiquetteLink);

            List<Link> list = new List<Link>();
            if (netiquetteLinkData.Count > 0)
            {
                foreach (var item in netiquetteLinkData)
                {
                    Link model = new Link
                    {
                        Description = item[0].ToString(),
                        Title = item[1].ToString(),
                        AddressLink = item[2].ToString()
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<NonAcamedicDaysInfo> GetNonAcamedicDaysInfo(int quarterId)
        {
            //var nonAcademicDays = ci.Quarter.NonAcademicDays.Select(nad =>
            //              new NonAcamedicDaysInfo
            //              {
            //                  StartDate = nad.StartDate,
            //                  EndDate = nad.EndTime,
            //                  Description = nad.Description,
            //                  Type = nad.Type
            //              }).ToList();

            string sqlQueryNonAcamedicDaysInfo = $@"select  n.StartDate,n.EndTime,n.Description,n.Type from NonAcademicDays n
                                                    inner join Quarter q on q.QuarterId = n.QuarterId
                                                    where n.QuarterId = {quarterId}";

            var nonAcamedicDaysInfoData = SQLHelper.RunSqlQuery(sqlQueryNonAcamedicDaysInfo);

            List<NonAcamedicDaysInfo> list = new List<NonAcamedicDaysInfo>();
            if (nonAcamedicDaysInfoData.Count > 0)
            {
                foreach (var item in nonAcamedicDaysInfoData)
                {
                    NonAcamedicDaysInfo model = new NonAcamedicDaysInfo
                    {
                        StartDate = (DateTime)item[0],
                        EndDate = (DateTime)item[1],
                        Description = item[2].ToString(),
                        Type = item[3].ToString()
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<StudentSupportResources> GetStudentSupportResource(int quarterId)
        {
            //var studentSupportResources = ci.Quarter.School.StudentSupportResources.Select(ssr =>
            //               new StudentSupportResources
            //               {
            //                   Title = ssr.Title,
            //                   Link = ssr.Link
            //               }
            //).ToList();

            string sqlQueryStudentSupportResource = $@"select ss.Link, ss.Title from [dbo].[StudentSupportResources] ss
                                                       inner join school s on s.SchoolId = ss.SchoolId
                                                       inner join Quarter q on q.SchoolId = ss.SchoolId
                                                       where q.QuarterId ={quarterId}";

            var studentSupportResourcesData = SQLHelper.RunSqlQuery(sqlQueryStudentSupportResource);

            List<StudentSupportResources> list = new List<StudentSupportResources>();
            if (studentSupportResourcesData.Count > 0)
            {
                foreach (var item in studentSupportResourcesData)
                {
                    StudentSupportResources model = new StudentSupportResources
                    {
                        Title = item[0].ToString(),
                        Link = item[1].ToString()
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<TentativeAssignmentSchedule> GetTentativeAssignmentSchedule(int courseInstanceId)
        {
            string sqlQueryQuarter = $@"exec TentativeAssignmentSchedule {courseInstanceId}";
            var tentativeAssignmentScheduleData = SQLHelper.RunSqlQuery(sqlQueryQuarter);

            List<TentativeAssignmentSchedule> list = new List<TentativeAssignmentSchedule>();


            if (tentativeAssignmentScheduleData.Count > 0)
            {
                foreach (var item in tentativeAssignmentScheduleData)
                {
                    TentativeAssignmentSchedule model = new TentativeAssignmentSchedule
                    {
                        Title = item[0].ToString(),//item.Week,
                        Topic = item[1].ToString(),//item.Topic,
                        QuizCount = (item[2] != DBNull.Value) ? (int)item[2] : 0,//item.Quizzes ?? 0,
                        AssignmentCount = (item[3] != DBNull.Value) ? (int)item[3] : 0,//item.Assignments ?? 0,
                        TypeOfTest = (item[4] != DBNull.Value) ? item[4].ToString() : String.Empty,//item.Test,
                        Meeting = (item[5] != DBNull.Value) ? item[5].ToString() : String.Empty,//item.Meeting,
                        DueDate = (item[6] != DBNull.Value) ? (DateTime)item[6] : DateTime.MinValue,//item.DueDate,
                    };
                    list.Add(model);
                }

            }
            return list;
        }
        private Course GetCourse(int courseInstanceId)
        {
            string sqlQueryCourse = $@"select c.Id, c.Name, c.Credits, c.Description from Course c
                                       inner join CourseInstance ci on ci.CourseId = c.Id
                                       where ci.Id = {courseInstanceId}";

            var courseData = SQLHelper.RunSqlQuery(sqlQueryCourse);
            Course courseinfo = null;

            if (courseData.Count > 0)
            {
                List<object> st = courseData[0];
                courseinfo = new Course
                {
                    Id = (int)st[0],
                    Name = st[1].ToString(),
                    Credits = (int)st[2],
                    Description = st[3].ToString()
                };
            }

            return courseinfo;
        }

        #endregion

        private VmQuarter GetCourseInstanceQuarter(int quarterId)
        {
            string sqlQueryVmQuarter = $@"select QuarterId,SchoolId, StartDate, EndDate,WithdrawDate,Active,Name
                                        from Quarter
                                        where QuarterId = {quarterId} ";

            var vmQuarterData = SQLHelper.RunSqlQuery(sqlQueryVmQuarter);
            VmQuarter quarterinfo = null;

            if (vmQuarterData.Count > 0)
            {
                List<object> st = vmQuarterData[0];
                quarterinfo = new VmQuarter
                {
                    QuarterId = (int)st[0],
                    SchoolId = (int)st[1],
                    StartDate = (DateTime)st[2],
                    EndDate = (DateTime)st[3],
                    WithdrawDate = (DateTime)st[4],
                    Active = (bool)st[5],
                    Name = st[6].ToString()
                };
            }

            return quarterinfo;
        }
        private CourseQuarter GetCourseQuarter(int quarterId)
        {
            string sqlQueryCourseQuarter = $@"select s.Name as SchoolName,s.AcademicCalendar,q.WithdrawDate,s.SyllabusMessage,q.Name from Quarter q
                                                inner join School s on q.SchoolId = s.SchoolId
                                                where QuarterId = {quarterId} and Active = 1";

            var courseQuarterData = SQLHelper.RunSqlQuery(sqlQueryCourseQuarter);
            CourseQuarter courseQuarterinfo = null;

            if (courseQuarterData.Count > 0)
            {
                List<object> st = courseQuarterData[0];

                courseQuarterinfo = new CourseQuarter
                {
                    SchoolName = (string)st[0],
                    Calendar = st[1].ToString(),
                    WithdrawDate = Convert.ToDateTime(st[2]).ToString("MMMM dd, yyyy"),
                    SyllabusMessage = st[3].ToString(),
                    Name = st[4].ToString()
                };
            }

            return courseQuarterinfo;
        }
        private List<CourseSession> GetCourseSession(int courseInstanceId)
        {
            string sqlStudentCourse = $@"select s.LectureDay,s.StartTime,s.EndTime,s.Description,s.Location from Session s
                                        inner join CourseInstanceSession cs on s.SessionId = cs.SessionId
                                        where cs.CourseInstanceId  = {courseInstanceId} and cs.Active = 1";




            var courseSessionData = SQLHelper.RunSqlQuery(sqlStudentCourse);
            List<CourseSession> courseSessions = new List<CourseSession>();

            if (courseSessionData.Count > 0)
            {
                foreach (var item in courseSessionData)
                {
                    CourseSession courseSession = new CourseSession
                    {
                        LectureDay = item[0].ToString() + " " + Convert12Hours((TimeSpan)item[1], (TimeSpan)item[2]),
                        Description = item[3].ToString(),
                        Location = item[4].ToString()
                    };
                    courseSessions.Add(courseSession);
                }
            }
            return courseSessions;
        }
        public VmCourseInstance GetCourseInstanceById(int id)
        {
            string sqlQueryCourseInstance = $@"select Id, Active,QuarterId,CourseId,Testing from CourseInstance where Id = {id} and Active = 1";

            var courseInstanceData = SQLHelper.RunSqlQuery(sqlQueryCourseInstance);
            VmCourseInstance courseInstanceinfo = null;

            if (courseInstanceData.Count > 0)
            {
                List<object> st = courseInstanceData[0];
                courseInstanceinfo = new VmCourseInstance
                {
                    Id = (int)st[0],
                    Active = (bool)st[1],
                    QuarterId = (int)st[2],
                    CourseId = (int)st[3],
                    Testing = (bool)st[4]
                };
            }

            return courseInstanceinfo;
        }
        private List<VmInstructorCourse> GetInstructorCourse(int courseInstanceId)
        {
            string sqlInstructorCourse = $@"select ic.InstructorId, ic.CourseInstanceId, ic.Role, ic.Active, i.InstructorName from InstructorCourse ic 
                                            inner join Instructor i on ic.InstructorId = i.Id
                                            where CourseInstanceId = {courseInstanceId} and ic.Active = 1";


            var instructorCourseData = SQLHelper.RunSqlQuery(sqlInstructorCourse);
            List<VmInstructorCourse> instructorCourses = new List<VmInstructorCourse>();

            if (instructorCourseData.Count > 0)
            {
                foreach (var item in instructorCourseData)
                {
                    VmInstructorCourse instructorCourse = new VmInstructorCourse
                    {
                        InstructorId = (int)item[0],
                        CourseInstanceId = (int)item[1],
                        Role = item[2].ToString(),
                        Active = (bool)item[3],
                        InstructorName = item[4].ToString(),
                    };
                    instructorCourses.Add(instructorCourse);
                }
            }
            return instructorCourses;
        }
        private List<ContactInfo> GetContactInfo(int instructorId)
        {
            string sqlContactInfo = $@"select ci.ContactInfo,ci.Preferred from ContactInformation ci 
                                    where ci.InstructorId = {instructorId} and ci.Active = 1";


            var iontactInfoData = SQLHelper.RunSqlQuery(sqlContactInfo);
            List<ContactInfo> contactInfos = new List<ContactInfo>();

            if (iontactInfoData.Count > 0)
            {
                foreach (var item in iontactInfoData)
                {
                    string title = (bool)item[1] ? "Preferred Contact" : "Contact";

                    ContactInfo contactInfo = new ContactInfo
                    {
                        Title = title,
                        Contact = item[0].ToString()
                    };
                    contactInfos.Add(contactInfo);
                }
            }
            return contactInfos;
        }
        private List<InstructorAvailableHoursInfo> GetInstructorAvailableHoursInfo(int instructorId)
        {
            string sqlInstructorAvailableHoursInfo = $@"select StartTime,EndTime,DayOfWeek from InstructorAvailableHours where InstructorId = {instructorId} ";


            var instructorAvailableHoursInfoData = SQLHelper.RunSqlQuery(sqlInstructorAvailableHoursInfo);
            List<InstructorAvailableHoursInfo> instructorAvailableHoursInfos = new List<InstructorAvailableHoursInfo>();

            if (instructorAvailableHoursInfoData.Count > 0)
            {
                foreach (var item in instructorAvailableHoursInfoData)
                {

                    InstructorAvailableHoursInfo instructorAvailableHoursInfo = new InstructorAvailableHoursInfo
                    {
                        StartTime = (TimeSpan)item[0],
                        EndTime = (TimeSpan)item[1],
                        DayOfWeek = item[2].ToString()
                    };
                    instructorAvailableHoursInfos.Add(instructorAvailableHoursInfo);
                }
            }
            return instructorAvailableHoursInfos;
        }
        private List<RequisiteInfo> GetRequisiteInfo(int courseId)
        {
            string sqlRequisiteInfo = $@"select c.Department, c.Number,cc.GroupId from CourseCorequisite cc
                                        inner join Course c on cc.CourseId = c.Id
                                        where cc.Type = 'Prerequisite' and c.Id = {courseId} ";


            var requisiteInfoData = SQLHelper.RunSqlQuery(sqlRequisiteInfo);
            List<RequisiteInfo> requisiteInfos = new List<RequisiteInfo>();

            if (requisiteInfoData.Count > 0)
            {
                foreach (var item in requisiteInfoData)
                {
                    RequisiteInfo requisiteInfo = new RequisiteInfo
                    {
                        Department = item[0].ToString(),
                        Id = item[1].ToString(),
                        GroupId = (int)item[2]
                    };
                    requisiteInfos.Add(requisiteInfo);
                }
            }
            return requisiteInfos;
        }
        private string Convert12Hours(TimeSpan time, TimeSpan time2)
        {
            string result = "";
            int timeint = Convert.ToInt32(time.Hours);
            int timeint2 = Convert.ToInt32(time2.Hours);

            string startTiem = "";
            string endTime = "";
            if (timeint - 12 >= 0)
            {
                //startTiem = Convert.ToString(timeint - 12) + ":" + Convert.ToString(time.Minutes) + " - " + Convert.ToString(timeint2 - 12) + ":" + Convert.ToString(time2.Minutes) + " PM";
                startTiem = Convert.ToString(timeint - 12) + ":" + time.Minutes.ToString("00");
            }
            else
            {
                startTiem = Convert.ToString(timeint) + ":" + time.Minutes.ToString("00");
            }
            if (timeint2 - 12 >= 0)
            {
                endTime = Convert.ToString(timeint2 - 12) + ":" + time.Minutes.ToString("00") + " PM";
            }
            else
            {
                endTime = Convert.ToString(timeint2) + ":" + time2.Minutes.ToString("00") + " AM";
            }
            result = startTiem + " - " + endTime;
            return result;
        }

        public class ResultInfo
        {
            public string CourseName { get; set; }
            public int Credits { get; set; }
            public List<CourseSession> Sessions { get; set; }
            public CourseQuarter Quarter { get; set; }
            public List<CourseInstructor> Instructors { get; set; }
            public string CourseDescription { get; set; }
            public List<RequisiteInfo> Prerequisites { get; set; }
            public string Corequisites { get; set; }
            public List<DescriptionInfo> Outcomes { get; set; }
            public List<DescriptionInfo> InstructionMethods { get; set; }
            public List<DescriptionInfo> Technologies { get; set; }
            public List<DescriptionInfo> Textbooks { get; set; }
            public List<DescriptionInfo> Tools { get; set; }
            public List<DescriptionInfo> Supplies { get; set; }
            public List<DescriptionInfo> Materials { get; set; }
            public string GradingPolicy { get; set; }
            public List<GradeScale> GradeScales { get; set; }
            public List<GradeScaleWeight> GradeScaleWeights { get; set; }
            public List<DescriptionInfo> Policies { get; set; }
            public List<DescriptionInfo> CommunityStandards { get; set; }
            public List<DescriptionInfo> CampusPublicSafeties { get; set; }
            public List<DescriptionInfo> SupportServices { get; set; }
            public List<DescriptionInfo> Netiquette { get; set; }
            public List<NonAcamedicDaysInfo> NonAcademicDays { get; set; }
            public List<StudentSupportResources> StudentSupportResources { get; set; }
            public List<TentativeAssignmentSchedule> TentativeAssignmentSchedule { get; set; }
        }
        public class TentativeAssignmentSchedule
        {
            public string Title { get; set; }
            public string Topic { get; set; }
            public int QuizCount { get; set; }
            public int AssignmentCount { get; set; }
            public string TypeOfTest { get; set; }
            public string Meeting { get; set; }
            public DateTime? DueDate { get; set; }

        }
        public class CourseSession
        {
            public string LectureDay { get; set; }
            public string Description { get; set; }
            public string Location { get; set; }
        }
        public class CourseInstructor
        {
            public string Name { get; set; }
            public List<ContactInfo> ContactInfo { get; set; }
            public List<InstructorAvailableHoursInfo> InstructorAvailableHours { get; set; }
        }
        public class ContactInfo
        {
            public string Title { get; set; }
            public string Contact { get; set; }
        }
        public class CourseQuarter
        {
            public string SchoolName { get; set; }
            public string Calendar { get; set; }
            public string WithdrawDate { get; set; }
            public string SyllabusMessage { get; set; }
            public string Name { get; set; }
        }
        public class GradeScale
        {
            public double GPA { get; set; }
            public string Point { get; set; }
        }
        public class GradeScaleWeight
        {
            public string Description { get; set; }
            public int Weight { get; set; }
        }
        public class DescriptionInfo
        {
            public string Subtitle { get; set; }
            public string Description { get; set; }
            public List<Point> Points { get; set; }
            public List<Link> Links { get; set; }

        }
        public class Point
        {
            public string Description { get; set; }
        }
        public class Link
        {
            public string Description { get; set; }
            public string Title { get; set; }
            public string AddressLink { get; set; }

        }
        public class NonAcamedicDaysInfo
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }
        }
        public class RequisiteInfo
        {
            public string Department { get; set; }
            public string Id { get; set; }
            public int GroupId { get; set; }
        }
        public class InstructorAvailableHoursInfo
        {
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
            public String DayOfWeek { get; set; }
        }
        public class StudentSupportResources
        {
            public string Title { get; set; }
            public string Link { get; set; }
        }
        public class Course
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Credits { get; set; }
            public string Description { get; set; }
        }
    }
}
