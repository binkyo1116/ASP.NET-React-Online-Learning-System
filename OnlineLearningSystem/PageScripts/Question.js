function putChoice(index) {
    submitCount += 1

    const select = document.getElementById(index + "ddAnswer");
    const txtAnswer = document.getElementById(index + "txtAnswer");

    if (submitCount > 4) document.getElementById(index + "btnSubmit").style.display = "none";

    txtAnswer.value = select.options[select.selectedIndex].text;
    submitAnswer(index, select.options[select.selectedIndex].value);
}

function clearOptions(select) {
    var i;
    var length = select.options.length;

    for (i = length - 1; i >= 0; i--) {
        select.remove(i);
    }
}

function setDropDown(choices, grade, answerShown, index, answer) {

    const txtAnswer = document.getElementById(index + "txtAnswer");
    const ddAnswer = document.getElementById(index + "ddAnswer");
    clearOptions(ddAnswer);

    if (choices == null || choices.length == 0) {
        txtAnswer.style.display = "";
        ddAnswer.style.display = "none";
    }
    else {
        if (!answerShown && grade == 0) {
            txtAnswer.style.display = "none";
            ddAnswer.style.display = "";

            var select = document.getElementById(index + "ddAnswer");
            select.add(document.createElement('option'));
            for (let i = 0; i < choices.length; i++) {
                var option = document.createElement('option');
                option.text = choices[i].Option;
                option.value = choices[i].Id;
                if (answer == choices[i].Id) option.selected = true;
                select.add(option);
            }
        } else {
            setCorrectAnswer(index, answer, "Dropdown");
        }
    }
}

function setCorrectAnswer(index, answer, questionType) {
    var choices = questions[index].Options;
    if (choices == null || choices.length == 0) {
        return;
    }

    if (questionType == "Dropdown") {
        const txtAnswer = document.getElementById(index + "txtAnswer");
        for (let i = 0; i < choices.length; i++) {
            if (choices[i].Id == answer) {
                txtAnswer.textContent = choices[i].Option;
                break;
            }
        }
    } else if (questionType == "Checkbox" || questionType == "Radio") {
        var answers = (answer != null) ? answer.split(",") : [];
        var inputs = document.getElementById(index + 'multipleChoice').getElementsByTagName('INPUT');
        for (var i = 0; i < inputs.length; i++) {
            if (choiceChecked(answers, extractChoiceId(inputs[i].id))) {
                inputs[i].checked = true;
            } else {
                inputs[i].checked = false;
            }
        }
    }
}

function setCheckbox(choices, index, answer, inputType) {

    if (choices == null || choices.length == 0) {
        return;
    }
    else {
        var answers = (answer != null) ? answer.split(",") : [];
        var container = document.getElementById(index + 'multipleChoice');

        for (let i = 0; i < choices.length; i++) {
            var choiceId = choices[i].Id + 'choice'; //index + 'choice' + i;
            var checkbox = document.createElement('input');
            checkbox.type = inputType;
            checkbox.id = choiceId;
            checkbox.value = choiceId;
            if (inputType == 'radio') {
                checkbox.name = index + inputType;

            }
            if (choiceChecked(answers, choices[i].Id)) {
                checkbox.setAttribute("checked", "true");
            }

            var label = document.createElement('label')
            label.htmlFor = choiceId;
            label.innerHTML = choices[i].Option;

            var br = document.createElement('br');

            container.appendChild(checkbox);
            container.appendChild(label);
            container.appendChild(br);
        }

    }


}

function disableMultipleChoice(index, questionType) {
    if (questionType == "Checkbox" || questionType == "Radio") {
        var inputs = document.getElementById(index + 'multipleChoice').getElementsByTagName('INPUT');
        for (var i = 0; i < inputs.length; i++) {
            inputs[i].setAttribute("disabled", "disabled");
        }
    } else if (questionType == "Dropdown") {
        document.getElementById(index + "txtAnswer").style.display = "";
        document.getElementById(index + "txtAnswer").contentEditable = false;
        document.getElementById(index + "ddAnswer").style.display = "none";
    }
}


function choiceChecked(checked, choice) {
    for (let i = 0; i < checked.length; i++) {
        if (checked[i] == choice) return true;
    }
    return false;
}

