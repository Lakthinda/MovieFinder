import React from 'react';
import { connect } from 'react-redux';

const Movie = (props) => {
    return (
        <div className="card">
        <img className="card-img-top" 
                 src={props.Poster}  
             alt="{props.Title}" 
             />
        <div className="card-body">        
        <p className="card-text">{props.Title}</p>
        <p><button>Buy</button></p>
        </div>
      </div>
    )
  }

const DisplayMovies = (props) => {
	return (
   	 <div className="card-columns">
    	  {props.movies.map(movie => <Movie {...movie} />)}
     </div>
  )
}

export default connect()(DisplayMovies);