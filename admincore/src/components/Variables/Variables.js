import React, {useState} from 'react';
import { faPlus } from '@fortawesome/free-solid-svg-icons'
import IconButton from '../IconButton/IconButton'
import VariableTable from '../VariableTable/VariableTable';
import './Variables.css';
const Variables = ({ handleChangeVariable, listVariables }) => {

    const [newVar, setNewVar] = useState("");
    const [varType, setVarType] = useState(0);
    const types = ["Simple Variable", "Without Memory", "MultiInstance", "Without Memory MultiInstance", "Tuple", "Without Memory Tuple"];
    const handleDeleteVariable = (variable) => {
        //handleChangeVariable receives name, value, id, isDelete
        handleChangeVariable(variable, "", -1, varType, true);
    }

    const hangleUpdateVariable = (variable, value, id) => {
        handleChangeVariable(variable, value, id, varType, false);
    }

    const addVariable = () => {
        if (newVar != "") {
            handleChangeVariable(newVar, "", -1, varType, false);
            setNewVar("");
        }
    }

    const handleChangeInput = (e) => {
        setNewVar(e.target.value);
    }

    return (
        <div>
            <input id="newVar" type="text" onChange={handleChangeInput} placeholder="Name" value={newVar}></input>
            <select id="newVarSelect" aria-label="Variable types" onChange={(e) => setVarType(e.target.value)}>
                    <option value="0">{types[0]}</option>
                    <option value="1">{types[1]}</option>
                    <option value="2">{types[2]}</option>
                    <option value="3">{types[3]}</option>
                    <option value="4">{types[4]}</option>
                    <option value="5">{types[5]}</option>
            </select>
            <IconButton id="addButton" icon={faPlus} onClick={addVariable} />
            <VariableTable variables={listVariables} handleChangeVariable={hangleUpdateVariable} handleDeleteVariable={handleDeleteVariable} types={types} />
        </div>
    );
};

export default Variables;
