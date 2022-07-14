
var listed = false;
//--------------Course Card|| By Sohel---------------------------------
//---------------------------------------------------------------------
var coursesList;
var gradeList;
var studentList;

function CourseCards() {

    const data = {
        IsAdmin: localStorage.getItem("IsAdmin"),
        AdminHash: localStorage.getItem("AdminHash"),
        Method: "Get"
    };

    fetchFunction("Course", data).then(courses => {

        coursesList = courses.CourseList;
        studentList = courses.StudentList;

        fetch("CourseCard.html").then(res => res.text()).then(d => {

            if (window.location.href.includes("CourseSelection")) {
                for (var course in coursesList) {
                    document.getElementById("pnlCourseCard").innerHTML +=
                        d.replace(/x_/g, coursesList[course].CourseInstanceId).
                        replace('href_url', GetUrl("CourseObjectives.html", "courseInstanceId", coursesList[course].CourseInstanceId));

                    BindCourseCard(course);
                   
                }
                displayStatistics()
            }
        });
        //------------------------Bind Student Dropdown list for Admin ---------------------------------
        if (localStorage.getItem("IsAdmin") == "true" & localStorage.getItem("AdminHash") != "") {
            if (studentList.length > 0) {
                document.getElementById("StudentListSection").style.display = "";
                BindDropdownStudent(studentList);
            }
        }
        else {
            document.getElementById("StudentListSection").style.display = "none";
        }
        //-----------------------------------------------------------------------------------------------
    });
}
CourseCards();

//--------------------------------------------------------------
function BindCourseCard(index) {
    d = coursesList[index];
    var appendage = d.CourseInstanceId;
    document.getElementById(appendage + "CourseTitle").textContent = d.Name;
    // if (d.Picture == "") {
    //    document.getElementById(appendage + "CourseImage").src = 'Content/images/card-default-img.jpg';
    // }
    // else {
    //    document.getElementById(appendage + "CourseImage").src = d.Picture;
    // }

    //displayStatistics(d.CourseInstanceId);

    //var courseInstanceId = d.CourseInstanceId;
    //document.getElementById(courseInstanceId + "totalGrade").innerHTML = d.TotalGrade + "%";

    //var totalCompletion = d.TotalCompletion;
    //document.getElementById(courseInstanceId + "totalCompletion").innerHTML = totalCompletion + "%";
    //document.getElementById(courseInstanceId + "TotalCompletionProgressBar").style.width = totalCompletion + "%";
    //document.getElementById(courseInstanceId + "TotalCompletionProgressBar").setAttribute("aria-valuenow", totalCompletion + "");

    //----------------Loder Spiner----------------------------
    // document.getElementById("loader-spinner").style.display = "none";
        //---------------------------------------------------------
}

//---------------------------------------------

function displayStatistics() {
    const data = {
        IsAdmin: localStorage.getItem("IsAdmin"),
        AdminHash: localStorage.getItem("AdminHash"),
        Method: "Grades"
    };

    fetchFunction("Course", data).then(courses => {
        gradeList = courses.CourseList;
        for (var d in gradeList) {
            document.getElementById(gradeList[d].CourseInstanceId + "totalGrade").innerHTML = gradeList[d].TotalGrade + "%";

            var totalCompletion = gradeList[d].TotalCompletion;
            document.getElementById(gradeList[d].CourseInstanceId + "totalCompletion").innerHTML = totalCompletion + "%";
            document.getElementById(gradeList[d].CourseInstanceId + "TotalCompletionProgressBar").style.width = totalCompletion + "%";
            document.getElementById(gradeList[d].CourseInstanceId + "TotalCompletionProgressBar").setAttribute("aria-valuenow", totalCompletion + "");

        }


 
        //----------------Loder Spiner----------------------------
        //document.getElementById("loader-spinner").style.display = "none";
        //---------------------------------------------------------
    });

}
//function displayStatistics(courseInstanceId) {
//    const data = {
//        CourseInstanceId: courseInstanceId
//    };

//    fetchFunction("CourseStatistics", data).then(d => {

//        document.getElementById(courseInstanceId + "totalGrade").innerHTML = d.Total.Grade + "%";
//        document.getElementById(courseInstanceId + "totalGPA").innerHTML = d.Total.GPA.toFixed(1);

//        var totalCompletion = d.Total.Completion;
//        document.getElementById(courseInstanceId + "totalCompletion").innerHTML = totalCompletion + "%";
//        document.getElementById(courseInstanceId + "TotalCompletionProgressBar").style.width = totalCompletion + "%";
//        document.getElementById(courseInstanceId + "TotalCompletionProgressBar").setAttribute("aria-valuenow", totalCompletion + "");

//        //----------------Loder Spiner----------------------------
//        document.getElementById("loader-spinner").style.display = "none";
//        //---------------------------------------------------------
//    });

//}
//--------------------------------------------------------------

function BindDropdownStudent(studentList) {
    var dropDownList = document.getElementById("ddlStudent");
    for (var op in studentList) {
        var option = document.createElement("option");
        option.text = studentList[op].Name;
        option.value = studentList[op].Id;
        dropDownList.add(option);
    }
}

function NavigateSelectedStudent() {
    //----------------------------Show Loder Image------------------------------
    //document.getElementById("loader-spinner").style.display = "block"
    //--------------------------------------------------------

    if (localStorage.getItem("IsAdmin") == "true" & localStorage.getItem("AdminHash") != "") {
        var selectedStudent = document.getElementById("ddlStudent").value;
        if (selectedStudent != "") {
            const data = {
                IsAdmin: localStorage.getItem("IsAdmin"),
                AdminHash: localStorage.getItem("AdminHash"),
                Method: "NavigateStudent",
                SelectedStudentId: selectedStudent
            };

            fetchFunction("Course", data).then(studentInfo => {
                const guid = studentInfo.studentIdHash;
                const error = studentInfo.error;
                if (guid == "-1") {
                    document.getElementById("lblMessage").textContent = error;
                    //----------------------------Hide Spinner------------------------------
                    // document.getElementById("loader-spinner").style.display = "none"
                    //-------------------------------------------------------------------------

                } else {
                    localStorage.setItem("Hash", guid);
                    localStorage.setItem("StudentName", studentInfo.StudentName);
                    localStorage.setItem("StudentProfileImage", studentInfo.Picture);

                    //window.location.href = "CourseSelection.html";
                    location.reload();
                }
            });

        } else {
            //----------------------------Hide Spinner------------------------------
            // document.getElementById("loader-spinner").style.display = "none"
            //-------------------------------------------------------------------------
        }
    }
}
