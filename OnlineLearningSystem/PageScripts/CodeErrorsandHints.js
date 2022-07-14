const DropDownList = (props) => {
    return (
        <section className="Alice-blue-bg" id="StudentListSection">
            <div className="container">
                <div className="margin-left-right-20">
                    <div className="row">
                        <div className="col-md-12">
                            <div className="student-card">
                                <div
                                    className="row"
                                    id="pnlStudentDropDownlist"
                                >
                                    <div className="col-md-2 margin-b-1-875">
                                        <label>{props.title}</label>
                                    </div>
                                    <div className="col-md-4 margin-b-1-875">
                                        <Select
                                            id="ddlStuden"
                                            onChange={event =>

                                                props.handleChange(event)
                                            }
                                            options={props.data}
                                            getOptionValue={elem =>
                                                elem[props.optionValue]
                                            }
                                            getOptionLabel={elem =>
                                                elem[props.optionLabel]
                                            }
                                            placeholder="--Select One --"
                                            value={props.value}
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    );
};

class CodeErrorsandHints extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            courseList: [],
            selectedCourse: null,
            errorList: null,
            hintsList: null,
            selectedError: null,
            selectedHint: null,
            hintToEdit: null
        };

        this.loadCourses = this.loadCourses.bind(this);
        this.handleCourseChange = this.handleCourseChange.bind(this);
        this.handleErrorChange = this.handleErrorChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleHintChange = this.handleHintChange.bind(this);
    }

    /**
     * Get courses
     * */
    async loadCourses() {
        const courses = await fetchFunction("CodeErrorsHints/GetCourses", {});

        this.setState({ courseList: courses });
    }

    async handleCourseChange(selectedCourse) {
     

        const errors = await fetchFunction("CodeErrorsHints/GetErrors", {
            CourseId: selectedCourse.Id
        });

        //update with new courses and removes errors and hints of previously selected course
        this.setState({
            selectedCourse,
            errorList: errors,
            hintsList: null,
            selectedHint: null,
            selectedError: null
        });
    }


    async handleErrorChange(selectedError) {

        const hints = await fetchFunction("CodeErrorsHints/GetHints", {
            ErrorId: selectedError.Id
        })

        //Update hints list for newly selected error
        this.setState({
            selectedError,
            hintsList: [{ Id: -1, Hint: "Add New Hint" }, ...hints],
            selectedHint: null
        });
    }

    handleHintChange(selectedHint) {

        this.setState({selectedHint})

        if (selectedHint.Id === -1) {
            this.setState({ hintToEdit: "" })
            return;
        }

        this.setState({hintToEdit: selectedHint.Hint})

    }

    async handleSubmit(event) {
        event.preventDefault();
        const data = {
            HintId: this.state.selectedHint.Id,
            UpdatedHint: this.state.hintToEdit,
            ErrorId: this.state.selectedError.Id
        }
   
        const res = await fetchFunction("CodeErrorsHints/UpdateHint", data);

        let hintsList;

        //Add new hint to the end of the list
        if (this.state.selectedHint.Id === -1) {
            hintsList = [...this.state.hintsList, res]
        }
        //Update existing hint
        else {
            hintsList = this.state.hintsList.map(hint => hint.Id !== res.Id ? hint : res);
        }
        
        this.setState({ 
            hintsList,
            selectedHint: res
        }, () => console.log(this.state.hintsList))

        
    }

    componentDidMount() {
        this.loadCourses();
    }

    render() {
        return (
            <div>
                <DropDownList
                    title="Select Course"
                    data={this.state.courseList}
                    handleChange={this.handleCourseChange}
                    optionLabel="Name"
                    optionValue="Id"
                    value={this.state.selectedCourse}
                />
                {this.state.errorList ? (
                    <DropDownList
                        title="Select Error"
                        data={this.state.errorList}
                        handleChange={this.handleErrorChange}
                        optionLabel="Error"
                        optionValue="Id"
                        value={this.state.selectedError}
                    />
                ) : null}
                {this.state.hintsList ? (
                    <DropDownList
                        title="Select Hint"
                        data={this.state.hintsList}
                        handleChange={this.handleHintChange}
                        optionLabel="Hint"
                        optionValue="Id"
                        value={this.state.selectedHint}
                    />
                ) : null}
                {this.state.selectedHint ? (
                    <form onSubmit={(event) => { this.handleSubmit(event) }}>
                        <input
                            type="text"
                            value={this.state.hintToEdit}
                            onChange={event =>
                                this.setState({
                                    hintToEdit: event.target.value
                                })
                            }
                        />
                        <input type="submit" value={this.state.selectedHint.Id === -1 ? "Add Hint" : "Update Hint"} />
                    </form>
                ) : null}
            </div>
        );
    }
}
const domContainer = document.querySelector("#content");
ReactDOM.render(<CodeErrorsandHints />, domContainer);
