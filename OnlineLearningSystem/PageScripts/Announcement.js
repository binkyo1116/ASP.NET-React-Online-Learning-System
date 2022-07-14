function showAnnouncements() {
    fetch("AnnouncementControl.html").then(rs => rs.text()).then(d => {
        loadAnnouncements(d);
    });
}

showAnnouncements();

function loadAnnouncements(control)
{
    const data = {
        CourseInstanceId: GetFromQueryString("courseInstanceId")
    };

    fetchFunction("CourseAnnouncement", data).then(d => {

        for (var i = 0; i < d.length; i++) {
            var announcement = d[i];

            document.getElementById("pnlAnnouncement").innerHTML += control.replace(/x_/g, i);
            document.getElementById(i + "Name").textContent = announcement.Name;
            document.getElementById(i + "Date").textContent = announcement.Date;
            document.getElementById(i + "Title").textContent = announcement.Title;
            document.getElementById(i + "Description").textContent = announcement.Description;
            document.getElementById(i + "Image").src = (announcement.Photo == "") ? "Content/images/photo.jpg" : announcement.Photo;
        }

    });
}