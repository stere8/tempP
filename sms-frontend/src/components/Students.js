import React, { useEffect, useState } from 'react'; 
import axiosInstance from './axiosInstance';
import { Link } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Table, Button, Container, Alert, Spinner,Badge } from 'react-bootstrap';

const Students = () => {
  const [students, setStudents] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchStudents = async () => {
      try {
        const url = `${BASE_URL}/api/students`;
        const response = await axiosInstance.get(url);
        console.log("Students response:", response.data);
        // Check if response.data is an array; if not, try to extract from $values property.
        const studentsData = Array.isArray(response.data)
          ? response.data
          : response.data.$values || [];
        setStudents(studentsData);
      } catch (error) {
        console.error('There was an error fetching the students!', BASE_URL, error);
        setError('Error fetching student data');
      } finally {
        setLoading(false);
      }
    };

    fetchStudents();
  }, []);

  const deleteStudent = async (id) => {
    try {
      await axiosInstance.delete(`${BASE_URL}/api/students/${id}`);
      setStudents(prevStudents => prevStudents.filter(student => student.studentId !== id));
    } catch (error) {
      console.error('Error deleting student:', error);
      setError('Error deleting student');
    }
  };

  const formatDate = (date) => {
    return date ? new Date(date).toLocaleDateString() : '';
  };

  if (loading) {
    return (
      <Container className="text-center mt-5">
        <Spinner animation="border" role="status">
          <span className="visually-hidden">Loading...</span>
        </Spinner>
        <p>Loading student data...</p>
      </Container>
    );
  }

  if (error) {
    return (
      <Container>
        <Alert variant="danger">{error}</Alert>
      </Container>
    );
  }

  return (
    <Container>
      <h1>Students</h1>
      <Button as={Link} to="/students/add" variant="primary" className="mb-3">
        Add Student
      </Button>
      {students.length === 0 ? (
        <Alert variant="warning">No students available.</Alert>
      ) : (
        <Table striped bordered hover responsive>
          <thead>
            <tr>
              <th>First Name</th>
              <th>Last Name</th>
              <th>Date of Birth</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {students.map(student => (
              <tr key={student.studentId}>
                                <td>
                  {student.firstName}
                  {!student.userId && (
                    <Badge
                      pill
                      bg="secondary"
                      className="ms-2"
                      title="No user account linked"
                    >
                      Unlinked
                    </Badge>
                  )}
                </td>
                <td>{student.lastName}</td>
                <td>{formatDate(student.dateOfBirth)}</td>
                <td>
                  <Button as={Link} to={`/students/edit/${student.studentId}`} variant="warning">
                    Edit
                  </Button>
                  <Button onClick={() => deleteStudent(student.studentId)} variant="danger" className="ms-2">
                    Delete
                  </Button>
                </td>
              </tr>
            ))}
          </tbody>
        </Table>
      )}
    </Container>
  );
};

export default Students;
