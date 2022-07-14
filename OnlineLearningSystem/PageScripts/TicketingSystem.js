var SupportTicketList;
var Open = true;
var CourseList;
var SupportTicketList;

function getSupportTicket() {
    loadPage();
}

function loadPage() {
    //----------------Loder Spiner----------------------------
    document.getElementById("loader-spinner").style.display = "block";
    //---------------------------------------------------------
    document.getElementById("fileUploadImage").addEventListener("change", readFile);
    //---------------------------------------------------------
    var openStatus = document.getElementById("rdOpenStatus").checked;
    var closeStatus = document.getElementById("rdCloseStatus").checked;
    const data = {
        CourseInstanceId: GetFromQueryString("courseInstanceId"),
        Opened: openStatus,
        Closed: closeStatus,
        Method: "GetList"
    };

    fetchFunction("TicketingSystem", data).then(d => {
       
        document.getElementById("CourseName").innerHTML = d.CourseName;

        SupportTicketList = d.SupportTicketList;
        createTable();
        //----------------Loder Spiner----------------------------
        document.getElementById("loader-spinner").style.display = "none";
    //---------------------------------------------------------
    });
   
}

function CheckStatus() {
    loadPage();
}

function submitSupportTicket() {
    //----------------Loder Spiner----------------------------
    document.getElementById("disabled-div").style.display = "block";
    //---------------------------------------------------------

    ClearMessage();
    var courseInstanceId = GetFromQueryString("courseInstanceId");
    fileBase64String = document.getElementById("ImageView").src;
    if (courseInstanceId !="") {
        var imgBase64String = "";
        if (fileBase64String != "") {

            if (isValidFileType()) {
                var substr = "base64,";
                var index = fileBase64String.indexOf(substr);
                if (index == -1) {
                    document.getElementById("lblOutputError").innerHTML = "Sorry! The File is not supported.";
                    document.getElementById("disabled-div").style.display = "none";
                    return false;
                }
                imgBase64String = fileBase64String.substring(index + substr.length);
            }
            else {
                document.getElementById("lblOutputError").innerHTML = "The File extension is not supported. <br/> Please submit a JPG or PNG file.";
                 document.getElementById("disabled-div").style.display = "none";
                return false;
            }
        }

        const data = {
            CourseInstanceId: courseInstanceId,
            Title: document.getElementById("txtSubject").value.trim(),
            Message: document.getElementById("txtMessage").value.trim(),
            Photo: imgBase64String,
            Priority: document.getElementById("ddPriority").value,
            Opened: true,
            Closed: false,
            Method: "SaveNewTicket"
        };

        const txtComment = document.getElementById("txtMessage");
        const lblOutputSuccess = document.getElementById("lblOutputSuccess");
        const lblOutput = document.getElementById("lblOutputError");

        if (validationCheck()) {
            return fetchFunction("TicketingSystem", data).then(d => {
                clear();
                lblOutputSuccess.innerHTML = "Your submission is successful!";
                document.getElementById("disabled-div").style.display = "none";
                loadPage(courseInstanceId);
            });
        }

        //----------------Loder Spiner----------------------------
        document.getElementById("disabled-div").style.display = "none";
        //---------------------------------------------------------
    }
}

function validationCheck() {
    const lblOutput = document.getElementById("lblOutputError");
    var result = true;
    if (document.getElementById("txtMessage").value.trim() == "") {
        lblOutput.innerHTML = "Sorry! The Support Request Detail field cannot be left blank.";
        result = false;
    }
    if (document.getElementById("txtSubject").value.trim() == "") {
        lblOutput.innerHTML = "Sorry! The Subject field cannot be left blank.";
        result = false;
    }
    return result;
}

function clear() {
    document.getElementById("txtMessage").value = "";
    document.getElementById("txtSubject").value = "";
    document.getElementById("fileUploadImage").value = "";
    document.getElementById("ddPriority").value = "low";
    document.getElementById("ImageView").src = "";
}

String.prototype.trim = function () {
    return this.replace(/^\s+|\s+$/g, "");
}

