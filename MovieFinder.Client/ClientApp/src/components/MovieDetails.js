import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { actionCreators } from '../store/MovieDetailsStore';

class MovieDetails extends React.Component{
  componentWillMount() {
    // This method runs when the component is first added to the page
    this.props.requestForMovieDetails("");    
  }

  componentWillReceiveProps(props) {
    this.props.requestForMovieDetails(props.match.params.movieTitle);
  }
  
    render(){
      return (
        <div>
          <div>
            {this.props.movieDetails.title}            
          </div>
          {this.props.isLoading ? <span>Loading...</span> : []}
        </div>
      );
    }
  }

  export default connect(
     state => state.movieDetails,
     dispatch => bindActionCreators(actionCreators,dispatch)
  )(MovieDetails);