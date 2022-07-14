/**
 * By: Hannan Abid
 * React Header to utilize states and load breadcrumbs
 * Based on original logout.html and logout.js
 * Revised: 2022-07-07, by Humam Babi
 */

/**
 * @description Render each breadcrumb
 * @param {any} props - Contains list of breadcrumbs
 */
const Breadcrumbs = (props) => {
    const { breadcrumbs } = props;

    return (
        <section id="breadcrumb-section">
            <div className="container px-xl-4">
                <div className="row">
                    <nav aria-label="breadcrumb" >
                        <ol className="breadcrumb" style={{ backgroundColor: "transparent" }}>
                            {breadcrumbs.map((crumb, index) => {
                                let active = "", currentPage = false;

                                if (index === breadcrumbs.length - 1) {
                                    active = "active";
                                    currentPage = true;
                                }

                                return (
                                    <li className={`breadcrumb-item ${active}`} key={crumb.name}>
                                        {/*Prevent current page from having a link*/}
                                        {currentPage ? (
                                            <React.Fragment>
                                                {crumb.name}
                                            </React.Fragment>
                                        ) : (
                                            <a href={crumb.link}>
                                                {crumb.name}
                                            </a>
                                        )}
                                    </li>
                                )
                            })}
                        </ol>
                    </nav>
                </div>
            </div>
        </section>
    )
}


/**
 * Header react component for rendering the header of the page
 * (Navbar + Breadcrumbs)
 */
