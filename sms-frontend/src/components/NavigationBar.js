import React, { useContext } from 'react';
import { Navbar, Nav, Container, Button, NavDropdown } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import { AuthContext } from './AuthContext';
import { useNavigate } from 'react-router-dom';

const NavigationBar = () => {
  const { userRole, studentId, staffId, parentId } = useContext(AuthContext);
  const navigate = useNavigate();
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
                <NavDropdown title="Administration" id="admin-nav-dropdown">
                  <LinkContainer to="/dashboard">
                    <NavDropdown.Item>Dashboard</NavDropdown.Item>
                  </LinkContainer>
                  <LinkContainer to="/attendance">
                    <NavDropdown.Item>Attendance</NavDropdown.Item>
                  </LinkContainer>
                  <LinkContainer to="/classes">
                    <NavDropdown.Item>Classes</NavDropdown.Item>
                  </LinkContainer>
                  <LinkContainer to="/lessons">
                    <NavDropdown.Item>Lessons</NavDropdown.Item>
                  </LinkContainer>
                  <LinkContainer to="/marks">
                    <NavDropdown.Item>Marks</NavDropdown.Item>
                  </LinkContainer>
                </NavDropdown>
                <NavDropdown title="Users" id="users-nav-dropdown">
                  <LinkContainer to="/staff">
                    <NavDropdown.Item>Staff</NavDropdown.Item>
                  </LinkContainer>
                  <LinkContainer to="/students">
                    <NavDropdown.Item>Students</NavDropdown.Item>
                  </LinkContainer>
                  <LinkContainer to="/parents">
                    <NavDropdown.Item>Parents</NavDropdown.Item>
                  </LinkContainer>
                </NavDropdown>
                <NavDropdown title="Other Management" id="other-nav-dropdown">
                  <LinkContainer to="/enrollments">
                    <NavDropdown.Item>Enrollments</NavDropdown.Item>
                  </LinkContainer>
                  <LinkContainer to="/teacher-enrollments">
                    <NavDropdown.Item>Teacher Enrollments</NavDropdown.Item>
                  </LinkContainer>
                  <LinkContainer to="/timetable">
                    <NavDropdown.Item>Timetable</NavDropdown.Item>
                  </LinkContainer>
                  <LinkContainer to="/parent-students">
                    <NavDropdown.Item>Parent Assignments</NavDropdown.Item>
                  </LinkContainer>
                </NavDropdown>
              </>
            )}

            {userRole && userRole === 'Teacher' && (
              <>
                <NavDropdown title="Teacher Panel" id="teacher-nav-dropdown">
                  <LinkContainer to="/dashboard">
                    <NavDropdown.Item>Dashboard</NavDropdown.Item>
                  </LinkContainer>
                  <LinkContainer to="/attendance">
                    <NavDropdown.Item>Attendance</NavDropdown.Item>
                  </LinkContainer>
                  <LinkContainer to="/classes">
                    <NavDropdown.Item>Classes</NavDropdown.Item>
                  </LinkContainer>
                  <LinkContainer to="/marks">
                    <NavDropdown.Item>Marks</NavDropdown.Item>
                  </LinkContainer>
                </NavDropdown>
              </>
            )}

            {userRole && userRole === 'Parent' && (
              <>
                <NavDropdown title="Parent Panel" id="parent-nav-dropdown">
                  <LinkContainer to="/dashboard">
                    <NavDropdown.Item>Dashboard</NavDropdown.Item>
                  </LinkContainer>
                  <LinkContainer to="/parents/students">
                    <NavDropdown.Item>My Students</NavDropdown.Item>
                  </LinkContainer>
                </NavDropdown>
              </>
            )}

            {userRole && userRole === 'Student' && (
              <>
                <NavDropdown title="Student Panel" id="student-nav-dropdown">
                  <LinkContainer to="/dashboard">
                    <NavDropdown.Item>Dashboard</NavDropdown.Item>
                  </LinkContainer>
                </NavDropdown>
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
