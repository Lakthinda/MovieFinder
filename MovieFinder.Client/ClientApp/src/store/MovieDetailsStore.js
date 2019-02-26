const requestMoviesType = 'REQUEST_MOVIES';
const receiveMoviesType = 'RECEIVE_MOVIES';
const initialState = { movieDetails: [], isLoading: false };

export const actionCreators = {

  requestForMovieDetails: movieTitle => async (dispatch,getState) => {
    if (typeof getState().movieDetails.movieDetails !== "undefined" && movieTitle === getState().movieDetails.movieDetails.title) {      
      return;
    }
    dispatch({ type: requestMoviesType, movieTitle});
    
    try{
        const url = `api/moviefinder/${movieTitle}`;
        const response = await fetch(url);
        const movieDetails = await response.json();

        dispatch({type: receiveMoviesType,movieTitle, movieDetails});          
    }catch (Exception){
      return;
    }    
  }
};

export const reducer = (state, action) => {
  state = state || initialState;

  if (action.type === requestMoviesType) {
    return {
      ...state,      
      isLoading: true
    };
  }

  if (action.type === receiveMoviesType) {
    return {
      ...state,
      movieDetails: action.movieDetails,
      isLoading: false
    };
  }

  return state;
};
