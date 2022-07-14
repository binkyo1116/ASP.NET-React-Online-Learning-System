/**
 * Returns each individual course card with progress, grade, and access button
 * @param {any} props
 * @requires course - The course object passed in as prop
 * @requires grade - The grade object for current course passed in as prop
 */

/*
 Admin's "StudentImpersonation"
*/
class StudentImpersonation extends React.Component {
    constructor(props) {
        super(props);

        this.svgImpersonate = '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" viewBox="0 0 1000 1000" xml:space="preserve"><g transform="matrix(37.9342 0 0 37.9342 499.9996 499.9997)" id="790935"><path style="stroke: none; stroke-width: 1; stroke-dasharray: none; stroke-linecap: butt; stroke-dashoffset: 0; stroke-linejoin: miter; stroke-miterlimit: 4; fill-rule: nonzero; opacity: 1;" vector-effect="non-scaling-stroke" transform=" translate(-12, -11)" d="M 23.6719 12.1718 L 21.7969 14.0468 C 21.5846 14.2564 21.2983 14.3739 21.0001 14.3739 C 20.7018 14.3739 20.4155 14.2564 20.2032 14.0468 L 18.3282 12.1718 C 18.1199 11.9667 17.9997 11.6885 17.993 11.3962 C 17.9864 11.1039 18.0937 10.8205 18.2924 10.606 C 18.491 10.3915 18.7653 10.2628 19.0573 10.2471 C 19.3492 10.2314 19.6358 10.3299 19.8563 10.5218 C 19.7577 8.92324 19.174 7.39272 18.183 6.13444 C 17.1921 4.87616 15.841 3.94995 14.3101 3.47936 C 12.7791 3.00877 11.1411 3.01618 9.61446 3.5006 C 8.08784 3.98501 6.74521 4.9234 5.76568 6.19059 C 5.58293 6.4268 5.31383 6.58073 5.01758 6.61853 C 4.72133 6.65633 4.4222 6.5749 4.186 6.39215 C 3.94979 6.2094 3.79585 5.9403 3.75805 5.64405 C 3.72025 5.3478 3.80168 5.04867 3.98443 4.81246 C 5.25203 3.16964 6.99377 1.95594 8.97391 1.33565 C 10.954 0.715363 13.0769 0.718441 15.0553 1.34447 C 17.0336 1.9705 18.7718 3.18924 20.0346 4.83574 C 21.2975 6.48224 22.0239 8.47698 22.1157 10.55 C 22.3334 10.359 22.6165 10.2593 22.9059 10.2717 C 23.1953 10.2842 23.4687 10.4078 23.6693 10.6168 C 23.8698 10.8258 23.982 11.1041 23.9825 11.3938 C 23.983 11.6835 23.8718 11.9621 23.6719 12.1718 Z M 20.0157 17.1875 C 18.7481 18.8303 17.0063 20.044 15.0262 20.6643 C 13.0461 21.2846 10.9232 21.2815 8.94484 20.6555 C 6.96651 20.0294 5.2283 18.8107 3.96547 17.1642 C 2.70264 15.5177 1.97619 13.523 1.88443 11.45 C 1.66667 11.641 1.38362 11.7406 1.09422 11.7282 C 0.804826 11.7157 0.531382 11.5921 0.33083 11.3831 C 0.130277 11.1741 0.018073 10.8958 0.0175798 10.6061 C 0.0170866 10.3165 0.128343 10.0378 0.328182 9.82809 L 2.20318 7.95309 C 2.3077 7.84821 2.43189 7.76499 2.56864 7.70821 C 2.70538 7.65143 2.85199 7.6222 3.00006 7.6222 C 3.14812 7.6222 3.29473 7.65143 3.43148 7.70821 C 3.56822 7.76499 3.69242 7.84821 3.79693 7.95309 L 5.67193 9.82809 C 5.77681 9.9326 5.86003 10.0568 5.91681 10.1935 C 5.97359 10.3303 6.00282 10.4769 6.00282 10.625 C 6.00282 10.773 5.97359 10.9196 5.91681 11.0564 C 5.86003 11.1931 5.77681 11.3173 5.67193 11.4218 C 5.45925 11.6309 5.1733 11.7487 4.87506 11.75 C 4.60668 11.7495 4.34733 11.653 4.14381 11.4781 C 4.23294 13.0796 4.81958 14.6134 5.82193 15.8656 C 6.54983 14.9411 7.47563 14.1915 8.53131 13.6718 C 7.85566 12.9876 7.39744 12.1189 7.2143 11.1749 C 7.03115 10.231 7.13126 9.25392 7.50203 8.3667 C 7.8728 7.47949 8.49767 6.72173 9.29803 6.18877 C 10.0984 5.6558 11.0385 5.37143 12.0001 5.37143 C 12.9616 5.37143 13.9017 5.6558 14.7021 6.18877 C 15.5024 6.72173 16.1273 7.47949 16.4981 8.3667 C 16.8689 9.25392 16.969 10.231 16.7858 11.1749 C 16.6027 12.1189 16.1445 12.9876 15.4688 13.6718 C 16.5281 14.1941 17.4571 14.9469 18.1876 15.875 L 18.2344 15.8093 C 18.4172 15.5731 18.6863 15.4192 18.9825 15.3814 C 19.2788 15.3436 19.5779 15.425 19.8141 15.6078 C 20.0503 15.7905 20.2043 16.0596 20.2421 16.3559 C 20.2799 16.6521 20.1984 16.9513 20.0157 17.1875 Z M 12.0001 12.875 C 12.5192 12.875 13.0268 12.721 13.4584 12.4326 C 13.8901 12.1441 14.2266 11.7342 14.4252 11.2545 C 14.6239 10.7749 14.6759 10.2471 14.5746 9.73785 C 14.4733 9.22865 14.2233 8.76092 13.8562 8.39381 C 13.4891 8.0267 13.0214 7.77669 12.5122 7.6754 C 12.003 7.57412 11.4752 7.6261 10.9955 7.82478 C 10.5159 8.02346 10.1059 8.35991 9.81745 8.79159 C 9.52901 9.22327 9.37506 9.73079 9.37506 10.25 C 9.37753 10.9454 9.65488 11.6116 10.1466 12.1034 C 10.6384 12.5951 11.3046 12.8725 12.0001 12.875 Z M 12.0001 18.875 C 13.6295 18.8816 15.2199 18.3767 16.5469 17.4312 C 16.0248 16.7164 15.3412 16.1349 14.552 15.734 C 13.7627 15.3331 12.89 15.1242 12.0047 15.1242 C 11.1195 15.1242 10.2468 15.3331 9.45751 15.734 C 8.66824 16.1349 7.98473 16.7164 7.46256 17.4312 C 8.78845 18.372 10.3743 18.8766 12.0001 18.875 Z" stroke-linecap="round" /></g></svg>';
        this.selectedStudent = {}; // Important: Clear the selected student here and every time Swal2 pops up!

        // Create a student array compatible with react-select
        this.optionsStudents = [];
        for (var iC = 0; iC < this.props.students.length; iC++) {
            this.optionsStudents.push(
                {
                    value: this.props.students[iC].Id,
                    label: this.props.students[iC].Name
                }
            );
        }

        // Bind methods to current class instance
        this.studentSelected = this.studentSelected.bind(this);
        this.navigateStudent = this.navigateStudent.bind(this);
        this.formImpersonateShow = this.formImpersonateShow.bind(this);
    }


