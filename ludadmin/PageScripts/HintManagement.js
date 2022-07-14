let mask = {
	show: () => {
		$('#mask').fadeIn("slow");
	},
	hide: () => {
		$('#mask').fadeOut("slow");
	}
}


const populateSingleSelect = (selectId, data) => {
	$("#" + selectId).empty();
	let qSelect = document.getElementById(selectId);
	for (var i = 0; i < data.length; i++) {
		var obj = data[i];
		var opt = document.createElement("option");
		opt.value = obj.Id;
		opt.innerHTML = obj.Text;
		qSelect.appendChild(opt);
	}
};

$(document).ready(function () {
	mask.show();
	getCourses();

	$('#hintSavedModal').on('hidden.bs.modal', function (e) {
		location.reload();
	})
	$('#validAnswerSavedModal').on('hidden.bs.modal', function (e) {
		location.reload();
	})
});

$("#courses").change(() => {
	let c = $("#courses").val();

	$('#inputHintContainer').addClass('d-none');
	console.log("data sent:", c);
	$('#hint, #submit').prop('disabled', true);

	if (c != -1) {
		mask.show();
		getQuestions(c);
    }
});

$('#questionsContainer').on('click', '.btn-sugg-hint', function () {
	let sugg = unescape(this.getAttribute('data-sugg'))
		, id = this.getAttribute('data-hint-id')
		, hintInput = document.getElementById('hint-input-' + id)
	hintInput.value = sugg
});

$('#questionsContainer').on('click', '.btn-submit-hint', function () {
	let answerId = $(this).parent().parent().parent().parent().attr('data-answer-id');
	let hintId = $(this).parent().parent().parent().parent().attr('data-hint-id');
	let hint = $(this).parent().parent().parent().parent().find('input[name="hint"]').val();
	let isCode = $(this).parent().parent().parent().parent().attr('data-iscode');
	let line = -1;
	if (isCode == "true")
		line = $(this).parent().parent().parent().find('input[name="line"]').val()
	saveHint(answerId, hint, isCode,line, hintId);
});

$('#questionsContainer').on('click', '.btn-delete-hint', function () {
	let answerId = $(this).parent().parent().parent().parent().attr('data-answer-id');
	let hintId = $(this).parent().parent().parent().parent().attr('data-hint-id');
	let isCode = $(this).parent().parent().parent().parent().attr('data-iscode');
	let line = -1;
	if (isCode == "true")
		line = $(this).parent().parent().parent().find('input[name="line"]').val()
	saveHint(answerId, null, isCode, line, hintId);
});

$('#questionsContainer').on('click', '.btn-valid-answer', function () {
	let answerId = $(this).parent().parent().parent().parent().attr('data-answer-id');
	saveAnswer(answerId);
});

var codes = {}
$('#questionsContainer').on('change', '[name="answer-selector"]', function () {
	let qId = $(this).attr('data-qid');
	let isCode = $(this).attr('data-iscode');
	let answerId = parseInt($(this).children("option").filter(":selected").val());
	let sol = $(this).attr('data-solution');
	console.log(qId, isCode, answerId);
	if (isCode == "true" && answerId != "-1") {
		var div = document.getElementById('viewCodes-' + qId);
		div.style.display = "block";
		showCodeIntoQuestionContainer(answerId, qId, codes[answerId], sol);
	}
	getHints(qId, codes[answerId], isCode, answerId);
});

$('#questionsContainer').on('click', '.btn-go-to-line', function () {
	let qId = $(this).parent().parent().parent().parent().parent().parent().prev().children().next().children().attr('data-qid');
	let answerId = $(this).attr('data-answer-id');
	let line = $(this).attr('data-line');
	getLinesTableau(answerId, qId ,line);
});

const paintMarkedLines = (str, arrNumLines, color) => {
	let strSplits = str.split("\n");
	let exp = /\n/g;
	let i;
	for (i = 0; i < strSplits.length; i++) {
		if (arrNumLines.includes(i + 1)) {
			strSplits[i] = (i + 1) + ": " + '<span style="background-color: ' + color + '">' + escapeHtml(strSplits[i].replace(/(\r\n|\n|\r)/gm, "")) + '</span >';
		} else {
			strSplits[i] = (i + 1) + ": " + escapeHtml(strSplits[i].replace(/(\r\n|\n|\r)/gm, ""));
        }
    }
	stringRes = strSplits.join('<br>').replace(/(\r\n|\n|\r)/gm, "");
	return stringRes;
}

var differences = {}
var greenLines = {}
var redLines = {}

const showCodeIntoQuestionContainer = (index, idQ, elem, solution) => {
	solution = paintMarkedLines(solution, greenLines[index], '#98FF98');
	code = paintMarkedLines(elem, redLines[index], '#FFBFAA');
	document.getElementById("codeSolutionBody-"+ idQ).innerHTML = "Solution Code: \n\n" + solution;
	document.getElementById("codeAnswerBody-" + idQ).innerHTML = "Student Code: \n\n" + code;
}

