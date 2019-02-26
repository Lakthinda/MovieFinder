import React from 'react';
import { connect } from 'react-redux';
import MovieFinder from './MovieFinder';

const Home = () => (
  <div className="container">
    <h1>Movie Finder</h1>
    <MovieFinder />
  </div>
);

export default connect()(Home);
