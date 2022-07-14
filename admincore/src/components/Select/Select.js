import React, {useState, useEffect } from "react";
import { Form, Row, Col } from "react-bootstrap";
import { Typeahead } from 'react-bootstrap-typeahead';
import Loader from "../Loader/Loader";
import 'react-bootstrap-typeahead/css/Typeahead.css';

const Select = ({ title, name, url, options, onChange, value }) => {

    const [options_list, setOptionsList] = useState([]);
    const [show, setShow] = useState(false);
    const [selected, setSelected] = useState([]);

    const getOptions = async () => {
        try {
            setShow(true);
            const response = await fetch(url);
            const data = await response.json()
            if (data && data.length > 0) {
                const key = Object.keys(data[0])
                let newOptions = data.map((e) => e[key]);
                setOptionsList(newOptions);
                if (value && value != "") setSelected([newOptions.find(e => e.Id == value)]);
            } else {
                setOptionsList([]);
            }
            setShow(false);
        } catch (error) {
            console.error(error);
        }
    }

    const onChangeSelected = (item) => {
        let e = {};
        if (item && item.length > 0)
            e = { target: { name: name, value: item[0].Id } };
        else
            e = { target: { name: name, value: 0 } };

        onChange(e);
        setSelected(item);
    }

    useEffect(() => {
        if (options) {
            let newOptions = options.map(e => { return { Id: e, Title: e } });
            setOptionsList(newOptions);
            if (value && value != "") setSelected([newOptions.find(e => e.Id == value)]);
        }
    }, [options, value]);

    useEffect(() => {
        if (url) getOptions();
    }, [url, value]);

    return (
        <div>
            <Form.Label className="d-flex flex-row">
                {title} <Loader show={show} />
            </Form.Label>
            <Typeahead
                clearButton
                id={"select" + name}
                onChange={onChangeSelected}
                options={options_list}
                placeholder={" --Select a " + title + " -- "}
                selected={selected}
                labelKey={e => e.Title}
                paginated={true}
                paginationText="More..."
            />
        </div>
    );
}

export default Select;