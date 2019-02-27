import React from 'react';
import { Col, Grid, Row } from 'react-bootstrap';
// import NavMenu from './NavMenu';

export default props => (
  <Grid fluid>
    <Row>
      <Col sm={6}>
      <div className="header">
        Movie Finder
      </div>
      </Col>
    </Row>
    <Row>
      <Col sm={6}>
        {props.children}      
      </Col>
    </Row>
  </Grid>
);
