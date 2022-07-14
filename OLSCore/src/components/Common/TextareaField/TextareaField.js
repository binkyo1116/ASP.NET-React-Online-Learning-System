import React from 'react';

// assets
import WarningIcon from '../../../assets/icons/warning.svg';

// styles
import './TextareaField.css'

const TextareaField = ({ className, name, error, onChange, placeholder, value, rows }) => {

  const isError = error !== '';
  let inputStyle = !isError ? 'inpfTextarea' : 'inpfTextarea error';
  
  return (
    <div>
      <div>
        <textarea
          className={inputStyle + " " + className}
          name={name}
          placeholder = {placeholder}
          onChange={onChange}
          rows={rows}
          value={value}
        >
        </textarea>
      </div>
      {isError && <div className="inpfTextareaErrorLabel"><img src={WarningIcon} alt="warn icon"/>{error}</div>}
    </div>
  );
};

export default TextareaField;
