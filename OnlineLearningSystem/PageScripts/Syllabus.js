
//--------------Syllabus|| By Sohel---------------------------------
//---------------------------------------------------------------------
var syllabusInfo;
function DisplaySyllabus() {
    const data = {
        CourseInstanceId: GetFromQueryString("courseInstanceId"),
    };

    fetchFunction("Syllabus", data).then(data => {
        if (data.CourseName == null) {
            alert("Syllabus Data is null");
            window.location.href = "CourseSelection.html";
        }

        data.NonAcademicDays.forEach(x => {
            let d1, o1, d2, o2, end, nad, li
            d1 = new Date(x.StartDate);
            o1 = { weekday: 'long', month: 'long', day: 'numeric' };
            d2 = new Date(x.EndDate);
            o2 = { day: 'numeric' };
            end = '';

            if (d1.getTime() != d2.getTime()) {
                end = ` - ${d2.toLocaleDateString("en-US", o2)}`;
            }

            nad = `${d1.toLocaleDateString("en-US", o1)}${end}, ${x.Description} (${x.Type})`;
            li = document.createElement('li');
            li.appendChild(document.createTextNode(nad));
            document.getElementById("NonAcademicDays").appendChild(li);
        });

        //---------------------------Syllabus Header------------------------
        document.getElementById("CourseName").innerHTML = data.CourseName;
        document.getElementById("QuarterName").innerHTML = data.Quarter.Name;
        document.getElementById("SchoolName").innerHTML = data.Quarter.SchoolName;

        //---------------------------Course Information-------------------------
        document.getElementById("CourseCredits").innerHTML = "Credits: " + data.Credits;
        NewUlList(data.Sessions, "LectureDay", "lecture", "Asynchronous Course - No Lecture")  // List FOr Lecture
        CreateLocationList(data.Sessions, "location");
        document.getElementById("WithdrawDate").innerHTML = "Last Day to Withdraw from Classes: " + data.Quarter.WithdrawDate;
        NewLinkLi(data.Quarter.Calendar, "Academic Calendar", "AcademicCalendar");

        //---------------------------Syllabus Message------------------------------
        document.getElementById("SyllabusMessage").innerHTML = data.Quarter.SyllabusMessage;

        //---------------------------Professor Contact Information--------------------
        CreateInstructorPanel(data.Instructors, "ProfessorInfoPanel");

        //---------------------------Course Description-----------------------------
        document.getElementById("CourseDescription").innerHTML = data.CourseDescription;

        //---------------------------Course PreRequisites-----------------------------

        let dpr = new Map();

        data.Prerequisites.forEach(pr => {

            let spr = `${pr.Department} ${pr.Id}`;

            if (pr.GroupId in dpr) {
                dpr[pr.GroupId] = `${dpr[pr.GroupId]} OR ${spr}`;
            }
            else {
                dpr[pr.GroupId] = spr;
            }
        });

        for (let key in dpr) {
            let li = document.createElement('li');
            li.appendChild(document.createTextNode(dpr[key]));
            document.getElementById("CoursePrerequisites").appendChild(li);
        }

        li = document.createElement('li');
        //---------------------------Course CoRequisites-----------------------------
        if (data.Corequisites != "") {
            li.appendChild(document.createTextNode(data.Corequisites));
        } else {
            li.appendChild(document.createTextNode("None"));
        }
        document.getElementById("CourseCorequisites").appendChild(li);


        //---------------------------Student Outcomes/Competencies-----------------------------
        NewUlList(data.Outcomes, "Description", "StudentOutcome")

        //---------------------------Method of Instruction/Course Delivery-----------------------------
        NewUlList(data.InstructionMethods, "Description", "InstructionMethod")

        //---------------------------Technology Requirements-----------------------------
        NewUlList(data.Technologies, "Description", "TechnologyRequirement")

        //---------------------------Textbook Requirements-----------------------------
        NewUlList(data.Textbooks, "Description", "TextbookRequirement")

        //---------------------------Required Tools-----------------------------
        NewUlList(data.Tools, "Description", "RequiredTool")

        //---------------------------Supplies-----------------------------
        NewUlList(data.Supplies, "Description", "CourseSupplies")

        //---------------------------Materials-----------------------------
        NewUlList(data.Materials, "Description", "CourseMaerial")

        //---------------------------Grading Policy-----------------------------
        document.getElementById("GradingPolicy").innerHTML = data.GradingPolicy;

        //---------------------------Grading Scale-----------------------------
        CreateGradingScaleTr(data.GradeScales, "GradingScale");

        //---------------------------Grading Scale Weighting-----------------------------
        CreateGradingScaleWeight(data.GradeScaleWeights, "GradingScaleWeight")

        //---------------------------Course Policies and Procedures-----------------------------
        CreatePointSection(data.Policies, "CoursePolicy");

        //--------------------------- Community Standards -----------------------------
        CreatePointSection(data.CommunityStandards, "CommunityStandard");

        //--------------------------- Tentative Assignment Schedule -----------------------------
        BuildAssignmentSchedule(data.TentativeAssignmentSchedule);

        //--------------------------- Student Conduct Expectations -----------------------------
        //data.StudentConduct

        //--------------------------- Campus Public Safety -----------------------------
        CreatePointSection(data.CampusPublicSafeties, "CampusPublicSafety");

        //--------------------------- Support Services During LWTech Remote Operations -----------------------------
        CreatePointSection(data.SupportServices, "SupportService");

        //--------------------------- Student Support Resources -----------------------------
        CreateListLink(data.StudentSupportResources, "StudentSupportResources");

        //--------------------------- Netiquette -----------------------------
        CreatePointSection(data.Netiquette, "Netiquette");

        //----------------Loder Spiner----------------------------
        document.getElementById("loader-spinner").style.display = "none";



        //---------------------------------------------------------


    });
}
DisplaySyllabus();

