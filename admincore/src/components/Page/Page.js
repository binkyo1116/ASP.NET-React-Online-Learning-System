import React, { useState} from "react";
import { Container } from "react-bootstrap";
import Header from '../Header/Header';
import Filters from '../Filters/Filters';
import OnlineSystemHeader from '../OnlineSystemHeader/OnlineSystemHeader'
import CodingProblem from '../CodingProblem/CodingProblem';
import "./Page.css"

const Page = ({ from }) => {

    const [codingProblem, setCodingProblem] = useState(0);

    const renderAdd = () => {
        return (
            <div className="page" >
                <OnlineSystemHeader/>
                <Container fluid>
                <Header title= "Add New Coding Problem" />
                <CodingProblem />
                </Container>
            </div>
         );
    }

    const renderUpdate = () => {
        return (
            <div className="page">
                <OnlineSystemHeader />
                <Container fluid>
                <Header title="Update Coding Problem" />
                <Filters onChangeCodingProblem={setCodingProblem} />
                    {(codingProblem > 0) ? <CodingProblem id={codingProblem} /> : ""}
                </Container>
            </div>
        );
    }

    return (from === "add") ? renderAdd() : renderUpdate();
}

export default Page;