function getAnswers(index) {
    var answers = [];
    var labels = document.getElementById(index + 'multipleChoice').getElementsByTagName('LABEL');
    for (var i = 0; i < labels.length; i++) {
        if (labels[i].htmlFor != '') {
            var choiceId = labels[i].htmlFor;
            var input = document.getElementById(choiceId);
            if (input.checked) {
                answers.push(extractChoiceId(choiceId));
            }
        }
    }

    return answers.join();
}

function extractChoiceId(choiceId) {
    var end = choiceId.indexOf("choice");
    if (end != -1)
        return choiceId.substring(0, end);
    else
        return -1;
}


function clickSubmit(index) {
    submitAnswer(index, questions[currentQuestion].ExpectedAnswer);
}

function getData(controller, data, index) {
    data.QuestionId = questions[index].Id;

    data.StudentId = localStorage.getItem("Hash");
    return fetchFunction(controller, data);
}

var g_ExpectedAnswer;
function getQuestion(index) {
    var d = questions[index];
    var appendage = index;
    currentQuestion = index;

    var answered = d.Answer == d.ExpectedAnswer;

    answerHistory[index] = ""; //initialize the history for this question

    if (window.location.href.includes("MaterialPage")) appendage = 0;

    document.getElementById(appendage + "lblPrompt1").textContent = d.Prompt1;
    document.getElementById(appendage + "lblPrompt2").textContent = d.Prompt2;

    if (d.Images != null)
        document.getElementById(appendage + "questionImage").innerHTML = d.Images;
    else
        document.getElementById(appendage + "questionImage").style.display = 'none';

    var element = document.getElementById(appendage + "textbox");


    if (d.EmbedAction) {
        element.style.position = "absolute";
        assignCSS(d, appendage);

        if (d.Type == "Click") {
            element = document.getElementById("elementArea");
        }
        else if (d.Type == "Dropdown") {
            setDropDown(d.Options, d.Grade, d.AnswerShown, appendage, d.Answer);
            element = document.getElementById(appendage + "textbox");
        }
        else if (d.Type == "Textbox") {
            element = document.getElementById(appendage + "textbox");

            document.getElementById(appendage + "btnSubmit").style.display = "none";
            document.getElementById(appendage + "btnReveal").style.display = "none";

        }
    }
    else if (d.Type == "Dropdown") {
        setDropDown(d.Options, d.Grade, d.AnswerShown, appendage, d.Answer);
    } else if (d.Type == "Checkbox" || d.Type == "Radio") {
        document.getElementById(appendage + "multipleChoice").style.display = "";
        setCheckbox(d.Options, appendage, d.Answer, d.Type.toLowerCase());
        document.getElementById(appendage + "txtAnswer").style.display = "none";
        document.getElementById(appendage + "ddAnswer").style.display = "none";
        document.getElementById(appendage + "btnSubmit").style.display = "";
    }
    else if (d.Type == "TextBox") {
        document.getElementById(appendage + "btnSubmit").style.display = "none";
        document.getElementById(appendage + "btnReveal").style.display = "none";
    }
    showResult(d.Grade, d.AnswerShown, d.Answer, appendage, d.MaxGrade, d.IsMultipleChoice, d.Type, "");
    setRating(d.QuestionRating, appendage);

    if (d.VideoSource != null)
        videoSetup(appendage, d.VideoTimestamp, d.VideoSource, element, d.EmbedAction, answered);

    if (d.PositionX != -1) {
        element.style.left = d.PositionX + "px";
    }
    if (d.PositionY != -1) {
        element.style.top = d.PositionY + "px";
    }
    if (d.Height != -1) {
        element.style.height = d.Height + "px";
    }
    if (d.Width != -1) {
        element.style.width = d.Width + "px";
    }

    let hint = questionHints[d.Id];
    if (hint) {
        let lblHintTeacher = document.getElementById(appendage + "lblHintTeacher");

        lblHintTeacher.innerHTML = "<b>Hint:</b> " + hint;
    }

    return false;
}