//--------------------------------------------------------------------------------------------------------------------

function CreateListLink(data, domId) {
    let div = document.getElementById(domId);
    let ul = document.createElement('ul');

    for (let i in data) {
        let link = data[i];
        let li = document.createElement('li');
        let a = document.createElement("a")
        a.setAttribute('href', link.Link);
        a.setAttribute("target", "_blank");
        a.textContent = link.Title;
        li.appendChild(a);
        ul.appendChild(li);

    }

    div.appendChild(ul);
}


function BuildAssignmentSchedule(TentativeAssignmentSchedule) {
    let tableHeaders = ["Week", "Topic", "Number of Quizzes", "Assignments", "Test", "Meeting", "Due Date"];
    let table, tableHeader, tableBody, tableHeaderRow
    let AssignmentSchedule = document.getElementById('AssignmentSchedule')

    while (AssignmentSchedule.firstChild) {
        AssignmentSchedule.removeChild(AssignmentSchedule.firstChild)
    }

    table = document.createElement('table')
    tableHeader = document.createElement('thead')
    tableBody = document.createElement('tbody')
    tableHeaderRow = document.createElement('tr')
    tableHeaderRow.style.textAlign = "center";

    table.className = 'academicTable table table-bordered table-responsive';
    tableHeader.className = 'academicTableHeader';
    tableHeaderRow.className = 'academicTableHeaderRow';

    tableHeaders.forEach(header => {
        let currentHeader = document.createElement('th')
        currentHeader.innerText = header
        tableHeaderRow.append(currentHeader)
    })

    tableHeader.append(tableHeaderRow)
    table.append(tableHeader)

    TentativeAssignmentSchedule.forEach(week => {
        let currentRow = document.createElement('tr')
        let currentItem;

        Object.entries(week).forEach(([key, value]) => {
            currentItem = document.createElement('td')
            currentItem.innerText = value
            currentRow.appendChild(currentItem)
        });

        tableBody.append(currentRow)
        table.append(tableBody)
    })

    document.getElementById('AssignmentSchedule').append(table);
}



function NewUlList(data, item, domId, emptyMessage = "None") {
    let ul = document.getElementById(domId);
    let li, t

    if (data.length > 0) {
        for (let i = 0; data.length > i; i++) {
            li = document.createElement("li");
            t = document.createTextNode(data[i][item]);
            li.appendChild(t);
            ul.appendChild(li);
        }

    } else {
        li = document.createElement("li");
        t = document.createTextNode(emptyMessage);
        li.appendChild(t);
        ul.appendChild(li);
    }

}

function NewLinkLi(data, description, DomId) {
    let li, t, a
    li = document.getElementById(DomId);
    t = document.createTextNode(description + ": ");
    li.appendChild(t);
    a = newLink(data);
    return li.appendChild(a);
}

