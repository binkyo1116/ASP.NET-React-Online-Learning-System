/**
 * StatisticsTable function returns table that holds all the statistics
 * @param {any} props
 * @requires stats - the statisic object passed in as prop
 */
const StatisticsTable = props => {
    const { stats } = props;

    return (
        <table className="table table-bordered font-size-13 box-bg-white" style={{ border: "#dee2e6"}}>
            <thead>
                <tr>
                    <th />
                    <th style={{ width: "30%", textAlign: "center" }}>
                        Completion
                    </th>
                    <th style={{ width: "15%", textAlign: "center" }}>
                        Weight
                    </th>
                    <th style={{ width: "15%", textAlign: "center" }}>Grade</th>
                    <th style={{ width: "15%", textAlign: "center" }}>Total</th>
                </tr>
            </thead>
            <tbody>
                {Object.keys(stats).map((key, index) => {
                    if (key !== "CourseName" && key !== "Total") {
                        return (
                            <Statistic
                                name={key}
                                key={key + index}
                                stats={stats[key]}
                            />
                        );
                    }
                })}
            </tbody>
        </table>
    );
};

/**
 * Statistic function returns statistics associated with the category passed in
 * @param {any} props
 * @requires name - The category name passed in as prop
 * @requries stats - The statisics object associated with the category passed as prop
 */
const Statistic = props => {
    const { name, stats } = props;

    return stats.Weight > 0 ? (
        <tr id={name + "Row"}>
            <td>{name}</td>
            <td>
                <div className="progress">
                    <div
                        id={name + "CompletionProgressBar"}
                        className="progress-bar"
                        role="progressbar"
                        aria-valuemin="0"
                        aria-valuemax="100"
                        aria-valuenow="3"
                        style={{ width: `${stats.Completion}%` }}
                    />
                    <span
                        className="progress-bar-percentage"
                        id={name + "Completion"}
                    >
                        {stats.Completion}%
                    </span>
                </div>
            </td>
            <td className="text-center">
                <span id={name + "Weight"}>{stats.Weight}%</span>
            </td>
            <td className="text-center">
                <span id={name + "CurrentGrade"}>{stats.Grade}%</span>
            </td>
            <td className="text-center">
                <span id={name + "WeightedGrade"}>{stats.WeightedGrade}%</span>
            </td>
        </tr>
    ) : null;
};

/**
 * Module function returns individual module that belongs to an objective
 * @param {any} props
 * @requires courseID of the current course
 * @requires objectiveID of the objective this module belongs to
 * @requires module object
 * @requires grade object for this module
 */
