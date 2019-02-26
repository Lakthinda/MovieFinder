import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';

const Movie = (props) => {
    return (
        <div className="card">
        <img className="card-img-top" 
                 src={props.poster}
             alt="{props.title}" 
             />
        <div className="card-body">        
        <p className="card-text">{props.title}</p>
        <p><a className='btn btn-default' href={`/moviedetails/${props.title}`}>Buy Movie</a></p>
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