function CreateInstructorPanel(data, DomId) {

    let ProfessorInfoPanel = document.getElementById(DomId);


    let instructors = document.createElement("ul");
    ProfessorInfoPanel.appendChild(instructors);

    for (let i in data) {
        let instructorLi, instructorUl, contactLi, contactMethods, hoursLi, hoursAvailable

        instructorLi = document.createElement("li");
        instructors.appendChild(instructorLi);
        instructorLi.appendChild(document.createTextNode(data[i].Name));

        instructorUl = document.createElement("ul");
        instructorLi.appendChild(instructorUl);

        contactLi = document.createElement("li");
        instructorUl.appendChild(contactLi);
        contactLi.appendChild(document.createTextNode("Contact Method"));

        contactMethods = document.createElement("ul");
        contactLi.appendChild(contactMethods);

        //-----------------------------
        for (let contact in data[i].ContactInfo) {
            let itemData, innerContact, a, t
            itemData = data[i].ContactInfo[contact];
            innerContact = document.createElement("li");
            a = newLink(itemData.Contact);
            t = document.createTextNode(` (${itemData.Title})`);
            innerContact.appendChild(a);
            innerContact.appendChild(t);

            contactMethods.appendChild(innerContact);
        }

        hoursLi = document.createElement("li");
        instructorUl.appendChild(hoursLi);
        hoursLi.appendChild(document.createTextNode("Available Hours"));

        hoursAvailable = document.createElement("ul");
        hoursLi.appendChild(hoursAvailable);

        if (data[i].InstructorAvailableHours.length > 0) {
            //-----------------------------
            for (let j in data[i].InstructorAvailableHours) {
                let hours = data[i].InstructorAvailableHours[j];
                let innerHours = document.createElement("li");
                t = document.createTextNode(`${hours.DayOfWeek}: ${hours.StartTime} - ${hours.EndTime}`);
                innerHours.appendChild(t);

                hoursAvailable.appendChild(innerHours);
            }
        }
        else
        {
            let innerHours = document.createElement("li");
            t = document.createTextNode("By Appointment (send e-mail)");
            innerHours.appendChild(t);
            hoursAvailable.appendChild(innerHours);
        }
    }
}

function CreateLocationList(data, DomId) {

    let LocationUl = document.getElementById(DomId);
    let li, t, a
    if (data.length > 0) {
        for (let i in data) {
            li = document.createElement("li");
            t = document.createTextNode(data[i].Description + " ");
            li.appendChild(t);
            a = newLink(data[i].Location)
            li.appendChild(a);
            LocationUl.appendChild(li);
        }
    }
    else {
        li = document.createElement("li");
        t = document.createTextNode("Online via ");
        hr = document.createElement("a");
        hr.appendChild(document.createTextNode("LetsUseData"));
        hr.setAttribute('href', 'http://letsusedata.com');
        li.appendChild(t);
        li.appendChild(hr);
        LocationUl.appendChild(li);
    }
}

function newLink(data) {
    let a = document.createElement("a")
    a.textContent = data;
    a.setAttribute('href', data);
    a.setAttribute('target', "_blank");
    return a;
}

function CreateGradingScaleTr(data, DomId) {

    let GradingScaleArea = document.getElementById(DomId);
    let tr, td1, td2, tgpa, tpoint
    for (let i in data) {
        tr = document.createElement("tr");
        td1 = document.createElement("td");
        tgpa = document.createTextNode(data[i].GPA);
        td1.appendChild(tgpa);

        td2 = document.createElement("td");
        tpoint = document.createTextNode(data[i].Point);
        td2.appendChild(tpoint);

        tr.appendChild(td1);
        tr.appendChild(td2);

        GradingScaleArea.appendChild(tr);
    }
}

function CreateGradingScaleWeight(data, DomId) {
    let GradingWeightArea = document.getElementById(DomId);
    for (let i in data) {
        let span = document.createElement("span");
        span.textContent = data[i].Description + ": " + data[i].Weight + "%";
        GradingWeightArea.appendChild(span);
    }
}

function CreatePointSection(data, DomId) {

    let InsertedDiv = document.getElementById(DomId);
    let i, p, t, a, li
    for (let x in data) {
        if (data[x].Subtitle != "") {
            i = document.createElement("i");
            i.textContent = data[x].Subtitle;
            InsertedDiv.appendChild(i);
        }

        if (data[x].Description != "") {
            p = document.createElement("p");
            p.textContent = data[x].Description;
            InsertedDiv.appendChild(p);
        }

        if (data[x].Points != null) {
            let ul = document.createElement("ul");
            for (let j = 0; data[x].Points.length > j; j++) {
                li = document.createElement("li");
                t = document.createTextNode(data[x].Points[j].Description);
                li.appendChild(t);
                ul.appendChild(li);
            }
            InsertedDiv.appendChild(ul);
        }

        if (data[x].Links != null) {
            let div = document.createElement("div");
            div.style.marginTop = "2rem"

            for (let j = 0; data[x].Links.length > j; j++) {
                let span = document.createElement("span");
                span.style.display = "block";
                span.style.marginBottom = "10px"

                a = NetiquetteLink(data[x].Links[j].Title, data[x].Links[j].AddressLink);
                span.appendChild(a);

                t = document.createTextNode(" - " + data[x].Links[j].Description);
                span.appendChild(t);
                div.appendChild(span);
            }
            InsertedDiv.appendChild(div);
        }
    }
}
function NetiquetteLink(title, link) {
    let a = document.createElement("a")
    a.textContent = title;
    a.setAttribute('href', link);
    a.setAttribute('target', "_blank");
    return a;
}