const populateQuestions = (data) => {
	var questionsContainer = document.getElementById("questionsContainer");
	questionsContainer.innerHTML = ""
	data.forEach((d, i) => {
		var isCode = d.IsCode;
		answers =
			'<div class="row margin-top-15">' +
			'	<div class="col-md-4">' +
			'		<span class="sp-label">Answer</span>' +
			'   </div>' +
			'	<div class="col-md-8">' +
		`		<select name="answer-selector" class="tex-box form-control" data-qid="${d.Id}" data-iscode ="${d.IsCode}" data-solution = "${d.Solution}"><option value="-1" selected disabled>Select Answer</option >`;
		d.Answers.forEach((and) => {
			var textOrCode = and.Text
			var diff = ''
			if (isCode) {
				textOrCode = textOrCode.slice(0, 30) + "..."
				codes[and.Id] = and.Text;
				textOrCode = escapeHtml(textOrCode);
				diff = and.LineDifference;
				differences[and.Id] = diff;
				greenLines[and.Id] = and.GreenLines;
				redLines[and.Id] = and.RedLines;
			}
			answers += `<option value="${and.Id}">${textOrCode}</option>`
		})
		answers += `</select></div></div>`
		var type = 'Quiz';
		answers += `<div class="text-right margin-top-15">`
		if (isCode) {
			answers += `<div id= "viewCodes-${d.Id}" style="display:none;">
							<div class="row">
                            <div class="col mt-6">
								<div style="white-space:pre; overflow-y: scroll; overflow-x: scroll; max-height: 50vh; text-align: left;" id="codeAnswerBody-${d.Id}" contentEditable="false">
                                </div>
                            </div>
                            <div class="col mt-6">
                                <div style="white-space:pre; overflow-y: scroll; overflow-x: scroll; max-height: 50vh; text-align: left;" id="codeSolutionBody-${d.Id}">
                                </div>
                            </div>                        
						</div>`
		}
		else
			answers += `<button type="button" class="btn btn-custom" data-toggle="modal" data-target="#codeModal" onclick="showSolutionModal('${escapeHtml(encodeURI(escapeHtml(d.Solution)))}', '${type}')">Solution</button>`
		answers += `</div>`
		d.Image = (!d.Image) ? '' : d.Image
		elem = `<div class="wraper-area  margin-bottom-15 box-bg-white">
                    <!--------------------------------Loader spinner ------------------------------------------>
                    <div id="2disabled-div" class="disabled-div" style="display:none"></div>
                    <div>
                        <div style="position:relative;" class="margin-b-1">
                            <div id="2videoPanel" class="video-panel" style="display:none;">
                                <div id="playVideo" class="material-play-btn" onclick="play(2)">
                                    <a href="#">
                                        <img src="Content/images/play-btn-x.png">
                                    </a>
                                </div>
                            </div>
                            <div class="question-img-area" id="2questionImage">
                                ${d.Image}
                            </div>
                            <div class="gradable-question-area">
                                <span id="2lblPrompt1">
                                    ${d.Text}
                                </span>
                                <div id="elementArea" style="background-color:rgba(255, 0, 0, 0.25); border:2px solid red;display:none;position:absolute;" onclick="clickSubmit(2)"></div>

                                <div id="2textbox" class="gradeable-textbox-area" style="position: relative;">
                                    <span class="gradable-hint arrow arrow-progress" id="2lblHint" style="visibility: hidden;"></span>
                                    <div type="text" id="2txtAnswer" class="form-control gradeable-textbox" onkeypress="rememberCharInput(2, event);" onfocusout="hideHint(2)" onkeydown="rememberNavKey(2, event);" oninput="checkAnswerOnInput(2)" contenteditable="true"></div>
                                    <select id="2ddAnswer" style="display: none;" onchange="putChoice(2);"></select>
                                    <!--<div class="gradeable-img-button"> </div>-->
                                </div>
                                <span id="2lblPrompt2"></span>
                            </div>
                           

                            <div class="gradable-checkbox-area" id="2multipleChoice" style="display:none;"></div>
                        </div>
                    </div>
                    <div class="gradable-padding"></div>
					${answers}
                    
                   <div id="question-${d.Id}-hints-container" class="row mt-3"></div>
                </div>`
		questionsContainer.innerHTML += elem
	})
};

function saveHint(Id, hint, isCode, line, hintId) {
	const data = {
		Type: 2,
		QuestionHintId: hintId,
		Hint: hint,
		IsCode: (isCode == "true"),
		Line: line,
		AnswerId : Id
	};
	fetchFunction("Hint", data).then(d => {
		$('#hintSavedModal').modal();
	});
}

function saveAnswer(Id) {
	const data = {
		Type: 6,
		QuestionId: Id
	};
	fetchFunction("Hint", data).then(d => {
		$('#validAnswerSavedModal').modal();
	});
}

function getCourses() {
	const data = {
		Type: 5
	};
	fetchFunction("Hint", data).then(d => {
		d.unshift({ Id: -1, Text: "--- Select a Course ---" });

		populateSingleSelect('courses', d);

		mask.hide();
	});
}

