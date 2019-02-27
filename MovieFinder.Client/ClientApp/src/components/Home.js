import React from 'react';
import { connect } from 'react-redux';
import MovieFinder from './MovieFinder';

const Home = () => (
  <div>    
    <MovieFinder />
  </div>
);

export default connect()(Home);
