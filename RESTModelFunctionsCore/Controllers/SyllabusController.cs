using LMS.Common.ViewModels;
using LMSLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyllabusController : ControllerBase
    {
        [HttpPost]
        public ActionResult<FeedbackResultInfo> Post([FromBody] SyllabusInfo si)
        {
            VmCourseInstance ci = GetCourseInstanceById(si.CourseInstanceId);
            SyllabusResultInfo syllabusInfo = new SyllabusResultInfo();
            if (ci == null)
            {
                return Ok(syllabusInfo);
            }
            CourseQuarter quarter = new CourseQuarter();
            string courseCoReq = "";

            List<CourseSession> sessions = new List<CourseSession>();
            List<CourseInstructor> instructors = new List<CourseInstructor>();
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

            VmQuarter ciQuarter = GetCourseInstanceQuarter(ci.QuarterId);

            if (ciQuarter.Active)
            {
                quarter = GetCourseQuarter(ci.QuarterId);
            }
            sessions = GetCourseSession(ci.Id);
            //---------------------InstructorCourses--------------------------------
            List<VmInstructorCourse> instructorCourse = GetInstructorCourse(ci.Id);
            foreach (VmInstructorCourse i in instructorCourse)
            {
                List<ContactInfo> conInfo = new List<ContactInfo>();
                conInfo = GetContactInfo(i.InstructorId);
                var instructorAvailableHours = GetInstructorAvailableHoursInfo(i.InstructorId);
                CourseInstructor cins = new CourseInstructor
                {
                    Name = i.InstructorName,
                    ContactInfo = conInfo,
                    InstructorAvailableHours = instructorAvailableHours
                };
                instructors.Add(cins);
            }

            List<RequisiteInfo> coursePreReq = GetRequisiteInfo(ci.CourseId);
            int CoReqCount = 1;

            List<VmCourseCorequisite> courseCorequisiteList = new List<VmCourseCorequisite>();
            courseCorequisiteList = CourseCorequisitesCount(ci.CourseId);
            int CoReqtotalCount = (int)courseCorequisiteList.Count();

            foreach (VmCourseCorequisite i in courseCorequisiteList)
            {
                if (CoReqCount == CoReqtotalCount)
                {
                    courseCoReq += i.TypeCourseName;
                }
                else if (CoReqtotalCount >= 3 && CoReqCount == (CoReqtotalCount - 1))
                {
                    courseCoReq += i.TypeCourseName + " Or ";
                }
                else
                {
                    courseCoReq += i.TypeCourseName + " Or ";
                }
                CoReqCount++;
            }
            outcomes = GetCourseObjectiveDescription(ci.CourseId);
            instructionMethods = GetInstructionMethodDescription(ci.Id);
            technologies = GetTechnologieDescription(ci.CourseId);

            textbooks = GetTextBookDescription(ci.CourseId);
            tools = GetToolsDescription(ci.CourseId);
            supplies = GetSupplieDescription(ci.CourseId);
            materials = GetMaterialDescription(ci.CourseId);
            VmGradingPolicy gradingPolicy = GetGradingPolicy(ci.QuarterId);
            VmGradeScaleGroup GScaleGroup = GetGradeScaleGroup(ci.CourseId);

            if (GScaleGroup != null)
            {
                gradeScales = GetGradeScales(GScaleGroup.Id);
            }
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

            policies = GetpoliciesDescription(ci.Id);
            communityStandards = GetCommunityStandardDescription(ci.Id);
            campusPublicSafeties = GetCampusPublicSafetiesDescription(ci.Id);
            supportServices = GetSupportServicesDescription(ci.Id);
            netiquette = GetNetiquetteDescription(ci.Id);
            var nonAcademicDays = GetNonAcamedicDaysInfo(ci.QuarterId);
            List<StudentSupportResources> studentSupportResources = GetStudentSupportResource(ci.QuarterId);
            List<TentativeAssignmentSchedule> TemporaryAssignmentScheduleList = GetTentativeAssignmentSchedule(si.CourseInstanceId);
            //-------------------------------------------Syllabus Data-------------------------------------------------------
            SyllabusCourse course = GetCourse(ci.Id);
            syllabusInfo = new SyllabusResultInfo()
            {
                CourseName = course.Name,
                Credits = course.Credits,
                Sessions = sessions,
                Quarter = quarter,
                Instructors = instructors,
                CourseDescription = course.Description,
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

            return Ok(syllabusInfo);
        }
        private List<VmCourseCorequisite> CourseCorequisitesCount(int courseId)
        {
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
        private VmGradingPolicy GetGradingPolicy(int courseInstanceId)
        {
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
                        GPA = Convert.ToDouble(item[0]),
                        Point = item[1].ToString() + "% - " + item[2].ToString() + "%"
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<DescriptionInfo> GetpoliciesDescription(int courseInstanceId)
        {
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
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<DescriptionInfo> GetCommunityStandardDescription(int courseInstanceId)
        {
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
                    };
                    list.Add(model);
                }
            }
            return list;
        }
        private List<DescriptionInfo> GetSupportServicesDescription(int courseInstanceId)
        {
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
        private SyllabusCourse GetCourse(int courseInstanceId)
        {
            string sqlQueryCourse = $@"select c.Id, c.Name, c.Credits, c.Description from Course c
                                       inner join CourseInstance ci on ci.CourseId = c.Id
                                       where ci.Id = {courseInstanceId}";

            var courseData = SQLHelper.RunSqlQuery(sqlQueryCourse);
            SyllabusCourse courseinfo = null;

            if (courseData.Count > 0)
            {
                List<object> st = courseData[0];
                courseinfo = new SyllabusCourse
                {
                    Id = (int)st[0],
                    Name = st[1].ToString(),
                    Credits = (int)st[2],
                    Description = st[3].ToString()
                };
            }
            return courseinfo;
        }
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
        private VmCourseInstance GetCourseInstanceById(int id)
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
    }

    public class SyllabusInfo
    {
        public int CourseInstanceId { get; set; }
    }
    public class SyllabusResultInfo
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
    public class SyllabusCourse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public string Description { get; set; }
    }
}