const Module = props => {
    const { courseID, objectiveID, module, grade } = props;
    const svgBtnArrow = '<svg width="47" height="16" viewBox="0 0 47 16" fill="none" xmlns="http://www.w3.org/2000/svg"><path d="M46.7071 8.70711C47.0976 8.31659 47.0976 7.68342 46.7071 7.2929L40.3431 0.928936C39.9526 0.538411 39.3195 0.538411 38.9289 0.928936C38.5384 1.31946 38.5384 1.95262 38.9289 2.34315L44.5858 8L38.9289 13.6569C38.5384 14.0474 38.5384 14.6805 38.9289 15.0711C39.3195 15.4616 39.9526 15.4616 40.3431 15.0711L46.7071 8.70711ZM-8.74228e-08 9L46 9L46 7L8.74228e-08 7L-8.74228e-08 9Z" fill="white" /></svg>';

    if (grade) {
        if (grade.Completion > 100) grade.Component = 100;
    }

    return (
        <div className="box courseobjs module-progress">
            <div className="d-flex flex-column flex-lg-row">

                {/* Left (or top on mobile) section */}
                <div className="modsec-lefttop">
                    <div className="module-title text-center text-lg-left" id={`${objectiveID}_${module.ModuleId}description`}>
                        {module.Description}
                    </div>

                    <div className="module-objectives-label" id={`${objectiveID}_${module.ModuleId}details`}>
                        {module.ModuleObjectives}
                    </div>

                    {module.DueDate !== "" ? (
                        <div className="due-date" id={`${objectiveID}_${module.ModuleId}dueDate`}>
                            <i class="fa-regular fa-clock"></i>{module.DueDate}
                        </div>
                    ) : null}

                    <div className={"progress" + (module.DueDate !== "" ? " due" : "")}>
                        {grade ? (
                            <div
                                id={`${objectiveID}_${module.ModuleId}strokeDashArray`}
                                className={"progress-bar" + (grade.Completion == 100 ? " full" : "")}
                                role="progressbar"
                                style={{ width: `${grade.Completion}%` }}
                                aria-valuenow={`${grade.Completion}%`}
                                aria-valuemin="0"
                                aria-valuemax="100"
                            />
                        ) : (
                            <div
                                className="bg-info progress-bar-striped progress-bar-animated"
                                role="progressbar"
                                style={{ width: "100%", height: "0.5rem", position: "absolute", top: 0, left: 0, borderRadius: "0.25rem" }}
                                aria-valuenow="100"
                                aria-valuemin="0"
                                aria-valuemax="100"
                            />
                        )}
                    </div>
                </div>

                {/* Right (or bottom on mobile) section */}
                <div className="modsec-rightbottom">
                    <a href={`ModulePage.html?courseInstanceId=${courseID}&moduleId=${module.ModuleId}`} className="w-100">
                        <button
                            type="button"
                            className={"mt-4 mt-lg-0 button solid courseobjs" + ((grade && grade.Completion == 100) ? " full" : "")}
                            id={`${objectiveID}_${module.ModuleId}btnModule`}
                        >
                            {(grade && grade.Completion == 100) ? "Review" : "Access"}
                            <span style={{ marginLeft: "1rem" }} dangerouslySetInnerHTML={{ __html: svgBtnArrow }} />
                        </button>
                    </a>
                </div>

            </div>
        </div>
    );
};

/*
 *             <h5>Course Objective:</h5>
            <span
                id={`${objective.Id}Description`}
                className="display-block margin-b-1"
            >
                {objective.Description}
            </span>
 * 
 * 
 * 
 * /

/**
 * Objective function returns each individual course objective which contains modules
 * @param {any} props
 * @requires grades object passed as prop
 * @requires the objective to render passed as prop
 * @requires courseID of the course passed as prop
 */
const Objective = props => {
    const { objective, courseID, grades } = props;

    return (
        <div className="course-objective-area">
            <div id={`${objective.Id}pnlLayout`}>
                <meta charSet="utf-8" />
                <title />

                {objective.Modules.map((value, index) => {
                    return (
                        <Module
                            key={value.ModuleId}
                            module={value}
                            objectiveID={objective.Id}
                            courseID={courseID}
                            grade={grades ? grades[index] : null}
                        />
                    );
                })}
            </div>
        </div>
    );
};

/**
 * Announcement Function returns the announcement section of course objecitves page
 * @param {any} props
 * @requires announcement link passed as prop
 * @requires The first announcement object passed as prop
 */
const Announcement = props => {
    const { link, announcement } = props;

    return (
        <div className="wraper-area box-bg-white">
            <div className="row">
                <div className="col-md-6">
                    <h6>Announcement</h6>
                </div>
                <div className="col-md-6 text-right">
                    <a href={link} id="a_announcemnts">
                        <input
                            type="button"
                            id="ButtonAnnouncementList"
                            className="btn btn-custom-round btn-sm"
                            value="Announcement List"
                        />
                    </a>
                </div>
            </div>
            <span id="LabelAnnouncementTitle" className="label-header">
                {announcement.Title}
            </span>
            <span id="LabelPublishedDate" className="label-header-time">
                {announcement.PublishDate}
            </span>
            <div className="margin-bottom-10" />
            <span
                id="LabelAnnouncementDescription"
                className="label-anouncement"
            >
                {announcement.Description}
            </span>
        </div>
    );
};

/**
 * React component for Courseobjectives.html page
 * */
