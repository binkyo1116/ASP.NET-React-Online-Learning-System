

var pollQuestionIdList = [];
loadPage();


function loadPage() {
    fetch("PollResponseControl.html").then(rs => rs.text()).then(d => {
        loadPollResponsePosts(d);
    });
}

function loadPollResponsePosts(control) {
    const data = {
        CourseInstanceId: GetFromQueryString("courseInstanceId"),
        ModuleObjectiveId: GetFromQueryString("moduleObjectiveId"),
        PollGroupId: GetFromQueryString("pollGroupId"),
        Method: "Get"
    };

    fetchFunction("PollResponse", data).then(d => {

        const isResponsed = d.filter(x => x.PollAnswers.length > 0).length > 0 ? true : false;

        for (var i = 0; i < d.length; i++) {
            var poll = d[i];
            var id = poll.PollQuestionId;
            pollQuestionIdList.push({ id: id, isOption: poll.isOption });

            document.getElementById("pnlPollQuestions").innerHTML += control.replace(/_x/g, id).replace(/x_/g, id);
            document.getElementById(id + "lblPollQues").textContent = poll.PollQuestion;

            if (poll.isOption) {
                if (poll.PollAnswers.length > 0) {
                    //--------Print Answer--------------------------
                    PrintOptionAnswerTable(id, poll.PollAnswers, poll.PollOptions)
                }
                else {
                    if (isResponsed) {
                        //PrintOptionAnswerTable(id, poll.PollAnswers, poll.PollOptions)
                        document.getElementById(id + "PollOptionList").style.display = "None";
                    } else {
                        document.getElementById(id + "PollResponseTable").style.display = "none";
                        document.getElementById(id + "PollOptionList").style.display = "block";
                        //--------Print Dropdown List---------------
                        PrintOption(id, poll.PollOptions)
                    }
                   
                }
            }
            else {
                
                if (poll.PollAnswers.length > 0) {
                    document.getElementById(id + "txtPollResponse").style.display = "none";
                    //--------Print Answer--------------------------
                    PrintAnswerTable(id, poll.PollAnswers)
                }
                else {
                    if (isResponsed) {
                        document.getElementById(id + "txtPollResponse").style.display = "none";
                    } else {
                        document.getElementById(id + "txtPollResponse").style.display = "block";
                        document.getElementById(id + "PollResponseTable").style.display = "none";
                    }
                   
                }
            }
        }
        if (isResponsed) {
            document.getElementById("poll-submit-btn").style.display = "none";
        }
        else {
            document.getElementById("poll-submit-btn").style.display = "";
        }
        //----------------Loder Spiner----------------------------
        document.getElementById("loader-spinner").style.display = "none";
        //---------------------------------------------------------
    });

}

function SubmitAnswer() {

    //----------------Disable page----------------------------
    document.getElementById("disabled-div").style.display = "block";
    //---------------------------------------------------------
    pollResponseData = [];

    for (var i in pollQuestionIdList) {
        var id = pollQuestionIdList[i].id;

        if (pollQuestionIdList[i].isOption) {
            var ans = document.getElementById(id + "PollOptionList").value;
            if (ans != "" & ans != "-") {

                pollResponseData.push({ PollQuestionId: id, OptionId: ans, TextAnswer: "" });
            } else if (document.getElementById(id + "PollOptionList").style.display != "none")
            {
                if (document.getElementById(id + "PollOptionList").value != "-") {
                //alert("You must answer all 'drop-down' questions!");
                document.getElementById("error-message").innerText = "Sorry! You must answer all 'drop-down' questions!";
                document.getElementById("disabled-div").style.display = "none";
                    return false;
                }
            }
        } else {
            var ans = document.getElementById(id + "txtPollResponse").value;
            if (ans != "") {
                pollResponseData.push({ PollQuestionId: id, OptionId: null, TextAnswer: ans });
            }
            //else {
            //    document.getElementById("error-message").textContent = "Sorry! You must answer all questions!"
            //    document.getElementById("disabled-div").style.display = "none";
            //    return false;
            //}
        }

    }
    if (pollResponseData.length != 0) {
        document.getElementById("disabled-div").style.display = "none";
        document.getElementById("loader-spinner").style.display = "";
        PostSubmitedData(pollResponseData)
    }
    else {
        //document.getElementById("error-message").textContent = "Sorry! Your response cannot be empty."
        //----------------Disable page----------------------------
        document.getElementById("disabled-div").style.display = "none";
        //---------------------------------------------------------
    }


}

function PrintAnswerTable(id, data) {
    for (var pn in data) {
        var pollAns = data[pn].Answer;
        var tr = document.createElement("tr");
        var td = document.createElement("td");
        var t = document.createTextNode(pollAns);
        td.appendChild(t);

        tr.appendChild(td);
        document.getElementById(id + "PollResponseTbody").appendChild(tr);
    }
}

function PrintOptionAnswerTable(id, answer, option) {
    var pollAns = answer[0];
    var ansOption = option.find(x => x.PollOptionId == pollAns.PollOptionId);
    var result = ansOption.Identity + ". " + ansOption.Title;

    var tr = document.createElement("tr");
    var td = document.createElement("td");
    var t = document.createTextNode(result);
    td.appendChild(t);

    tr.appendChild(td);
    document.getElementById(id + "PollResponseTbody").appendChild(tr);
}

function PrintOption(id, optionData) {
    var dropDownList = document.getElementById(id + "PollOptionList");
    for (var op in optionData) {
        var option = document.createElement("option");
        option.text = optionData[op].Identity + ". " + optionData[op].Title;
        option.value = optionData[op].PollOptionId;
        dropDownList.add(option);
    }
    //----------No Answer --------------------
    var option = document.createElement("option");
    option.text = "No Answer";
    option.value = "-";
    dropDownList.add(option);
}
function PostSubmitedData(pollResponseData) {
    const data = {
        CourseInstanceId: GetFromQueryString("courseInstanceId"),
        ModuleObjectiveId: GetFromQueryString("moduleObjectiveId"),
        PollGroupId: GetFromQueryString("pollGroupId"),
        StudentResponses: pollResponseData,
        Method: "Add"
    };

    fetchFunction("PollResponse", data).then(d => {
        Navigate("PollResponse.html");
    });
}