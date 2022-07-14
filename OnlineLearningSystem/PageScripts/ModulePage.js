var studentId;

function fetchModuleObjectives() {
    fetch("ModuleObjectiveProgress.html").then(rs => rs.text()).then(d => {
        getModuleObjectives(d);
    });

}

function getModuleObjectives(control) {

    const data = {
        ModuleId: GetFromQueryString("moduleId"),
        CourseInstanceId: GetFromQueryString("courseInstanceId"),
    };

    fetchFunction("ModuleObjective", data).then(d => {

        studentId = d.StudentId;
        ModuleObjectiveData = d.ModuleObjectives;

        var moduleObjectiveList = d.ModuleObjectives;

        for (var i = 0; i < moduleObjectiveList.length; i++) {

            var moduleObjective = moduleObjectiveList[i];
            var moId = moduleObjective.Id;
            document.getElementById("pnlModuleObjectives").innerHTML += control.replace(/x_/g, moId);

            //document.getElementById(moId + "LabelModuleTitle").textContent = d.Description.toUpperCase() + ". " + moduleObjective.Description + ".";

            getModuleObjectiveElements("Material", moduleObjective.Materials, "lstMaterials", moId);
            //--------------------------------------------------------------------
            getModuleObjectiveElements("Quiz", moduleObjective.Quizzes, "lstActivites", moId);
            getModuleObjectiveElements("Assessment", moduleObjective.Assessments, "lstAssessments", moId);
            getModuleObjectiveElements("Poll", moduleObjective.Polls, "lstPolls", moId);
            getModuleObjectiveElements("Discussion", moduleObjective.Discussions, "lstDiscussions", moId);

            document.getElementById(moId + "lblActivites").style.display = moduleObjective.Quizzes.length > 0 ? "inline-block" : "none";
            document.getElementById(moId + "lblMaterials").style.display = moduleObjective.Materials.length > 0 ? "inline-block" : "none";
            document.getElementById(moId + "lblAssessments").style.display = moduleObjective.Assessments.length > 0 ? "inline-block" : "none";
            document.getElementById(moId + "lblPolls").style.display = moduleObjective.Polls.length > 0 ? "inline-block" : "none";
            document.getElementById(moId + "lblDiscussionBoard").style.display = moduleObjective.Discussions.length > 0 ? "block" : "none";
            //--------------------Visible area-----------------------------------------------------
            document.getElementById(moId + "QuizArea").style.display = moduleObjective.Quizzes.length > 0 ? "block" : "none";
            document.getElementById(moId + "MaterialArea").style.display = moduleObjective.Materials.length > 0 ? "block" : "none";
            document.getElementById(moId + "AssessmentArea").style.display = moduleObjective.Assessments.length > 0 ? "block" : "none";
            var PollDiscussionArea = false
            if (moduleObjective.Polls.length > 0 || moduleObjective.Discussions.length > 0) {
                PollDiscussionArea = true;
            }
            document.getElementById(moId + "PollArea").style.display = PollDiscussionArea ? "block" : "none";


        }
        //----------------Loder Spiner----------------------------
        //document.getElementById("loader-spinner").style.display = "none";
        //---------------------------------------------------------
    });

}

fetchModuleObjectives();

