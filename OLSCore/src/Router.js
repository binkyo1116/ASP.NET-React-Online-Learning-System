import React from 'react';
import { Route, Routes } from "react-router-dom";
import PreventedRoute from './layouts/PreventedRoute';
import ProtectedRoute from './layouts/ProtectedRoute';
import Sidebar from './components/Shared/Sidebar/index';
import ErrorPage from './components/Shared/ErrorPage/index';
import routes from './routes';
import { createBrowserHistory } from "history";

const history = createBrowserHistory();

function Router() {
   
    return (
        <React.Suspense>
            <Routes>
                {routes?.filter(x => !x.isShared).map((x, idx) => {
                    return (
                        <Route
                            key={idx}
                            path={x.path}
                            element={
                                x.isPreventedRoute ?
                                    <PreventedRoute meta={x.meta}>
                                        <x.component history={history}/>
                                    </PreventedRoute>
                                    :
                                    <ProtectedRoute meta={x.meta}>
                                        <div className='row dashboard-container'>
                                        <Sidebar history={history}/>
                                        <x.component history={history}/>
                                        </div>
                                    </ProtectedRoute>
                            }
                        />
                    )
                })}
                <Route path="*" element={<ErrorPage />} />
            </Routes>
        </React.Suspense>
    )
}

export default Router;