import React, { useEffect, useState } from "react";
import './EditableTable.css';
import { faPlus } from '@fortawesome/free-solid-svg-icons'
import IconButton from '../IconButton/IconButton'
import TestLine from "./TestLine";

const EditableTable = ({ items, onAdd, handleDelete, handelChange, handleNewRow}) => {
    
    const onChange = (index, propertyName, newValue) => {
        handelChange(index, propertyName, newValue);
    }
   
    const onDelete = (index) => {
        handleDelete(index);
    }

    const addRow = (index) => {
        handleNewRow(index);
    }

    const testLines = items.map((item, index) => {
        return (<TestLine index={index} handleChange={onChange} testItem={item} active={true} handleDelete={onDelete} handleNewRow={addRow} />);
    });



    return (
        <table border="1">
            <tbody>
                <tr>
                    <th>Test Name</th>
                    <th>Test result</th>
                    <th>Expected result</th>
                    <th>Ok message</th>
                    <th>Error message</th>
                    <th >Test amount</th>
                    <td colSpan="2"/>
                </tr>
                {testLines}
            </tbody>
        </table>
    );
};

export default EditableTable;