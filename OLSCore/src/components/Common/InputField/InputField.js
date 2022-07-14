import React, {useState} from 'react';

// assets
import WarningIcon from '../../../assets/icons/warning.svg';
import EyeIcon from '../../../assets/icons/eye.svg';
import EyeSlashIcon from '../../../assets/icons/eye-slash.svg';

// styles
import './InputField.css'

const InputField = ({ className, label, leftIcon, hasRightIcon, showPassIcon, name, error, onChange, placeholder, value, type = '' }) => {

  const [showPass, setShowPass] = useState(false)
  const [inputType, setInputType] = useState(type)
  const isError = error !== '';
  let inputStyle = !isError ? 'inpfInput' : 'inpfInput error';
  let inputContainerStyle = 'input-container';
  if (leftIcon) {
    inputContainerStyle += ' has-left-icon'
  }
  if (hasRightIcon) {
    inputContainerStyle += ' has-right-icon'
  }
  let inputLeftIconStyle = 'left-icon';
  let inputRightIconStyle = 'right-icon';

  const onEyeIconClick = () => {
    if (showPass)
      setInputType("password")
    else
      setInputType("text")
    setShowPass(prev=>!prev)
  }
  
  return (
    <div className={className}>
      {label ? 
      <div className="inpfLabel">{label}</div>
      :""
      }
      <div className={inputContainerStyle }>
        {leftIcon ? <img src={leftIcon} alt="input icon" className={inputLeftIconStyle} /> : null}
        <input
          type={inputType}
          className={inputStyle}
          name={name}
          value={value}
          placeholder = {placeholder}
          onChange={onChange}
        />
        {/* {hasRightIcon && rightIcon ? <img src={rightIcon} alt="input icon" className={inputIconStyle} /> : null} */}
        {hasRightIcon && showPassIcon ? 
          showPass ? 
          <img src={EyeSlashIcon} alt="eye-slash icon" className={inputRightIconStyle} onClick={onEyeIconClick} />: 
          <img src={EyeIcon} alt="eye icon" className={inputRightIconStyle} onClick={onEyeIconClick} />
          : null}
      </div>
      {isError && <div className="inpfErrorLabel"><img src={WarningIcon} alt="warn icon"/>{error}</div>}
    </div>
  );
};

export default InputField;