    // Event: A student was selected from the list items
    studentSelected = (studentItem) => {
        this.selectedStudent.Id = studentItem.value;
        this.selectedStudent.Name = studentItem.label;
    };


    // Function: Impersonate the selected student.
    async navigateStudent() {
        if (!this.selectedStudent.Id) return;

        let { data } = this.props;
        data["Method"] = "NavigateStudent";
        data["SelectedStudentId"] = this.selectedStudent.Id;

        const {
            studentIdHash,
            error,
            StudentName,
            Picture
        } = await fetchFunction("Course", data);

        if (studentIdHash == "-1") {
            Swal.fire({ text: error, icon: "warning" });
            return;
        }

        // Set proper localStorage items to update to selected student
        localStorage.setItem("Hash", studentIdHash);
        localStorage.setItem("StudentName", StudentName);
        localStorage.setItem("StudentProfileImage", Picture);

        // Reload window for changes to take effect
        // Note: When implemented with react, reload is not necessary. Use global states and page should rerender
        // automatically. Global state should also be used in header in order for it to update properly.
        window.location.reload(false);
    }


    /* Function: Show the impersonation form to select a student */
    formImpersonateShow = () => {
        const styledSwal = Swal.mixin({
            buttonsStyling: false,
            customClass: {
                popup: 'swal-usermenu-impersonate-popup',
                title: 'swal-usermenu-impersonate-title',
                htmlContainer: 'swal-usermenu-impersonate-htmlcontainer',
                validationMessage: 'swal-usermenu-impersonate-invalidmsg',
                actions: 'swal-usermenu-impersonate-actions',
                confirmButton: 'button solid swal-usermenu-impersonate-button',
            }
        });

        // Create a react-select component with student list
        var swalHtml = document.createElement("div");
        swalHtml.id = "swal-html";
        swalHtml.style.lineHeight = "1rem";

        const customStyles = {
            option: (provided, state) => {
                const fontSize = "1rem";
                const fontWeight = 500;
                const textAlign = "left";
                return { ...provided, fontSize, fontWeight, textAlign };
            },
            control: (provided, state) => {
                const borderWidth = "1px!important";
                const backgroundColor = "var(--palette-gray-light)";
                const fontSize = "1rem";
                const fontWeight = 500;
                const height = "2.75rem";
                const borderRadius = "1.688rem";
                const paddingLeft = "calc(1rem - 8px - 2px)"; // Internal control's additional padding must be substracted!
                const color = "#78868C!important";
                return { ...provided, borderWidth, backgroundColor, fontSize, fontWeight, height, borderRadius, paddingLeft, color };
            }
        }

        ReactDOM.render(
            <React.Fragment>
                <label style={{ color: "var(--palette-gray-dark)", width: "100%", textAlign: "left", fontSize: "0.75rem", marginBottom: "0.5rem" }}>
                    Select Student
                </label>

                <Select
                    styles={customStyles}
                    options={this.optionsStudents}
                    placeholder="Select"
                    blurInputOnSelect={true}
                    onChange={(selItem) => {
                        this.studentSelected(selItem);

                        // Undo the 'invalid' state and remove the message
                        const elmSelect = document.getElementById("select-student-impersonate");
                        const elmSelectControl = $(elmSelect).children("div:nth-child(1)");
                        elmSelectControl.removeClass("react-select-invalid");
                        document.getElementsByClassName('swal-usermenu-impersonate-actions')[0].classList.remove('invalid');
                        swalImpersonate.resetValidationMessage();
                    }}
                    id="select-student-impersonate"
                />
            </React.Fragment>,
            swalHtml);

        // Clear the currently selected student
        this.selectedStudent = {};

        // Show custom-styled Swal popup, with already rendedred student list
        const swalImpersonate = styledSwal.fire({
            title: "Impersonate Student",
            html: swalHtml,
            confirmButtonText: 'Impersonate',
            showCloseButton: true,
            preConfirm: () => {
                if (typeof (this.selectedStudent.Id) == 'undefined') {
                    const elmSelect = document.getElementById("select-student-impersonate");
                    const elmSelectControl = $(elmSelect).children("div:nth-child(1)");

                    elmSelectControl.addClass("react-select-invalid");
                    document.getElementsByClassName('swal-usermenu-impersonate-actions')[0].classList.add('invalid');

                    swalImpersonate.showValidationMessage("Please, select a student!");
                    return false; // Prevent closing the swal2 popup
                } else {
                    // Impersonate the selected student after a small timeout, to allow form closing animation
                    setTimeout(() => {
                        this.navigateStudent();
                    }, 150);
                }
            }
        });
    };

