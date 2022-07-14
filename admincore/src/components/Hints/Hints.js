import React from "react";
import { Modal, Button, Accordion } from "react-bootstrap";
import { useState, useEffect } from "react";
import Papa from 'papaparse';
import './Hints.css'
import csvFile from './files/hints.csv'

const Hints = ({ show, handleClose }) => {
    const [items, setItems] = useState([]);

    useEffect(() => {
        Papa.parse(csvFile, {
            download: true,
            complete: function (input) {
                const records = input.data;
                addHints(records);
            }
        });
    }, []);

    const addHints = (allRows) => {
        if (allRows && allRows.length > 0) {
            let accordionItems = [];
            for (var i = 1; i < allRows.length; i++) {
                var data = allRows[i];
                let pictureUrl = "./pictures/" + data[2];
                accordionItems.push(<Accordion.Item eventKey={i} key={i}>
                    <Accordion.Header><b>{data[1] + ':'} </b> {data[0]} </Accordion.Header>
                    <Accordion.Body>
                        <p><b>{"Syntax: " + data[0]}</b></p>
                        <p>{data[3]}</p>
                        <p>Example:</p>
                        <img className="hintsPicture" src={require('./pictures/' + data[2])} />
                    </Accordion.Body>
                </Accordion.Item>);
            }
            setItems(accordionItems);
        }
    }

    return (
        <Modal show={show} onHide={handleClose} size="lg">
            <Modal.Header closeButton>
                <Modal.Title>Metalanguage Hints</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Accordion>
                    {items.length > 0 ? items : <p>Hints file is not available</p>}
                </Accordion>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>
                    Close
                </Button>
            </Modal.Footer>
        </Modal>
    );
}

export default Hints;