function assignCSS(data, appendage) {
    if (data.Color != undefined) {
        document.getElementById(appendage + "txtAnswer").style.color = data.Color;
    }
    if (data.FontFamily != undefined) {
        document.getElementById(appendage + "txtAnswer").style.fontFamily = data.FontFamily;
    }
    if (data.FontSize != undefined) {
        document.getElementById(appendage + "txtAnswer").style.fontSize = data.FontSize;
    }
    if (data.BackgroundColor != undefined) {
        document.getElementById(appendage + "txtAnswer").style.backgroundColor = data.BackgroundColor;
    }
    if (data.Border != undefined) {
        document.getElementById(appendage + "txtAnswer").style.border = data.Border;
    }
    if (data.PaddingLeft != undefined) {
        document.getElementById(appendage + "txtAnswer").style.paddingLeft = data.PaddingLeft;
    }
    if (data.PaddingRight != undefined) {
        document.getElementById(appendage + "txtAnswer").style.paddingRight = data.PaddingRight;
    }

    document.getElementById(appendage + "txtAnswer").className = "";
}

function revealClick(index) {
    if (window.location.href.includes("Quiz")) {
        currentQuestion = index;
    }
    //document.getElementById(index + "disabled-div").style.display = "block";
    document.getElementById("disabled-div").style.display = "block";


    if (!confirm("Revealing the answer will deduct points from your grade.  Are you sure you wish to reveal the answer?")) {
        document.getElementById("disabled-div").style.display = "none";
        return false;
    }

    const data = {
        History: answerHistory[currentQuestion]
    };

    answerHistory[currentQuestion] = "";

    getData("Reveal", data, currentQuestion).then(d => {
        questions[currentQuestion].AnswerShown = true;
        questions[currentQuestion].Answer = d.Answer;
        questions[currentQuestion].Grade = 0;
        var questionType = questions[currentQuestion].Type;
        setCorrectAnswer(currentQuestion, d.Answer, questionType);
        showResult(0, true, d.Answer, index, d.MaxGrade, questions[currentQuestion].IsMultipleChoice, questionType, "");
        const txtGrade = document.getElementById("txtGrade");
        const txtRevealed = document.getElementById("txtRevealed");

        txtGrade.textContent = d.TotalGrade + "%";
        txtRevealed.textContent = d.TotalShown + "%";

        if (window.location.href.includes("Material")) {
            document.getElementById(index + "textbox").style.display = "none";
            nextStep(index);
        }

        document.getElementById("disabled-div").style.display = "none";

    });

    hideHint(index);
    return false;
}

function submitAnswer(index, answer) {
    if (window.location.href.includes("Quiz")) {
        //		document.getElementById(index +"disabled-div").style.display = "block";
        currentQuestion = index;
    }
    //document.getElementById(index + "disabled-div").style.display = "block";
    document.getElementById("disabled-div").style.display = "block";

    if (answer == undefined) {

        if (document.getElementById(index + "multipleChoice").style.display == "none")
            answer = document.getElementById(index + "txtAnswer").innerHTML;
        else
            answer = getAnswers(index);
    }

    const data = {
        Answer: answer,
        Location: window.location.href,
        History: answerHistory[currentQuestion]
    };

    answerHistory[currentQuestion] = "";

    getData("Submit", data, currentQuestion).then(d => {

        questions[currentQuestion].Answer = answer;
        questions[currentQuestion].Grade = d.Grade;
        var questionType = questions[currentQuestion].Type;
        if (questionType == "Dropdown") setCorrectAnswer(currentQuestion, answer, questionType);
        showResult(d.Grade, false, answer, index, d.MaxGrade, questions[currentQuestion].IsMultipleChoice, questionType, d.Hint, d.HintId, d.HintRating);
        const txtGrade = document.getElementById("txtGrade");
        const txtRevealed = document.getElementById("txtRevealed");

        txtGrade.textContent = d.TotalGrade + "%";
        txtRevealed.textContent = d.TotalShown + "%";

        if (window.location.href.includes("Material") && d.Grade > 0) {
            document.getElementById(index + "textbox").style.display = "none";
            nextStep(index);
        }
        document.getElementById("disabled-div").style.display = "none";

        var lblHintTeacher = document.getElementById(index + "lblHintTeacher");
        if (d.Grade != d.MaxGrade) {
            lblHintTeacher.innerHTML += "<b> Your submission has been received.</b>";
        }
    });
    hideHint(index);

}

function sendRating(clientRating, index) {
    const data = {
        rating: clientRating,
    };

    getData("QuestionRating", data, index);

    setRating(clientRating, index);
    return false;
}

