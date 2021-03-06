import React from 'react';
import { connect } from 'react-redux';
import DisplayMovies from './DisplayMovies';
import SearchMovie from './SearchMovie';
import { bindActionCreators } from 'redux';
import { actionCreators } from '../store/MoviesStore';

class MovieFinder extends React.Component{
  componentWillMount() {
    // This method runs when the component is first added to the page
    this.props.requestMovies();
    // console.log('this is called..');
  }
  
    render(){
      return (
        <div>
          {/* <SearchMovie /> */}
          <DisplayMovies movies={this.props.movies} />
          {this.props.isLoading ? <span>Loading...</span> : []}
        </div>
      );
    }
  }

  export default connect(
     state => state.movies,
     dispatch => bindActionCreators(actionCreators,dispatch)
  )(MovieFinder);