function createTable() {
    $("table").children().remove(); //clear table
    var table = document.getElementById("SupportTicketList");
    var row = table.insertRow(0);
    row.style.fontWeight = '600';
    row.style.textAlign = "center";
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    var cell3 = row.insertCell(2);
    var cell4 = row.insertCell(3);
    var cell5 = row.insertCell(4);
    cell1.innerHTML = "Token No";
    cell2.innerHTML = "Subject";
    cell3.innerHTML = "Status";
    cell4.innerHTML = "Unread Message";
    cell5.innerHTML = "Action";

    var index = 1;
    for (var i = 0; i < SupportTicketList.length; i++) {
        addRow(index, SupportTicketList[i].Id, SupportTicketList[i].TokenNo, SupportTicketList[i].Title, SupportTicketList[i].Status, SupportTicketList[i].UnreadMessage);
        index++;
    }
}

function addRow(index, Id, TokenNo, Title, Status, UnreadMessage) {
    var table = document.getElementById("SupportTicketList");
    var row = table.insertRow(index);
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    var cell3 = row.insertCell(2);
    var cell4 = row.insertCell(3);
    var cell5 = row.insertCell(4);
    cell1.innerHTML = TokenNo;
    cell2.innerHTML = Title;
    cell3.innerHTML = Status;
    cell4.innerHTML = UnreadMessage;
    cell4.style.textAlign  = "center";
    if (UnreadMessage > 0) {
        cell4.style.background = "#c8ebf5";
    }
    var a = newLink(Id)
    cell5.appendChild(a);
}

var fileBase64String = "";

function readFile() {
    document.getElementById("lblOutputError").innerHTML = "";
    document.getElementById("ImageView").src = "";

    if (this.files && this.files[0]) {
        if (isValidFileType()) {
            var FR = new FileReader();
            FR.addEventListener("load", function (e) {
                fileBase64String = e.target.result;
                var substr = "base64,";
                var index = fileBase64String.indexOf(substr);
                if (index == -1) {
                    document.getElementById("lblOutputError").innerHTML = "Sorry! The File is not supported.";
                    this.value = null;
                    return false;
                }
                document.getElementById("ImageView").src = e.target.result;
            });

            FR.readAsDataURL(this.files[0]);
            var img = document.getElementById('ImageView');
            var width = img.clientWidth;
            var height = img.clientHeight;
        }
        else {
            document.getElementById("lblOutputError").innerHTML = "The File extension is not supported. <br/> Please submit a JPG or PNG file.";
            this.value = null;
        }
    }
    else {
        var fileBase64String = "";
    }
}

function newLink(data) {
    var a = document.createElement("a")
    a.textContent = "Detail";
    a.setAttribute('onclick', 'SupportTicketDetail(' + data + ')');
    a.setAttribute('href', '#');
    return a;
}

function SupportTicketDetail(id) {
    Navigate("SupportTicketDetail.html", "SupportTicketId", id);
}
//=================================Support Ticket Message============================
var messageList;

function loadMessagePage() {
    //----------------Loder Spiner----------------------------
    document.getElementById("loader-spinner").style.display = "block";
    //---------------------------------------------------------
    document.getElementById("fileUploadImage").addEventListener("change", readFile);
    //---------------------------------------------------------
    const data = {
        CourseInstanceId: GetFromQueryString("courseInstanceId"),
        SupportTicketId: GetFromQueryString("SupportTicketId"),
        Method: "GetMessage"
    };

    fetchFunction("TicketingSystem", data).then(d => {

        document.getElementById("CourseName").innerHTML = d.CourseName;
        document.getElementById("Subject").innerHTML = d.Title;
        document.getElementById("TokenNo").innerHTML = d.TokenNo;
        document.getElementById("Priority").innerHTML = d.Priority;
        document.getElementById("Status").innerHTML = d.Status;
        if (d.Status=="Open") {
            document.getElementById("TextMessagingArea").style.display = "";
            document.getElementById("btnCloseTicket").style.display = "";
        }
        messageList = d.SupportTicketMessageList;
        //------------------------------------------------------------------------
        fetch("SupportTicketDetailControl.html").then(rs => rs.text()).then(d => {
            loadMessageList(d);
        });
       
        //----------------Loder Spiner----------------------------
        document.getElementById("disabled-div").style.display = "none";
        document.getElementById("loader-spinner").style.display = "none";
        //---------------------------------------------------------
    });
}

function loadMessageList(control) {
    for (var i = 0; i < messageList.length; i++) {
        var message = messageList[i];
        var id = message.Id;

        document.getElementById("pnlMessageList").innerHTML += control.replace(/x_/g, id);
        document.getElementById(id + "personName").textContent = message.PersonName;
        document.getElementById(id + "support-ticket-message").textContent = message.Message;
        if (message.ContentImage == null || message.ContentImage == "data:image;base64,") {
            document.getElementById(id + "support-ticket-img").style.display ="none" ;
        } else {
            document.getElementById(id + "support-ticket-img").src = message.ContentImage;
        }

        document.getElementById(id + "personImage").src = (message.PersonImage==null) ? "Content/images/photo.jpg" : message.PersonImage;

    }
}

