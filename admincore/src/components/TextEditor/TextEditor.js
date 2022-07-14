import React, { Component, useState } from 'react';
import { CKEditor } from '@ckeditor/ckeditor5-react';
import DecoupledEditor from '@ckeditor/ckeditor5-build-decoupled-document';


import "./TextEditor.css"

import { useEffect } from "react";
import { text } from '@fortawesome/fontawesome-svg-core';

const TextEditor = ({ name, language, onChange, variables, input}) => {
	const [editorMode, setEditorMode] = useState("");

	const getVariables = (newValue) => {    
		//Regular Expresion to get variables
		const regexp = /\$\{((?:\{(?:\{.*?\}|.)*?\}|.)*?)\}/g;
		const vars = [...newValue.matchAll(regexp)];
		let lstVariables = {};
		for (let i = 0; i<vars.length; i++)
		{
			if (vars[i] && vars[i][1])
			{
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

    return (
		<CKEditor
			onReady={editor => {
				editor.ui.getEditableElement().parentElement.insertBefore(
					editor.ui.view.toolbar.element,
					editor.ui.getEditableElement()
				);
			}}
			onChange={(event, editor) => {
				const data = editor.getData();
				const variables = getVariables(data);
				onChange(variables, { target: { name: name, value: data } });
			}}
			editor={DecoupledEditor}
			data={input}
		/>
        );
}

export default TextEditor;