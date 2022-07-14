function getQuiz() {

	if (window.location.href.includes("Quiz")) {
		if (localStorage.getItem("isDemo") == "true") {
			document.getElementById("demoNavigate").style.display = "";
		}
		else {
			document.getElementById("demoNavigate").style.display = "none";
		}
	}

	const questionSetId = GetFromQueryString('questionSetId');

	var questionSetType;

	
	if (window.location.href.includes("Material")) {
		questionSetType = "Material";
	} else {
		questionSetType = "Quiz";
	}

	const data = {
		QuestionSetId: questionSetId,
		QuestionSetType: questionSetType,
	};

	fetchFunction("Material", data).then(d => {
		questions = d.QuizQuestions;
		questionHints = d.QuizQuestionHints;
        answerHistory = new Array(questions.length);
        currentAnswerList = new Array(questions.length);
		currentQuestion = 0;
		//document.getElementById("QuizTitle").textContent = d.Title;
		document.getElementById("txtGrade").textContent = d.TotalGrade + "%";
		document.getElementById("txtRevealed").textContent = " " + d.TotalShown + "%";
		displayMaterials(d.Materails);
		getQuizQuestion();
	});
}

function displayMaterials(materials)
{
	if (materials == null || materials.length == 0) return;

	var list = document.getElementById("quizMaterials");
	for (let i = 0; i < materials.length; i++)
	{
		var aElem = document.createElement("a");
		aElem.href = "#";
		aElem.setAttribute("onclick", "openMaterial('" + materials[i].Link + "')");
		var linkText = document.createTextNode(materials[i].Title);
		aElem.appendChild(linkText);
		list.appendChild(aElem);
		var br = document.createElement("br");
		list.appendChild(br);
	}
}

function openMaterial(link)
{
	window.open(link, '_blank');
}

var questions;
var answerHistory;
var currentAnswerList;
var currentQuestion;
getQuiz();


function fetchGradableTextBox() {
	fetch("GradableTextBoxControl.html").then(res => res.text()).then(d => {
		if (window.location.href.includes("Quiz")) {
			for (var i = 0; i < questions.length; i++) {
				document.getElementById("pnlQuestions").innerHTML += d.replace(/x_/g, i);
				document.getElementById(i + "textbox").style.position = "relative";
				getQuestion(i);
			}
			//----------------Loder Spiner----------------------------
			document.getElementById("loader-spinner").style.display = "none";
			//---------------------------------------------------------
		}
		else {
			if (document.getElementById("pnlQuestion").innerHTML == "") {
				document.getElementById("pnlQuestion").innerHTML = d.replace(/x_/g, currentQuestion);
			}
			getQuestion(currentQuestion);
			//----------------Loder Spiner----------------------------
			document.getElementById("loader-spinner").style.display = "none";
			//---------------------------------------------------------
		}
		// Find fix for loader spinner to work in MaterialPage.html then move back here
	});
}

function getQuizQuestion() {
	fetchGradableTextBox();
}

function saveAllHistory()
{

    for (var i = 0; i < answerHistory.length; i++)
    {
        if (answerHistory[i] !== "")
        {
            submitAnswer(i, currentAnswerList[i]);
        }
    }
}

