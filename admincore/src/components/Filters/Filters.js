import React, { useEffect, useState } from "react";
import { Row, Col } from "react-bootstrap";
import { API_URL } from "../../Global"
import Select from "../Select/Select";
import "./Filters.css"

const Filters = ({ onChangeCodingProblem }) => {
    const [filters, setFilters] = useState({});
    const [quarterUrl, setQuarterUrl] = useState(API_URL + "CourseInstances?getQuarter=true");
    const [courseInstancesUrl, setCourseInstancesUrl] = useState(API_URL + "CourseInstances");
    const [moduleObjectiveUrl, setModuleObjectiveUrl] = useState(API_URL + "ModuleObjectives");
    const [codingProblemUrl, setCodingProblemUrl] = useState(API_URL + "CodingProblem");

    const handleChange = (e) => {
        if (e.target.name == "course") {
            setFilters({ course: (isNaN(e.target.value) ? 0 : e.target.value) });
        }
        else {
            let copyFilters = { ...filters };
            copyFilters[e.target.name] = (isNaN(e.target.value) ? 0 : e.target.value);
            setFilters(copyFilters);
        }
        console.log(filters);
    }

    useEffect(() => {
        updateQuarterUrl();
        updateCourseInstanceUrl();
        updateModuleObjectiveUrl();
        updateCodingProblemUrl();
    }, [filters]);

    const updateQuarterUrl = () => {
        var url = API_URL + "CourseInstances?";
        url += "courseId=" + ((filters.course) ? filters.course : "0") + "&getQuarter=true";
        setQuarterUrl(url);
    }

    const updateCourseInstanceUrl = () => {
        var url = API_URL + "CourseInstances?";
        url += "courseId=" + ((filters.course) ? filters.course : "0") + "&getQuarter=false";
        setCourseInstancesUrl(url);
    }

    const updateModuleObjectiveUrl = () => {
        var url = API_URL + "ModuleObjectives?";
        url += "courseId=" + ((filters.course) ? filters.course : "0");
        setModuleObjectiveUrl(url);
    }

    const updateCodingProblemUrl = () => {
        var url = API_URL + "codingproblem?";
        url += "idCourse=" + ((filters.course) ? filters.course : "0") + "&";
        url += "idModule=" + ((filters.module) ? filters.module : "0");
        setCodingProblemUrl(url);
        console.log(codingProblemUrl);
    }

    return (
        <Row>
            <Col md={6}>
                <div className="filter_select">
                    <Select
                        title="Course"
                        name="course"
                        url={API_URL + "Courses"}
                        onChange={handleChange}
                    />
                </div>
                <div className="filter_select">
                    <Select
                        title="Quarter"
                        name="quarter"
                        url={quarterUrl}
                        onChange={handleChange}
                    />
                </div>
                <div className="filter_select">
                    <Select
                        title="Course Instance"
                        name="instance"
                        url={courseInstancesUrl}
                        onChange={handleChange}
                    />
                </div>
            </Col>
            <Col md={6}>
                <div className="filter_select">
                    <Select className="filter_select"
                        title="Module Objective"
                        name="module"
                        url={moduleObjectiveUrl}
                        onChange={handleChange}
                    />
                </div>
                <div className="filter_select">
                    <Select className="filter_select"
                    title="Coding Problem"
                    name="coding_problem"
                    url={codingProblemUrl}
                    onChange={(e) => onChangeCodingProblem((isNaN(e.target.value)) ? 0 : e.target.value)}
                    />
                </div>
            </Col>
        </Row>
    );
}

export default Filters;