var today = new Date();
var currentMonth = today.getMonth();
var currentYear = today.getFullYear();
var currentDay = today.getDate();
var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
var monthAndYear = document.getElementById("monthAndYear");

var startDate = today;
var endDate = today;
var dates = [];
var onlyUncompleted = true;
var courses = [];
var chosenCourses = [];
var activityTypes = [];

var colors = ['#00b0f6','#09b29f','#f4ac2b','#d63549','black'];

function getCalendar()
{
    const data = {
        CourseInstanceId: GetFromQueryString("courseInstanceId")
    };
    //console.log(data);
    fetchFunction("Calendar", data).then(d => {
        
        var sd = new Date(Date.parse(d.StartDate));
        var ed = new Date(Date.parse(d.EndDate));

        startDate = new Date(sd.getFullYear(), sd.getMonth());
        endDate = new Date(ed.getFullYear(), ed.getMonth());
        dates = d.Dates;
        courses = d.Courses;
        
        activityTypes = d.ActivityTypes;

        createCourseFilter();
        createActivityFilter();
        ShowCalendar();

        //----------------Loder Spiner----------------------------
        document.getElementById("loader-spinner").style.display = "none";
        //---------------------------------------------------------


    });

}

function createCourseFilter()
{
    var courseFilter = GetFromQueryString("courseInstanceId");
    if (courses) {
        for (let i = 0; i < courses.length; i++) {
            if (courses[i].CourseInstanceId == courseFilter) {
                chosenCourses.push({ CourseInstanceId: courses[i].CourseInstanceId, CourseName: courses[i].CourseName });
            }
        }
        if (chosenCourses.length == 0) chosenCourses = courses.slice(0);

        var div = document.getElementById("courseFilter");
        for (let i = 0; i < courses.length; i++) {
            var courseDiv = createCourseDiv(courses[i].CourseName, courses[i].CourseInstanceId, i);
            div.appendChild(courseDiv);
        }
    }

}

function createActivityFilter() {
    var div = document.getElementById("activityFilter");
    if (activityTypes) {
        for (let i = 0; i < activityTypes.length; i++) {
            var typeDiv = createTypeDiv(activityTypes[i]);
            div.appendChild(typeDiv);
        }
    }

}

function createCourseDiv(courseName, courseInstanceId, index)
{
    let courseDiv = document.createElement("div");
    courseDiv.className = "block";

    var checkbox = document.createElement('input');
    checkbox.type = "checkbox";
    checkbox.checked = courseChecked(courseInstanceId);
    checkbox.id ="C"+ courseInstanceId+"_courseCheckbox";
    checkbox.onclick = function () { checkCourse(courseInstanceId, courseName,this); };
    //--------------------------
    var color = getCourseColor(courseInstanceId);
    document.styleSheets[0].insertRule('#' + "C" + courseInstanceId + "_courseCheckbox" + ':before { background: ' + color + '; }', 0);
    //--------------------------
    var label = document.createElement('label')
    label.htmlFor = courseInstanceId + "_courseCheckbox";
    label.style.color = colors[index % colors.length];
    label.appendChild(document.createTextNode(courseName));

    courseDiv.append(checkbox);
    courseDiv.append(label);
    return courseDiv;
}

function createTypeDiv(type) {
    let typeDiv = document.createElement("div");
    typeDiv.className = "block";

    var checkbox = document.createElement('input');
    checkbox.type = "checkbox";
    checkbox.checked = true;
    checkbox.id = type + "_typeCheckbox";
    checkbox.onclick = function () { checkType(type); };
    
    var label = document.createElement('label')
    label.htmlFor = type + "_typeCheckbox";
    label.appendChild(document.createTextNode(type));

    typeDiv.append(checkbox);
    typeDiv.append(label);
    return typeDiv;
}

function checkCompletion()
{
    onlyUncompleted = !onlyUncompleted;
    ShowCalendar();
}

