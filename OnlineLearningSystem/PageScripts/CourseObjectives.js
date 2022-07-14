var shown = false;
var loadStatistics = false;
var courseObjectives;

fetchCourseObjectives();
DisplayCourse();

document.getElementById('syllabusLink').href = GetUrl("Syllabus.html");
document.getElementById('a_announcemnts').href = GetUrl("AnnouncementPage.html");

function DisplayCourse() {
    displayAnnouncements();
    displayStatistics();
}

function displayAnnouncements()
{
    const data = {
        CourseInstanceId: GetFromQueryString("courseInstanceId")
    };

    fetchFunction("CourseAnnouncement", data).then(d => {
        if (d.length > 0)
        {
            document.getElementById("PanelAnnouncement").style.display = "block";
            document.getElementById("LabelAnnouncementTitle").innerHTML = d[0].Title;
            document.getElementById("LabelAnnouncementDescription").innerHTML = d[0].Description;
            document.getElementById("LabelPublishedDate").innerHTML = d[0].PublishedDate;
        }
    });
}

function displayStatistics()
{
    const data = {
        CourseInstanceId: GetFromQueryString("courseInstanceId"),
    };

    fetchFunction("CourseStatistics", data).then(d => {
        loadStatistics = true;
        //document.getElementById("lblCourseTitle").innerHTML = d.CourseName;

        if (d.CourseName.startsWith('LIS') || d.CourseName.startsWith('B BUS'))
            document.getElementById('syllabusLink').style.display = 'none';

        var assessmentWeight = d.Assessment.Weight;
        if (assessmentWeight > 0) {
            document.getElementById("assessmentRow").style.display = "table-row";

            var assessmentCompletion = d.Assessment.Completion;
            document.getElementById("assessmentWeight").innerHTML = assessmentWeight + "%";
            document.getElementById("currentAssessmentGrade").innerHTML = d.Assessment.Grade + "%";
            document.getElementById("weightedAssessmentGrade").innerHTML = d.Assessment.WeightedGrade+"%";

            document.getElementById("assessmentCompletion").innerHTML = assessmentCompletion + "%";
            document.getElementById("AssessmentCompletionProgressBar").style.width = assessmentCompletion + "%";
            document.getElementById("AssessmentCompletionProgressBar").setAttribute("aria-valuenow", assessmentCompletion + "");
        }

        var quizWeight = d.Quiz.Weight;
        if (quizWeight > 0) {
            document.getElementById("quizRow").style.display = "table-row";

            var quizCompletion = d.Quiz.Completion;
            document.getElementById("quizWeight").innerHTML = quizWeight + "%";
            document.getElementById("currentQuizGrade").innerHTML = d.Quiz.Grade + "%";
            document.getElementById("weightedQuizGrade").innerHTML = d.Quiz.WeightedGrade + "%";

            document.getElementById("quizCompletion").innerHTML = quizCompletion + "%";
            document.getElementById("QuizCompletionProgressBar").style.width = quizCompletion + "%";
            document.getElementById("QuizCompletionProgressBar").setAttribute("aria-valuenow", quizCompletion + "");
        }

        var materialWeight = d.Material.Weight;
        if (materialWeight > 0) {
            document.getElementById("materialRow").style.display = "table-row";

            var materialCompletion = d.Material.Completion;
            document.getElementById("materialWeight").innerHTML = materialWeight + "%";
            document.getElementById("currentMaterialGrade").innerHTML = d.Material.Grade + "%";
            document.getElementById("weightedMaterialGrade").innerHTML = d.Material.WeightedGrade + "%";

            document.getElementById("materialCompletion").innerHTML = materialCompletion + "%";
            document.getElementById("MaterialCompletionProgressBar").style.width = materialCompletion + "%";
            document.getElementById("MaterialCompletionProgressBar").setAttribute("aria-valuenow", materialCompletion + "");
        }

        var midtermWeight = d.Midterm.Weight;
        if (midtermWeight > 0) {
            document.getElementById("midtermRow").style.display = "table-row";

            var midtermCompletion = d.Midterm.Completion;
            document.getElementById("midtermWeight").innerHTML = midtermWeight + "%";
            document.getElementById("currentMidtermGrade").innerHTML = d.Midterm.Grade + "%";
            document.getElementById("weightedMidtermGrade").innerHTML = d.Midterm.WeightedGrade + "%";

            document.getElementById("midtermCompletion").innerHTML = midtermCompletion + "%";
            document.getElementById("MidtermCompletionProgressBar").style.width = midtermCompletion + "%";
            document.getElementById("MidtermCompletionProgressBar").setAttribute("aria-valuenow", midtermCompletion + "");
        }

        var finalWeight = d.Final.Weight;
        if (finalWeight > 0) {
            document.getElementById("finalRow").style.display = "table-row";

            var finalCompletion = d.Final.Completion;
            document.getElementById("finalWeight").innerHTML = finalWeight + "%";
            document.getElementById("currentFinalGrade").innerHTML = d.Final.Grade + "%";
            document.getElementById("weightedFinalGrade").innerHTML = d.Final.WeightedGrade + "%";

            document.getElementById("finalCompletion").innerHTML = finalCompletion + "%";
            document.getElementById("FinalCompletionProgressBar").style.width = finalCompletion + "%";
            document.getElementById("FinalCompletionProgressBar").setAttribute("aria-valuenow", finalCompletion + "");
        }

        var pollWeight = d.Poll.Weight;
        if (pollWeight > 0) {
            document.getElementById("pollRow").style.display = "table-row";

            var pollCompletion = d.Poll.Completion;
            document.getElementById("pollWeight").innerHTML = pollWeight + "%";
            document.getElementById("currentPollGrade").innerHTML = d.Poll.Grade + "%";
            document.getElementById("weightedPollGrade").innerHTML = d.Poll.WeightedGrade + "%";

            document.getElementById("pollCompletion").innerHTML = pollCompletion + "%";
            document.getElementById("PollCompletionProgressBar").style.width = pollCompletion + "%";
            document.getElementById("PollCompletionProgressBar").setAttribute("aria-valuenow", pollCompletion + "");
        }

        var discussionWeight = d.Discussion.Weight;
        if (discussionWeight > 0) {
            document.getElementById("discussionRow").style.display = "table-row";

            var discussionCompletion = d.Discussion.Completion;
            document.getElementById("discussionWeight").innerHTML = discussionWeight + "%";
            document.getElementById("currentDiscussionGrade").innerHTML = d.Discussion.Grade + "%";
            document.getElementById("weightedDiscussionGrade").innerHTML = d.Discussion.WeightedGrade + "%";

            document.getElementById("discussionCompletion").innerHTML = discussionCompletion + "%";
            document.getElementById("DiscussionCompletionProgressBar").style.width = discussionCompletion + "%";
            document.getElementById("DiscussionCompletionProgressBar").setAttribute("aria-valuenow", discussionCompletion + "");
        }

        document.getElementById("totalGrade").innerHTML = "Course Predicted Grade: " + d.Total.Grade + "%";
        document.getElementById("totalGPA").innerHTML = d.Total.GPA.toFixed(1);

        document.getElementById("totalCurrentGrade").innerHTML = "Course Current Grade: " + d.Total.CurrentGrade + "%";
        document.getElementById("totalCurrentGPA").innerHTML = d.Total.CurrentGPA.toFixed(1);

        var totalCompletion = d.Total.Completion;
        document.getElementById("totalCompletion").innerHTML = totalCompletion + "% COMPLETED";
        document.getElementById("hr-tag").appendChild(document.createElement("hr"));

        //----------------Loder Spiner----------------------------
        document.getElementById("loader-spinner-static-area").style.display = "none";
        //---------------------------------------------------------
    });
}