class Course extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            data: null,
            stats: null,
            grades: null,
            objectives: null,
            announcements: [],
            syllabusLink: "#",
            a_announcement: "#"
        };

        this.loadData = this.loadData.bind(this);
    }

    // Asynchrounously load data needed for statistics, announcements, grades, and objectives
    async loadData() {
        // Load courseID
        await this.setState({
            data: {
                CourseInstanceId: GetFromQueryString("courseInstanceId")
            }
        });

        //Load Statistics
        /*
        this.setState({
            stats: await fetchFunction("CourseStatistics", this.state.data)
        });
        */

        //Load Announcement
        /* TODO
        this.setState({
            announcements: await fetchFunction(
                "CourseAnnouncement",
                this.state.data
            )
        });
        */

        // Load Objectives
        this.setState({
            objectives: await fetchFunction("CourseObjective", {
                ...this.state.data,
                Method: "GetCourseObjective"
            })
        });

        // Load grades
        this.setState({
            grades: await fetchFunction("CourseObjective", {
                ...this.state.data,
                Method: "LoadGrades"
            })
        });
    }

    componentDidMount() {
        this.loadData();

        //Set appropriate syllabus and announcement links
        this.setState({
            syllabusLink: GetUrl("Syllabus.html")
        });
        this.setState({
            a_announcement: GetUrl("AnnouncementPage.html")
        });
    }

    render() {
        return (
            <React.Fragment>
                {this.state.objectives ? (
                    <React.Fragment>
                        {/*<!------------------------------------------- page title section ----------------------->*/}
                        {/*<section className="page-title">
                    <div className="container">
                        <div className="row">
                            <div className="col-lg-12">
                                <h2>
                                    <span id="lblCourseTitle">
                                        {this.state.objectives
                                            ? this.state.objectives.Name
                                            : ""}
                                    </span>
                                </h2>
                            </div>
                        </div>
                    </div>
                </section>*/}
                        {/* <section>
                    <div className="container">
                        <div className="row">
                            <div className="col-lg-12">
                                <h2>
                                    <span id="lblCourseTitle">
                                        {this.state.objectives
                                            ? this.state.objectives.Name
                                            : ""}
                                    </span>
                                </h2>
                            </div>
                        </div>
                    </div>
                </section>*/}
                        {/*<!-------------------------------------- end page title section ----------------------->*/}


                        {/* Show Course Objectives when they are ready */}
                        <section className="page-content">
                            <div className="container px-xl-4">
                                <div className="row">
                                    <div className="mb-4">
                                        <a href={this.state.syllabusLink} id="syllabusLink">Syllabus</a>
                                    </div>

                                    <div className="course-static-area" style={{ display: "none" }}>
                                        {/* <!-- -----------------------------calculate the students current and overall grade-------------------------------->*/}
                                        <div className="row">
                                            <div className="col-md-12 col-lg-6">
                                                {/* <h3>Summary</h3>*/}
                                                {this.state.stats ? (
                                                    <>
                                                        {/*<!--<h4>*/}
                                                        {/*<span id="lblCourseTitle"></span>*/}
                                                        {/*</h4>-->*/}
                                                        <span id="totalCurrentGrade">
                                                            Course Current
                                                            Grade:{" "}
                                                            {
                                                                this.state.stats
                                                                    .Total
                                                                    .CurrentGrade
                                                            }%
                                                        </span>
                                                        {/*    <!--&nbsp (GPA &nbsp-->*/}
                                                        <span
                                                            style={{
                                                                visibility:
                                                                    "hidden"
                                                            }}
                                                            id="totalCurrentGPA"
                                                        />
                                                        {/*    <!--)-->*/}
                                                        <br />
                                                        <span id="totalGrade">
                                                            Course Predicted
                                                            Grade:{" "}
                                                            {
                                                                this.state.stats
                                                                    .Total.Grade
                                                            }%
                                                        </span>
                                                        {/*  <!--&nbsp (GPA &nbsp-->*/}
                                                        <span
                                                            style={{
                                                                visibility:
                                                                    "hidden"
                                                            }}
                                                            id="totalGPA"
                                                        />
                                                        {/*<!--)-->*/}
                                                        <br />

                                                        {this.state.stats
                                                            .Total
                                                            .Completion == 100 ? (
                                                            <span id="totalCompletion">
                                                                Completed
                                                            </span>
                                                        ) : (
                                                            <span id="totalCompletion">
                                                                Progress:
                                                                {" "}
                                                                {
                                                                    this.state.stats
                                                                        .Total
                                                                        .Completion
                                                                }%
                                                            </span>
                                                        )}

                                                        <br />
                                                    </>
                                                ) : null}

                                                <div className="course-objective-intro">
                                                    <a
                                                        href=""
                                                        style={{
                                                            visibility: "hidden"
                                                        }}
                                                    >
                                                        Start here
                                                    </a>
                                                    <br />
                                                    <a
                                                        href={
                                                            this.state
                                                                .syllabusLink
                                                        }
                                                        id="syllabusLink"
                                                    >
                                                        Syllabus
                                                    </a>
                                                    <br />
                                                </div>
                                            </div>
                                            {/* ---------------------------------------Individual Grade Component Table --------------------------------- */}
                                            <div
                                                className=" col-md-12 col-lg-6"
                                                style={{ position: "relative" }}
                                            >
                                                {this.state.stats ? (
                                                    <div
                                                        id="pnlOverallGrade"
                                                        className="overall-grade margin-b-1"
                                                    >
                                                        <div>
                                                            <StatisticsTable
                                                                stats={
                                                                    this.state
                                                                        .stats
                                                                }
                                                            />
                                                        </div>
                                                    </div>
                                                ) : (
                                                    <div
                                                        id="loader-spinner-static-are"
                                                        className="loader-img"
                                                        style={{
                                                            diplay: "none",
                                                            position:
                                                                "relative",
                                                            height: "auto"
                                                        }}
                                                    >
                                                        {/*<img
                                                                width="50"
                                                                src="Content/images/Loder_Trancparent_bg.gif"
                                                            />*/}
                                                    </div>
                                                )}
                                            </div>
                                            {/*-----------------------------------END Individual Grade Component Table----------------------------------*/}
                                        </div>
                                    </div>

                                    {/*-------------------------------------Announcement-------------------------------*/}
                                    {/*
                                    <div className="">
                                        <div
                                            id="PanelAnnouncement"
                                            sytle={{ display: "block" }}
                                        //className="margin-top-10 margin-bottom-30"
                                        >
                                            {this.state.announcements[0] ? (
                                                <Announcement
                                                    link={
                                                        this.state
                                                            .a_announcement
                                                    }
                                                    announcement={
                                                        this.state
                                                            .announcements[0]
                                                    }
                                                />
                                            ) : null}
                                        </div>
                                    </div>
                                    */}
                                    {/*-----------------------------------------------------------------------------------*/}

                                    <div id="pnlPanel">
                                    {
                                        this.state.objectives ?
                                            this.state.objectives.CourseObjectiveList.map((value, index) => {
                                                return (
                                                    <Objective
                                                        key={value.Id}
                                                        objective={value}
                                                        courseID={this.state.data.CourseInstanceId}
                                                        grades={this.state.grades ? this.state.grades[index].Modules : null}
                                                    />
                                                );
                                            }
                                        ) : null
                                    }
                                    </div>
                                </div>
                            </div>
                        </section>
                    </React.Fragment>
                ) : (
                    <React.Fragment>
                        {/* Show the loader-spinner, until (this.state) changes */}
                        <div id="loader-spinner" className="loader-img loader-bg" style={{ display: "block" }}>
                            <img src="Content/images/Loder_Trancparent_bg.gif" />
                        </div>
                    </React.Fragment>
                )}
            </React.Fragment>
        );
    }
}

const domContainer = document.querySelector("#course_objectives");
ReactDOM.render(<Course />, domContainer);
