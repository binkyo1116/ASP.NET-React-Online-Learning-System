import React from 'react';
import { faTrash, faRedo, faCopy } from '@fortawesome/free-solid-svg-icons'
import IconButton from '../IconButton/IconButton';
import "./variableLine.css"

const VariableLine = ({id, name, value, expression, type, types, handleChange, handleDelete}) => {
  //types: 0: ordinary variable, 1: without memory, 2: multiInstance, 3: withoutMemoryMultiInstance, 4: tuples, 5: withoutMemoryTuple 

  
  const onNameChange = (e) => {
    handleChange(e.target.value,value, id);
  }
  const onValueChange = (e) => {
    handleChange(name, e.target.value, id);
  }
  const onDelete = (e) => {
    handleDelete(name);
  }

  const copyValue = async () => {
      await navigator.clipboard.writeText(expression);
  }

    return (
      <tr>
        <td>{name}</td>
        <td><input className="inputName" type="text" value={value} onChange={onValueChange}></input></td>
        <td>{types[type]}</td>
        <td><IconButton icon={faCopy} onClick={copyValue} /><IconButton icon={faTrash} onClick={onDelete} /></td>
      </tr>
  );
};

export default VariableLine;