function setRating(value, index) {
    const thumbUp = document.getElementById(index + "RateUp");
    const thumbDown = document.getElementById(index + "RateDown");


    thumbDown.src = "../Content/images/ThumsDown.png"
    thumbUp.src = "../Content/images/ThumsUp.png";

    //thumbUp.disabled = value == 1;
    //thumbDown.disabled = value == -1;

    if (value == 1) {
        thumbUp.src = "../Content/images/ThumsUp_select.png";
        thumbUp.setAttribute('onclick', 'sendRating(0,' + index + ')');
        thumbDown.setAttribute('onclick', 'sendRating(-1,' + index + ')');
    }
    else if (value == -1) {
        thumbDown.src = "../Content/images/ThumsDown_select.png";
        thumbDown.setAttribute('onclick', 'sendRating(0,' + index + ')');
        thumbUp.setAttribute('onclick', 'sendRating(1,' + index + ')');
    }
    else if (value == 0) {
        thumbUp.setAttribute('onclick', 'sendRating(1,' + index + ')');
        thumbDown.setAttribute('onclick', 'sendRating(-1,' + index + ')');
    }
}

function sendRatingHint(clientRating, index) {
    const data = {
        rating: clientRating,
        hintId: questions[index].hintId
    };

    getData("HintRating", data, index);

    setRatingHint(clientRating, index);
    return false;
}

function setRatingHint(value, index) {
    const thumbUp = document.getElementById(index + "HintRateUp");
    const thumbDown = document.getElementById(index + "HintRateDown");

    thumbDown.src = "../Content/images/ThumsDown.png"
    thumbUp.src = "../Content/images/ThumsUp.png";

    thumbUp.disabled = value == 1;
    thumbDown.disabled = value == -1;

    if (value == 1) {
        thumbUp.src = "../Content/images/ThumsUp_select.png";
    }
    else if (value == -1) {
        thumbDown.src = "../Content/images/ThumsDown_select.png";
    }
}

function showResult(grade, answerShown, answer, index, maxGrade, isMultipleChoice, questionType, hint, hintId = 0, hintRating = 0) {
    var show = !(grade > 0 || answerShown);

    if (answer !== null && !isMultipleChoice) {
        document.getElementById(index + "txtAnswer").innerHTML = answer;
    }

    if (!isMultipleChoice) {
        document.getElementById(index + "txtAnswer").contentEditable = show;
        if (!show) {
            document.getElementById(index + "ddAnswer").style.display = "none";
            document.getElementById(index + "txtAnswer").style.display = "";
        }
    } else if (!show) {
        disableMultipleChoice(index, questionType);
    }

    var lblResult = document.getElementById(index + "lblResult");
    var lblHintTeacher = document.getElementById(index + "lblHintTeacher");
    if (window.location.href.includes("Material")) {
        lblResult.textContent = "Step " + (currentQuestion + 1) + "/" + questions.length + " (" + grade + " out of " + maxGrade + " points" + ((answerShown) ? " / Answer Revealed)" : ")");
    } else {
        lblResult.textContent = grade + " out of " + maxGrade + " points" + ((answerShown) ? " / Answer Revealed" : "");
        if (hint) {
            lblHintTeacher.innerHTML = "<b>Hint:</b> " + hint;

            questions[index].hintId = hintId;

            $("#" + index + "areaHint").removeClass('d-none');

            setRatingHint(hintRating, index);
        }
        else {
            lblHintTeacher.innerHTML = "";

            questions[index].hintId = 0;

            $("#" + index + "areaHint").addClass('d-none');
        }
    }

    if (!show) {
        document.getElementById(index + "btnSubmit").disabled = true;
        document.getElementById(index + "btnReveal").disabled = true;
    }

}


