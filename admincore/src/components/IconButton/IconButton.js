import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

const IconButton = ({icon, onClick}) => {
  return (
      <button type="button" className="btn" onClick={onClick}><FontAwesomeIcon icon={icon} /></button>
  );
};

export default IconButton;