//-----------------------------------------------------------------------------------------------------------------
function getModuleObjectiveElements(area, data, Domid, index) {
    fetch("ModuleObjectiveProgressElement.html").then(rs => rs.text()).then(cntrl => {
        if (data.length > 0) {

            if (area == "Discussion") {
                document.getElementById(index + "PollArea").style.display = "block";
            }
            else {
                document.getElementById(index + area + "Area").style.display = "block";
            }
        }

        for (var j in data) {
            var boxArea = document.getElementById(index + Domid);
            var itemId = data[j].ActivityId;

            cntrl1 = cntrl.replace(/x_/g, index).replace(/y_/g, itemId).replace(/a_/g, area);

            if (area == 'Material') {
                cntrl1 = cntrl1.replaceAll('href_url', GetUrl('Material.html', 'materialId', itemId));
                cntrl1 = cntrl1.replaceAll('_self', '_blank');
            }
            else if (area == 'Quiz') {
                cntrl1 = cntrl1.replaceAll('href_url', GetUrl('QuizPage.html', 'questionSetId', itemId));
            }
            else if (area == 'Assessment') {
                cntrl1 = cntrl1.replaceAll('href_url', GetUrl('Interaction.html', 'codingProblemId', itemId));
            }
            else if (area == 'Poll') {
                cntrl1 = cntrl1.replaceAll('href_url', GetUrl('PollResponse.html', 'pollGroupId', itemId, "moduleObjectiveId", index));
            }
            else if (area == 'Discussion') {
                cntrl1 = cntrl1.replaceAll('href_url', GetUrl('DiscussionBoardPage.html', 'discussionBoardId', itemId, "moduleObjectiveId", index));
            }
            else {
                throw 'Activity Not Supported: ' + area;
            }
            boxArea.innerHTML += cntrl1;
            var mt = data[j].Title;
            if (mt.length > 50) {
                mt = data[j].Title.substring(0, 50) + "...";
            }
            document.getElementById(index + "_" + itemId + area + "Title").textContent = mt;

            if (area == "Quiz" || area == "Assessment" || area == "Poll" || area == "Discussion") {
                loadProgressData(data[j], area, index,);
                //--------------Hide On Hover Effect------------------------
                //document.getElementById(index + "_" + itemId + area + "PerformanceArea").style.display="block";
                document.getElementById(index + "_" + itemId + area + "Progress").style.display = "block";
                if (data[j].DueDate != null) {
                    if (data[j].DueDate != "") {
                        document.getElementById(index + "_" + itemId + area + "due-time-area").style.display = "block";
                        //document.getElementById(index + "_" + itemId + area + "DueDate").style.display = "block";
                        //document.getElementById(index + "_" + itemId + area + "DueTime").style.display = "block";
                    }
                }
            }
            else {

            }
        }

        let module = GetFromQueryString('moduleId');
        let step = GetFromQueryString('step');

        if (module == 443 && step == null)
            step = 'material';

        if (step != null) {
            let name = `div_${step}_tutorial`;
            document.getElementById(name).style.display = '';
        }
    });
}

function loadProgressData(data, area, index) {
    var itemId = data.ActivityId;

    document.getElementById(index + "_" + itemId + area + "Grade").textContent = "Grade: " + data.Grade + "%";
    document.getElementById(index + "_" + itemId + area + "Correct").textContent = "Correct: " + data.Correct;
    document.getElementById(index + "_" + itemId + area + "Revealed").textContent = "Revealed: " + data.Revealed;
    //if (data.Completion == 100) {
    //    document.getElementById(index + "_" + itemId + area + "Completion").textContent = "Completed";
    //}
    //else {
    //    document.getElementById(index + "_" + itemId + area + "Completion").textContent = "Progress: " + data.Completion + "%";
    //}
    document.getElementById(index + "_" + itemId + area + "Completion").textContent = data.Completion + "%";
    document.getElementById(index + "_" + itemId + area + "ModuleProgressBar").style.width = data.Completion + "%";
    document.getElementById(index + "_" + itemId + area + "ModuleProgressBar").setAttribute("aria-valuenow", data.Completion + "");

    document.getElementById(index + "_" + itemId + area + "Uncompleted").textContent = "Remaining: " + (100 - data.Completion) + "%";
    if (data.DueDate != null) {
        if (data.DueDate != "") {

            var todayDate = new Date();
            var dueDate = new Date(data.DueDate);
            
            if (dueDate < todayDate) {
                document.getElementById(index + "_" + itemId + area + "DueDate").style.color = "#F15656"
                document.getElementById(index + "_" + itemId + area + "DueTime").style.color = "#F15656"
                document.getElementById(index + "_" + itemId + area + "DueTime_Icon").setAttribute("src", "Content/images/clock_icon_red.png")
            }
            document.getElementById(index + "_" + itemId + area + "DueDate").textContent = getDateFormate(data.DueDate);
            document.getElementById(index + "_" + itemId + area + "DueTime").textContent = getTimeFormate(data.DueDate);
        }
    }
}