// ==========  Video Stuff ============
var TIMESTAMP;
function videoSetup(index, timestamp, source, element, embedded, answered) {
    TIMESTAMP = timestamp;


    document.getElementById(index + "videoPanel").style.display = "";

    if (player == null) {
        var Options = {
            url: source,
            controls: false,
            width: 1024
        };

        player = new Vimeo.Player(index + "videoPanel", Options);
    }

    if (embedded) {
        document.getElementById(index + "lblPrompt1").style.display = "none";
        document.getElementById(index + "lblPrompt2").style.display = "none";

        document.getElementById(index + "btnSubmit").style.display = "none";
        document.getElementById(index + "btnReveal").style.display = "none";
        element.style.display = "none";

        player.getCuePoints().then(function (cuePoints) {
            let cuePointExists = false;
            for (let i = 0; i < cuePoints.length; i++) {
                if (cuePoints[i].time == timestamp) {
                    cuePointExists = true;
                    break;
                }
            }
            if (!cuePointExists) {
                player.addCuePoint(timestamp, { customKey: timestamp });
                player.on('cuepoint', () => pauseVideo(event, element, timestamp, index, answered));
            }
        });

    }
    player.on('play', () => play(index, embedded));
}

function pauseVideo(event, element, timestamp, index, answered) {

    if (event.data.data.time == timestamp) {
        player.pause();
        element.style.display = "";
        answered = (questions[currentQuestion].Answer == questions[currentQuestion].ExpectedAnswer);
        if (answered) {
            setTimeout(play(index, true), 150);
            nextStep(index);
        }
        else {
            document.getElementById("btnPause1").disabled = true;
            document.getElementById(index + "lblPrompt1").style.display = "";
            document.getElementById(index + "lblPrompt2").style.display = "";
            document.getElementById("btnRepeat1").style.visibility = "visible";
            document.getElementById("btnReveal1").style.visibility = "visible";
        }

    }
}

function pause() {
    player.getPaused().then(function (paused) {
        if (!paused) {
            player.pause();
            document.getElementById("btnPause1").value = "Play";
        } else {
            player.play();
            document.getElementById("btnPause1").value = "Pause";
        }

    });
}

var player;
function play(index, embedded) {
    document.getElementById("playVideo").style.display = "none";
    document.getElementById("elementArea").style.display = "none";
    document.getElementById(index + "lblPrompt1").style.display = "none";
    document.getElementById(index + "lblPrompt2").style.display = "none";
    document.getElementById("btnPause1").disabled = false;
    document.getElementById("btnForward1").disabled = (currentQuestion >= questions.length - 1);
    document.getElementById("btnReset1").disabled = false;


    if (embedded)
        document.getElementById(index + "textbox").style.display = "none";
    player.play();
}

var submitCount = 0;
var shift = 0;
function rememberNavKey(index, event) {

    if (window.location.href.includes("Quiz")) {
        currentQuestion = index;
    }
    // Navigation keys: right arrow, left arrow, Delete, backspace
    // Codes in database: 
    // Backspace    BS
    // Delete       DL
    // Right arrow  RA
    // Left arrow   LA
    if ((event.keyCode == 46) || (event.keyCode == 8)) {

        //------------Delete & backspace-------------------
        submitCount += 1;

        if (submitCount >= 50) document.getElementById(index + "btnReveal").style.display = "";

        const txtbox = document.getElementById(index + "txtAnswer");

        var indexToRemove = 0;
        var keyCode = "";
        if (event.keyCode == 46) {
            if (shift == 0) {
                return;
            } else {
                indexToRemove = txtbox.textContent.length + shift;
                shift++;
            }
            keyCode = "DL";

        } else if (event.keyCode == 8) {
            if (txtbox.textContent.length == Math.abs(shift)) {
                return;
            } else {
                indexToRemove = txtbox.textContent.length + shift - 1;
            }
            keyCode = "BS";
        }

        const currentAnswer = txtbox.textContent.slice(0, indexToRemove) + txtbox.textContent.slice(indexToRemove + 1);
        answerHistory[currentQuestion] += keyCode + ",";
        currentAnswerList[currentQuestion] = currentAnswer;

    } else if (event.keyCode == 37) {
        //------------Left arrow-------------------
        var answer = document.getElementById(index + "txtAnswer").textContent;
        if (Math.abs(shift) < answer.length) {
            answerHistory[currentQuestion] += "LA,";
            shift--;
        }
    } else if (event.keyCode == 39) {
        //------------Right arrow-------------------
        var answer = document.getElementById(index + "txtAnswer").textContent;
        if (shift < 0) {
            answerHistory[currentQuestion] += "RA,";
            shift++;
        }
    }

}

function hideHint(index) {
    document.getElementById(index + "lblHint").style.visibility = 'hidden';
    shift = 0;
}