function getQuestions(course) {
	const data = {
		Type: 0,
		CourseId: course
	};

	fetchFunction("Hint", data).then(d => {
		mask.hide();
		populateQuestions(d);
	});
}

let toHtml = (unsafe) => {
	if (unsafe == null) {
		return "";
	}
	return unsafe
		.replace(/&#039;/g, "'")
		.replace(/&quot;/g, '"')
		.replace(/&gt;/g, '>')
		.replace(/&lt;/g, '<')
		.replace(/&amp;/g, '&')
}

let escapeHtml = (unsafe) => {
	if (unsafe == null)
	{
		return "";
	}
	return unsafe
		.replace(/&/g, "&amp;")
		.replace(/</g, "&lt;")
		.replace(/>/g, "&gt;")
		.replace(/"/g, "&quot;")
		.replace(/'/g, "&#039;")
}

var createInputs = (data, IsCode) => {
	res = ''
	data.forEach((d, idx) => {
		let disabled = ""
		if (IsCode == "true") {
			disabled = "disabled"
		}
		var isTableau = d.IsTableau;
		let haveHint = d.Text != null;
		d.Text = (!d.Text) ? "" : d.Text
		res +=
			'<div class="col-md-12 mb-4" data-answer-id="' + d.AnswerId + '" data-hint-id ="' + d.Id + '" data-iscode="' + IsCode + '">' +
			'	<div class="row margin-top-15">' +
			'		<div class="col-md-4">' +
			'			<span class="sp-label">Hint</span>' +
			'		</div>' +
			'		<div class="col-md-8">' +
			`			<input id= "hint-input-${d.Id}" type = "text" name = "hint" placeholder = "Hint content here..." class="tex-box form-control" style = "float: left" value=" ${escapeHtml(d.Text)} ">
					</div>`;
		if (IsCode == "true") {
			res +=
				'</div>' +
				'<div class="row margin-top-10">' +
				'<div class="col-md-4">' +
				'	<span class="sp-label">Line Number</span>' +
				'</div>' +
				'<div class="col-md-8">' +
				`	<input id= "line-input-${d.Id}" type = "number" name = "line" class="tex-box form-control" style = "float: left" value = "${d.LineNumber}">
					</div>`;
		}
		else if (haveHint) {
			res +=
				'</div>' +
				'<div class="row margin-top-10">' +
				'	<div class="col-md-4">' +
				'		<span class="sp-label">Positive Ratings Count</span>' +
				'	</div>' +
				'	<div class="col-md-8">' +
			`			<input type = "number" name = "line" class="tex-box form-control" style = "float: left" value = "${d.PositiveRatingsCount}" disabled>
					</div>`+
				'</div>' +
					'<div class="row margin-top-10">' +
					'	<div class="col-md-4">' +
					'		<span class="sp-label">Negative Ratings Count </span>' +
					'	</div>' +
					'	<div class="col-md-8">' +
					`			<input type = "number" name = "line" class="tex-box form-control" style = "float: left" value = "${d.NegativeRatingsCount}" disabled>
						</div>`;
        }
		res += `<div class="col-md-12 text-right margin-top-15">					
						<span class="input-group-btn ml-3">`;
		if (isTableau) {
			res += `<button data-answer-id="${d.AnswerId}" data-line="${d.LineNumber}" type="button" class="btn-go-to-line btn btn-custom-light mr-3">Go to Line</button>`;
		}
		res += `<button type="button" class="btn-submit-hint btn btn-custom mr-3">Submit</button>`;
		if (haveHint) res += '<button type="button" class="btn-delete-hint btn btn-custom-light mr-3">Delete</button>'
		else res += 	`<button data-answer-id="${d.AnswerId}" data-hint-id ="${d.Id}" type="button" data-sugg="${escape(d.SuggestedHint)}" class="btn-sugg-hint btn btn-custom-light btn-sm mr-3">Suggestion</button> <button type="button" class="btn-valid-answer btn btn-custom-light" ${disabled}>Valid answer</button>`
		res += 			`</span>
					</div>
				</div>
			</div>`;
	});
	return res
};


function getHints(questionId, answer, isCode, answerId) {
	const data = {
		Type: 3,
		QuestionId: questionId,
		Answer: answer,
		IsCode: (isCode == "true"),
		AnswerId: answerId
	};
	fetchFunction("Hint", data).then(d => {
		elems = createInputs(d, isCode);
		let hContainer = document.getElementById(`question-${questionId}-hints-container`)
		hContainer.innerHTML = elems
	});
}

function getLinesTableau(answerId, qId, line) {
	const data = {
		Type: 9,
		AnswerId: answerId,
		QuestionId: qId,
		Line: line
	};
	fetchFunction("Hint", data).then(d => {
		let ansTextArea = document.getElementById(`codeAnswerBody-${qId}`);
		let solTextArea = document.getElementById(`codeSolutionBody-${qId}`);
		ansTextArea.focus();
		ansTextArea.scrollTo(0, 22 * line)
		solTextArea.focus();
		solTextArea.scrollTo(0, 22 * d)
		document.activeElement.blur()
	});
}
