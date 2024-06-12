import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>FamBudget</h1>
        <p>Managing family finances doesn't have to be stressful. FamBudget is here to simplify budgeting, helping you and your loved ones achieve financial peace and prosperity.</p>
        <p><b>Why Choose FamBudget?</b></p>
        <ul>
          <li>Easy to Use: Our intuitive interface makes budgeting a breeze, even for beginners.</li>
          <li>Customizable Budgets: Tailor your budget to fit your unique family needs and goals.</li>
          <li>Collaborative Tools: Share your budget with family members and work together to reach your financial goals.</li>
        </ul>
        <p><b>Sign Up Now</b></p>
        <p>Ready to take control of your family's finances? Sign up for FamBudget today and start your journey towards financial freedom.</p>
        <p style={{marginTop:'20%'}}>The application was built by <strong>Dmytro Moroziuk ⓒ Copyright 2024</strong></p>
      </div>
    );
  }
}
