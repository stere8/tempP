import React, { useContext } from 'react';
import { Navbar, Nav, Container, Button } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import { AuthContext } from './AuthContext';
import { useNavigate } from 'react-router-dom';

const NavigationBar = () => {
  const { userRole, studentId, staffId, parentId } = useContext(AuthContext);
  const navigate = useNavigate();
  console.log(userRole);
  console.log(userRole);
  console.log(userRole);
  console.log(userRole);
  // Determine the edit profile route based on the role-specific ID
  const getEditRoute = () => {
    if (staffId) return `/staff/edit/${staffId}`;
    if (studentId) return `/student/edit/${studentId}`;
    if (parentId) return `/parent/edit/${parentId}`;
    return '/account/edit';
  };

  const handleLogin = () => navigate('/login');
  const handleRegister = () => navigate('/register');

  return (
    <Navbar bg="dark" variant="dark" expand="lg">
      <Container>
        <LinkContainer to="/">
          <Navbar.Brand>School Management System</Navbar.Brand>
        </LinkContainer>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="me-auto">
            {userRole && userRole === 'Admin' && (
              <>
                <LinkContainer to="/dashboard">
                  <Nav.Link>Dashboard</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/attendance">
                  <Nav.Link>Attendance</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/classes">
                  <Nav.Link>Classes</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/enrollments">
                  <Nav.Link>Enrollments</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/teacher-enrollments">
                  <Nav.Link>Teacher Enrollments</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/lessons">
                  <Nav.Link>Lessons</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/marks">
                  <Nav.Link>Marks</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/staff">
                  <Nav.Link>Staff</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/students">
                  <Nav.Link>Students</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/parents">
                  <Nav.Link>Parents</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/timetable">
                  <Nav.Link>Timetable</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/parent-students">
                  <Nav.Link>Parent Assignments</Nav.Link>
                </LinkContainer>
              </>
            )}

            {userRole && userRole === 'Teacher' && (
              <>
                <LinkContainer to="/dashboard">
                  <Nav.Link>Dashboard</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/attendance">
                  <Nav.Link>Attendance</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/classes">
                  <Nav.Link>Classes</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/marks">
                  <Nav.Link>Marks</Nav.Link>
                </LinkContainer>
              </>
            )}

            {userRole && userRole === 'Parent' && (
              <>
                <LinkContainer to="/dashboard">
                  <Nav.Link>Dashboard</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/parents/students">
                  <Nav.Link>My Students</Nav.Link>
                </LinkContainer>
              </>
            )}

            {userRole && userRole === 'Student' && (
              <>
                <LinkContainer to="/dashboard">
                  <Nav.Link>Dashboard</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/attendance">
                  <Nav.Link>Attendance</Nav.Link>
                </LinkContainer>
                <LinkContainer to="/timetable">
                  <Nav.Link>Timetable</Nav.Link>
                </LinkContainer>
              </>
            )}
          </Nav>
          <Nav>
            {userRole ? (
              <>
                <LinkContainer to={getEditRoute()}>
                  <Nav.Link>Edit Profile</Nav.Link>
                </LinkContainer>
                                <LinkContainer to="/logout">
                  <Nav.Link>Logout</Nav.Link>
                </LinkContainer>
              </>
            ) : (
              <>
                <Button variant="outline-light" onClick={handleLogin}>
                  Login
                </Button>
                <Button variant="outline-light" onClick={handleRegister} className="ms-2">
                  Register
                </Button>
              </>
            )}
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default NavigationBar;
