var errors;
var input;
var lang;
var keywords;
var keywordsOutput;
var testCases;
var language;
var oldText;
var lastSubmissionCode;
var tabSize = 4;
var tab = "";
var exegrade = 0;
var moduleObjectiveId = GetFromQueryString("moduleId");
var courseInstanceId = GetFromQueryString("courseInstanceId");
var codingProblemSolution;
var codingProblemKeywords;
var codingProblemPrevious;
var exReMess;
var emptyHints;
Setup()
async function CallFunction1(name, continuation) {

    const codingProblemId = GetFromQueryString("codingProblemId");

    const code = '';
    const data = { codingProblemId, code, courseInstanceId, moduleObjectiveId };
    fetchFunction(name, data).then(d => continuation(d));
}

async function CallFunction2(name, continuation) {

    const codingProblemId = GetFromQueryString("codingProblemId");

    const code = '';
    const data = { codingProblemId, code, courseInstanceId, moduleObjectiveId };
    fetchCompiler(name, data).then(d => continuation(d));
}

async function CallFunction(name, continuation) {

    const codingProblemId = GetFromQueryString("codingProblemId");
    const files = document.getElementById("fileUpload").files;

    var file = undefined;

    if (files.length > 0 && (language == "Tableau" || language == 'HTML')) {
        file = files[0];
        fr = new FileReader();
        fr.readAsText(file);
        fr.onload = fcontent => {
            code = fcontent.target.result;
            const data = { codingProblemId, code, courseInstanceId, moduleObjectiveId };
            fetchCompiler(name, data).then(d => continuation(d));
        }
    } else if (files.length > 0 && (language == "Excel" || language == "Image")) {

        file = files[0];
        fr = new FileReader();
        fr.readAsDataURL(file);
        fr.onload = fcontent => {
            code = GetFileBase64String(fcontent.target.result);
            const data = { codingProblemId, code, courseInstanceId, moduleObjectiveId };
            fetchCompiler(name, data).then(d => continuation(d));
        }

    }
    else if (!IsFileUploadProblem()) {

        let code = document.getElementById("txtCode1").textContent;
        if (code != '') {
            let editor = ace.edit('txtCode1');
            code = editor.getSession().getValue();
        }
        var points = compare();
        const codeStructurePoints = points.extra - points.deducted;
        const data = { codingProblemId, code, courseInstanceId, codeStructurePoints };
        fetchCompiler(name, data).then(d => continuation(d));
    } else {
        const code = lastSubmissionCode;
        const data = { codingProblemId, code, courseInstanceId, moduleObjectiveId };
        fetchCompiler(name, data).then(d => continuation(d));
    }


}

function IsCodeStructureProblem() {
    return IsFileUploadProblem() || language == 'SQL' || language == 'Cpp';
}

function IsFileUploadProblem() {
    return (language == "Tableau" || language == "Excel" || language == 'Image' || language == 'HTML');
}

function GetFileBase64String(base64Str) {
    var substr = "base64,";
    var index = base64Str.indexOf(substr);
    if (index == -1) {
        return "";
    }

    return base64Str.substring(index + substr.length);
}
var SubmittedFIle;
function Setup() {

    for (let i = 0; i < tabSize; i++) tab += "\u0020";

    /*CallFunction1("codingproblem1", d => {*/
    CallFunction2("codingproblem", d => {
        language = d.Language;
        document.getElementById("txtInstructions").innerHTML = d.Instructions;
        //document.getElementById("lblLanguage").textContent = d.Language;
        //document.getElementById("lbeAssessmentTitle").textContent = d.Title;

        document.getElementById("lblSubmissions").textContent = d.submissions + "/" + d.Attempts;

        // Experiment
        if (d.submissions >= d.Attempts) {
            document.getElementById('code-submit').style.display = 'none';
            document.getElementById('btnReset').style.display = 'none';
        }

        if (GetFromQueryString("codingProblemId") != '330') {
            document.getElementById("txtGrade").value = (d.grade != null) ? d.grade : "0%";
        }
        else {
            document.getElementById('summarydiv').style.display = 'none';
        }
        keywords = d.Keywords;
        codingProblemKeywords = d.Keywords;
        keywordsOutput = d.KeywordsOutput;
        input = d.Solution.toString();
        lang = d.Language.toString();
        lastSubmissionCode = d.last;
        codingProblemPrevious = d.last;        
        //------------Add Commentt-------------
        document.getElementById("comment-area").style.display = 'none';
        if (d.comment != "") {
            document.getElementById("comment-area").style.display = '';
            document.getElementById("comment-title").textContent = 'Comment:';
            document.getElementById("comment-detail").textContent = d.comment;
        }
        //------------------------------------
        if (IsFileUploadProblem()) {
            document.getElementById("interaction-file-area").style.display = '';
            document.getElementById("fileUpload").style.display = '';
            document.getElementById("file-submit").style.display = '';
            document.getElementById("code-submit").style.display = 'none';
            document.getElementById("coding-section").style.display = 'none';
            document.getElementById("txtCode1").style.display = 'none';
            document.getElementById("gradeSturture").style.display = 'none';
            document.getElementById("btnReset").style.display = 'none';
            document.getElementById("FileExtension").innerText = SupportedFileExtension();
        }
        else {
            document.getElementById("interaction-file-area").style.display = 'none';
            document.getElementById("coding-section").style.display = '';
            document.getElementById("fileUpload").style.display = 'none';
            document.getElementById("file-submit").style.display = 'none';
            document.getElementById("code-submit").style.display = '';
            document.getElementById("gradeSturture").style.display = 'none';

            document.getElementById("btnReset").style.display = '';

            var elem = document.getElementById('txtCode1');
            elem.spellcheck = false;
            elem.focus();
            elem.blur();

            let languageMode = getLanguage(lang.toLowerCase()); // assign language mode to a string or false.

            if (d.last === undefined) {
                document.getElementById("txtCode1").textContent = ((d.Before) ? d.Before + "\n\n" : "") + d.Script;

                //may need to set up else
                if (languageMode != false) {
                    // this means language was found
                    ace.require("ace/ext/language_tools");
                    editor = ace.edit('txtCode1');
                    editor.setTheme("ace/theme/dreamweaver");
                    editor.session.setMode(languageMode);
                    editor.setOptions({
                        enableBasicAutocompletion: true,
                        enableSnippets: true,
                        enableLiveAutocompletion: true
                    });
                }

            }
            else {
                document.getElementById("txtCode1").textContent = d.last; //.innerHTML = d.last;
                //may need to set up else
                if (languageMode != false) {
                    // this means language was found
                    ace.require("lib/ext/language_tools");
                    editor = ace.edit('txtCode1');
                    editor.setTheme("ace/theme/dreamweaver");
                    editor.session.setMode(languageMode);
                    editor.setOptions({
                        enableBasicAutocompletion: true,
                        enableSnippets: true,
                        enableLiveAutocompletion: true
                    });
                }
            }
            if (d.TestCodeForStudent && d.TestCodeForStudent !== '') {
                document.getElementById("test-code-download").style.display = '';
                document.getElementById("test-code").value = d.TestCodeForStudent;
                document.getElementById("test-code-filename").value = d.TestCodeFilename;
            }
        }

        //----------------Loder Spiner----------------------------
        document.getElementById("loader-spinner").style.display = "none";
        //---------------------------------------------------------

        if (d.submissions == 0) {
            BlockOnSubmissions(d.pastdue, d.maxreached, d.duedate, d.now);
        } else if (!IsFileUploadProblem() || lastSubmissionCode != null) {
            //----------------------------Download file-------------------------
            SubmittedFIle = d.last;
            if (language == "Excel") {
                document.getElementById("download-excel-file").style.display = "";
            }
            if (language == "Image") {
                document.getElementById("download-image-file").style.display = "";
            }
            //---------------------------------------------------------------
        }
    });

    return false;
}

