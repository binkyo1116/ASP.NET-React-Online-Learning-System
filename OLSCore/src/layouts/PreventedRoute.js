import React from 'react';
import { Navigate } from "react-router-dom";

const PreventedRoute = (props) => {
    const isLoggedIn = localStorage.getItem('loginData');
    let meta = props.meta;

    React.useEffect(() => {
        document.title = meta.title;
    }, [meta])

    if (isLoggedIn) {
        // user is not authenticated
        return <Navigate to="/dashboard" />;
    }
    return props.children;
};

export default PreventedRoute;
