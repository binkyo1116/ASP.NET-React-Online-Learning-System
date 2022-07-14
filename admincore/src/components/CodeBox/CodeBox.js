import React, { useState, useRef, useEffect } from "react";
import AceEditor from 'react-ace';

// import mode-<language> , this imports the style and colors for the selected language.
import 'ace-builds/src-noconflict/ext-language_tools';
import 'ace-builds/src-noconflict/mode-java';
import 'ace-builds/src-noconflict/mode-sql';
import 'ace-builds/src-noconflict/mode-python';
import 'ace-builds/src-noconflict/mode-xml';
import 'ace-builds/src-noconflict/mode-c_cpp';
import 'ace-builds/src-noconflict/mode-csharp';
import 'ace-builds/src-noconflict/mode-r';

import "./CodeBox.css"


const CodeBox = ({ name, language, onChange, variables, input, startEndCode, positionRequired }) => {
	const [editorMode, setEditorMode] = useState("");
	const [markers, setMarkers] = useState([]);
	const [position, setPosition] = useState({});
	const aceEditor = useRef(null);

	const getVariables = (newValue) => {
		//Regular Expresion to get variables
		const regexp = /\$\{((?:\{(?:\{.*?\}|.)*?\}|.)*?)\}/g;
		const vars = [...newValue.matchAll(regexp)];
		let lstVariables = {};
		for (let i = 0; i < vars.length; i++) {
			if (vars[i] && vars[i][1]) {
				//types: 0: ordinary variable, 1: without memory, 2: multiInstance, 3: withoutMemoryMultiInstance, 4: tuples, 5: withoutMemoryTuple 
				let varName = vars[i][1];
				let type = 0;
				//Variable detected
				//Check if it has this format: ${{A}}
				const regWithoutMemory = /{([^$]*)? *}/g;
				const result = [...varName.matchAll(regWithoutMemory)];
				if (result.length != 0) {
					if (result[0] && result[0][1]) {
						varName = result[0][1];
						type = 1;
					}
				}
				const regMultiple = /([^$^{,]*), *(\d\d*)*/g;
				const resultM = [...varName.matchAll(regMultiple)];
				if (resultM.length != 0) {
					varName = resultM[0][1];
					if (type == 0) {
						//it has this format: ${A,1}
						type = 2;
					}
					else {
						//it has this format: ${{A,1}}
						type = 3;
					}
				}
				else {
					const regTuple = /(([^\$]*)?(\[\d+]|\[\d+],\d))/g;
					const resultT = [...varName.matchAll(regTuple)];
					if (resultT.length != 0) {
						varName = resultT[0][2];
						if (type == 0) {
							//it has this format: ${A[0]}
							type = 4;
						}
						else {
							//it has this format: ${{A[0]}}
							type = 5;
						}
					}
				}

				if (!lstVariables[varName]) {
					lstVariables[varName] = {
						name: varName, value: "", expression: vars[i][0], id: -1, permanent: false, active: true, index: {}, type: type
					};
					console.log(varName);
				}
				if (!lstVariables[varName].index[name]) {
					lstVariables[varName].index[name] = [];
				}
				lstVariables[varName].index[name].push(vars[i].index);
			}
		}
		return lstVariables;
	}

    useEffect(() => {
        var lang = "";
        if (language) {
            if (language === "C#") lang = "csharp";
            else if (language === "Cpp") lang = "c_cpp";
            else if (language === "Tableau") lang = "xml";
            else if (language === "Java" || language === "SQL" || language === "Python" || language === "R")
                lang = language.toLowerCase();
        }
        setEditorMode(lang);
    }, [language]);
	
	useEffect(() => {
		const paintVariables = () => {
			let marks = [];
			for (const variable in variables)
			{
				for (let i = 0; variables[variable].index[name] && i < variables[variable].index[name].length; i++)
				{
					if (variables[variable].active)
					{
						let subs = input.substring(0, variables[variable].index[name][i]);
						let enters = (subs.match(/\n/g) || []).length;
						let lastEnter = subs.lastIndexOf("\n");
						let start = variables[variable].index[name][i]-lastEnter-1;
						let end = start + variables[variable].expression.length;
						let varClassName = "variable_"+variable+" variable_marker_red";
				
						if(variables[variable].value)
						{
							varClassName = "variable_"+variable+" variable_marker_green";
						}
	  
						marks.push({startRow: enters, startCol: start, endRow: enters, endCol: end, className: varClassName, type: 'text' });
					}
				}
			}
			setMarkers(marks);
		}

		paintVariables();
	}, [variables]);

	useEffect(() => {
		if (startEndCode && !positionRequired && aceEditor && aceEditor.current && aceEditor.current.editor) {
			let pos = aceEditor.current.editor.getCursorPosition()
			startEndCode(pos);
		}
	}, [positionRequired])

	useEffect(() => {
		if (onChange && input) {
			const variables = getVariables(input);
			onChange(variables, { target: { name: name, value: input } });
		}
	}, [input])


	return (
		<AceEditor
			name={name}
			ref={aceEditor}
			onChange={(e) => {
				if (onChange && e != undefined) {
					const variables = getVariables(e);
					onChange(variables, { target: { name: name, value: e } });
				}
			}}
			fontSize={14}
			mode={editorMode}
			showPrintMargin={false}
			showGutter={true}
			highlightActiveLine={false}
			markers={markers}
			value={input}
			minLines={15}
            setOptions={{
                enableBasicAutocompletion: true,
				enableLiveAutocompletion: true,
				enableSnippets: true,
				maxLines: 20,
                showLineNumbers: true,
                tabSize: 4,
            }}
        />
        );
}

export default CodeBox;