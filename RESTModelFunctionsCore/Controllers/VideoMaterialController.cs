using LMS.Common.ViewModels;
using LMSLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RESTModelFunctionsCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoMaterialController : ControllerBase
    {
        [HttpPost]
        public ActionResult<VideoMaterialResultInfo> Post([FromBody] VideoMaterialInfo si)
        {
            VideoMaterialResultInfo ri = new VideoMaterialResultInfo();

            VmVideoMaterial vm = GetVideoMaterials(si.VideoMaterialId);
            ri.Title = vm.Title;
            ri.Url = vm.Url;

            if (vm != null)
            {
                List<VmVideoMaterialStep> videoMaterialStepList = new List<VmVideoMaterialStep>();
                videoMaterialStepList = GetVideoMaterialStepList();
                if (videoMaterialStepList.Count > 0)
                {
                    ri.Steps = vm.VideoMaterialSteps
                .OrderBy(s => s.TimeStamp)
                .Select(vms =>
                        new VideoMaterialStepInfo
                        {
                            TimeStamp = vms.TimeStamp,
                            Action = vms.Action,
                            Xs1 = vms.Xs1,
                            Ys1 = vms.Ys1,
                            Xs2 = vms.Xs2,
                            Ys2 = vms.Ys2,
                            Xe1 = vms.Xe1,
                            Ye1 = vms.Ye1,
                            Xe2 = vms.Xe2,
                            Ye2 = vms.Ye2,
                            Text = vms.Text,
                            Style = vms.Style
                        }
                    ).ToArray();
                }
            }

            return Ok(ri);
        }
        private List<VmVideoMaterialStep> GetVideoMaterialStepList()
        {
            string sqlQueryVideoMaterialStep = $@"select vs.VideoMaterialId, vs.TimeStamp, vs.Action, 
                                                  vs.Xs1, vs.Ys1, vs.Xs2, vs.Ys2, vs.Xe1, vs.Ye1, vs.Xe2, 
                                                  vs.Ye2, vs.Text, vs.Id, vs.Style from VideoMaterialStep vs";

            var videoMaterialStepData = SQLHelper.RunSqlQuery(sqlQueryVideoMaterialStep);
            List<VmVideoMaterialStep> videoMaterialStepList = new List<VmVideoMaterialStep>();

            foreach (var item in videoMaterialStepData)
            {
                VmVideoMaterialStep videoMaterialStep = null;
                List<object> st = item;
                videoMaterialStep = new VmVideoMaterialStep
                {
                    VideoMaterialId = (int)st[0],
                    TimeStamp = (int)st[1],
                    Action = st[2].ToString(),
                    Xs1 = (int)st[3],
                    Ys1 = (int)st[4],
                    Xs2 = (int)st[5],
                    Ys2 = (int)st[6],
                    Xe1 = (int)st[7],
                    Ye1 = (int)st[8],
                    Xe2 = (int)st[9],
                    Ye2 = (int)st[10],
                    Text = st[11].ToString(),
                    Id = (int)st[12],
                    Style = st[13].ToString()
                };
                videoMaterialStepList.Add(videoMaterialStep);
            }

            return videoMaterialStepList;
        }
        private VmVideoMaterial GetVideoMaterials(int videoMaterialId)
        {
            string sqlQueryVmVideoMaterial = $@"select v.Id,v.Title,v.Url from VideoMaterial v where v.Id = '{videoMaterialId}'";

            var videoMaterialData = SQLHelper.RunSqlQuery(sqlQueryVmVideoMaterial);
            VmVideoMaterial videoMaterialInfo = null;

            if (videoMaterialData.Count > 0)
            {
                List<object> st = videoMaterialData[0];

                videoMaterialInfo = new VmVideoMaterial
                {
                    Id = (int)st[0],
                    Title = st[1].ToString(),
                    Url = st[2].ToString()
                };
            }
            return videoMaterialInfo;
        }
    }
    public class VideoMaterialInfo
    {
        public int VideoMaterialId { get; set; }
        public string Hash { get; set; }
    }
    public class VideoMaterialStepInfo
    {
        public int TimeStamp { get; set; }
        public string Action { get; set; }
        public int Xs1 { get; set; }
        public int Ys1 { get; set; }
        public int Xs2 { get; set; }
        public int Ys2 { get; set; }
        public int Xe1 { get; set; }
        public int Ye1 { get; set; }
        public int Xe2 { get; set; }
        public int Ye2 { get; set; }
        public string Text { get; set; }

        public string Style { get; set; }
    }
    public class VideoMaterialResultInfo
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public VideoMaterialStepInfo[] Steps { get; set; }

    }
}
