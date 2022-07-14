import React from 'react';
import VariableLine from '../VariableLine/VariableLine';
import './VariableTable.css';

const VariableTable = ({variables, handleChangeVariable, handleDeleteVariable, handleReactivateVariable, types}) => {

  const items = [];

  for (const v in variables) {
    const idVariable = "var"+v;
    const value = variables[v].value ? variables[v].value : "";
    if (variables[v].active) {
    items.push(<VariableLine key={idVariable}
                             name={variables[v].name}
                             value={value}
                             id={variables[v].id}
                             expression={variables[v].expression}
                             type={variables[v].type}
                             types={types}
                             handleChange={handleChangeVariable}
                             handleDelete={handleDeleteVariable}
                             handleReactivate={handleReactivateVariable} />);
    }
  }

    return (
      <div className="tableWrapper">
        <table id="table">
           <thead className="tableHeader">
              <tr>
                <th>Name</th>
                <th>Values</th>
                <th>Type</th>
                <th>Actions</th>
                    </tr>
           </thead>
           <tbody>
              {items.length != 0 ? items : <tr><td colSpan="4">No variables</td></tr>}
           </tbody>
        </table>
     </div>
  );
};

export default VariableTable;