function RunCode() {

    let codingProblemId = GetFromQueryString("codingProblemId");
    if (codingProblemId == 292) {
        location.href = 'experiment/material.html';
    }

    //----------------Disabled Page----------------------------
    //---------------------------------------------------------
    //----------------Loder Spiner----------------------------
    document.getElementById("loader-spinner").style.display = "block";
    //--------------------------------------------------------------------------
    CallFunction("runcode", d => {
        keywordsOutput = d.KeywordsOutput;
        lastSubmissionCode = d.last;
        document.getElementById("lblSubmissions").textContent = d.Submissions + "/" + d.Attempts;
        
        // Experiment
        if (d.Submissions >= d.Attempts) {
            document.getElementById('code-submit').style.display = 'none';
            document.getElementById('btnReset').style.display = 'none';

        }

        exegrade = d.GradeTable.TestsGrade;
        testCases = d.Tests;
        exReMess = d.ExeResult.TestCodeMessages;
        //expected = d.ExeResult.expected;
        var compilationFailed = (d.GradeTable.CompilationGrade == 0);
        var error = "";
       // var table = document.createElement("table");
        
        if ((language == "C#") || (language == "Java") || (language == "Cpp") || (language == "AzureDO") || (language == "REST") || (language == "Browser") || (language == "R") || (language == "WebVisitor") || (language == "CosmoDB") || (language == "Python")) {
            if (!d.ExeResult.Compiled) {

               // table.innerHTML += "<tr><th>Error</th><th>" //Hint</th></tr>
               // if (d.CodeHints != null) {
               //    for (i = 0; i < d.CodeHints.length; i++) {
               //         error += d.ExeResult.Message[i] + "<br />";
               //         table.innerHTML += "<tr> <td>" + d.CodeHints[i].Error + "</td><td>"; //+ d.CodeHints[i].Hint + "</td></tr>"
               //     }
               //}
                if (d.ExeResult.Message != null) {
                    for (i = 0; i < d.ExeResult.Message.length; i++) {
                        error += d.ExeResult.Message[i] + '\n';
                    }
                }
                if (d.codeHints != null) {
                    for (i = 0; i < d.CodeHints.length; i++) {
                        error += d.CodeHints[i].Error + '\n';
                    }
                }

            }
            else {
                error = "Compilation Successful";

            }
        } else {
            if (language == "SQL") {
                strErrors = testCases[0].ActualErrors.join(String.fromCharCode(13, 10));
                error = strErrors;
            } if (language == "DB")
            {
                strErrors = testCases[0].ActualErrors.join(String.fromCharCode(13, 10));
                error = strErrors;
            }
            else
                error = (d.ExMessage ? d.ExMessage + "<br />" : "") +
                    ((d.ExeResult && d.ExeResult.ExMessage) ? d.ExeResult.ExMessage + "<br />" : "") +
                    ((d.ExeResult && d.ExeResult.ErrorList) ? d.ExeResult.ErrorList + "<br />" : "") +
                    ((d.ExeResult && (d.ExeResult.Succeeded == false) && d.ExeResult.Output) ? d.ExeResult.Output : "");
        }

        DisplaySubmission(compilationFailed, d.BestGrade);

        DisplayTestCases();
        //NumerateLines();
        // GetHints(keywords,solution, submission)
        /*GetHints(codingProblemKeywords, d.KeywordCount); */// To be uncommented later
        if (d.Hints != null) {
            ShowHints(d.Hints);
        }

        //document.getElementById("resultInfoTable").innerHTML = "";
        //document.getElementById("resultInfoTable").appendChild(table)
        if (error != "") {
            document.getElementById("CompletionResult").hidden = false;
            document.getElementById("CompletionResult").value = error;
            emptyHints = false;
        }
        else {
            document.getElementById("CompletionResult").hidden = true;
            emptyHints = true;
        }


        //error == "" ? document.getElementById("CompletionResult").style.display = "none" : table.style.display = "none";

        DisplayGradeTable(d.GradeTable, d.TestPassed, d.TestCount, d.PastDue, d.DueDate, d.Now, d.BestGrade);
        BlockOnSubmissions(d.PastDue, d.MaxReached, d.DueDate, d.Now);
        document.getElementById("comment-area").style.display = 'none';
        //----------------Enable Page----------------------------
        //---------------------------------------------------------
        //----------------Loder Spiner----------------------------
        document.getElementById("loader-spinner").style.display = "none";
        //---------------------------------------------------------
    });
    return false;
}

function download(filename, text) {
    var element = document.createElement('a');
    element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(text));
    element.setAttribute('download', filename);

    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}

function DowloadTestCode() {
    const filename = document.getElementById("test-code-filename").value;
    let editor = ace.edit('txtCode1');
    code = editor.getSession().getValue();
    const content = code + '\r\n\n' + document.getElementById("test-code").value;
    download(filename, content);
    return false;
}

function ShowHints(data) {
    document.getElementById("TableauSubmission").innerHTML = "";
    if (data == null) return;

    document.getElementById("TableauSubmission").innerHTML = data.join('\n');
}

function showTestData(i) {
    //document.getElementById("testResult").innerText = exegrade
    if (language == "SQL") {
        document.getElementById("testErrors").hidden = true;
        document.getElementById("testActual").hidden = true;
        document.getElementById("moreDetailsModal").hidden = true;
        document.getElementById("sqlOutputSolution").hidden = false;
        document.getElementById("sqlOutputCode").hidden = false;

        document.getElementById("sqlOutputSolution").innerHTML = "";
        document.getElementById("sqlOutputCode").innerHTML = "";

        if (testCases[i].Expected != null) DisplaySQLQueryTable("sqlOutputSolution", testCases[i].Expected);
        if (testCases[i].Actual != null) DisplaySQLQueryTable("sqlOutputCode", testCases[i].Actual);

    } else {
        if (testCases[i].ActualErrors.length != 0) {
            document.getElementById("testErrors").hidden = false;
            strErrors = testCases[i].ActualErrors.join(String.fromCharCode(13, 10));
            document.getElementById("testErrors").value = strErrors;
        }
        else {
            document.getElementById("testErrors").hidden = true;
        }
        if (exReMess.length != 0) {
            document.getElementById("testActual").hidden = false;
            document.getElementById("testActual").value = "";
            for (var j = 0; j < exReMess.length; j++) {
                document.getElementById("testActual").value += exReMess[j] + "\n";
            }
        }
        else {
            document.getElementById("testActual").hidden = true;
        }
        if (testCases[i].Actual != null) {
            document.getElementById("testMoreDetails").hidden = false;
            document.getElementById("testMoreDetails").value = testCases[i].Actual;
            document.getElementById("moreDetailsModal").hidden = false;
        }
        else {
            document.getElementById("testMoreDetails").hidden = true;
            document.getElementById("moreDetailsModal").hidden = true;
        }
        //document.getElementById("testErrors").hidden = false;
        //document.getElementById("testActual").hidden = false;
        document.getElementById("sqlOutputSolution").hidden = true;
        document.getElementById("sqlOutputCode").hidden = true;
    }

}

