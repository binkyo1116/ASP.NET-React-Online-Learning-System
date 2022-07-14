let codingProblems;

$(document).ready(function () {
	getHTMLProblems();
});

$('#ansSelector').on('change', function () {
	let index = this.selectedIndex;

	clearHints();
	$('#viewAnsSelected').empty();

	if (index == 0) {
		$('#viewAnsContainer').addClass('d-none');
		$('#viewBlockContainer').addClass('d-none');
	}
	else {
		$('#viewAnsContainer').removeClass('d-none');
		$('#viewBlockContainer').removeClass('d-none');
		index--;
		$('#viewAnsSelected').val(codes[index]);
		getHints(this.value);
	}
});

$('#codSelector').on('change', function () {
	let index = this.selectedIndex;

	$('#viewAnsContainer').addClass('d-none')
	$('#viewBlockContainer').addClass('d-none')

	clearHints();

	if (index == 0) {
		$('#codingProblemContainer').addClass('d-none');
	}
	else {
		$('#codingProblemContainer').removeClass('d-none');

		index--;
		$('#viewHTMLSelected').val(codingProblems[index].Solution);	

		clearAnswers();

		getAnswers(codingProblems[index].Id);
    }
});

$("#btnSubmitHTMLBlock").click(() => {
	let code = $("#inputHTMLBlock").val();
	let problemId = $('#ansSelector').val();
	let line = $('#inputLine').val();
	addHtmlBlock(problemId, line,code);
});

$("#buttonSubmitNewHint").click(() => {
	let problemId = $('#codSelector option:selected').attr('data-problem-id');
	let code = $("#inputNewCode").val();
	let hint = $("#inputNewHint").val();

	createHint(problemId, code, hint);
});

$('#hintsContainer').on('click', 'button[name="buttonUpdateHint"]', function () {
	let parent = $(this.parentElement);
	let problemHintId = parent.parent().attr('data-problem-hint-id');
	let hint = parent.parent().find('textarea[name="inputHint"]').val();

	updateHint(problemHintId, hint);

});

$('#hintsContainer').on('click', 'button[name="buttonSuggestionHint"]', function () {
	let parent = $(this.parentElement);
	let problemId = $("#codSelector").val();
	let solution = parent.parent().find('.htmlcode')[0].innerHTML;
	let solutionId = parent.parent().attr("data-problem-hint-id");


	getHintsHTML(problemId, solution, solutionId);
});

function showHints(d) {
	d.forEach((item, idx) => {

		let container = $('<div data-problem-hint-id="' + item.HintId + '" class="wraper-area margin-bottom-15 box-bg-white row"/>');
		let codeContainer = $('<div class="col-md-6"><span id="Label4" class="sp-label">HTML </span></div>');
		let code = $('<textarea name="inputCode" type="text" class="font-size-13 form-control custome-textarea htmlcode" disabled/>');
		let hint = (!item.Hint) ? "" : item.Hint
		let other = $(
			'<div class="col-md-6">' +
				'<span id="Label4" class="sp-label">Hint </span>'+
				'<textarea name="inputHint" type="text" class="font-size-13 form-control custome-textarea margin-bottom-10">' + hint + '</textarea>' +
			'</div>'
		);

		let footer = '<div class="col-md-6 margin-top-10"><span class="sp-label">Line: </span> <input type="number" min="-1" value = "' + item.Line + '" disabled/></div >' +
					 '<div class="col-md-6 margin-top-10" style="text-align: right">' +
				 		'<button name="buttonUpdateHint" type="button" class="btn btn-custom mr-3">Submit</button>' +
						'<button name="buttonSuggestionHint" type="button" class="btn btn-custom-light">Get Suggestion</button>' +
					'</div>';

		code[0].innerText = item.Block;
		codeContainer.append(code);

		container.append(codeContainer);
		container.append(other);
		container.append(footer);

		

		$('#hintsContainer').append(container);
	});
}

function clearHints() {
	$('#hintsContainer').empty();
}

function clearAnswers() {
	$('#ansSelector').empty();
	codes = {}
}

function loadProblems(d) {
	$('#codSelector').append('<option data-problem-id="-1" value="-1">--- Select a Coding Problem ---</option>');
	d.forEach((item, idx) => {
		$('#codSelector').append('<option data-problem-id="' + item.Id + '" value="' + item.Id + '">' + item.Title + '</option>');
	});	
}

function populateAnswers(d) {
	$('#ansSelector').append('<option data-problem-id="-1" value="-1">--- Select a Coding Problem ---</option>');
	d.forEach((item, idx) => {
		var ans = item.Answer.slice(0, 30) + "..."
		codes[idx] = item.Answer
		$('#ansSelector').append('<option data-problem-id="' + item.Id + '" value="' + item.Id + '">' + ans + '</option>');
	});
}


var answers = {}

function addHtmlBlock(problemId, line, code) {
	const data = {
		Type: 4,
		Line: line,
		Code: code,
		ProblemId: problemId
	};

	fetchFunction("HTMLHint", data).then(d => {
		clearHints();

		let index = $('#codSelector')[0].selectedIndex;

		index--;

		getHints(problemId);
	});
}



function createHint(problemId, code, hint) {
	const data = {
		Type: 0,
		ProblemId: problemId,
		Code: code,
		Hint: hint
	};

	fetchFunction("HTMLHint", data).then(d => {
	});
}

function getHints(problemId) {
	const data = {
		Type: 1,
		ProblemId: problemId
	};

	fetchFunction("HTMLHint", data).then(d => {
		showHints(d);
	});
}

function getHTMLProblems() {
	const data = {
		Type: 2
	};

	fetchFunction("HTMLHint", data).then(d => {
		codingProblems = d;

		loadProblems(d);
	});
}

function updateHint(problemHintId, hint) {
	const data = {
		Type: 3,
		ProblemHintId: problemHintId,
		Hint: hint
	};

	fetchFunction("HTMLHint", data).then(d => {
		alert("ok");
	});
}

function getHintsHTML(problemId, solution, solutionId) {
	const data = {
		Type: 7,
		ProblemId: problemId,
		Solution: solution,
		SolutionId: solutionId

	};

	fetchFunction("Hint", data).then(d => {
	});
}

function getAnswers(problemId) {
	const data = {
		Type: 5,
		ProblemId: problemId
	};

	fetchFunction("HTMLHint", data).then(d => {
		populateAnswers(d);
	});
}