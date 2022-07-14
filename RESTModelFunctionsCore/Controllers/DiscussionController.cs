using System.Text;
using LMS.Common.ViewModels;
using LMSLibrary;
using RESTModelFunctionsCore.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscussionController : ControllerBase
    {
        [HttpPost]
        public ActionResult<DiscussionResultInfo> Post([FromBody] DiscussionStudentInfo si)
        {
            VmStudent student = VmModelHelper.GetStudentInfoByHash(si.Hash);
            VmCourseInstance courseInstance = VmModelHelper.GetCourseInstanceById(si.CourseInstanceId);
            VmDiscussionBoard discussionBoard = GetDiscussionBoard(courseInstance.Id, si.DiscussionBoardId, si.ModuleObjectiveId);

            if (si.Method == "Get")
            {
                bool testStudent = student.Test.HasValue && student.Test.Value;
                List<VmGroupDiscussion> discussionBoardPosts = GetGroupDiscussion(testStudent, discussionBoard.Id);

                IList<DiscussionPost> posts = new List<DiscussionPost>();
                foreach (VmGroupDiscussion gd in discussionBoardPosts)
                {
                    VmStudent author = GetStudentInfoById(gd.StudentId);
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

                DiscussionResultInfo ri = new DiscussionResultInfo()
                {
                    BoardTitle = discussionBoard.Title,
                    Posts = (List<DiscussionPost>)posts
                };

                return Ok(ri);
            }
            else if (si.Method == "Delete")
            {
                VmGroupDiscussion discussionPost = GetGroupDiscussionByPostId(si.DiscussionPostId);
                if (discussionPost != null)
                {
                    if (discussionPost.StudentId == student.StudentId)
                    {
                        GroupDiscussionPostDelete(si.DiscussionPostId);
                        return Ok(new DiscussionResultInfo() { Result = "Your post was deleted" });
                    }
                    else
                    {
                        return Ok(new DiscussionResultInfo() { Result = "You have no rights to delete this post" });
                    }
                }
                else
                {
                    return Ok(new DiscussionResultInfo() { Result = "Post is not found" });
                }
            }
            else if (si.Method == "Update")
            {
                VmGroupDiscussion discussionPost = GetGroupDiscussionByPostId(si.DiscussionPostId);
                if (discussionPost != null)
                {
                    if (discussionPost.StudentId == student.StudentId)
                    {
                        GroupDiscussionPostUpdate(si.DiscussionPostId, discussionPost.Title, discussionPost.Description);
                        return Ok(new DiscussionResultInfo() { Result = "Your post was updated" });
                    }
                    else
                    {
                        return Ok(new DiscussionResultInfo() { Result = "You have no rights to update this post" });
                    }
                }
                else
                {
                    return Ok(new DiscussionResultInfo() { Result = "Post is not found" });
                }
            }
            else if (si.Method == "Add")
            {
                string sqlQueryStudent = $@"INSERT INTO GroupDiscussion (CourseInstanceId, DiscussionBoardId, Title, Description, StudentId, PublishedDate, LastUpdateDate, Active)
                                            VALUES ({si.CourseInstanceId}, {discussionBoard.Id}, '{si.NewPostTitle.Trim()}', '{si.NewPostDescription.Trim()}', {student.StudentId}, '{DateTime.Now}', '{DateTime.Now}', {1});";
                bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryStudent);
                return Ok(new DiscussionResultInfo() { Result = "Your post was saved" });
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
        private List<VmGroupDiscussion> GetGroupDiscussion(bool testStudent, int discussionBoardId)
        {
            string sqlQueryStudent = $@"select * from GroupDiscussion gd where gd.DiscussionBoardId = {discussionBoardId} and ((gd.Active = 1) or ('{true}' = '{testStudent}'))
                                                order by gd.PublishedDate";

            var groupDiscussionsData = SQLHelper.RunSqlQuery(sqlQueryStudent);
            List<VmGroupDiscussion> groupDiscussionsList = new List<VmGroupDiscussion>();

            foreach (var item in groupDiscussionsData)
            {
                VmGroupDiscussion groupDiscussions = null;
                List<object> st = item;
                groupDiscussions = new VmGroupDiscussion
                {
                    //x_CourseId = (int)st[0],
                    //x_CourseObjectiveId = (int)st[1],
                    //x_ModuleId = (int)st[2],
                    //x_ModuleObjetiveId = (int)st[3],
                    //x_DiscussionBoardId = (int)st[4],
                    //x_GroupDiscussionId = (int)st[5],
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
            string sqlQueryStudent = $@"delete from GroupDiscussion where Id = {discussionPostId}";
            bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryStudent);
            return isSucess;
        }
        private bool GroupDiscussionPostUpdate(int discussionPostId, string title, string description)
        {
            string sqlQueryStudent = $@"Update GroupDiscussion set Title = '{title}', Description = '{description}', LastUpdateDate = {DateTime.Now} where Id = {discussionPostId}";
            bool isSucess = SQLHelper.RunSqlUpdate(sqlQueryStudent);
            return isSucess;
        }
    }
    public class DiscussionStudentInfo
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
    public class DiscussionResultInfo
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