function DisplaySQLQueryTable(id, text) {
    var tblSQL = document.getElementById(id);
    tblSQL.innerHTML = "";
    const lines = text.split('\n');
    if (lines.length > 0) {
        document.getElementById(id).innerHTML = "<thead id='" + id + "_head'><tr></tr></thead>" + 
            "<tbody></tbody>";
        const columns = lines[0].split("||");
        const percent = 100 / columns.length;
        for (var j = 0; j < columns.length; j++) {
            document.getElementById(id + "_head").firstElementChild.insertAdjacentHTML('beforeend', "<th style=\"width:" + percent.toString() + "%; text - align:center \">" + columns[j] + "</th>");
        }

        ClearTable(tblSQL);
        var tBody = tblSQL.tBodies[0];

        for (var j = 1; j < lines.length; j++) {
            let elements = lines[j].split("||");
            var tr = NewTr();
            for (var k = 0; k < elements.length; k++) {

                tr.appendChild(NewTd(elements[k].toString()));
            }
            tBody.appendChild(tr);
        }
    }
}

function DisplayTestCases() {

    document.getElementById("testResult").innerText = "";
    document.getElementById("testActual").value = "";
    document.getElementById("testMoreDetails").value = "";

    if (testCases == null || testCases.length == 0) {
        return;
    }

    if (testCases.length > 0) {
        showTestData(0);
    }
}


function DisplaySubmission(compilationFailed, grade) {
    if (GetFromQueryString("codingProblemId") != '330') {
        document.getElementById("txtGrade").value = (grade != null) ? grade : "0%";
    }
    else {
        document.getElementById('summarydiv').style.display = 'none';
    }

    if (compilationFailed) return;
}

function DisplayGradeTable(gradeInfo, testsPassed, testCount, pastdue, duedate, now, grade) {

    document.getElementById("gradeSturture").style.display = "";
    var tblSummary = document.getElementById("gradeSturture");
    ClearTable(tblSummary);
    var tBody = tblSummary.tBodies[0];

    if (IsFileUploadProblem()) {
        document.getElementById("CodeIssueModalHeader").innerText = "Results";
        document.getElementById("lblCodeIssueModalResult").innerText = "";
        //---------------File Upload----------------
        //2nd row
        var tr = NewTr();
        tr.appendChild(NewTd("Result"));
        tr.appendChild(NewOutputTd(gradeInfo.TestsWeight));
        tr.appendChild(NewTd(gradeInfo.TestsWeight + "%", "text-center"));
        tr.appendChild(NewTd(gradeInfo.TestsGrade + "%", "text-center"));
        tBody.appendChild(tr);
    }
    else {
        document.getElementById("CodeIssueModalHeader").innerText = "Code Issues"
        //---------------compilation----------------
        //2nd row
        var tr = NewTr();
        tr.appendChild(NewTd("Code"));
        if (gradeInfo.CompilationGrade != 0)
            tr.appendChild(NewCodeIssuesTd("Succeeded", ""));
        else
            tr.appendChild(NewCodeIssuesTd("Failed", "text-danger"));

        tr.appendChild(NewTd(gradeInfo.CompilationWeight + "%", "text-center"));
        tr.appendChild(NewTd(gradeInfo.CompilationGrade + "%", "text-center"));
        tBody.appendChild(tr);

        //-----------------Output------------
        var tr = NewTr();
        tr.appendChild(NewTd("Execution"));
        tr.appendChild(NewOutputTd(gradeInfo.TestsWeight));
        tr.appendChild(NewTd(gradeInfo.TestsWeight + "%", "text-center"));
        tr.appendChild(NewTd(gradeInfo.TestsGrade + "%", "text-center"));
        tBody.appendChild(tr);

        ////-----------------Due Date------------
        if (pastdue) {
            var tr = NewTr();
            tr.appendChild(NewTdForDate("Due Date", "Submission Date"));
            tr.appendChild(NewTdForDate(duedate, now));
            tr.appendChild(NewTd(""));
            tr.appendChild(NewTd(""));
            tBody.appendChild(tr);
        }

        //total
        var tr = NewTr();
        tr.appendChild(NewTd("Total"));
        tr.appendChild(NewTd(""));
        tr.appendChild(NewTd("100%", "text-center"));
        tr.appendChild(NewTd(gradeInfo.TotalGrade + "%", "text-center"));
        tBody.appendChild(tr);
    }


}
function NewOutputTd(weight) {
    var td = document.createElement("td");
    var tx = document.createTextNode(exegrade + " of " + weight);
    td.appendChild(tx);
    if (testCases[0].ActualErrors.length != 0 || exReMess.length != 0) {
        var t = document.createTextNode(" (");
        td.appendChild(t);
        var a = newOutputLink();
        td.appendChild(a);
        var t2 = document.createTextNode(")");
        td.appendChild(t2);
    }

    return td;
}

function newOutputLink() {
    var a = document.createElement("a")
    a.textContent = "Details";
    a.setAttribute('href', "#");
    a.setAttribute('style', "color: #2a92ce; text-decoration: underline;");
    a.setAttribute('data-toggle', "modal");
    a.setAttribute('data-target', "#TestModal");

    return a;
}



function NewCodeIssuesTd(text, className, empty) {
    var td = document.createElement("td");
    var tx = document.createTextNode(text);
    if (className != "") {
        td.className = className;
    }
    td.appendChild(tx);

    //if (!emptyHints) {
        var t = document.createTextNode(" (");
        td.appendChild(t);
        var a = newCodeIssuesLink();
        td.appendChild(a);
        var t2 = document.createTextNode(")");
        td.appendChild(t2);
   // }

    return td;
}

