import React from "react";
import { useState, useEffect, useRef } from "react";
import { Form, Row, Col, Accordion, Button, Modal, ButtonGroup } from "react-bootstrap";
import { faRedo, faListOl, faCheckCircle, faExclamationCircle, faStream } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { API_URL, VALIDATION_API_URL} from "../../Global"
import Select from "../Select/Select";
import CodeBox from "../CodeBox/CodeBox";
import Variables from "../Variables/Variables";
import TextEditor from "../TextEditor/TextEditor"
import EditableTable from "../EditableTable/EditableTable";
import Loader from "../Loader/Loader";
import "./CodingProblem.css";

const roleValues = ["Assessment", "Final", "Midterm"];

const CodingProblem = ({ id , clear, handleClear}) => {
    const [codingProblem, setCodingProblem] = useState({});
    const [from, setFrom] = useState("add");
    const [mutex, setMutex] = useState(true);
    const [message, setMessage] = useState({});
    const [listVariables, setListVariables] = useState({});
    const [counter, setCounter] = useState(1);
    const [tests, setTests] = useState([{ testName: '', testResult: '', expectedResult: '', okMessage: '', errorMessage: '', testAmount: '' }]);
    const [fileContent, setFileContent] = useState("");
    const [templates, setTemplates] = useState([]);
    const [languageData, setLanguageData] = useState({});

    //Code here button states
    const [scriptPosition, setScriptPosition] = useState({});
    const [positionRequired, setPositionRequired] = useState(true);

    //More Details Modal
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const [testTemplateDescription, setTestTemplateDescription] = useState("");
    const [testTemplate, setTestTemplate] = useState({});


    //Loaders
    const [showSubmit, setShowSubmit] = useState(false);
    const [showUpdate, setShowUpdate] = useState(false);
    const [showTest, setShowTest] = useState(false);
    const [disableSubmit, setDisableSubmit] = useState(false);
    const [disableUpdate, setDisableUpdate] = useState(false);
    const [disableTest, setDisableTest] = useState(false);

    // Resizable column

    const sidebarRef = useRef(null);
    const [isResizing, setIsResizing] = useState(false);
    const [sidebarWidth, setSidebarWidth] = useState(550);

    const startResizing = React.useCallback((mouseDownEvent) => {
        setIsResizing(true);
    }, []);

    const stopResizing = React.useCallback(() => {
        setIsResizing(false);
    }, []);

    const resize = React.useCallback(
        (mouseMoveEvent) => {
            let resizingValue = mouseMoveEvent.clientX - sidebarRef.current.getBoundingClientRect().left;
            if (isResizing && resizingValue > 162 && resizingValue < 1000) {
                setSidebarWidth(resizingValue);
            }
        },
        [isResizing]
    );

    useEffect(() => {
        window.addEventListener("mousemove", resize);
        window.addEventListener("mouseup", stopResizing);
        return () => {
            window.removeEventListener("mousemove", resize);
            window.removeEventListener("mouseup", stopResizing);
        };
    }, [resize, stopResizing]);

    //Handle functions
    const handleChange = (e) => {
        if (mutex && (Object.keys(codingProblem).length > 0 || !id)) {
            let copyCodingProblem = { ...codingProblem };
            if (e.target.name == "Active") {
                copyCodingProblem.Active = !copyCodingProblem.Active;
            } else if (e.target.name == "Role") {
                copyCodingProblem.Role = roleValues.indexOf(e.target.value);
            } else {
                copyCodingProblem[e.target.name] = e.target.value;
            }
            if (e.target.name === "Language") {
                onLanguageChange(e.target.value);
            }
            setCodingProblem(copyCodingProblem);
        }
   
    }

    const onLanguageChange = async(newLanguage) => {
        const templates = await getLanguageTestTemplate(newLanguage);
        setTemplates(templates);
        getStartEndCode(newLanguage);
    }

    //Upload file

    const GetFileBase64String = (base64Str) => {
        var substr = "base64,";
        var index = base64Str.indexOf(substr);
        if (index == -1) {
            return "";
        }
        return base64Str.substring(index + substr.length);
    }

    const uploadFile = (e) => {
        const files = e.target.files;
        let code = "";

        if (files.length > 0 && (codingProblem.Language == "Tableau" || codingProblem.Language == 'HTML')) {
            let file = files[0];
            let fr = new FileReader();
            fr.readAsText(file);
            fr.onload = fcontent => {
                code = fcontent.target.result;
                setFileContent(code);
                let copyCodingProblem = { ...codingProblem };
                copyCodingProblem.ExpectedOutput = code;
                setCodingProblem(copyCodingProblem);
            }

        } else if (files.length > 0 && codingProblem.Language == "Excel") {
            let file = files[0];
            let fr = new FileReader();
            fr.readAsDataURL(file);
            fr.onload = fcontent => {
                code = GetFileBase64String(fcontent.target.result);
                setFileContent(code);
                let copyCodingProblem = { ...codingProblem };
                copyCodingProblem.ExpectedOutput = code;
                setCodingProblem(copyCodingProblem);
            }
        }
    }

    const reloadExpectedOutputFile = () => {
        let newExpected = fileContent ? fileContent : "";
        let copyCodingProblem = { ...codingProblem };
        copyCodingProblem.ExpectedOutput = newExpected;
        setCodingProblem(copyCodingProblem);
    }

    const setGrades = () => {
        let expected = codingProblem.ExpectedOutput;
        if (expected) {
            let expectedLines = expected.split('\n').filter(line => line.replace('\r', "") != "");
            if (expectedLines.length == 0) return;

            const maxGrade = codingProblem.MaxGrade ? codingProblem.MaxGrade : 100;

			var lineGrade = Math.floor(maxGrade / expectedLines.length)

            var newExpected = "";

            for (var i = 0; i < expectedLines.length; i++) {
                let line = expectedLines[i];

                if (i == expectedLines.length - 1 && maxGrade % expectedLines.length > 0)
					lineGrade = maxGrade % expectedLines.length;
                const dashIndex = line.indexOf('-');
				let newLine = "";

                if (dashIndex >= 0) {
                    const number = parseInt(line.substring(0, dashIndex));
                    if (isNaN(number)) newLine = line.trim();
                    else newLine = line.substring(dashIndex + 1).trim();
                } else {
                    newLine = line.trim();
                }

                newExpected += lineGrade + " - " + newLine + (i < expectedLines.length - 1 ? "\n" : "");
            }
        }

        let copyCodingProblem = { ...codingProblem };
        copyCodingProblem.ExpectedOutput = newExpected;
        setCodingProblem(copyCodingProblem);
    }

    //Code here button

    const setStart = () => {
        setPositionRequired(false);
    }

    useEffect(() => {
        let copyCodingProblem = { ...codingProblem };
        if (copyCodingProblem.Script) {
            let value = copyCodingProblem.Script.split('\n');
            if (value && scriptPosition && languageData && languageData.CodeStart && languageData.CodeEnd && value.length > scriptPosition.row) {
                value[scriptPosition.row] = value[scriptPosition.row].slice(0, scriptPosition.column) + "\n" + languageData.CodeStart + "\n \n" + languageData.CodeEnd + "\n" + value[scriptPosition.row].slice(scriptPosition.column);
            }
            copyCodingProblem.Script = value.join('\n');
        }
        else {
            copyCodingProblem.Script = languageData.CodeStart + "\n \n" + languageData.CodeEnd;
        }
        setCodingProblem(copyCodingProblem);
        setPositionRequired(true);
    }, [scriptPosition])

    const handleStartEndCode = (position) => {
        if (!positionRequired) {
            setScriptPosition(position);
        }
    }

    //Check coding problem data
    const codingProblemValidation = (cp) => {
        let errors = [];

        if (cp.Title == "") errors.push("Title");
        if (cp.Instructions == "") errors.push("Instructions");
        if (cp.Type == "") errors.push("Type");
        if (cp.Type == "code" && cp.Solution == "") errors.push("Solution");
        if (cp.Type == "file" && cp.ExpectedOutput == "") errors.push("Expected file");
        if (cp.Language == "") errors.push("Language");
        if (cp.Role == -1) errors.push("Role");


        return errors;
    }

    //update a coding problem
    const onClickUpdate = () => {
        setShowUpdate(true);
        setDisableUpdate(true);
        setMessage({});

        let variables = {};
        for (const v in listVariables) {
            if (listVariables[v].active)
                variables[v] = listVariables[v].value;
        }

        let newCodingProblem = {
            Instructions: (codingProblem.Instructions) ? codingProblem.Instructions : "",
            Script: (codingProblem.Script) ? codingProblem.Script : "",
            Solution: (codingProblem.Solution) ? codingProblem.Solution : "",
            ClassName: "",
            MethodName: "",
            ParameterTypes: "",
            Language: (codingProblem.Language) ? codingProblem.Language : "",
            TestCaseClass: "",
            Before: (codingProblem.Before) ? codingProblem.Before : "",
            After: (codingProblem.After) ? codingProblem.After : "",
            MaxGrade: (codingProblem.MaxGrade) ? codingProblem.MaxGrade : 100,
            Title: (codingProblem.Title) ? codingProblem.Title : "",
            Type: (codingProblem.Type) ? codingProblem.Type : "",
            Attempts: (codingProblem.Attempts) ? codingProblem.Attempts : 100,
            Active: (codingProblem.Active) ? codingProblem.Active : "",
            Role: (codingProblem.Role >= 0) ? codingProblem.Role : -1,
            ExpectedOutput: (codingProblem.ExpectedOutput) ? codingProblem.ExpectedOutput : "",
            Parameters: "",
            TestCode: (codingProblem.TestCode) ? codingProblem.TestCode : "",
            TestCodeForStudent: (codingProblem.TestCodeForStudent) ? codingProblem.TestCodeForStudent : "",
            VarValuePairs: variables
        };

        let validationErrors = codingProblemValidation(newCodingProblem);
        const correct = validationErrors.length == 0;

        let status = 0;
        let obtainedData = {};

        if (correct) {
            const requestOptions = {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newCodingProblem)
            };
            fetch(API_URL + 'CodingProblem/' + id, requestOptions)
                .then(response => {
                    status = response.status;
                    return response.json();
                })
                .then(data => {

                    obtainedData.success = status == 200;
                    obtainedData.output = data;
                    obtainedData.ready = true;
                    setMessage(obtainedData);

                    setShowUpdate(false);
                    setDisableUpdate(false);
                });
        }
        else {
            obtainedData.success = false;
            obtainedData.output = "Sorry! Operation has been failed. Required Fields: ";
            validationErrors.forEach(e => obtainedData.output += (e + ' - '));
            obtainedData.ready = true;
            setMessage(obtainedData);
            setShowUpdate(false);
            setDisableUpdate(false);
        }
    }

    //Create new coding problem
    const onClickAdd = (e) => {
        setShowSubmit(true);
        setDisableSubmit(true);
        setMessage({});

        let variables = {};
        for (const v in listVariables) {
            if (listVariables[v].active)
                variables[v] = listVariables[v].value;
        }

        let newCodingProblem = {
            Instructions: (codingProblem.Instructions) ? codingProblem.Instructions : "",
            Script: (codingProblem.Script) ? codingProblem.Script : "",
            Solution: (codingProblem.Solution) ? codingProblem.Solution : "",
            ClassName: "",
            MethodName: "",
            ParameterTypes: "",
            Language: (codingProblem.Language) ? codingProblem.Language : "",
            TestCaseClass: "",
            Before: (codingProblem.Before) ? codingProblem.Before : "",
            After: (codingProblem.After) ? codingProblem.After : "",
            MaxGrade: (codingProblem.MaxGrade) ? codingProblem.MaxGrade : 100,
            Title: (codingProblem.Title) ? codingProblem.Title : "",
            Type: (codingProblem.Type) ? codingProblem.Type : "",
            Attempts: (codingProblem.Attempts) ? codingProblem.Attempts : 100,
            Active: (codingProblem.Active) ? codingProblem.Active : "",
            Role: (codingProblem.Role >= 0) ? codingProblem.Role : -1,
            ExpectedOutput: (codingProblem.ExpectedOutput) ? codingProblem.ExpectedOutput : "",
            Parameters: "",
            TestCode: (codingProblem.TestCode) ? codingProblem.TestCode : "",
            TestCodeForStudent: (codingProblem.TestCodeForStudent) ? codingProblem.TestCodeForStudent : "",
            VarValuePairs: variables
        };

        let validationErrors = codingProblemValidation(newCodingProblem);
        const correct = validationErrors.length == 0;

        let status = 0;
        let obtainedData = {};

        if (correct) {
            const requestOptions = {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newCodingProblem)
            };
            fetch(API_URL + 'CodingProblem', requestOptions)
                .then(response => {
                    status = response.status;
                    return response.json()
                })
                .then(data => {
                    obtainedData.success = status == 200;
                    obtainedData.output = data;
                    obtainedData.ready = true;
                    setMessage(obtainedData);

                    setShowSubmit(false);
                    setDisableSubmit(false);
                });
        }
        else {
            obtainedData.success = false;
            obtainedData.output = "Sorry! Operation has been failed. Required Fields: ";
            validationErrors.forEach(e => obtainedData.output += (e + ' - '));
            obtainedData.ready = true;
            setMessage(obtainedData);
            setShowSubmit(false);
            setDisableSubmit(false);
        }
    }


    //Test the coding problem's changes
    const onClickTest = () => {
        setMessage({});
        setShowTest(true);
        setDisableTest(true);
        let variables = {};
        for (const v in listVariables) {
            if (listVariables[v].active)
                variables[v] = listVariables[v].value;
        }

        let newCodingProblem = {
            Instructions: (codingProblem.Instructions) ? codingProblem.Instructions : "",
            Script: (codingProblem.Script) ? codingProblem.Script : "",
            Solution: (codingProblem.Solution) ? codingProblem.Solution : "",
            ClassName: "",
            MethodName: "",
            ParameterTypes: "",
            Language: (codingProblem.Language) ? codingProblem.Language : "",
            TestCaseClass: "",
            Before: (codingProblem.Before) ? codingProblem.Before : "",
            After: (codingProblem.After) ? codingProblem.After : "",
            MaxGrade: (codingProblem.MaxGrade) ? codingProblem.MaxGrade : 100,
            Title: (codingProblem.Title) ? codingProblem.Title : "",
            Type: (codingProblem.Type) ? codingProblem.Type : "",
            Attempts: (codingProblem.Attempts) ? codingProblem.Attempts : 100,
            Active: (codingProblem.Active) ? codingProblem.Active : "",
            Role: (codingProblem.Role >= 0) ? codingProblem.Role : -1,
            ExpectedOutput: (codingProblem.ExpectedOutput) ? codingProblem.ExpectedOutput : "",
            Parameters: "",
            TestCode: (codingProblem.TestCode) ? codingProblem.TestCode : "",
            VarValuePairs: variables
        };

        let validationErrors = codingProblemValidation(newCodingProblem);
        const correct = validationErrors.length == 0;
        let obtainedData = {};
        if (correct) {
            const requestOptions = {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newCodingProblem)
            };
            fetch(VALIDATION_API_URL + 'ValidateCodingProblem', requestOptions)
                .then(response => response.json())
                .then(data => {
                    console.log({ response: data });
                    
                    obtainedData.success = (data.Succeeded && data.Compiled);
                    obtainedData.output = obtainedData.success ? "OK!" : "Error: ";
                    if (data.Message && data.Message.length > 0) {
                        obtainedData.content = "";
                        if (Array.isArray(data.Message)) {
                            for (var i = 0; i < data.Message.length; i++) {
                                obtainedData.content += data.Message[i].trim();
                                obtainedData.content += (i < data.Message.length - 1) ? '\n' : "";
                            }
                        }
                        else {
                            obtainedData.content = data.Message;
                        }
                    }
                   
                    obtainedData.script = data.allCode;
                    obtainedData.ready = true;
                    setMessage(obtainedData);
                    setShowTest(false);
                    setDisableTest(false);
                });
        }
        else {
            obtainedData.success = false;
            obtainedData.output = "Sorry! Operation has been failed. Required Fields: ";
            validationErrors.forEach(e => obtainedData.output += (e + ' - '));
            obtainedData.ready = true;
            setMessage(obtainedData);
            setShowTest(false);
            setDisableTest(false);
        }
    }

    const getCodingProblem = async () => {
        setMutex(false);
        setCodingProblem({});
        try {
            const url = API_URL +"codingproblem/" + id;
            const response = await fetch(url);
            const data = await response.json();
            let obtainedCodingProblem = { ...data.CodingProblem}
            onLanguageChange(obtainedCodingProblem.Language);
            if (data.CodingProblem.Active == "true") obtainedCodingProblem.Active = true;
            else if (data.CodingProblem.Active == "false") obtainedCodingProblem.Active = false;
            obtainedCodingProblem.Role = parseInt(data.CodingProblem.Role);
            let codingProblemVariables = {};
            if (data.CodingProblem.VariableValues) {
                for (var v in data.CodingProblem.VariableValues.VariableValueElement) {
                    let varValue;
                    if (Array.isArray(data.CodingProblem.VariableValues.VariableValueElement)) {
                        varValue = data.CodingProblem.VariableValues.VariableValueElement[v];
                    }
                    else {
                        varValue = data.CodingProblem.VariableValues.VariableValueElement;
                    }
                    codingProblemVariables[varValue.Name] = {
                        name: varValue.Name, value: varValue.PossibleValues, expression: "${" + varValue.Name + "}", type: 0, id: counter, permanent: true, active: true, index: {}
                    }
                    setCounter(counter + 1);
                }
                setListVariables(codingProblemVariables);
            }
            setCodingProblem(obtainedCodingProblem);
            console.log(obtainedCodingProblem);
            setMutex(true);
        } catch (error) {
            console.error(error);
            setMutex(true);
        }
    }

    const getStartEndCode = async (language) => {
        try {
            if (language) {
                const url = API_URL + "Languages?name=" + encodeURIComponent(language);
                const response = await fetch(url);
                const data = await response.json();
                let obtainedLanguage = { ...data.Language };
                setLanguageData(obtainedLanguage);
            }
        } catch (error) {
            console.error(error);
        }
    }

    // To update variable table with the codebox new ones
    const handleChangeCodeBox = (variables, e) => {
        //run onChange
        handleChange(e);

        let resultantVariables = [];

        //Update existent ones
        for (const v in listVariables) {
            if (variables[listVariables[v].name]) {
                resultantVariables[v] = { ...listVariables[v] };
                resultantVariables[v].index[e.target.name] = variables[listVariables[v].name].index[e.target.name];
                resultantVariables[v].type = variables[listVariables[v].name].type;
                resultantVariables[v].expression = variables[listVariables[v].name].expression;
                variables[listVariables[v].name].considered = true;
            }
            else if (listVariables[v].permanent) {
                resultantVariables[v] = { ...listVariables[v] };
                if (resultantVariables[v].index[e.target.name]) {
                    delete resultantVariables[v].index[e.target.name];
                }
            }
            //Check if exists on some other textbox
            else if (Object.keys(listVariables[v].index).length > 0) {
                resultantVariables[v] = { ...listVariables[v] };
                delete resultantVariables[v].index[e.target.name];
                if (Object.keys(resultantVariables[v].index).length == 0) {
                    delete resultantVariables[v];
                }
            }
        }
        //Create new ones
        for (const v in variables) {
            if (!variables[v].considered) {
                resultantVariables[variables[v].name] = variables[v];
                resultantVariables[variables[v].name].id = counter;
                setCounter(counter + 1);
            }
        }
        setListVariables(resultantVariables);

    }

    // Add to the state new variables created in the table
    const handleChangeVariable = (variable, value, id, type, isDelete) => {
        let copyVariables = { ...listVariables };
        //If isDelete, I delete it
        if (isDelete) {
            copyVariables[variable].active = false;
        }
        //Else If variable doesnt exist (or it is deactivated), I create it
        else if (!copyVariables[variable] || !copyVariables[variable].active) {

            let varExpression = "";
            if (type == 0)
                varExpression = "${" + variable + "}";
            else if (type == 1)
                varExpression = "${{" + variable + "}}";
            else if (type == 2)
                varExpression = "${" + variable + ",0}";
            else if (type == 3)
                varExpression = "${{" + variable + ",0}}";
            else if (type == 4)
                varExpression = "${" + variable + "[0]}";
            else if (type == 5)
                varExpression = "${{" + variable + "[0]}}";

            copyVariables[variable] = {
                name: variable,
                value: value,
                expression: copyVariables[variable] ? copyVariables[variable].expression : varExpression,
                type: copyVariables[variable] ? copyVariables[variable].type : type,
                id: counter,
                permanent: true,
                active: true,
                index: copyVariables[variable] ? copyVariables[variable].index : {}
            };
            setCounter(counter + 1);
        }
        //Else I update it
        else {
            copyVariables[variable].value = value;
            copyVariables[variable].permanent = true;
        }
        //Refresh state
        setListVariables(copyVariables);
    }

    const onTestChange = (index, propertyName, newValue) => {
        let listTests = [...tests];
        listTests[index][propertyName] = newValue;
        setTests(listTests);
    }
    const onDeleteTest = (index) => {
        if (tests.length > 1) {
            let listTests = [...tests];
            listTests.splice(index, 1);
            setTests(listTests);
        }
    }
    const addNewRow = () => {
        let listTests = [...tests];
        const test = {
            testName: '',
            testResult: '',
            expectedResult: '',
            okMessage: '',
            errorMessage: '',
            testAmount: ''
        };
        listTests.push(test);
        setTests(listTests);
    }


    // Test code

    const assignGeneretedTestCode = () => {
        const testCode = createTestCode();
        const codingProblemCopy = {...codingProblem};
        codingProblemCopy.TestCode = testCode;
        setCodingProblem(codingProblemCopy);
    }

    const createTestCode = () => {
        const regex = new RegExp(testTemplate.generatedCodeStart + '(.*)' + testTemplate.generatedCodeEnd, 's');
        const singleTestTemplate = testTemplate.template.match(regex)[1];
        const evaluationFunction = createFunctionForEvaluation(singleTestTemplate);
        let resultTests = "";
        tests.forEach((test) => {
            for (let idx = 1; idx <= test.testAmount; idx++) {
                resultTests = resultTests
                    + evaluationFunction(
                        test.testName.replace("<i>", idx), test.expectedResult.replace("<i>", idx),
                        test.testResult.replace("<i>", idx), test.errorMessage.replace("<i>", idx),
                        test.okMessage.replace("<i>", idx));
            }
        });

        return testTemplate.template.replace(regex, resultTests);
    }

    const createFunctionForEvaluation = (singleTestTemplate) => {
        return Function(`return (nombre_test, res_esperado, res_obtenido, texto_error, texto_ok) => \`
            ${singleTestTemplate}
        \``)();
    }

    const getLanguageTestTemplate = async (language) => {
        try {
            if (language) {
                const url = API_URL + "TestTemplate?languageName=" + encodeURIComponent(language);
                const response = await fetch(url);
                const data = await response.json();
                return [...data.map(d => d.TestTemplateElement)];
            }
        } catch (error) {
            console.error(error);
        }
    }

    const onTemplateChange = (e) => {
        const testTemplate = templates.find(t => t.description === e.target.value);
        setTestTemplate(testTemplate);
        setTestTemplateDescription(e.target.value);
    }

    //Effects
    useEffect(() => {
        if (id && id > 0) {
            setMessage({});
            getCodingProblem();
            setFrom("update");
        } else {
            setCodingProblem({});
        }
    }, [id]);

    useEffect(() => {
        const cpc = {...codingProblem};
        setCodingProblem(cpc);
        setTestTemplateDescription("");
    }, [codingProblem.Language]);

    return (
        <div>
            <h5>Coding Problem</h5>
            <Form id="form" onSubmit={(from == "add") ? onClickAdd : onClickUpdate}>
                <Row>
                    <div className="column sidebar"
                         ref={sidebarRef}
                         style={{ width: (sidebarWidth-12) }}>
                        <Form.Group className="form_group">
                            <Form.Label>Title</Form.Label>
                            <Form.Control
                                type="text"
                                name="Title"
                                value={(codingProblem.Title) ? codingProblem.Title : ""}
                                onChange={handleChange}
                            />
                        </Form.Group>
                        <Row>
                            <Col md={4} lg={4}>
                                <Form.Group controlId="typeSelect" className="form_group">
                                    {from == "add" ?
                                        <Select
                                            title="Type"
                                            name="Type"
                                            options={["code", "file"]}
                                            onChange={handleChange}
                                            value={(codingProblem.Type) ? codingProblem.Type : ""}
                                        />
                                        :
                                        <div>
                                            <Form.Label>Type</Form.Label>
                                            <Form.Control
                                                type="text"
                                                value={(codingProblem.Type) ? codingProblem.Type : ""}
                                                readOnly
                                            />
                                        </div>
                                    }
                                </Form.Group>
                            </Col>
                            <Col md={4} lg={4}>
                                {(codingProblem.Type) ?
                                    <Form.Group controlId="languageSelect" className="form_group">
                                        <Select
                                            title="Language"
                                            name="Language"
                                            url={API_URL + "Languages"}
                                            value={(codingProblem.Language) ? codingProblem.Language : ""}
                                            onChange={handleChange}
                                        />
                                    </Form.Group> : ""}
                            </Col>
                            <Col md={4} lg={4}>
                                <Form.Group controlId="activeCheck" className="form_group">
                                    <Form.Check
                                        className="active_cb"
                                        name="Active"
                                        type="switch"
                                        id="activeCheck"
                                        label="Active"
                                        value="On"
                                        defaultChecked={codingProblem.Active}
                                        onChange={handleChange}
                                    />
                                </Form.Group>
                            </Col>
                            </Row><Row>
                            <Col md={4} lg={4}>
                                <Form.Group controlId="roleSelect" className="form_group">
                                    <Select
                                        title="Role"
                                        name="Role"
                                        options={roleValues}
                                        value={codingProblem.Role >= 0 ? roleValues[codingProblem.Role] : ""}
                                        onChange={handleChange}
                                    />
                                </Form.Group>
                            </Col>
                            <Col md={4} lg={4}>
                                <Form.Group controlId="attempts" className="form_group">
                                    <Form.Label>Attempts</Form.Label>
                                    <Form.Control
                                        name="Attempts"
                                        type="number"
                                        placeholder="Enter Attempts"
                                        value={(codingProblem.Attempts) ? codingProblem.Attempts : 100}
                                        onChange={handleChange}
                                    />
                                        </Form.Group>
                            </Col>
                            <Col md={4} lg={4}>
                                <Form.Group controlId="maxGrade" className="form_group">
                                    <Form.Label>MaxGrade</Form.Label>
                                    <Form.Control
                                        name="MaxGrade"
                                        type="number"
                                        placeholder="Enter Max Grade"
                                        value={(codingProblem.MaxGrade) ? codingProblem.MaxGrade : 100}
                                        onChange={handleChange}
                                    />
                                </Form.Group>
                            </Col>
                        </Row>

                        <Form.Group controlId="instructionsCodeBox" className="form_group">
                            <Form.Label>Instructions</Form.Label>
                            <div className="instructions">
                                <TextEditor
                                    name="Instructions"
                                    input={(codingProblem.Instructions) ? codingProblem.Instructions : ""}
                                    language={codingProblem.Language}
                                    onChange={handleChangeCodeBox}
                                    variables={listVariables}
                                />
                            </div>
                        </Form.Group>

                        <Form.Group controlId="variables" className="form_group">
                            <Form.Label>Variables</Form.Label>
                            <Variables
                                handleChangeVariable={handleChangeVariable}
                                listVariables={listVariables}
                            />
                        </Form.Group>
                    </div>
                    <div className="resizer" style={{ width: 10 }} onMouseDown={startResizing} ></div>
                    <div className="column" style={{ width: 'calc(100% - ' + (sidebarWidth) + 'px)' }}>
                        {(codingProblem.Type && codingProblem.Type == "code") ?
                            <Form.Group controlId="beforeCodeBox">
                                <Accordion>
                                    <Accordion.Item eventKey="0">
                                        <Accordion.Header><Form.Label>Before</Form.Label></Accordion.Header>
                                        <Accordion.Body>
                                            <CodeBox
                                                name="Before"
                                                input={(codingProblem.Before) ? codingProblem.Before : ""}
                                                language={codingProblem.Language}
                                                onChange={handleChangeCodeBox}
                                                variables={listVariables}
                                            />
                                        </Accordion.Body>
                                    </Accordion.Item>
                                </Accordion>
                            </Form.Group> : ""}


                        {(codingProblem.Type && codingProblem.Type == "code") ?
                            <Form.Group controlId="scriptCodeBox">
                                <Accordion>
                                    <Accordion.Item eventKey="1">
                                        <Accordion.Header><Form.Label>Script</Form.Label></Accordion.Header>
                                        <Accordion.Body>
                                            <CodeBox
                                                name="Script"
                                                input={(codingProblem.Script) ? codingProblem.Script : ""}
                                                language={codingProblem.Language}
                                                onChange={handleChangeCodeBox}
                                                variables={listVariables}
                                                startEndCode={handleStartEndCode}
                                                positionRequired={positionRequired}
                                            />
                                            <Button
                                                className="codeboxButton"
                                                variant="light"
                                                onClick={setStart}
                                                disabled={!codingProblem.Language || codingProblem.Language == "" || Object.keys(languageData) == 0 }>
                                                <FontAwesomeIcon icon={faStream} /> Code here </Button>
                                        </Accordion.Body>
                                    </Accordion.Item>
                                </Accordion>
                        </Form.Group> : "" }

                        {(codingProblem.Type && codingProblem.Type == "code") ?
                            <Form.Group controlId="solutionCodeBox">
                                <Accordion>
                                    <Accordion.Item eventKey="2">
                                        <Accordion.Header><Form.Label>Solution</Form.Label></Accordion.Header>
                                        <Accordion.Body>
                                            <CodeBox
                                                name="Solution"
                                                input={(codingProblem.Solution) ? codingProblem.Solution : ""}
                                                language={codingProblem.Language}
                                                onChange={handleChangeCodeBox}
                                                variables={listVariables}
                                            />
                                        </Accordion.Body>
                                    </Accordion.Item>
                                </Accordion>
                        </Form.Group> : ""}

                        {(codingProblem.Type && codingProblem.Type == "code") ?
                            <Form.Group controlId="afterCodeBox">
                                <Accordion>
                                    <Accordion.Item eventKey="3">
                                        <Accordion.Header><Form.Label>After</Form.Label></Accordion.Header>
                                        <Accordion.Body>
                                            <CodeBox
                                                name="After"
                                                input={(codingProblem.After) ? codingProblem.After : ""}
                                                language={codingProblem.Language}
                                                onChange={handleChangeCodeBox}
                                                variables={listVariables}
                                            />
                                        </Accordion.Body>
                                    </Accordion.Item>
                                </Accordion>
                        </Form.Group> : ""}

                        {(codingProblem.Type && codingProblem.Type == "code") ?
                            <Form.Group controlId="testCodeForStudentBox">
                                <Accordion>
                                    <Accordion.Item eventKey="5">
                                        <Accordion.Header><Form.Label>Test Code for Student</Form.Label></Accordion.Header>
                                        <Accordion.Body >
                                            <CodeBox
                                                name="TestCodeForStudent"
                                                input={(codingProblem.TestCodeForStudent) ? codingProblem.TestCodeForStudent : ""}
                                                language={codingProblem.Language}
                                                onChange={handleChangeCodeBox}
                                                variables={listVariables}
                                            />
                                        </Accordion.Body>
                                    </Accordion.Item>
                                </Accordion>
                            </Form.Group> : ""}

                        {(codingProblem.Type && codingProblem.Type == "code") ?
                            <Form.Group controlId="testCodeBox">
                                <Accordion>
                                    <Accordion.Item eventKey="6">
                                        <Accordion.Header><Form.Label>Test Code</Form.Label></Accordion.Header>
                                        <Accordion.Body >
                                            <CodeBox
                                                name="TestCode"
                                                input={(codingProblem.TestCode) ? codingProblem.TestCode : ""}
                                                language={codingProblem.Language}
                                                onChange={handleChangeCodeBox}
                                                variables={listVariables}
                                            />
                                        </Accordion.Body>
                                    </Accordion.Item>
                                </Accordion>
                        </Form.Group> : ""}
                        {(codingProblem.Type && codingProblem.Type === "code")?
                            <Form.Group controlId="testCodeTable">
                            <Accordion>
                                <Accordion.Item eventKey="7">
                                    <Accordion.Header><Form.Label>Test Table</Form.Label></Accordion.Header>
                                    <Accordion.Body id="scrolling-body">
                                        <div className={"row"} style={{margin:"15px 0", minHeight: "115px"}}>
                                            <Col md={6} lg={6}>
                                                {(templates) ?
                                                    <Select
                                                        title="Template"
                                                        name="Template"
                                                        options={templates.map(t=>t.description)}
                                                        value={testTemplateDescription}
                                                        onChange={onTemplateChange}
                                                    /> : ""}
                                            </Col>
                                        </div>
                                        {(testTemplateDescription) ?
                                            (<div>
                                                <div className={"row"} style={{margin:"15px 0"}}>
                                                    <Col md={12} lg={12}>
                                                        <EditableTable handelChange={onTestChange} items={tests}
                                                                       handleDelete={onDeleteTest}
                                                                       handleNewRow={addNewRow}/>
                                                    </Col>
                                                </div>
                                                <div className={"row"} style={{margin:"15px 0"}}>
                                                    <Col md={4} lg={4}>
                                                        <Button className="submitButton" onClick={assignGeneretedTestCode}
                                                                type="button">Create Test Code</Button>
                                                    </Col>
                                                </div>
                                            </div>) : ""
                                        }
                                    </Accordion.Body>
                                </Accordion.Item>
                            </Accordion>

                            </Form.Group>: ""}

                        {(codingProblem.Type != 0 && codingProblem.Language && codingProblem.Type == "file") ?
                            <Form.Group controlId="uploadFile" className="form_group">
                                <Form.Label>Expected Output File</Form.Label>
                                <Form.Control type="file" name="fileInput" onChange={ uploadFile } />
                            </Form.Group>:""}

                        {((codingProblem.Type && (codingProblem.Type == "code" || (codingProblem.Type == "file" && codingProblem.Language != "Excel"))) ?
                            <Form.Group controlId="expectedCodeBox">
                            <Accordion>
                                <Accordion.Item eventKey="8">
                                        <Accordion.Header><Form.Label>Expected Output</Form.Label></Accordion.Header>
                                        <Accordion.Body>
                                            <CodeBox
                                                name="ExpectedOutput"
                                                input={(codingProblem.ExpectedOutput) ? codingProblem.ExpectedOutput : ""}
                                                language=""
                                                onChange={handleChangeCodeBox}
                                                variables={listVariables}
                                        />

                                        <ButtonGroup aria-label="Basic example">
                                            <Button
                                                className="codeboxButton"
                                                variant="light"
                                                onClick={setGrades}
                                                disabled={!codingProblem.ExpectedOutput || codingProblem.ExpectedOutput == ""}>
                                            <FontAwesomeIcon icon={faListOl} />  Set Grades </Button>

                                        {(codingProblem.Type == "file") &&
                                            <Button
                                                className="codeboxButton"
                                                variant="light"
                                                onClick={reloadExpectedOutputFile}
                                                disabled={!fileContent || fileContent == ""} >
                                            <FontAwesomeIcon icon={faRedo} />  Reload File </Button>
                                        }

                                        </ButtonGroup>

                                        </Accordion.Body>
                                    </Accordion.Item>
                                </Accordion>
                            </Form.Group>: "")}

                        {(codingProblem.Type && (codingProblem.Type == "code" || codingProblem.Type == "file")) ?
                        <Form.Group controlId="buttons" className="buttons d-flex flex-row">
                            {from == "add" ?
                                <Button type="button" onClick={ onClickAdd } className="submitButton" disabled={disableSubmit}><Loader show={showSubmit} />{!showSubmit && "Add"}</Button> :
                                <Button type="button" onClick={ onClickUpdate } className="submitButton" disabled={disableUpdate}><Loader show={showUpdate} />{!showUpdate && "Update"}</Button>}

                            {(codingProblem.Language == "Java" || codingProblem.Language == "C#" || codingProblem.Language == "C++"
                                || codingProblem.Language == "Python" || codingProblem.Language == "R") ?
                                <Button id="testButton" disabled={disableTest} onClick={onClickTest} type="button"><Loader show={showTest} />{!showTest && "Test"}</Button> : ""}

                            {message.ready &&
                                (message.success ?
                                <h6 id="message" className="greenText">
                                    <FontAwesomeIcon icon={faCheckCircle} /> { message.output }
                                    </h6> :
                                <h6 id="message" className="redText">
                                    <FontAwesomeIcon icon={faExclamationCircle} /> {message.output}
                                    {message.content && <a href="javascript:;" onClick={handleShow}> see more details </a>}
                                </h6>)}

                            <Modal className="detailsModal" show={show} onHide={handleClose} size="lg" scrollable={true}>
                                <Modal.Header closeButton>
                                    <Modal.Title>More details</Modal.Title>
                                </Modal.Header>
                                <Modal.Body>
                                    <div>
                                        {message.content && <pre>{message.content}</pre>}
                                    </div>
                                    {message.script &&
                                        <CodeBox
                                            name="details"
                                            input={(message.script) ? message.script : ""}
                                            language={codingProblem.Language}
                                            onChange={null}
                                            variables={{}}
                                        />}
                                </Modal.Body>
                                <Modal.Footer>
                                    <Button variant="secondary" onClick={handleClose}>
                                        Close
                                    </Button>
                                </Modal.Footer>
                            </Modal>

                        </Form.Group>:""}

                    </div>
                </Row>
            </Form>
        </div>
        );
}

export default CodingProblem;