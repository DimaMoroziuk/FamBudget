import React, { Component, useState, useEffect } from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import Reporting from './components/Statistic/Reporting';
import Purchase from './components/Purchase/Purchase';
import PurchaseAddPage from './components/Purchase/PurchaseAddPage';
import Income from './components/Income/Income';
import IncomeAddPage from './components/Income/IncomeAddPage';
import Category from './components/Category/Category';
import CategoryAddPage from './components/Category/CategoryAddPage';
import Access from './components/Access/Access';
import AccessAddPage from './components/Access/AccessAddPage';
import Login from './components/Login/Login';
import Registration from './components/Login/Registration';
import { useUser, userContext } from './contexts/userContext'


import './custom.css'


export default function App (){
  const displayName = App.name;
  const { user } = useUser();

    return (
      <Layout>
        <userContext.Provider value={user}>
          <Route path='/reporting' component={Reporting} />
          <Route path='/purchases' component={Purchase} />
          <Route path='/purchase/add/:id?' component={PurchaseAddPage} />
          <Route path='/incomes' component={Income} />
          <Route path='/income/add/:id?' component={IncomeAddPage} />
          <Route path='/categories' component={Category} />
          <Route path='/category/add/:id?' component={CategoryAddPage} />
          <Route path='/accesses' component={Access} />
          <Route path='/access/add/:id?' component={AccessAddPage} />
          <Route path='/login' component={Login} />
          <Route path='/registration' component={Registration} />
          <Route exact path='/' component={Home} />
        </userContext.Provider>
      </Layout>
    );
}