class Header extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            profileImg: "Content/images/photo.jpg",
            profileLink: "#",
            calendarLink: "#",
            feedbackLink: null,
            studentName: "Student Name",
            breadcrumbs: []
        }

        this.svgLogout = '<svg id="svg" version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="-75, -40, 485, 485"><g id="svgg"><path id="path0" d="M72.000 27.212 C 70.790 27.386,68.090 27.724,66.000 27.964 C 37.975 31.179,15.808 54.328,12.726 83.600 C 11.959 90.891,11.952 309.504,12.719 316.600 C 16.120 348.058,37.905 369.160,70.763 372.824 C 81.466 374.017,196.871 374.014,207.400 372.820 C 239.738 369.153,261.189 348.537,265.273 317.200 C 265.519 315.317,265.789 305.299,265.879 294.743 L 266.042 275.686 264.991 270.943 C 262.061 257.730,253.133 250.000,240.800 250.000 C 228.835 250.000,220.020 257.249,216.963 269.600 L 215.824 274.200 215.612 294.200 C 215.371 316.975,215.486 316.197,211.822 319.758 C 208.192 323.286,214.599 323.000,139.200 323.000 C 76.874 323.000,72.455 322.955,70.336 322.299 C 67.603 321.454,64.764 318.884,63.472 316.088 L 62.600 314.200 62.600 200.400 C 62.600 89.433,62.619 86.551,63.362 84.623 C 64.373 82.004,66.736 79.509,69.309 78.345 L 71.400 77.400 139.000 77.400 L 206.600 77.400 208.712 78.354 C 215.159 81.268,215.600 83.022,215.600 105.780 C 215.600 129.511,216.305 133.563,221.694 140.832 C 230.898 153.243,250.520 153.419,259.737 141.173 C 262.910 136.956,263.514 135.630,264.807 130.032 L 266.042 124.688 265.879 105.644 C 265.706 85.353,265.529 82.938,263.598 74.423 C 260.563 61.047,252.406 48.596,241.000 39.933 C 231.362 32.613,222.539 29.207,209.000 27.583 C 202.209 26.768,77.447 26.431,72.000 27.212 M312.600 119.885 C 302.127 121.325,294.544 127.956,291.883 138.000 C 290.302 143.964,290.246 144.922,291.204 149.600 C 292.617 156.505,293.659 158.190,301.597 166.400 C 306.598 171.573,307.187 172.888,305.053 174.111 C 304.390 174.491,282.497 174.646,206.800 174.805 L 109.400 175.009 105.638 176.114 C 96.194 178.886,90.608 184.421,88.184 193.407 C 84.414 207.392,90.634 219.058,104.334 223.692 L 108.200 225.000 206.000 225.200 C 280.817 225.353,303.996 225.512,304.633 225.878 C 307.271 227.393,306.922 228.346,301.806 233.600 C 295.127 240.458,293.535 242.993,291.756 249.600 C 285.790 271.761,309.452 288.626,328.678 275.915 C 334.356 272.162,390.055 216.195,392.117 212.172 C 396.161 204.282,395.021 192.662,389.424 184.717 C 386.738 180.905,332.097 126.603,328.600 124.271 C 325.605 122.273,322.575 121.054,318.400 120.167 C 315.811 119.616,314.882 119.571,312.600 119.885 "></path></g></svg>';
        this.loadCrumbs = this.loadCrumbs.bind(this);
    }

    // Load breadcrumbs for the current page
    async loadCrumbs() {
        const crumbs = await GetBreadCrumbs(); // Function in Global.js

        this.setState({
            breadcrumbs: crumbs
        })
    }


    componentDidMount() {
        this.loadCrumbs();

        var imageSrc = localStorage.getItem("StudentProfileImage");

        // Update profile image if one exists
        if (imageSrc != "") {
            this.setState({
                profileImg: imageSrc
            })
        }

        // Populate with student information and links
        this.setState({
            studentName: localStorage.getItem("StudentName"),
            profileLink: GetUrlClean("UpdateProfile.html"),
            //calendarLink: GetUrlClean("Calendar.html")
            calendarLink: GetUrlClean("Calendar.html", "courseInstanceId", GetFromQueryString("courseInstanceId"))
        })

        // Provide feedback button at page with moduleId
        if (GetFromQueryString("moduleId")) {
            this.setState({
                feedbackLink: GetUrlClean("Feedback.html", "courseInstanceId", GetFromQueryString("courseInstanceId"))
            })
        }
    }

    render() {
        // (Refer to Logout.html for structure)
        return (
            <React.Fragment>
                <section className="header d-flex align-items-center">
                    <div className="container">
                        <div className="row">
                            <div className="col-md-12">
                                <nav className="navbar wow fadeInDown navbar-expand-lg navbar-light">
                                    <div className="container-fluid">
                                        <div className="nav-area-logo">
                                            <a href="CourseSelection.html">
                                                <img src="Content/images/lets-use-data-logo.svg" width="200" />
                                            </a>
                                        </div>
                                        <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbar-toggle" aria-controls="navbar-toggle" aria-expanded="false" aria-label="Toggle navigation">
                                            <span className="navbar-toggler-icon"></span>
                                        </button>
                                        <div className="collapse navbar-collapse" id="navbar-toggle">
                                            <ul className="navbar-nav ml-auto">
                                                {/*Calender Link*/}
                                                <li className="nav-item">
                                                    <a className="nav-link" id="btnCalendar" href={this.state.calendarLink}>
                                                        Calendar
                                                        <span className="sr-only">(current)</span>
                                                    </a>
                                                </li>

                                                {/*Render feedBack only if valid link present*/}
                                                {this.state.feedbackLink ? (
                                                    <li className="nav-item">
                                                        <a className="nav-link" id="btnComment" onClick={() => Feedback()} href={this.state.feedbackLink}>Feedback<span className="sr-only">(current)</span></a>
                                                    </li>
                                                ) : null}

                                                <li className="nav-item">
                                                    <a className="nav-link" id="btnContact" href="/Contact.html">
                                                        Contact
                                                        <span className="sr-only">(current)</span>
                                                    </a>
                                                </li>
                                            </ul>
                                        </div>
                                        <hr className="navlinks_divider d-none d-lg-block" />
                                        <ul className="navbar-nav pull-right">
                                            <li className="dropdown d-flex align-items-center">
                                                <a className="dropdown-toggle caret-none user-image" data-toggle="dropdown">
                                                    <img id="user-profile-image" src={this.state.profileImg} />
                                                </a>
                                                <ul className="dropdown-menu usermenu">
                                                    <li>
                                                        <p className="user-name d-lg-none d-md-block text-center" id="usermenu-student-name">{this.state.studentName}</p>
                                                    </li>
                                                    <li><div className="dropdown-divider d-lg-none d-md-block"></div></li>

                                                    <li>
                                                        <a className="dropdown-item" id="ButtonUpdateProfile" href={this.state.profileLink}><i class="fa-regular fa-circle-user"></i>Profile</a>
                                                    </li>

                                                    <li>
                                                        <a className="dropdown-item" id="btnLogout" href="LoggedOut.html">
                                                            <div dangerouslySetInnerHTML={{ __html: this.svgLogout }} />
                                                            Logout
                                                        </a>
                                                    </li>
                                                </ul>
                                                <p className="user-name mr-4 d-none d-lg-block" id="top-bar-student-name">{this.state.studentName}</p>
                                            </li>
                                        </ul>
                                    </div>
                                </nav>
                            </div>
                        </div>
                    </div>
                </section>

                {/*Load BreadCrumbs component*/}
                <Breadcrumbs breadcrumbs={this.state.breadcrumbs} />
            </React.Fragment>
        )
    }
}

const domContainer = document.querySelector('#header');
ReactDOM.render(<Header />, domContainer);