    /* Render the final DOM (Privileged menu item) */
    render() {
        return (
            <a class="dropdown-item" id="menuitemImpersonate" href="javascript:;" onClick={this.formImpersonateShow}>
                <div dangerouslySetInnerHTML={{ __html: this.svgImpersonate }} />
                Impersonate
            </a>
        );
    }
}


/*
 * React CourseSelection component
 * (Creates the DOM for course selection boxes)
 */
const CourseCard = props => {
    const { course, grade } = props;

    return (
        <div className="boxcontainer selcourse col-12 col-md-6 col-lg-4">
            <div className="box selcourse">
                <div className="course-quarter">{course.Quarter}</div>
                <h1 className="course-name" id={`${course.CourseInstanceId}CourseTitle`}>{course.Name}</h1>

                <div className="progresssection selcourse">
                    {grade ? (
                        <React.Fragment>
                            <div className="progress progressbody selcourse">
                                <div
                                    id={`${course.CourseInstanceId}TotalCompletionProgressBar`}
                                    className={"progress-bar progressfillsolid selcourse" + (grade.TotalCompletion == 100 ? " full" : "")}
                                    role="progressbar"
                                    style={{ width: `${grade.TotalCompletion}%` }}
                                    aria-valuenow={`${grade.TotalCompletion}%`}
                                    aria-valuemin="0"
                                    aria-valuemax="100">
                                </div>
                            </div>
                        </React.Fragment>
                    ) : (
                        <React.Fragment>
                            <div className="progress progressbody selcourse">
                                <div id="LoadingCompletionProgressBar" className="bg-info progress-bar-striped progress-bar-animated progressanistripe selcourse" role="progressbar" style={{ width: "100%", height: "4px", position: "absolute" }} aria-valuenow="100" aria-valuemin="0" aria-valuemax="100"></div>
                            </div>
                        </React.Fragment>
                    )}
                </div>

                <div className="d-flex flex-row justify-content-between align-items-center">
                    <div className="d-flex flex-column gradecompleted">
                        <div>
                            Completed:&nbsp;
                            <span className={(grade && grade.TotalCompletion == 100) ? "full" : ""}>
                                {grade ? grade.TotalCompletion + "%" : <i class='fa-solid fa-sync fa-spin'></i>}
                            </span>
                        </div>
                        <div className={(grade && grade.TotalGrade) ? "d-block" : "d-none"}>
                            Grade:&nbsp;
                            <span className={(grade && grade.TotalCompletion == 100) ? "full" : ""}>
                                {grade ? grade.TotalGrade : <i class='fa-solid fa-sync fa-spin'></i>}
                            </span>
                        </div>
                    </div>
                    <div>
                        <a href={`CourseObjectives.html?&courseInstanceId=${course.CourseInstanceId}`}>
                            <button type="button" className={"button solid selcourse" + ((grade && grade.TotalCompletion == 100) ? " full" : "")}>
                                {grade ? (grade.TotalCompletion == 0 ? "Start" : (grade.TotalCompletion == 100 ? "Review" : "Continue")) : <i class='fa-solid fa-sync fa-spin'></i>}
                            </button>
                        </a>
                    </div>
                </div>
            </div>
        </div>
    );
};

