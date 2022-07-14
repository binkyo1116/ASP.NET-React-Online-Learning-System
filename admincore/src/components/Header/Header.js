import React from "react";
import { Row } from "react-bootstrap";
import { useState } from "react";
import { ADMIN_URL } from "../../Global"
import Hints from '../Hints/Hints';
import './Header.css'


const Header = ({ title }) => {

    const [show, setShow] = useState(false);

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);

    return (
        <div className="headerPage">
            <div className="buttonsRow d-flex justify-content-between">
                <div>
                    <a href={ADMIN_URL+"Admin"}>Home</a>
                </div>
                <div>
                    <a id="openHints" onClick={handleShow}>Hints</a>
                    <a href={ADMIN_URL + "AssignCodingProblemToCourseInstance"}>Assign Coding Problem to Course Instance</a>
                    <a onClick={() => window.location.reload(false)}>Clear All</a>
                    <a href={ADMIN_URL + "Logout"}>Logout</a>
                </div>
            </div>
            <Hints show={show} handleClose={handleClose}/>
            <Row className="pageTitle">
                <h3>{title}</h3>
            </Row>
        </div>
    );
}

export default Header;