function rememberCharInput(index, event) {

    if (event.keyCode == 13) {
        event.preventDefault();
        return;
    }
    const txtbox = document.getElementById(index + "txtAnswer");

    if (txtbox.textContent.length > 30) {
        txtbox.textContent = txtbox.textContent.substring(31, 1);
        return false;
    }

    var char = event.which || event.keyCode;
    const currentAnswer = getCurrentAnswer(txtbox.textContent, char);

    answerHistory[currentQuestion] += char + ",";
    currentAnswerList[currentQuestion] = currentAnswer;
}

function checkAnswerOnInput(index) {
    if (!questions[currentQuestion].UsesHint) {
        const txtbox = document.getElementById(index + "txtAnswer");

        var currentAnswer = txtbox.textContent;
        var txtln = currentAnswer.length;
        if (txtln > 30) {
            currentAnswer = currentAnswer.substring(0, 31);
            txtbox.textContent = currentAnswer;
        }
        const expectedAnswer = questions[currentQuestion].ExpectedAnswer;

        var editing = evaluateAnswer(currentAnswer, expectedAnswer, questions[currentQuestion].CaseSens)

        document.getElementById(index + "lblHint").style.visibility = 'visible';
        document.getElementById(index + "lblHint").innerHTML = getHint(currentAnswer, editing);

        if (editing == "") {
            submitAnswer(index, currentAnswer);
        }

        if (txtbox.textContent != "") {
            document.getElementById(index + "btnSubmit").style.display = "inline-block";
            document.getElementById(index + "btnReveal").style.display = "inline-block";
        }
        else {
            document.getElementById(index + "btnSubmit").style.display = "none";
            document.getElementById(index + "btnReveal").style.display = "none";
        }
    }
}


function getCurrentAnswer(text, character) {
    if (ignoreCharacter(character)) {
        return text;
    } else {
        if (shift != 0) {
            var position = text.length + shift;
            var output = text.substring(0, position) + String.fromCharCode(character) + text.substring(position);
            return output;
        }
        else
            return text + String.fromCharCode(character);
    }
}

function ignoreCharacter(code) {
    return (code < 32 || code > 126);
}

function nextStep(index, clicked) {
    if (index == undefined) index = 0;

    currentQuestion += 1;

    hideHint(index);

    var videoEnded = currentQuestion == questions.length;
    document.getElementById("btnForward1").disabled = (currentQuestion >= questions.length - 1);
    document.getElementById("btnPrev1").disabled = false;
    document.getElementById("btnPause1").disabled = false;
    document.getElementById("btnRepeat1").style.visibility = "hidden";
    document.getElementById("btnReveal1").style.visibility = "hidden";

    if (videoEnded && localStorage.getItem("isDemo") == "true") {
        navigateDemo();
    }

    player.getPaused().then(function (paused) {
        if (paused) {
            play(index, true);
            document.getElementById("btnPause1").value = "Pause";
        }

        if (!videoEnded) {
            getQuizQuestion();
        }

        if (clicked) {
            var timestamp = getStepStartTime(currentQuestion);
            player.setCurrentTime(timestamp);
        }

    });

}

function prevStep() {
    currentQuestion -= 1;

    var ctSet = getStepStartTime(currentQuestion);

    player.getPaused().then(function (paused) {
        if (paused) {
            play(0, true);
            document.getElementById("btnPause1").value = "Pause";
        }

        document.getElementById("btnForward1").disabled = false;
        document.getElementById("btnPrev1").disabled = currentQuestion == 0;
        document.getElementById("btnPause1").disabled = false;
        document.getElementById("btnRepeat1").style.visibility = "hidden";
        document.getElementById("btnReveal1").style.visibility = "hidden";

        getQuizQuestion();
        player.setCurrentTime(ctSet);
    });

}

function repeatQuestion() {


    player.getCurrentTime().then(function (seconds) {
        document.getElementById("btnRepeat1").disabled = true;
        play(0, true);
        player.setCurrentTime(seconds - 5);
    }).catch(function (error) {
        // an error occurred
    });

}

function getStepStartTime(index) {
    var prevQuestion = questions[index - 1];
    if (prevQuestion == null) return 0;
    else return prevQuestion.VideoTimestamp + 1;
}