class CourseSelection extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            data: null,
            courseList: null,
            studentList: null,
            gradesList: null
        };

        this.loadData = this.loadData.bind(this);
    }

    async fetchData(data) {
        return await fetchFunction("Course", data);
    }

    // Asynchrounously load data
    async loadData() {
        // Get values from local storage
        await this.setState({
            data: {
                IsAdmin: localStorage.getItem("IsAdmin"),
                AdminHash: localStorage.getItem("AdminHash"),
                Hash: localStorage.getItem("Hash"),
                StudentName: localStorage.getItem("StudentName"),
                StudentProfileImage: localStorage.getItem("StudentProfileImage")
            }
        });

        // Load student and course lists
        let data = await this.fetchData({ ...this.state.data, Method: "Get" });
        this.setState({
            courseList: data.CourseList,
            studentList: data.StudentList
        });

        // Load grade list for courses
        data = await this.fetchData({ ...this.state.data, Method: "Grades" });

        // If data.CourseList.TotalCompletion > 100 then round it back to 100
        for (var iC = 0; iC < data.CourseList.length; iC++) {
            if (data.CourseList[iC].TotalCompletion > 100) data.CourseList[iC].TotalCompletion = 100;
        }

        this.setState({
            gradesList: data.CourseList
        });
    }

    componentDidMount() {
        this.loadData();
    }

    render() {
        // Make privileged changes to te page ONLY for admin
        if (this.state.studentList && this.state.studentList.length > 0) {
            const elmImpersonateMenuItem = document.getElementById("privileged-item");

            // Add "impersonate" item to the user menu
            if (elmImpersonateMenuItem) {
                elmImpersonateMenuItem.classList.remove("d-none");
                elmImpersonateMenuItem.classList.add("d-block");

                ReactDOM.render(<StudentImpersonation students={this.state.studentList} data={this.state.data} />, elmImpersonateMenuItem);
            }
        }

        // Create the normal page contents
        return (
            <React.Fragment>
                {this.state.courseList ? (
                    <React.Fragment>
                        {/* Page contents: A container that maps course cards */}
                        <section className="page-content margin-t-3">
                            <div className="container px-xl-4">
                                <div className="row" id="pnlCourseCard">
                                    {this.state.courseList.map(
                                        (course, index) => {
                                            return (
                                                <CourseCard
                                                    key={course.CourseInstanceId}
                                                    course={course}
                                                    grade={this.state.gradesList ? this.state.gradesList[index] : null}
                                                />
                                            );
                                        }
                                    )}
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

const domContainer = document.querySelector("#ReactCourseSelection");
ReactDOM.render(<CourseSelection />, domContainer);
