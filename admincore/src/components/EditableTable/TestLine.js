import React, { useState } from 'react';
import { faTrash } from '@fortawesome/free-solid-svg-icons'
import { faPlus } from '@fortawesome/free-solid-svg-icons'
import IconButton from '../IconButton/IconButton';

const TestLine = ({ index, testItem, active, handleChange, handleDelete, handleNewRow }) => {
    const onChange = (e) => {
        handleChange(index, e.target.name, e.target.value);
    }

    const onDelete = (e) => {
        handleDelete(index);
    }

    const addRow = (e) => {
        handleNewRow(index);
    }


    return (
        <tr>
            <td><input name="testName" type="text" value={testItem.testName} onChange={onChange}/></td>
            <td><input name="testResult" type="text" value={testItem.testResult} onChange={onChange}/></td>
            <td><input name="expectedResult" type="text" value={testItem.expectedResult} onChange={onChange}/></td>
            <td><input name="okMessage" type="text" value={testItem.okMessage} onChange={onChange}/></td>
            <td><input name="errorMessage" type="text" value={testItem.errorMessage} onChange={onChange}/></td>
            <td><input name="testAmount" type="number" value={testItem.testAmount} onChange={onChange}/></td>
            {active &&
            <td><IconButton id="addButton" icon={faPlus} onClick={addRow} /></td>
            }
            {active &&
                <td><IconButton icon={faTrash} onClick={onDelete} /></td>
            }
        </tr>
    );
};

export default TestLine;
