function Logout() {
    localStorage.removeItem("Hash");
    localStorage.removeItem("KeepMeLoggedIn");

    localStorage.removeItem("IsAdmin");
    localStorage.removeItem("AdminHash");

    NavigateClean("index.html");
}


function manageNavLinks()
{
    document.getElementById('btnHome').href = GetUrlClean("CourseSelection.html");

    document.getElementById('btnCourse').href = GetUrlClean("CourseObjectives.html",
        "courseInstanceId", GetFromQueryString("courseInstanceId"));

    document.getElementById('btnWeek').href = GetUrlClean("ModulePage.html",
        "courseInstanceId", GetFromQueryString("courseInstanceId"),
        "moduleId", GetFromQueryString("moduleId"));

    document.getElementById('btnCalendar').href = GetUrlClean("Calendar.html");

    document.getElementById('btnComment').href = GetUrlClean("Feedback.html",
        "courseInstanceId", GetFromQueryString("courseInstanceId"));

    document.getElementById('btnContact').href = GetUrlClean("Contact.html");

    document.getElementById('ButtonUpdateProfile').href = GetUrlClean("UpdateProfile.html");

    document.getElementById("btnWeek").style.display = 'none';
    document.getElementById("btnCourse").style.display = 'none';
    document.getElementById("btnComment").style.display = 'none';

    if (GetFromQueryString("codingProblemId") != null) {
        document.getElementById("btnWeek").style.display = 'block';
        document.getElementById("btnCourse").style.display = 'block';
        document.getElementById("btnComment").style.display = 'block';
    }

    if (GetFromQueryString("moduleId") != null) {
        document.getElementById("btnCourse").style.display = 'block';
        document.getElementById("btnComment").style.display = 'block';
    }

    document.getElementById("top-bar-student-name").innerHTML = localStorage.getItem("StudentName");
    document.getElementById("usermenu-student-name").innerHTML = localStorage.getItem("StudentName");

    var imagurl = localStorage.getItem("StudentProfileImage");
    if (imagurl!="") {
        document.getElementById("user-profile-image").src = imagurl
    }
    if (window.location.href.includes("Calendar")) {
        if (GetFromQueryString("courseInstanceId")) {
            document.getElementById("btnCourse").style.display = 'block';
        }
        document.getElementById("btnCalendar").style.display = 'none';
    }
    if (window.location.href.includes("Contact")) {
        document.getElementById("btnContact").style.display = 'none';
    }
}

manageNavLinks();