function newCodeIssuesLink() {
    var a = document.createElement("a")
    a.textContent = "Details";
    a.setAttribute('href', "#");
    a.setAttribute('style', "color: #2a92ce; text-decoration: underline;");
    a.setAttribute('data-toggle', "modal");
    a.setAttribute('data-target', "#CodeIssueModal");

    return a;
}
function newHindsLink() {
    var a = document.createElement("a")
    a.textContent = "Hints";
    a.setAttribute('href', "#");
    a.setAttribute('style', "color: #2a92ce; text-decoration: underline;");
    a.setAttribute('data-toggle', "modal");
    a.setAttribute('data-target', "#HintsModal");

    return a;
}
function Reset() {

    //----------------Loder Spiner----------------------------
    document.getElementById("loader-spinner").style.display = '';
    //---------------------------------------------------------

    /*CallFunction1("codingproblem1", d => {*/
    CallFunction2("codingproblem", d => {
        

        language = d.Language;
        document.getElementById("txtInstructions").innerHTML = d.Instructions;
        //document.getElementById("lblLanguage").textContent = d.Language;
        //document.getElementById("lbeAssessmentTitle").textContent = d.Title;
        document.getElementById("lblSubmissions").textContent = d.submissions + "/" + d.Attempts;

        // Experiment
        if (d.Submissions >= d.Attempts) {
            document.getElementById('code-submit').style.display = 'none';
            document.getElementById('btnReset').style.display = 'none';

        }

        document.getElementById("txtGrade").value = (d.grade != null) ? d.grade : "0%";
        keywords = d.Keywords;
        codingProblemKeywords = d.Keywords;
        keywordsOutput = d.KeywordsOutput;
        input = d.Solution.toString();
        lang = d.Language.toString();
        lastSubmissionCode = d.last;
        codingProblemPrevious = d.last;
        //------------Add Commentt-------------
        document.getElementById("comment-area").style.display = 'none';
        if (d.comment != "") {
            document.getElementById("comment-area").style.display = '';
            document.getElementById("comment-title").textContent = 'Comment:';
            document.getElementById("comment-detail").textContent = d.comment;
        }
        //------------------------------------
        if (IsFileUploadProblem()) {
            document.getElementById("interaction-file-area").style.display = '';
            document.getElementById("fileUpload").style.display = '';
            document.getElementById("file-submit").style.display = '';
            document.getElementById("code-submit").style.display = 'none';
            document.getElementById("coding-section").style.display = 'none';
            document.getElementById("txtCode1").style.display = 'none';
            document.getElementById("gradeSturture").style.display = 'none';
            document.getElementById("btnReset").style.display = 'none';
            document.getElementById("FileExtension").innerText = SupportedFileExtension();
        }
        else {
            document.getElementById("interaction-file-area").style.display = 'none';
            document.getElementById("coding-section").style.display = '';
            document.getElementById("fileUpload").style.display = 'none';
            document.getElementById("file-submit").style.display = 'none';
            document.getElementById("code-submit").style.display = '';
            document.getElementById("gradeSturture").style.display = 'none';

            document.getElementById("btnReset").style.display = '';

            var elem = document.getElementById('txtCode1');
            elem.spellcheck = false;
            elem.focus();
            elem.blur();

            let languageMode = getLanguage(lang.toLowerCase()); // assign language mode to a string or false.

            //document.getElementById("txtCode1").textContent = ((d.Before) ? d.Before + "\n\n" : "") + d.Script;

            //may need to set up else
            if (languageMode != false) {
                // this means language was found
                ace.require("ace/ext/language_tools");
                editor = ace.edit('txtCode1');
                editor.setTheme("ace/theme/dreamweaver");
                editor.session.setMode(languageMode);
                editor.setOptions({
                    enableBasicAutocompletion: true,
                    enableSnippets: true,
                    enableLiveAutocompletion: true
                });
                var resetScript = ((d.Before) ? d.Before + "\n\n" : "") + d.Script;
                editor.session.setValue(resetScript);
            }
        }

        //----------------Loder Spiner----------------------------
        document.getElementById("loader-spinner").style.display = 'none';
        //---------------------------------------------------------

        if (d.submissions == 0) {
            BlockOnSubmissions(d.pastdue, d.maxreached, d.duedate, d.now);
        } else if (!IsFileUploadProblem() || lastSubmissionCode != null) {
            //----------------------------Download file-------------------------
            SubmittedFIle = d.last;
            if (language == "Excel") {
                document.getElementById("download-excel-file").style.display = "";
            }
            if (language == "Image") {
                document.getElementById("download-image-file").style.display = "";
            }
            //---------------------------------------------------------------
        }

        /*
        testCases = null;
        errors = null;

        editor = ace.edit('txtCode1');
        var resetScript = ((d.Before) ? d.Before + "\n\n": "") + d.Script;
        editor.session.setValue(resetScript);

        document.getElementById("lblCompilationError").innerHTML = "";
        DisplayCompilationTable();
        DisplayTestCases();
        ClearTable(document.getElementById("gradeSturture"));

        ClearTable(document.getElementById("badStructureTbl"));
        document.getElementById("codeStructureTotal").innerHTML = "";
        document.getElementById("GreatJob").textContent = "";
        */
    
    });
    return false;
    }
    

function BlockOnSubmissions(pastdue, maxreached, duedate, now) {
    if (maxreached) {
        document.getElementById("lblError").textContent = "\nMaximum number of submissions reached";
        document.getElementById("lblError").enabled = false;
    }
}

function NewTd(text, className) {
    var td = document.createElement("td");
    var t = document.createTextNode(text);
    if (className) {
        td.className = className;
    }
    td.appendChild(t);
    return td;
}
function NewTdForDate(text, text2,) {
    var td = document.createElement("td");
    var t = document.createTextNode(text);
    var br = document.createElement("br");
    var t2 = document.createTextNode(text2);

    td.appendChild(t);
    td.appendChild(br);
    td.appendChild(t2);
    return td;
}
function NewTdI(className) {
    var td = document.createElement("td");
    var i = document.createElement("i");
    i.className = className;
    td.appendChild(i);
    return td;
}

function NewTdA(href, onclick, value) {
    var td = document.createElement("td");
    var aElem = document.createElement("a");
    aElem.href = href;
    aElem.setAttribute("onclick", onclick);
    var linkText = document.createTextNode(value);
    aElem.appendChild(linkText);
    td.appendChild(aElem);
    return td;
}

function NewTr(id) {
    var newRow = document.createElement("tr");
    if (id) {
        newRow.setAttribute("id", id);
    }
    return newRow;
}

function ClearTable(tbl) {
    if (tbl != null) {
        tbl.tBodies[0].innerHTML = "";
    }
}

function SplitErrorList(errorList) {
    if (language == "C#")
        return SplitErrorCSharp(errorList);
    else if (language == "Java")
        return SplitErrorJava(errorList);
}

function SplitErrorCSharp(errorList) {
    //(10,12): error CS1001: Message...
    errors = [];

    var lineNumber = 0;
    var type = "";
    var code = "";
    var message = "";

    for (var i = 0; i < errorList.length; i++) {
        var error = errorList[i];
        // extracting (1,1)
        var posColon = error.indexOf(":");
        if (posColon < 5) {
            errors.push({ line: '', type: 'error', code: '', message: error });
            continue;
        }

        if ((error.charAt(0) != '(') || (error.charAt(posColon - 1) != ')')) continue;

        var posComma = error.indexOf(",");
        if ((posComma < 2) || (posComma > posColon - 3)) continue;

        lineNumber = error.slice(1, posComma);

        // extracting type
        if (error.length < posColon + 2 + 1) {
            errors.push({ line: lineNumber, type: type, code: code, message: message });
            continue;
        }
        error = error.slice(posColon + 2);
        var posSpace = error.indexOf(" ");
        if (posSpace == -1) {
            errors.push({ line: lineNumber, type: type, code: code, message: message });
            continue;
        }
        type = error.slice(0, posSpace);

        //extract code
        if (error.length < posSpace + 1) {
            errors.push({ line: lineNumber, type: type, code: code, message: message });
            continue;
        }
        error = error.slice(posSpace + 1);
        posColon = error.indexOf(":");
        if (posColon == -1) {
            errors.push({ line: lineNumber, type: type, code: code, message: message });
            continue;
        }
        code = error.slice(0, posColon);

        //extract message
        if (error.length < posColon + 2) {
            errors.push({ line: lineNumber, type: type, code: code, message: message });
            continue;
        }
        message = error.slice(posColon + 2);

        errors.push({ line: lineNumber, type: type, code: code, message: message });
    }
    return errors;
}

