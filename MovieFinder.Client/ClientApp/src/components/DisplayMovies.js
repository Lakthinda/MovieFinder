import React from 'react';
import { connect } from 'react-redux';
import './DisplayMovies.css';

const Movie = (props) => {
    return (
      <div className="display-movie-col col-sm-4">
      <a href={`/moviedetails/${props.title}`}>
      <div className="card">
        <img className="card-img-top" 
                 src={props.poster}                 
                 onError={(e)=>{e.target.onerror = null; e.target.src="http://www.bsmc.net.au/wp-content/uploads/No-image-available.jpg"}}
             alt={props.title} 
             />        
        <div className="card-body">        
        <p className="card-text">{props.title}</p>        
        <p><a className='btn btn-default' href={`/moviedetails/${props.title}`}>Select Movie</a></p>
        </div>
      </div>
      </a>
      </div>
    )
  }

const DisplayMovies = (props) => {
	return (
   	 <div>
    	  {props.movies.map(movie => <Movie {...movie} />)}
     </div>
     
  )
}

export default connect()(DisplayMovies);