function submitSupportTicketMessage() {
    //----------------Loder Spiner----------------------------
    document.getElementById("disabled-div").style.display = "block";
    //---------------------------------------------------------
    ClearMessage();
    fileBase64String = document.getElementById("ImageView").src;
    var imgBase64String = "";
    if (fileBase64String != "") {
        if (isValidFileType()) {
            var substr = "base64,";
            var index = fileBase64String.indexOf(substr);
            if (index == -1) {
                document.getElementById("lblOutputError").innerHTML = "Sorry! The File is not supported.";
                document.getElementById("disabled-div").style.display = "none";
                return false;
            }
            imgBase64String = fileBase64String.substring(index + substr.length);
        }
        else {
            document.getElementById("lblOutputError").innerHTML = "The File extension is not supported. <br/> Please submit a JPG or PNG file.";
            document.getElementById("disabled-div").style.display = "none";
            return false;
        }
    }

    const data = {
        CourseInstanceId: GetFromQueryString("courseInstanceId"),
        SupportTicketId: GetFromQueryString("SupportTicketId"),
        Message: document.getElementById("txtMessage").value.trim(),
        Photo: imgBase64String,
        Method: "SaveMessage"
    };

    const txtComment = document.getElementById("txtMessage");
    const lblOutputSuccess = document.getElementById("lblOutputSuccess");
    const lblOutputError = document.getElementById("lblOutputError");

    if (txtComment.value == undefined || txtComment.value.trim() == "") {
        lblOutputError.innerHTML = "Sorry! The Comment field cannot be left blank.";
        //----------------Loder Spiner----------------------------
        document.getElementById("disabled-div").style.display = "none";
        return false;
    }

    return fetchFunction("TicketingSystem", data).then(d => {
        lblOutputSuccess.innerHTML = "Your submission is successful!";
        document.getElementById("txtMessage").value = "";
        document.getElementById("fileUploadImage").value = "";
        document.getElementById("ImageView").src = "";
        document.getElementById("pnlMessageList").innerHTML = "";
        loadMessagePage();
    });
}

function ClossSupportTicket() {
    //----------------Loder Spiner----------------------------
    document.getElementById("disabled-div").style.display = "block";
    //---------------------------------------------------------
    ClearMessage();
    const data = {
        CourseInstanceId: GetFromQueryString("courseInstanceId"),
        SupportTicketId: GetFromQueryString("SupportTicketId"),
        Method: "CloseTicket"
    };
    const lblOutput = document.getElementById("lblOutputSuccess");

    return fetchFunction("TicketingSystem", data).then(d => {
        lblOutput.innerHTML = "Your ticket is closed successfully!";
        document.getElementById("TextMessagingArea").style.display = "none";
        document.getElementById("btnCloseTicket").style.display = "none";
        document.getElementById("Status").innerHTML = "Closed";

        //----------------Loder Spiner----------------------------
        document.getElementById("disabled-div").style.display = "none";
        //---------------------------------------------------------
    });
}


function ClearMessage() {
    document.getElementById("lblOutputError").innerText = "";
    document.getElementById("lblOutputSuccess").innerText = "";
}

function clearTable() {
    $("table").children().remove();
    document.getElementById("CourseName").innerHTML = "";
}

function isValidFileType() {
        const fileType = document.getElementById("fileUploadImage").files[0].type;
        var fileName = document.getElementById("fileUploadImage").files[0].name;
        var index = fileName.split(".").length - 1
        var type2 = fileName.split(".")[index];

        return (fileType == "image/jpeg" || fileType == "image/png" || type2 == "jpg" || type2 == "png");
}

document.getElementById('a_support').href = "SupportTicket.html?courseInstanceId=" + GetFromQueryString("courseInstanceId"); 

if (GetUrl("Feedback.html")) {
    //var input = document.getElementById('a_feedback').href;
    //var inputVal = "";
    //if (input) {
    //    inputVal = input.value;

    //    alert("sucess" + inputVal);
    //}
    //else {
    //    alert(input)
    //}

    document.getElementById('a_feedback').href = GetUrl("Feedback.html");
}