function SplitErrorJava(errorList) {
    errors = [];
    //"java:14: error:message..."
    var lineNumber;
    var type;
    var code;
    var message;

    var errorPosition = errorList.indexOf("java:");
    if (errorPosition == -1) return errorList;
    while (errorPosition != -1) {

        lineNumber = 0;
        type = "";
        code = "";
        message = "";

        // extracting line
        if (errorList.length < errorPosition + 6) {
            break;
        }
        errorList = errorList.slice(errorPosition + 5);

        var posColon = errorList.indexOf(":");
        if (posColon == -1) {
            break;
        }

        lineNumber = errorList.slice(0, posColon);

        // extracting type
        if (errorList.length < posColon + 3) {
            errors.push({ line: lineNumber, type: type, code: code, message: message });
            break;
        }
        errorList = errorList.slice(posColon + 2);

        posColon = errorList.indexOf(":");
        if (posColon == -1) {
            message = errorList;
            errors.push({ line: lineNumber, type: type, code: code, message: message });
            break;
        }

        type = errorList.slice(0, posColon);
        errorList = errorList.slice(posColon + 1);

        errorPosition = errorList.indexOf("java:");
        if (errorPosition == -1) {
            message = errorList;
        } else {
            message = errorList.slice(0, errorPosition);
        }

        errors.push({ line: lineNumber, type: type, code: code, message: message });
    }
    return "";
}
/**
 * Gets Breaks down the solution and submission to keywords then compares the keywords. Populates items on the front end for user to review.
 * 
 * Utilizes two helper functions: getTheKeywords() & splitIntoWords()
 * 
 * @todo Remove all solution related content from the front end
 * 
 * @param {string} keywords
 * @param {any} solution
 */
function GetHints(keywords, solution) {
    var theWords = keywords.split(',');
    let theHints = '';

    if (solution && Object.keys(solution).length !== 0 && Object.getPrototypeOf(solution) === Object.prototype) {
        for (var i = 0; i < theWords.length; i++) {
            if (solution[theWords[i]] != undefined) {
                if (solution[theWords[i]] < 0) {
                    theHints += `<li class="list-group-item">The solution has ${-solution[theWords[i]]} less <span class="font-weight-bold" style="color:#23A3DD;">${theWords[i]}</span>(s) than your code.</li>`;
                } else if (solution[theWords[i]] > 0) {
                    theHints += `<li class="list-group-item">The solution has ${solution[theWords[i]]} more <span class="font-weight-bold" style="color:#23A3DD;">${theWords[i]}</span>(s) than your code.</li>`;
                }
            }
        } // end of for

        if (theHints != '') {
            document.getElementById("theHints").innerHTML = theHints;
            //document.getElementById("hintHeading").textContent = 'Hints';
            //------------------Create a link------------------------------
            var hintsa = newHindsLink();
            document.getElementById("hintContainer").innerHTML = "";
            document.getElementById("hintContainer").appendChild(hintsa)
        }
    } // end of if (JSON.stringify(solutionKeywordCount) !== JSON.stringify(submissionKeywordCount))

    document.getElementById("theHints").innerHTML = theHints;
}

function ErrorInLine(lineNumber) {
    if (errors == null) return false;
    for (var i = 0; i < errors.length; i++) {
        if (errors[i].line == lineNumber && errors[i].type == 'error') {
            return true;
        }
    }
    return false;
}

function UnderlineErrorLines(elem) {
    if (errors == null || errors.length == 0) return;

    var text = elem.innerHTML;
    var newHTML = "";
    var lineNumber = 1;
    var isErrorLine = ErrorInLine(lineNumber);
    var startedUnderlining = false;
    text = text.replace(/&nbsp;/g, ' ');
    for (var i = 0; i < text.length; i++) {
        var symbol = text[i];
        if (isErrorLine && !startedUnderlining && text[i] != ' ' && text[i] != '\u00a0' && text[i] !== '\t' && text[i] !== '&nbsp;') {
            newHTML += "<span class='error-underline-dotted'>";
            startedUnderlining = true;
        }
        newHTML += text[i];

        if (text[i] === '\n') {
            if (startedUnderlining)
                newHTML += "</span>";
            startedUnderlining = false;
            lineNumber++;
            isErrorLine = ErrorInLine(lineNumber);
        }
    }
    elem.innerHTML = newHTML;
}

function DisplayCompilationTable(compiled, errors) {
    var tblErrors = document.getElementById("tblCompilation");
    ClearTable(tblErrors);

    if (compiled)
        return;

    var tBody = tblErrors.tBodies[0];

    var tr = NewTr();
    tr.appendChild(NewTd("  "));
    tr.appendChild(NewTd("Code"));
    tr.appendChild(NewTd("Description"));
    tr.appendChild(NewTd("Line"));
    tBody.appendChild(tr);

    if (typeof errors != 'undefined') {
        for (var i = 0; i < errors.length; i++) {
            tr = NewTr();
            var icon = (errors[i].type == "error") ? "fa fa-times text-danger" : "fa fa-check text-success";
            tr.appendChild(NewTdI(icon));
            tr.appendChild(NewTd(errors[i].code));
            tr.appendChild(NewTd(errors[i].message));
            tr.appendChild(NewTd(errors[i].line));
            tBody.appendChild(tr);
        }
    }
}

/*=======================Aidan's code=======================*/
function AddIdentation(event) {
    if (event == null) return;

    if (event.keyCode === 13) { // new line
        event.preventDefault();
        var identation = getIndentation();
        if (identation == "") return false;
        var editor = document.getElementById("txtCode1");
        var doc = editor.ownerDocument.defaultView;
        var sel = doc.getSelection();
        var range = sel.getRangeAt(0);

        var tabNode = document.createTextNode(identation); //tab
        range.insertNode(tabNode);

        range.setStartAfter(tabNode);
        range.setEndAfter(tabNode);
        sel.removeAllRanges();
        sel.addRange(range);
    }
}

function getIndentation() {
    var elem = document.getElementById('txtCode1');
    var text = elem.textContent;
    var newLine = true;
    var indentation = "";

    //var end = getCaretPosition(elem)[0];
    var end = getCaretPosition(elem);

    text = text.replace(/&nbsp;/g, ' ');
    for (var i = 0; i < end; i++) {
        if (text[i] === '\n' && i != (end - 1)) {

            indentation = "";
            newLine = true;
        } else if ((text[i] === '\u0009' || text[i] === '\u0020' || text[i] === '\u00a0') && newLine) {
            indentation += text[i];
        } else {
            newLine = false;
        }
    }
    return indentation;
}

