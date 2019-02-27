import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';
import MovieDetails from './components/MovieDetails';

export default () => (
  <Layout>
    <Route exact path='/' component={Home} />
    <Route path='/moviedetails/:movieTitle?' component={MovieDetails} />
    <Route path='/counter' component={Counter} />
    <Route path='/fetchdata/:startDateIndex?' component={FetchData} />
  </Layout>
);
