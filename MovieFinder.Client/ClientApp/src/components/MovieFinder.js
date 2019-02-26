import React from 'react';
import { connect } from 'react-redux';
import DisplayMovies from './DisplayMovies';
import SearchMovie from './SearchMovie';


class MovieFinder extends React.Component{
    state = {
        movies: [
          {
              "Title": "Star Wars: Episode IV - A New Hope",
              "Year": "1977",
              "ID": "cw0076759",
              "Type": "movie",
              "Poster": "http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg"
          },
          {
              "Title": "Star Wars: Episode IV - A New Hope",
              "Year": "1977",
              "ID": "cw0076759",
              "Type": "movie",
              "Poster": "http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg"
          },
          {
              "Title": "Star Wars: Episode IV - A New Hope",
              "Year": "1977",
              "ID": "cw0076759",
              "Type": "movie",
              "Poster": "http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg"
          }
          ]
    };
      render(){
      return (
        <div>
          <SearchMovie />
          <DisplayMovies movies={this.state.movies} />
        </div>
      );
    }
  }

  export default connect()(MovieFinder);