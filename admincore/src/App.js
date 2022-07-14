import React, { Component } from 'react';
import {
	BrowserRouter as Router,
	Switch,
	Route
} from "react-router-dom";

import { Home } from './components/Home';
import Page from './components/Page/Page';

import './custom.css'

export default class App extends Component {
	static displayName = App.name;

	render() {
		return (
			<Router>
				<Switch>
					<Route path="/AddCodingProblem">
						<Page from="add" />
					</Route>
					<Route path="/UpdateCodingProblem">
						<Page from="update" />
					</Route>
					<Route path="/">
					<Home />
					</Route>
				</Switch>
			</Router>
		);
	}
}