function compare(event) {
    AddIdentation(event);

    if (IsCodeStructureProblem() || keywords == null || input == "" || input == "-") return {
        deducted: 0,
        extra: 0
    };


    var input2 = document.getElementById("txtCode1").textContent;
    var terms = keywords.split(",");
    //var termsOutput = keywordsOutput.split(",");

    var hshOne = new Object();
    var hshTwo = new Object();
    var hshC = new Object();
    var hshCC = new Object();
    var few = 0;
    var many = 0;

    var pointsDeducted = 0;
    var pointsExtra = 0;
    var check = 0;
    var c1, c2, temp;

    for (var k in terms) {
        c1 = 0;
        c2 = 0;
        for (var j = 0; j < input.length - terms[k].length; j++) {
            if (input.substring(j, j + terms[k].length) == terms[k]) {
                if (j != 0 && j != input.length - terms[k].length - 1) {
                    if ((!isLetter(input[j - 1])) && (!isLetter(input[j + terms[k].length]))) {
                        c1++;
                    }
                }
            }
        }
        for (var j = 0; j < input2.length - terms[k].length; ++j) {
            if (input2.substring(j, j + terms[k].length) == terms[k]) {
                if (j != 0 && j != input2.length - terms[k].length - 1) {
                    if ((!isLetter(input2[j - 1])) && (!isLetter(input2[j + terms[k].length]))) {
                        c2++;
                    }
                }
            }
        }

        hshC[terms[k]] = c1;
        hshCC[terms[k]] = c2;

        if (c1 != c2) {
            check++;
            if (c2 < c1) {
                hshOne[terms[k]] = c1 - c2;
                few++;
            }
            else {
                hshTwo[terms[k]] = c2 - c1;
                many++;
            }
        }
    }

    return {
        deducted: pointsDeducted,
        extra: pointsExtra
    };
}

function count(main_str, sub_str) {
    main_str += '';
    sub_str += '';

    if (sub_str.length <= 0) {
        return main_str.length + 1;
    }

    subStr = sub_str.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
    return (main_str.match(new RegExp(subStr, 'gi')) || []).length;
}

function isLetter(strng) {
    strng.toLowerCase();
    return strng.length === 1 && strng.match(/[a-z]/i);
}

//------------  syntax highlighting  --------------------

function browserSupport() {
    // Get the user-agent string 
    var userAgentString =
        navigator.userAgent;

    // Detect Chrome 
    var chromeAgent =
        userAgentString.indexOf("Chrome") > -1;

    // Detect Internet Explorer 
    var IExplorerAgent =
        userAgentString.indexOf("MSIE") > -1 ||
        userAgentString.indexOf("rv:") > -1;

    // Detect Firefox 
    var firefoxAgent =
        userAgentString.indexOf("Firefox") > -1;


    // Detect Safari 
    var safariAgent =
        userAgentString.indexOf("Safari") > -1;

    // Discard Safari since it also matches Chrome 
    if ((chromeAgent) && (safariAgent))
        safariAgent = false;

    // Detect Opera 
    var operaAgent =
        userAgentString.indexOf("OP") > -1;

    // Discard Chrome since it also matches Opera      
    if ((chromeAgent) && (operaAgent))
        chromeAgent = false;

    return (chromeAgent || safariAgent || firefoxAgent);
}

function getStyle(word) {

    var keywordsArray = keywords.split(",");
    var style = "";
    if ((word.length > 1) && IsOpeningComment(word[1], word[0])) {
        style = "color: rgb(0, 128, 0);";
    }
    else if ((word.length > 1) && (word[0] === '"')) {
        style = "color: rgb(163, 21, 21); ";
    } else if (keywordsArray.includes(word)) {
        style = "color: rgb(0, 0, 255); font-weight: bold;";
    }
    return style;
}

function onKeyDown(e) {
    if (e.keyCode === 9) { // tab key

        e.preventDefault();
        var editor = document.getElementById("txtCode1");
        var doc = editor.ownerDocument.defaultView;
        var sel = doc.getSelection();
        var range = sel.getRangeAt(0);

        var tabNode = document.createTextNode(tab);
        range.insertNode(tabNode);

        range.setStartAfter(tabNode);
        range.setEndAfter(tabNode);
        sel.removeAllRanges();
        sel.addRange(range);
    } else if (e.keyCode === 13) {

    }

}

//----------------------------------------------------------
function DeleteErrorFromList(lineNumber) {
    if (errors == null) return;
    for (var i = 0; i < errors.length; i++) {
        if (errors[i].line == lineNumber) {
            errors.splice(i, 1);
            return;
        }
    }
}

function AdjustErrorList(elem, position) {
    var newText = elem.textContent;

    var changedLines = FindDifferentLines(newText);
    for (var i = 0; i < changedLines.length; i++) {
        DeleteErrorFromList(changedLines[i]);
    }
}

function FindDifferentLines(newText) {
    var newLines = newText.split('\n');
    var oldLines = oldText.split('\n');

    var differentLines = [];

    var i = 0;
    while (i < newLines.length || i < oldLines.length) {
        if (i >= newLines.length || i >= oldLines.length) { // 
            while (i < Math.max(newLines.length, oldLines.length)) {
                differentLines.push(i + 1);
                i++;
            }
        } else if (newLines[i].length != oldLines[i].length) {
            differentLines.push(i + 1);
        } else {
            var newLine = newLines[i];
            var oldLine = oldLines[i];
            for (var j = 0; j < newLine.length; j++) {
                if (newLine[j] != oldLine[j]) {
                    differentLines.push(i + 1);
                    break;
                }
            }
        }

        i++;
    }
    return differentLines;
}
//-------------------------------------------------------------------------------

function HighlightSyntax() {
    var elem = document.getElementById('txtCode1');

    var positionArr = getCaretPosition(elem);
    if (positionArr == undefined) {
        return;
    }
    var position = positionArr[0];

    AdjustErrorList(elem, position);

    BuildNewHTML(elem);

    SetCaretPosition(elem, position);
}

/**
 * Receives language from setup() and will set up the IDE for the right language. 
 * 
 * To add more langauge supports, find the following file PageScripts/lib/ext.modelist.js
 * find var supportedModes, the key is what you need to add.
 * 
 * example: 
 * We need to add php (it's making a comeback?) to the file above, find the key for PHP
 * we find that the key is PHP. We'll add a case 'php' (must be lowercase) and problemLanguage
 * will be = 'ace/mode/php'
 * 
 * @param {string} theLanguage 
 * @returns string if language is found, false if not
 * 
 * @todo
 * - confirm the rest of the languages. Once that's confirmed get languages set up or removed.
 */