function checkCourse(courseInstanceId, courseName, me)
{
    //--------------by sohel-------------
    var color = getCourseColor(courseInstanceId);
    document.styleSheets[0].insertRule('#' + me.id + ':before { background: ' + color +'; }', 0);
    //-----------------------------------
    var index = -1;
    for (let i = 0; i < chosenCourses.length; i++)
    {
        if (chosenCourses[i].CourseInstanceId == courseInstanceId) {
            index = i;
            break;
        }
    }
    if (index == -1) {
        chosenCourses.push({ CourseInstanceId: courseInstanceId, CourseName: courseName });
    } else
    {
        chosenCourses.splice(index, 1);
    }
    ShowCalendar();
}

function checkType(type) {
    var index = -1;
    for (let i = 0; i < activityTypes.length; i++) {
        if (activityTypes[i] == type) {
            index = i;
            break;
        }
    }
    if (index == -1) {
        activityTypes.push(type);
    } else {
        activityTypes.splice(index, 1);
    }
    ShowCalendar();
}


function ShowCalendar()
{
    var firstDay = new Date( currentYear, currentMonth).getDay(); //first day of the month (day of the week)
    var daysInMonth = 32 - new Date( currentYear, currentMonth, 32).getDate();

    var tbl = document.getElementById("calendar-body");
    tbl.innerHTML = "";

    monthAndYear.innerHTML = months[currentMonth] + " " +  currentYear;

    var date = 1;
    for (let i = 0; i < 6; i++) {
        let row = document.createElement('tr');

        for (let j = 0; j < 7; j++) {

            var cell = document.createElement('td');
            var container = document.createElement("div");
            container.style.minHeight = '150px';
            container.style.minWidth = '150px';
            var blockDiv = document.createElement("div");
            blockDiv.className = "block";
            var textSpan = document.createElement("span");
            var dateText = ((i === 0 && j < firstDay) || (date > daysInMonth)) ? "" : date;
            textSpan.append(dateText);
            blockDiv.append(textSpan);
            container.append(blockDiv);
            //---------------by Sohle--------
            var containerColaps = document.createElement("div");
            containerColaps.className = "collapse";
            containerColaps.id = "cellColaps" + date;

            var containerVisible = document.createElement("div");

            var colapsBtn = document.createElement("a");
            colapsBtn.style.display = "none";
            colapsBtn.className = "btn-calender-colap";
            colapsBtn.setAttribute("data-toggle", "collapse");
            colapsBtn.setAttribute("href", "#cellColaps"+date );
            colapsBtn.innerText = "...";
            //-------------------------------
            if (currentMonth == today.getMonth() && currentDay == date)
            {
                cell.style.backgroundColor = '#ededed';
            }

            if (i === 0 && j < firstDay) { //previous month
                cell.appendChild(container);
                row.appendChild(cell);
                continue;
            } else if (date > daysInMonth) { //next month
                cell.appendChild(container);
                row.appendChild(cell);
            } else { //this month

                var index = getIndex(date);
                if (dates) {
                    for (activity of dates) {
                        if (activity != null) {
                            activityDue = new Date(activity.DueDate);
                            if (activityDue.getFullYear() == currentYear &&
                                activityDue.getMonth() == currentMonth &&
                                activityDue.getDate() == (date)) {
                                getActivityElements(activity.Activities, date, containerVisible, containerColaps, colapsBtn, activity.DueDate);
                                //----------------by sohel------------
                                container.append(containerVisible);
                                container.append(containerColaps);
                                container.append(colapsBtn);
                                //-------------------------------------
                                cell.appendChild(container);
                                row.appendChild(cell);
                            }
                        }
                    }
                }

                //----------------by sohel------------
                container.append(containerVisible);
                container.append(containerColaps);
                container.append(colapsBtn);
                //-------------------------------------
                cell.appendChild(container);
                row.appendChild(cell);
            }

            date++;
        }

        tbl.appendChild(row);
        if (date > daysInMonth) break;
    }

}

