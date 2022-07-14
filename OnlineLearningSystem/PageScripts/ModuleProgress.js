function getModule(index,d)
{
    document.getElementById(index + "details").innerHTML = d.ModuleObjectives;
    document.getElementById(index + "description").innerHTML = d.Description;
    document.getElementById(index+"grade").innerHTML = "Grade: "+d.Percent + "% ( GPA " + d.GPA.toFixed(1)+")"; 
    document.getElementById(index+"completion").innerHTML = d.Completion + "%";
    document.getElementById(index + "strokeDashArray").setAttribute("aria-valuenow", d.Completion);
    document.getElementById(index + "strokeDashArray").style.width =  d.Completion + "%";
    document.getElementById(index+"dueDate").innerHTML = "Due: " + d.DueDate; 
}

function chooseModule(courseObjIndex, moduleIndex) {

    Navigate("ModulePage.html", "moduleId", courseObjectives[courseObjIndex].Modules[moduleIndex].ModuleId);
    return false;
}