function fetchCourseObjectives()
{
    fetch("CourseObjectiveProgress.html").then(rs => rs.text()).then(d1 => {
        fetch("ModuleProgress.html").then(rs => rs.text()).then(d2 => {
            getCourseObjectives(d1, d2);
            loadGrades()
        });
    });
}

function getCourseObjectives(courseProgressControl, moduleProgressControl)
{
    const data = {
        CourseInstanceId: GetFromQueryString("courseInstanceId"),
        Method: "GetCourseObjective"
    };

    fetchFunction("CourseObjective", data).then(d => {
        courseObjCount = d.CourseObjectiveList.length;
        courseObjectives = d.CourseObjectiveList;
        document.getElementById("lblCourseTitle").innerHTML = d.Name;
        for (var i = 0; i < d.CourseObjectiveList.length; i++) {
            document.getElementById("pnlPanel").innerHTML += courseProgressControl.replace(/y_/g, d.CourseObjectiveList[i].Id);
            document.getElementById(d.CourseObjectiveList[i].Id + "Description").textContent = d.CourseObjectiveList[i].Description;

            var panelLayout = document.getElementById(d.CourseObjectiveList[i].Id + "pnlLayout");
            getModules(panelLayout, i, d.CourseObjectiveList[i].Id, d.CourseObjectiveList[i].Modules, moduleProgressControl);
        }
        //----------------Loder Spiner----------------------------
        document.getElementById("loader-spinner").style.display = "none";
        if (!loadStatistics) {
            document.getElementById("loader-spinner-static-area").style.display = "block";
        }
        //---------------------------------------------------------
    });
}

function fetchModules() {
    fetch("ModuleProgress.html").then(rs => rs.text()).then(d => {
        getModules(d);
    });
}

function getModules(panel, i, courseObjectiveId, modules, control) {
    for (var j = 0; j < modules.length; j++) {
        ctrltext = control.replace(/x_/g, courseObjectiveId).replace(/y_/g, modules[j].ModuleId);
        ctrltext = ctrltext.replace('href_url', GetUrl("ModulePage.html", "moduleId", courseObjectives[i].Modules[j].ModuleId));
        panel.innerHTML += ctrltext;
        getModule(courseObjectiveId + "_" + modules[j].ModuleId, modules[j]);
    }
}

//-----------------------Load Grads--------------------------------
function loadGrades() {
    const data = {
        CourseInstanceId: GetFromQueryString("courseInstanceId"),
        Method: "LoadGrades"
    };
    fetchFunction("CourseObjective", data).then(d => {
        courseObjCount = d.length;
        courseObjectives = d;
        for (var i = 0; i < d.length; i++) {
            var modules = d[i].Modules;
            var courseObjectiveId = d[i].Id;

            for (var j = 0; j < modules.length; j++) {
                var index = courseObjectiveId + "_" + modules[j].ModuleId;
                //document.getElementById(index + "grade").innerHTML = "Grade: " + modules[j].Percent + "% ( GPA " + modules[j].GPA.toFixed(1) + ")";
                document.getElementById(index + "completion").innerHTML = modules[j].Completion + "%";
                document.getElementById(index + "strokeDashArray").setAttribute("aria-valuenow", modules[j].Completion);
                document.getElementById(index + "strokeDashArray").style.width = modules[j].Completion + "%";
            }
        }
    });
}


