import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { actionCreators } from '../store/MovieDetailsStore';
import "./MovieDetails.css"
import CurrencyFormat from 'react-currency-format';

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
        <div className="display-movies">    
          {this.props.isLoading ? <span>Loading...</span> : []}  
          <div className="card">
            <img className="card-img-top" 
                 src={this.props.movieDetails.poster}                 
                 onError={(e)=>{e.target.onerror = null; e.target.src="http://www.bsmc.net.au/wp-content/uploads/No-image-available.jpg"}}
             alt={this.props.movieDetails.title} 
             />
            <div className="card-body">
              <h5 className="card-title">{this.props.movieDetails.title}</h5>
              <h6 className="card-subtitle mb-2">{this.props.movieDetails.year}</h6>
              <p className="card-text">{this.props.movieDetails.plot}</p>
              <p>
                <ul>                  
                  <li>Genre: {this.props.movieDetails.genre}</li>
                  <li>Director: {this.props.movieDetails.director}</li>
                  <li>Actors: {this.props.movieDetails.actors}</li>
                  <li>Language: {this.props.movieDetails.language}</li>
                  <li>Country: {this.props.movieDetails.country}</li>
                  <li>Metascore: {this.props.movieDetails.metascore}</li>
                  <li>Votes: {this.props.movieDetails.votes}</li>                         
                </ul>
              </p>                                        
            </div>
            <div className="card-footer text-muted">
            Price: <CurrencyFormat value={this.props.movieDetails.price} displayType={'text'} decimalScale={2} fixedDecimalScale={true} prefix={'$'} />            
            </div>                    
          </div>                      
        </div>
      );
    }
  }

  export default connect(
     state => state.movieDetails,
     dispatch => bindActionCreators(actionCreators,dispatch)
  )(MovieDetails);