function LoadDiscussionProgressTable(index, itemId, area) {
    document.getElementById(index + "_" + itemId + area + "ProgressTable").style.display = "inline-table";
    var tHead = document.getElementById(index + "_" + itemId + area + "ProgressTableHeader");
    var tBody = document.getElementById(index + "_" + itemId + area + "ProgressTableBody");
    var tr = NewTr();
    tr.appendChild(NewTh("Total Posts"));
    tr.appendChild(NewTh("Unread Posts"));
    tr.appendChild(NewTh("Replise To Me"));
    tr.appendChild(NewTh("Total Participants"));
    tr.appendChild(NewTh(""));

    tHead.appendChild(tr)

    var tr2 = NewTr();
    tr2.appendChild(NewTd("0"));
    tr2.appendChild(NewTd("0"));
    tr2.appendChild(NewTd("0"));
    tr2.appendChild(NewTd("0"));

    tr2.appendChild(NewCheckBox());
    tBody.appendChild(tr2);
}

function LoadPollProgressTable(index, itemId, area) {
    document.getElementById(index + "_" + itemId + area + "ProgressTable").style.display = "inline-table";
    var tHead = document.getElementById(index + "_" + itemId + area + "ProgressTableHeader");
    var tBody = document.getElementById(index + "_" + itemId + area + "ProgressTableBody");
    var tr = NewTr();


    tr.appendChild(NewTh("Total Questions"));
    tr.appendChild(NewTh("Answered Questions"));
    tr.appendChild(NewTh(""));

    tHead.appendChild(tr)
    var tr2 = NewTr();
    tr2.appendChild(NewTd("0"));
    tr2.appendChild(NewTd("0"));
    tr2.appendChild(NewCheckBox());

    tBody.appendChild(tr2);
}

function NewTh(text) {
    var th = document.createElement("th");
    var t = document.createTextNode(text);
    th.appendChild(t);
    return th;
}

function NewTd(text) {
    var td = document.createElement("td");
    var t = document.createTextNode(text);
    td.appendChild(t);
    return td;
}

function NewTr() {
    return document.createElement("tr");
}

function NewCheckBox() {
    var td = document.createElement("td");
    var check = document.createElement("INPUT");
    check.setAttribute("type", "checkbox");
    td.appendChild(check);
    return td;
}

function BindOtherStudentGrade(sdata, indGrade) {
    var StudentResult = {};
    sg = sdata.split(",");
    sg.push(indGrade);
    StudentResult = ({ data: sg, individual: indGrade });
    return StudentResult;
}
var ModuleObjectiveData;
function ShowHideGrades(x) {

    if (x.textContent.toLowerCase() == "show grades") {
        x.textContent = "Hide Grades";
        var g = document.getElementsByClassName("WithGrade");
        for (var i = 0; i < g.length; i++) {
            g[i].style.display = "block";
        }

        var g = document.getElementsByClassName("progress");
        for (var i = 0; i < g.length; i++) {
            g[i].classList.add("hoverWraper");
            g[i].style.cursor = "pointer";
        }

    }
    else {
        x.textContent = "Show Grades";
        var g = document.getElementsByClassName("WithGrade");
        for (var i = 0; i < g.length; i++) {
            g[i].style.display = "none";
        }
        var g = document.getElementsByClassName("progress");
        for (var i = 0; i < g.length; i++) {
            g[i].classList.remove("hoverWraper");
            g[i].style.cursor = "";

        }

    }
}
function getDateFormate(date) {
    //MONTH / DAY / YEAR HH: MM AM / PM
    var newDate = new Date(date)
    var m = newDate.getMonth() + 1;
    var month = m > 9 ? m : '0' + m;
    var day = newDate.getDate() > 9 ? newDate.getDate() : '0' + newDate.getDate();
    var year = newDate.getFullYear();

    return month + '/' + day + '/' + year;
}
function getTimeFormate(date) {
    var newDate = new Date(date)
    var hours = newDate.getHours();
    var minutes = newDate.getMinutes();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    hours = hours < 10 ? '0' + hours : hours;
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var result = hours + ':' + minutes + ':00 ' + ampm;
    return result;
}

