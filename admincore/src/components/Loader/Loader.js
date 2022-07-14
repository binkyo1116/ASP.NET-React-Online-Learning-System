import React from "react";
import './Loader.css'

const Loader = ({ show }) => {

    return (
        <div>
            {show && <img id="loader" src={require('./loader.gif')} />}
        </div>
    );
}

export default Loader;