function getActivityElements(data, day, cell, cellColaps, colapsBtn, dueDate) {
    fetch("CalendarActivityElement.html").then(rs => rs.text()).then(cntrl => {
        var collapsCount = 1;
        for (var j in data) {
            if (onlyUncompleted && (data[j].Completion == 100)) continue;
            if (!courseChecked(data[j].CourseInstanceId)) continue;
            if (!typeChecked(data[j].Type)) continue;

            var color = getCourseColor(data[j].CourseInstanceId);
            var itemId = data[j].Id;
            var itemType = data[j].Type;
            //------------By sohel-------------     
            var appendDiv;
            if (collapsCount > 3) {
                appendDiv = cellColaps;
                colapsBtn.style.display = "block";
            } else {
                appendDiv = cell;
            }
            collapsCount++;
            //----------------------------------
            appendDiv.innerHTML += cntrl.replace(/x_/g, day).replace(/y_/g, itemId).replace(/a_/g, itemType);
            var a = document.getElementById(day + "_" + itemId + itemType + "btnContinue");

            url = '';

            if (itemType == 'Material') {
                url = GetUrl('Material.html', 'materialId', itemId);
            }
            else if (itemType == 'Quiz') {
                url = GetUrl('QuizPage.html', 'questionSetId', itemId);
            }
            else if (itemType == 'Assessment') {
                url = GetUrl('Interaction.html', 'codingProblemId', itemId);
            }
            else if (itemType == 'Poll') {
                url = GetUrl('PollResponse.html', 'pollGroupId', itemId, "moduleObjectiveId", data[j].ModuleObjectiveId);
            }
            else if (itemType == 'Discussion') {
                url = GetUrl('DiscussionBoardPage.html', 'discussionBoardId', itemId, "moduleObjectiveId", data[j].ModuleObjectiveId);
            }
            else {
                throw 'Activity Not Supported: ' + area;
            }

            a.setAttribute('href', url);

            var mt = data[j].Title;
            if (mt.length > 15) {
                mt = data[j].Title.substring(0, 15) + "...";
            }
            document.getElementById(day + "_" + itemId + itemType + "Title").textContent = mt;
            document.getElementById(day + "_" + itemId + itemType + "Title").style.color = color;
            document.getElementById(day + "_" + itemId + itemType + "Container").style.borderColor = color;

            loadProgressData(data[j], day, itemId, itemType, dueDate);
           
        }
    });


}

function loadProgressData(data, day, itemId, itemType, dueDate) {

    document.getElementById(day + "_" + itemId + itemType + "ActivityType").textContent = data.Type;
    document.getElementById(day + "_" + itemId + itemType + "ActivityTitle").textContent = data.Title;
    document.getElementById(day + "_" + itemId + itemType + "DueDate").textContent = "Due: " + dueDate;
    document.getElementById(day + "_" + itemId + itemType + "Completion").textContent = data.Completion + "% COMPLETED";

}

function courseChecked(id)
{
    for (let i = 0; i < chosenCourses.length; i++) {
        if (chosenCourses[i].CourseInstanceId == id) {
            return true;
        }
    }
    return false;
}

function getCourseColor(id) {
    
    for (let i = 0; i < courses.length; i++) {
        if (courses[i].CourseInstanceId == id) {
            return colors[i % colors.length];
        }
    }
    return "black";
}

function typeChecked(type) {
    
    for (let i = 0; i < activityTypes.length; i++) {
        if (activityTypes[i] == type) {
            return true;
        }
    }
    return false;
}

function getIndex(day)
{
    var one_day = 1000 * 60 * 60 * 24;
    var date = new Date(currentYear, currentMonth, day);
    var result = Math.round(date - startDate) / (one_day);
    return result.toFixed(0);
}

function previous()
{
    
    var newCurrentYear = (currentMonth === 0) ? currentYear - 1 : currentYear;
    var newCurrentMonth = (currentMonth === 0) ? 11 : currentMonth - 1;

    var newDate = new Date(newCurrentYear, newCurrentMonth);
    if (newDate >= startDate)
    {
        currentYear = newCurrentYear;
        currentMonth = newCurrentMonth;
        ShowCalendar();
    }

}

function next()
{
    newCurrentYear = (currentMonth === 11) ? currentYear + 1 : currentYear;
    newCurrentMonth = (currentMonth === 11) ? 0 : currentMonth + 1;

    var newDate = new Date(newCurrentYear, newCurrentMonth);
    if (newDate <= endDate) {
        currentYear = newCurrentYear;
        currentMonth = newCurrentMonth;
        ShowCalendar();
    }
}

