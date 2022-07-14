
var feedbackList;
var onlyMyFeedbacks = false;
var CourseList;
function submitFeedback() {
	//----------------Loder Spiner--------------------------------------
    document.getElementById("disabled-div").style.display = "block";
    //------------------------------------------------------------------
    ClearMessage();

	const data = {
        CourseInstanceId: GetFromQueryString("courseInstanceId"),
        Feedback: document.getElementById("txtComment").value,
        Method: "Save"
	};

	const txtComment = document.getElementById("txtComment");
    const lblOutputSuccess = document.getElementById("lblOutputSuccess");
    const lblOutputError = document.getElementById("lblOutputError");

	if (txtComment.value == undefined || txtComment.value.trim() == "") {
        lblOutputError.innerHTML = "Sorry! The Comment field cannot be left blank.";
        //----------------Loder Spiner----------------------------
        document.getElementById("disabled-div").style.display = "none";
        return false;
	}

    return fetchFunction("Feedback", data).then(d => {
        if (d == 1) {
            lblOutputSuccess.innerHTML = "Thank you for your feedback.";
            loadFeedbackList();
        } else {
            lblOutputError.innerHTML = "Sorry! The feedback submission failed.";
        }
        txtComment.value = "";
		//----------------Loder Spiner----------------------------
        document.getElementById("disabled-div").style.display = "none";
        //---------------------------------------------------------
	});
}

String.prototype.trim = function () {
	return this.replace(/^\s+|\s+$/g, "");
}

function loadFeedbackList( )
{
    //----------------Loder Spiner--------------------------------------
    document.getElementById("loader-spinner").style.display = "block";
    //------------------------------------------------------------------
    const data = {
        CourseInstanceId: GetFromQueryString("courseInstanceId"),
        Method : "GetList"
    };

    fetchFunction("Feedback", data).then(d => {
        document.getElementById("CourseName").innerHTML = d.CourseName;
        feedbackList = d.FeedbackList;
        createTable();
        //----------------Loder Spiner----------------------------
        document.getElementById("loader-spinner").style.display = "none";
    });
}

function createTable()
{
    $("table").children().remove(); //clear table
    var table = document.getElementById("feedbackList");

    var row = table.insertRow(0);
    row.style.fontWeight = '600';
    row.style.textAlign = "center";
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    var cell3 = row.insertCell(2);
    cell1.innerHTML = "Description";
    cell2.innerHTML = "Status";
    cell3.innerHTML = "Comment";

    var index = 1;
    for (var i = 0; i < feedbackList.length; i++) {
        if (!onlyMyFeedbacks || (onlyMyFeedbacks && feedbackList[i].IsMine))
        {
            addRow(index, feedbackList[i].Description, feedbackList[i].Status, feedbackList[i].Comment);
            index++;
        }
    }
}

function addRow(index, description, status, comment)
{
    var table = document.getElementById("feedbackList");
    var row = table.insertRow(index);
    var cell1 = row.insertCell(0);
    cell1.classList.add("feedbackDes");
    var cell2 = row.insertCell(1);
    var cell3 = row.insertCell(2);
    cell3.classList.add("feedbackComm");
    cell1.innerHTML = description;
    cell2.innerHTML = status;
    cell3.innerHTML = comment;
}

function filterStudent(theForm)
{
    onlyMyFeedbacks = theForm.checked;
    createTable();
    ClearMessage();
} 

loadFeedbackList();
function ClearMessage() {
    document.getElementById("lblOutputError").innerText = "";
    document.getElementById("lblOutputSuccess").innerText = "";
}

document.getElementById('a_support').href = GetUrl("SupportTicket.html");