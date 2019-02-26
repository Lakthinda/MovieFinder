import React from 'react';
import { connect } from 'react-redux';

class SearchMovie extends React.Component{
  render(){
  	return (
    	<form>
      	<input type="text" placeholder="Search for movies in the list" />
        <button type="submit">Search</button>
      </form>
    );
  }
}
export default connect()(SearchMovie);