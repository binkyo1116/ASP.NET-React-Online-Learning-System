using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using LMSLibrary;
using LMS.Common.ViewModels;
using RESTModelFunctions.Helper;
//using EFModel;

namespace RESTModelFunctions.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DiscussionController : ApiController
    {
        public class StudentInfo
        {
            public int CourseInstanceId { get; set; }
            public int ModuleObjectiveId { get; set; }
            public int DiscussionBoardId { get; set; }
            public int DiscussionPostId { get; set; }
            public string NewPostTitle { get; set; }
            public string NewPostDescription { get; set; }
            public string Hash { get; set; }
            public string Method { get; set; }
        }

        // POST api/Course
        public IHttpActionResult Post([FromBody] StudentInfo si)
        {
            //MaterialEntities model = new MaterialEntities();
            VmStudent student = VmModelHelper.GetStudentInfoByHash(si.Hash);
            VmCourseInstance courseInstance = VmModelHelper.GetCourseInstanceById(si.CourseInstanceId); //model.CourseInstances.Where(ci => ci.Id == si.CourseInstanceId).FirstOrDefault();
            VmDiscussionBoard discussionBoard = GetDiscussionBoard(courseInstance.Id,si.DiscussionBoardId, si.ModuleObjectiveId);


            //MaterialEntities model = new MaterialEntities();
            //Student student = model.Students.Where(stud => stud.Hash == si.Hash).FirstOrDefault();
            //CourseInstance courseInstance = model.CourseInstances.Where(ci => ci.Id == si.CourseInstanceId).FirstOrDefault();
            //DiscussionBoard discussionBoard = courseInstance.CourseInstanceDiscussionBoards.Where(cidb => cidb.DiscussionBoardId == si.DiscussionBoardId && cidb.ModuleObjectiveId == si.ModuleObjectiveId).Select(cidBoard => cidBoard.DiscussionBoard).FirstOrDefault();


            if (si.Method == "Get")
            {
                bool testStudent = student.Test.HasValue && student.Test.Value;
                List<VmGroupDiscussion> discussionBoardPosts = GetGroupDiscussion(testStudent, discussionBoard.Id);
                //List<GroupDiscussion> discussionBoardPosts = discussionBoard.GroupDiscussions.Where(gd => gd.Active || testStudent).OrderBy(gd => gd.PublishedDate).ToList();

                IList<DiscussionPost> posts = new List<DiscussionPost>();
                foreach (VmGroupDiscussion gd in discussionBoardPosts)
                {
                    //Student author = model.Students.Where(stud => stud.StudentId == gd.StudentId).FirstOrDefault();
                    VmStudent author = GetStudentInfoById(gd.StudentId);//model.Students.Where(stud => stud.StudentId == gd.StudentId).FirstOrDefault();
                    byte[] photo = author.Photo;
                    string imgURL = "";

                    if (photo != null)
                    {
                        byte[] img = photo.ToArray();
                        imgURL = "data:image;base64," + Convert.ToBase64String(img);
                    }

                    DiscussionPost post = new DiscussionPost()
                    {
                        Photo = imgURL,
                        Name = author.Name,
                        IsAuthor = (student.StudentId == author.StudentId),
                        Title = gd.Title,
                        Id = gd.Id,
                        Description = gd.Description,
                        PublishedDate = gd.PublishedDate.ToString("MMM dd, yyyy")
                    };
                    posts.Add(post);
                }

                ResultInfo ri = new ResultInfo()
                {
                    BoardTitle = discussionBoard.Title,
                    Posts = (List<DiscussionPost>)posts
                };

                return Ok(ri);
            }
            else if (si.Method == "Delete")
            {
                VmGroupDiscussion discussionPost = GetGroupDiscussionByPostId(si.DiscussionPostId);
                //GroupDiscussion discussionPost = discussionBoard.GroupDiscussions.Where(gd => gd.Id == si.DiscussionPostId).FirstOrDefault();
                if (discussionPost != null)
                {
                    if (discussionPost.StudentId == student.StudentId)
                    {
                        GroupDiscussionPostDelete(si.DiscussionPostId);
                        //model.GroupDiscussions.Remove(discussionPost);
                        //model.SaveChanges();
                        return Ok(new ResultInfo() { Result = "Your post was deleted" });
                    }
                    else
                    {
                        return Ok(new ResultInfo() { Result = "You have no rights to delete this post" });
                    }
                }
                else
                {
                    return Ok(new ResultInfo() { Result = "Post is not found" });
                }

            }
            else if (si.Method == "Update")
            {
                VmGroupDiscussion discussionPost = GetGroupDiscussionByPostId(si.DiscussionPostId);
                //GroupDiscussion discussionPost = discussionBoard.GroupDiscussions.Where(gd => gd.Id == si.DiscussionPostId).FirstOrDefault();
                if (discussionPost != null)
                {
                    if (discussionPost.StudentId == student.StudentId)
                    {
                        GroupDiscussionPostUpdate(si.DiscussionPostId, discussionPost.Title, discussionPost.Description);
                        //discussionPost.Title = si.NewPostTitle.Trim();
                        //discussionPost.Description = si.NewPostDescription.Trim();
                        //discussionPost.LastUpdateDate = DateTime.Now;
                        //model.SaveChanges();
                        return Ok(new ResultInfo() { Result = "Your post was updated" });
                    }
                    else
                    {
                        return Ok(new ResultInfo() { Result = "You have no rights to update this post" });
                    }
                }
                else
                {
                    return Ok(new ResultInfo() { Result = "Post is not found" });
                }

            }
            else if (si.Method == "Add")
            {
                string sqlQueryStudent = $@"INSERT INTO GroupDiscussion (CourseInstanceId, DiscussionBoardId, Title, Description, StudentId, PublishedDate, LastUpdateDate, Active)
                                            VALUES ({si.CourseInstanceId}, {discussionBoard.Id}, '{si.NewPostTitle.Trim()}', '{si.NewPostDescription.Trim()}', {student.StudentId}, {DateTime.Now}, {DateTime.Now}, {true});";
                bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryStudent);

                //GroupDiscussion discussion = new GroupDiscussion
                //{
                //    CourseInstanceId = si.CourseInstanceId,
                //    DiscussionBoardId = discussionBoard.Id,
                //    Title = si.NewPostTitle.Trim(),
                //    Description = si.NewPostDescription.Trim(),
                //    StudentId = student.StudentId,
                //    PublishedDate = DateTime.Now,
                //    LastUpdateDate = DateTime.Now,
                //    Active = true
                //};
                //model.GroupDiscussions.Add(discussion);
                //model.SaveChanges();
                return Ok(new ResultInfo() { Result = "Your post was saved" });

            }
            else
            {
                return Ok();
            }

        }

        private VmDiscussionBoard GetDiscussionBoard(int courseInstanceId, int discussionBoardId, int moduleObjectiveId)
        {
            string sqlQueryGradeScale = $@"select db.Id, db.Title, db.Active from DiscussionBoard db
                                            inner join CourseInstanceDiscussionBoard cd on cd.DiscussionBoardId = db.Id
                                            where cd.CourseInstanceId = {courseInstanceId} and cd.DiscussionBoardId = {discussionBoardId} and cd.ModuleObjectiveId = {moduleObjectiveId}";

            var gradeScaleData = SQLHelper.RunSqlQuery(sqlQueryGradeScale);
            VmDiscussionBoard VmDiscussionBoardinfo = null;

            if (gradeScaleData.Count > 0)
            {
                List<object> st = gradeScaleData[0];

                VmDiscussionBoardinfo = new VmDiscussionBoard
                {
                    Id = (int)st[0],
                    Title = st[1].ToString(),
                    Active = (bool)st[2]
                };
            }

            return VmDiscussionBoardinfo;
        }

        //TODO - Check this function
        private List<VmGroupDiscussion> GetGroupDiscussion(bool testStudent, int discussionBoardId)
        {
            string sqlQueryStudent = $@"select * from GroupDiscussion gd where gd.DiscussionBoardId = {discussionBoardId} and ((gd.Active = 1) or (1 = {testStudent}))
                                                order by gd.PublishedDate";

            var groupDiscussionsData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            List<VmGroupDiscussion> groupDiscussionsList = new List<VmGroupDiscussion>();

            foreach (var item in groupDiscussionsData)
            {
                VmGroupDiscussion groupDiscussions = null;
                List<object> st = item;
                groupDiscussions = new VmGroupDiscussion
                {
                    x_CourseId = (int)st[0],
                    x_CourseObjectiveId = (int)st[1],
                    x_ModuleId = (int)st[2],
                    x_ModuleObjetiveId = (int)st[3],
                    x_DiscussionBoardId = (int)st[4],
                    x_GroupDiscussionId = (int)st[5],
                    Title = st[6].ToString(),
                    Description = st[7].ToString(),
                    PublishedDate = (DateTime)st[8],
                    StudentId = (int)st[9],
                    LastUpdateDate = (DateTime)st[10],
                    Active = (bool)st[11],
                    Id = (int)st[12],
                    DiscussionBoardId = (int)st[13],
                    CourseInstanceId = (int)st[14]
                };

                groupDiscussionsList.Add(groupDiscussions);
            }

            return groupDiscussionsList;
        }
        private VmStudent GetStudentInfoById(int Id)
        {
            string sqlQueryStudent = $@"select StudentId, Name, Photo from Student s where s.StudentId = '{Id}'";

            var studentData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            VmStudent studentinfo = null;

            if (studentData.Count > 0)
            {
                List<object> st = studentData[0];

                studentinfo = new VmStudent
                {
                    StudentId = (int)st[0],
                    Name = st[1].ToString(),
                    Photo = (byte[])st[2]
                };
            }

            return studentinfo;
        }
        private VmGroupDiscussion GetGroupDiscussionByPostId(int discussionPostId)
        {
            string sqlQueryStudent = $@"select gd.StudentId from GroupDiscussion gd where gd.Id = {discussionPostId}";

            var groupDiscussionData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            VmGroupDiscussion groupDiscussioninfo = null;

            if (groupDiscussionData.Count > 0)
            {
                List<object> st = groupDiscussionData[0];

                groupDiscussioninfo = new VmGroupDiscussion
                {
                    StudentId = (int)st[0]
                };
            }

            return groupDiscussioninfo;
        }
        private bool GroupDiscussionPostDelete(int discussionPostId)
        {
            string sqlQueryStudent = $@"delete from GroupDiscussion gd where gd.Id = {discussionPostId}";
            bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryStudent);
            return isSucess;
        }
        private bool GroupDiscussionPostUpdate(int discussionPostId, string title, string description)
        {
            string sqlQueryStudent = $@"Update GroupDiscussion set Title = '{title}', Description = '{description}', LastUpdateDate = {DateTime.Now} where Id = {discussionPostId}";
            bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryStudent);
            return isSucess;
        }

        public class ResultInfo
        {
            public string BoardTitle { get; set; }
            public string Result { get; set; }
            public List<DiscussionPost> Posts { get; set; }
        }
        public class DiscussionPost
        {
            public string Photo { get; set; }
            public bool IsAuthor { get; set; }
            public string Name { get; set; }
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string PublishedDate { get; set; }
        }
    }
}