function reset() {

    currentQuestion = 0;
    document.getElementById("btnForward1").disabled = false;
    document.getElementById("btnPause1").disabled = false;
    document.getElementById("btnPrev1").disabled = true;
    document.getElementById("btnRepeat1").style.visibility = "hidden";
    document.getElementById("btnReveal1").style.visibility = "hidden";

    player.getPaused().then(function (paused) {
        if (paused) play(0, true);
        getQuizQuestion();
        player.setCurrentTime(0);
    });

}


//---------------------------------------------------------
function getHint(answer, editing) {

    if (editing == "") {
        return "<span style='color: green;font-weight: bold;'>" + answer + "<\/span>"
    }

    var hint = "";
    var answerIndex = 0;
    for (var i = 0; i < editing.length; i++) {
        if (editing.charAt(i) == "E") {
            hint += "<span style='color: green;font-weight: bold;'>" + answer.charAt(answerIndex) + "<\/span>"
            answerIndex++;
        } else if (editing.charAt(i) == "C") {
            hint += "<span style='color: red;font-weight: bold;'>" + answer.charAt(answerIndex) + "<\/span>"
            answerIndex++;
        } else if (editing.charAt(i) == "D") {
            hint += "<span style='color: red;text-decoration:line-through;font-weight: bold;'>" + answer.charAt(answerIndex) + "<\/span>"
            answerIndex++;
        } else if (editing.charAt(i) == "I") {
            hint += "<span style='color: red;font-weight: bold;'>_<\/span>"
        }
    }
    return hint;
}




// ---------- distance editing algorithm  -----------------

function diff(a, b) {
    if (a == b) {
        return 0;
    }
    else {
        return 1;
    }
}

function evaluateAnswer(currentAnswer, expectedAnswer, caseSensitive) {

    currentAnswer = currentAnswer.replace(String.fromCharCode(160), " "); //&nbsp;

    if (!caseSensitive) {
        currentAnswer = currentAnswer.toLowerCase();
        expectedAnswer = expectedAnswer.toLowerCase();
    }

    if (expectedAnswer == currentAnswer) {
        return "";
    }

    var expectedLength = expectedAnswer.length;
    var currentLength = currentAnswer.length;

    var d = Array.from(Array(currentLength + 1), () => new Array(expectedLength + 1));

    for (var i = 0; i < currentLength + 1; i++) {
        d[i][0] = i;
    }
    for (var j = 0; j < expectedLength + 1; j++) {
        d[0][j] = j;
    }

    var moves = Array.from(Array(currentLength + 1), () => new Array(expectedLength + 1));

    for (var i = 0; i < currentLength + 1; i++) {
        for (var j = 0; j < expectedLength + 1; j++) {
            if (i == 0 && j == 0) d[i][j] = 0;
            else if (i == 0) {
                d[i][j] = j;
                moves[i][j] = "I";
            }
            else if (j == 0) {
                d[i][j] = i;
                moves[i][j] = "D";
            }
            else {
                var c = diff(currentAnswer[i - 1], expectedAnswer[j - 1]);
                var res1 = d[i][j - 1] + 1;
                var res2 = d[i - 1][j] + 1;
                var res3 = d[i - 1][j - 1] + c;
                var result;
                if (res1 <= res2 && res1 <= res3) {
                    result = res1;
                    moves[i][j] = "I";
                } else {
                    if (res2 <= res3) {
                        result = res2;
                        moves[i][j] = "D";
                    } else {
                        result = res3;
                        moves[i][j] = (c == 0) ? "E" : "C";
                    }
                }

                d[i][j] = result;
            }
        }
    }

    var i = currentLength;
    var j = expectedLength;
    editing = "";

    while (i > 0 || j > 0) {
        editing = moves[i][j] + editing;
        if (moves[i][j] == "I") {
            j--;
        } else if (moves[i][j] == "D") {
            i--;
        } else {
            i--;
            j--;
        }
    }

    return editing;
}



/*====Demo Code====*/
function navigateDemo() {
    if (window.location.href.includes("Material")) {
        window.location.href = "QuizPage.html?questionSetId=" + d.Id;
    }
    else if (window.location.href.includes("Quiz")) {
        window.location.href = "Interaction.html?codingProblemId=83";
    }
}

