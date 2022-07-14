import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>Hello!</h1>
        <p><a href="/AddCodingProblem">Go to Add Coding Problem</a></p>
        <p><a href="/UpdateCodingProblem">Go to Update Coding Problem</a></p>
      </div>
    );
  }
}