function getLanguage(theLanguage) {

    let problemLanguage = '';
    switch (theLanguage) {
        case 'azuredo':
            //problemLanguage = 'ace/mode/csharp';
            problemLanguage = false;
            break;
        case 'browser':
            //problemLanguage = 'ace/mode/csharp';
            problemLanguage = false;
            break;
        case 'c#':
            problemLanguage = 'ace/mode/csharp';
            break;
        case 'cpp':
            problemLanguage = 'ace/mode/c_cpp';
            break;
        case 'db':
            //problemLanguage = 'ace/mode/csharp';
            problemLanguage = false
            break;
        case 'java':
            problemLanguage = 'ace/mode/java';
            break;
        case 'javascript':
            problemLanguage = 'ace/mode/javascript';
            break;
        case 'python':
            problemLanguage = 'ace/mode/python';
            break;
        case 'r':
            problemLanguage = 'ace/mode/r';
            break;
        case 'rest':
            //problemLanguage = 'ace/mode/python';
            problemLanguage = false;
            break;
        case 'sql':
            problemLanguage = 'ace/mode/sql';
            break;
        case 'webvisitor':
            problemLanguage = 'ace/mode/python';
            //problemLanguage = false;
            break;
        default:
            problemLanguage = false;
            break;
    }
    return problemLanguage;
}

function BuildNewHTML(elem) {
    var words = splitIntoWords(elem);
    var newHTML = "";

    words.forEach(word => {
        if (style = getStyle(word)) {
            word = word.replace('<', '&lt;').replace('>', '&gt;');
            newHTML += "<span style='" + style + "'>" + word + "</span>";
        }
        else {
            word = word.replace('<', '&lt;').replace('>', '&gt;');
            newHTML += word;
        }

    });

    elem.innerHTML = newHTML;
    oldText = elem.textContent;
    NumerateLines();
}
/**
 * Takes string that represents code and breaks it down the characters into an array. 
 * Then it removes all indices that directly match the symbols in the var EndWordSymbols
 * 
 * At one time this function was rendered obsolete, then later repurposed.
 * @param {String} el
 * @returns {Object} result
 */
function splitIntoWords(el) {

    var result = [];
    var endWordSymbols = ['\u00a0', '\u0020', '\u0009', '\n', '\t', ' ', '(', '{', '[', ')', '}', ']', '=', '+', '-', '/', '*', '<', '>', '!', ',', ';', ':', '?', '\r'];
    var currentWord = "";
    var isString = false;
    var isComment = false;
    var isBlockComment = false;

    if (typeof el != 'string') {
        if (typeof el != 'undefined') {
            var text = el.textContent;
        }
    } else {
        var text = el;
    }

    function pushWordAndSymbol(symbol) {
        if (currentWord !== "") {
            result.push(currentWord);
            currentWord = "";
        }
        result.push(symbol);
    }
    function pushWordWithSymbol(symbol) {
        result.push(currentWord + symbol);
        currentWord = "";
    }
    function pushWordAddSymbol(symbol) {
        if (currentWord !== "") {
            result.push(currentWord);
        }
        currentWord = symbol;
    }
    /**
     * Removes any item from the result array that is received. 
     * Used to clean up items we wouldn't look for to provide hints
     * 
     * @param {String} item
     */
    function removeItemFromResult(item) {
        let myindex;
        while ((myindex = result.indexOf(item)) > -1) {
            result.splice(myindex, 1);
        }
    }

    for (var i = 0; i < text.length; i++) {

        var symbol = text[i];

        if ((symbol === '\n') && !isBlockComment) {
            isString = false;
            isComment = false;
            pushWordAndSymbol(symbol);
        } else if (symbol === '"' && isString && !isComment) {
            isString = false;
            pushWordWithSymbol(symbol);
        } else if (symbol === '"' && !isString && !isComment) {
            isString = true;
            pushWordAddSymbol(symbol);
        } else if (IsOpeningComment(symbol, text[i - 1]) && !isString) {
            isComment = true;
            currentWord += symbol;
            isBlockComment = IsBlockComment(symbol, text[i - 1]);
        } else if (CommentStarts(symbol) && !isString && !isComment) {
            pushWordAddSymbol(symbol);
        } else if (CommentEnds(symbol) && isComment && !isString) {
            currentWord += symbol;
        } else if (IsClosingComment(symbol, text[i - 1]) && isBlockComment && !isString) {
            isComment = false;
            isBlockComment = false;
            pushWordWithSymbol(symbol);
        } else if (endWordSymbols.includes(symbol) && !isComment && !isString) {
            pushWordAndSymbol(symbol);
        } else {
            currentWord += symbol;
        }

    }

    if (currentWord !== "") {
        result.push(currentWord);
    }

    for (var j = 0; j < endWordSymbols.length; j++) {
        removeItemFromResult(endWordSymbols[j]); // removes ' '
    }

    return result;
}
/**
 * Strips out everything but the keywords from param words, then returns an object of just keywords. 
 * @param {Array} keywords
 * @param {Object} words
 * @returns object
 */
function getTheKeywords(keywords, words) {
    let result = [];
    /**
     * Removes any item from the result array that is received. 
     * Used to clean up items we wouldn't look for to provide hints
     * 
     * @param {string} item
     */
    function removeItemFromResult(item) {
        let myindex;
        while ((myindex = words.indexOf(item)) > -1) {
            result.push(words.splice(myindex, 1)[0]);
        }
    }
    for (var k = 0; k < keywords.length; k++) {
        removeItemFromResult(keywords[k]); // removes indice that is not a keyword
    }

    return result;
}

function CommentStarts(symbol) {
    switch (language) {
        case "C#":
            if (symbol === '/') return true;
            break;
        case "Java":
            if (symbol === '/') return true;
            break;
        case "Python":
            if (symbol === '#') return true;
            break;
        case "R":
            if (symbol === '#') return true;
            break;
        case "SQL":
            if (symbol === '/') return true;
            break;
    }
    return false;

}

function CommentEnds(symbol) {
    switch (language) {
        case "C#":
            if (symbol === '*') return true;
            break;
        case "Java":
            if (symbol === '*') return true;
            break;
        case "SQL":
            if (symbol === '*') return true;
            break;
    }
    return false;
}

function IsBlockComment(current, previous) {
    switch (language) {
        case "C#":
            if ((previous === '/') && (current === '*')) return true;
            break;
        case "Java":
            if ((previous === '/') && (current === '*')) return true;
            break;
        case "SQL":
            if ((previous === '/') && (current === '*')) return true;
            break;
    }
    return false;
}

function IsOpeningComment(current, previous) {
    switch (language) {
        case "C#":
            if ((previous === '/') && ((current === '*') || (current === '/'))) return true;
            break;
        case "Java":
            if ((previous === '/') && ((current === '*') || (current === '/'))) return true;
            break;
        case "Python":
            if ((previous === '#')) return true;
            break;
        case "R":
            if ((previous === '#')) return true;
            break;
        case "SQL":
            if ((previous === '/') && (current === '*')) return true;
            break;
    }
    return false;
}

function IsClosingComment(current, previous) {
    switch (language) {
        case "C#":
            if ((previous === '*') && (current === '/')) return true;
            break;
        case "Java":
            if ((previous === '*') && (current === '/')) return true;
            break;
        case "SQL":
            if ((previous === '*') && (current === '/')) return true;
            break;
    }
    return false;
}

