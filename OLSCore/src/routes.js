import ContactUs from './pages/ContacUs';
import ForgotPassword from './pages/ForgotPassword';
import Login from './pages/Login';
import RequestLogin from './pages/RequestLogin';
import Dashboard from './pages/Dashboard';
import Calender from './pages/Calender';


var dashRoutes = [
    /** Add all authentication routing (not required session) here */
    {
        path: "/",
        name: "Login",
        component: Login,
        icon: "",
        invisible: false,
        meta: {
            title: 'LUD | Login'
        },
        isPreventedRoute: true
    },
    {
        path: "/login",
        name: "Login",
        component: Login,
        icon: "",
        invisible: false,
        meta: {
            title: 'LUD | Login'
        },
        isPreventedRoute: true
    },
    {
        path: "/forgotpassword",
        name: "ForgotPassword",
        component: ForgotPassword,
        icon: "",
        invisible: false,
        meta: {
            title: 'LUD | Forgot Password'
        },
        isPreventedRoute: true
    },
    {
        path: "/requestlogin",
        name: "RequestLogin",
        component: RequestLogin,
        icon: "",
        invisible: false,
        meta: {
            title: 'LUD | Request Login'
        },
        isPreventedRoute: true
    },
    {
        path: "/contactus",
        name: "ContactUs",
        component: ContactUs,
        icon: "",
        invisible: false,
        meta: {
            title: 'LUD | Contact Us'
        },
        isPreventedRoute: true
    },

    /** Add all protected routing (requires session) here */
    {
        path: "/dashboard",
        name: "Dashboard",
        component: Dashboard,
        icon: "",
        invisible: false,
        meta: {
            title: 'LUD | Dashboard'
        },
        isPreventedRoute: false
    },
    {
        path: "/calendar",
        name: "Calendar",
        component: Calender,
        icon: "",
        invisible: false,
        meta: {
            title: 'LUD | Calendar'
        },
        isPreventedRoute: false
    },
    
]

export default dashRoutes;