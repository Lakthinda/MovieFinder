import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import MovieDetails from './components/MovieDetails';

export default () => (
  <Layout>
    <Route exact path='/' component={Home} />
    <Route path='/moviedetails/:movieTitle?' component={MovieDetails} />    
  </Layout>
);