// node_walk: walk the element tree, stop when func(node) returns false
function node_walk(node, func) {
    var result = func(node);
    for (node = node.firstChild; result !== false && node; node = node.nextSibling)
        result = node_walk(node, func);
    return result;
};

// getCaretPosition: return [start, end] as offsets to elem.textContent that
//   correspond to the selected portion of text
//   (if start == end, caret is at given position and no text is selected)
function getCaretPosition(elem) {
    var sel = window.getSelection();
    var cum_length = [0, 0];

    if (sel.anchorNode == elem)
        cum_length = [sel.anchorOffset, sel.extentOffset];
    else {
        var nodes_to_find = [sel.anchorNode, sel.extentNode];
        if (!elem.contains(sel.anchorNode) || !elem.contains(sel.extentNode))
            return undefined;
        else {
            var found = [0, 0];
            var i;
            node_walk(elem, function (node) {
                for (i = 0; i < 2; i++) {
                    if (node == nodes_to_find[i]) {
                        found[i] = true;
                        if (found[i == 0 ? 1 : 0])
                            return false; // all done
                    }
                }

                if (node.textContent && !node.firstChild) {
                    for (i = 0; i < 2; i++) {
                        if (!found[i])
                            cum_length[i] += node.textContent.length;
                    }
                }
            });
            cum_length[0] += sel.anchorOffset;
            cum_length[1] += sel.extentOffset;
        }
    }

    if (cum_length[0] <= cum_length[1])
        return cum_length;
    return [cum_length[1], cum_length[0]];
}

function SetCaretPosition(el, pos) {

    // Loop through all child nodes
    for (var node of el.childNodes) {
        if (node.nodeType == 3) { // we have a text node
            if (node.length >= pos) {
                // finally add our range
                var range = document.createRange(),
                    sel = window.getSelection();
                range.setStart(node, pos);
                range.collapse(true);
                sel.removeAllRanges();
                sel.addRange(range);
                return -1; // we are done
            } else {
                pos -= node.length;
            }
        } else {
            pos = SetCaretPosition(node, pos);
            if (pos == -1) {
                return -1; // no need to finish the for loop
            }
        }
    }
    return pos; // needed because of recursion stuff
}
function instructionToggle(x) {
    x.classList.toggle("fa-arrow-down");
}

//-----------------line numeration---------------------------------------


function NumerateLines() {
    var elem = document.getElementById('txtCode1');
    var divHeight = elem.scrollHeight -
        parseFloat(window.getComputedStyle(elem, null).getPropertyValue('padding-top')) -
        parseFloat(window.getComputedStyle(elem, null).getPropertyValue('padding-bottom'));

    var linesDiv = document.getElementById('lines');
    linesDiv.innerHTML = ""; //remove previous lines

    var firstLine = AddNewLine(linesDiv, 1);
    var lines = elem.innerHTML.split('\n');
    var numberOfLines = lines.length;

    for (var i = 2; i <= numberOfLines; i++) {
        AddNewLine(linesDiv, i);
    }
    linesDiv.scrollTop = elem.scrollTop;

    ShowErrors();
    UnderlineErrorLines(elem);
}

function ShowErrors() {
    if ((errors == null) || (errors.length == 0))
        return;

    var divs = document.getElementById('lines').children;
    for (var i = 0; i < errors.length; i++) {
        if (errors[i].type != "error") continue;

        var line = errors[i].line;
        if (line < 1 || line > divs.length)
            continue;

        var lineDiv = divs[line - 1];
        lineDiv.className += " text-danger";
        lineDiv.textContent = "*";
    }
}

function AddNewLine(parentDiv, lineNumber) {
    var newLine = document.createElement("div");
    newLine.appendChild(document.createTextNode(lineNumber));
    newLine.className = "numerated-line";
    parentDiv.appendChild(newLine);
    return newLine;
}

function OnScroll(div) {
    var linesDiv = document.getElementById('lines');
    linesDiv.scrollTop = div.scrollTop;
}
//------------------------------Download Function----------------------------
function DownloadSubmitedFile(fileType) {
    //var base64Result = SubmittedFIle;
    var anchor_href;
    var fileName;
    if (fileType == "Excel") {
        anchor_href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + SubmittedFIle;
        fileName = "ExcelFile.xlsx";
    }
    else if (fileType == "Image") {
        anchor_href = "data:image/png;base64," + SubmittedFIle;
        fileName = "imageFile.png";
    }
    const exportLinkElement = document.createElement('a');
    exportLinkElement.hidden = true;
    exportLinkElement.download = fileName;
    exportLinkElement.href = anchor_href;
    exportLinkElement.text = "downloading...";

    document.body.appendChild(exportLinkElement);
    exportLinkElement.click();
    exportLinkElement.remove();

}

//-----------------------------------------------------
function SubmitFile() {
    if (IsFileUploadProblem) {
        const Upfile = document.getElementById("fileUpload").files;
        if (Upfile.length == 0) {
            document.getElementById("lblFileUploadError").innerHTML = "The File field cannot be left blank.<br/> Please submit a supported file.";
            return false;
        }
        else {
            document.getElementById("lblFileUploadError").innerText = "";
            if (isValidFileType()) {
                RunCode();
                FileLoadAfterSubmit();
            } else {
                var extension = document.getElementById("FileExtension").innerText;
                document.getElementById("lblFileUploadError").innerHTML = "The File extension is not supported. <br/> Please submit a " + extension + " file.";
            }
        }
    }
}

function isValidFileType() {
    const fileType = document.getElementById("fileUpload").files[0].type;
    var fileName = document.getElementById("fileUpload").files[0].name;
    var index = fileName.split(".").length - 1
    var type2 = fileName.split(".")[index];
    if (language == "Tableau") {
        return (type2 == "tbw");
    } else if (language == "Excel") {
        return (fileType == "application/vnd.ms-excel"
            || fileType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || type2 == "xlsx" || type2 == "xls");
    } else if (language == "Image") {
        return (fileType == "image/jpeg" || fileType == "image/png" || type2 == "jpg" || type2 == "png");
    } else if (language == "HTML") {
        return (fileType == "text/html" || type2 == "html");
    } else {
        return false;
    }
}

function FileLoadAfterSubmit() {
    var file = "";
    var fr = "";
    const files = document.getElementById("fileUpload").files;
    if (files.length > 0 && (language == "Tableau" || language == 'HTML')) {
        file = files[0];
        fr = new FileReader();
        fr.readAsText(file);
        fr.onload = fcontent => {
            SubmittedFIle = fcontent.target.result;
        }
    } else if (files.length > 0 && (language == "Excel" || language == "Image")) {

        file = files[0];
        fr = new FileReader();
        fr.readAsDataURL(file);
        fr.onload = fcontent => {
            SubmittedFIle = GetFileBase64String(fcontent.target.result);
        }
    }
}

function SupportedFileExtension() {
    if (language == "Tableau") {
        return "tbw";
    } else if (language == "Excel") {
        return "xlsx, xls";
    } else if (language == "Image") {
        return "jpg, png";
    } else if (language == "HTML") {
        return "html";
    } else {
        return "";
    } 
}
