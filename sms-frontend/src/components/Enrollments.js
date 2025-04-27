import React, { useEffect, useState } from 'react';
import axiosInstance from './axiosInstance';
import { Link } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Table, Button, Container, Alert, Spinner } from 'react-bootstrap';

const Enrollments = () => {
  const [enrollments, setEnrollments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchEnrollments = async () => {
      try {
        const url = `${BASE_URL}/api/enrollments`;
        const response = await axiosInstance.get(url);
        console.log("Enrollments response:", response.data);
        // If the response isn't an array, try to extract from a property like $values or enrollments.
        const enrollmentsData = Array.isArray(response.data)
          ? response.data
          : response.data.$values || response.data.enrollments || [];
        setEnrollments(enrollmentsData);
      } catch (error) {
        console.error('There was an error fetching the enrollments!', error);
        setError('Error fetching enrollment data');
      } finally {
        setLoading(false);
      }
    };

    fetchEnrollments();
  }, []);

  const deleteEnrollment = async (id) => {
    try {
      await axiosInstance.delete(`${BASE_URL}/api/enrollments/${id}`);
      setEnrollments(prevEnrollments => prevEnrollments.filter(e => e.enrollmentRef !== id));
    } catch (error) {
      console.error('Error deleting enrollment:', error);
      setError('Error deleting enrollment record');
    }
  };

  if (loading) {
    return (
      <Container className="text-center mt-5">
        <Spinner animation="border" role="status">
          <span className="visually-hidden">Loading...</span>
        </Spinner>
        <p>Loading enrollments...</p>
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
      <h1>Enrollments</h1>
      <Button as={Link} to="/enrollments/add" variant="primary" className="mb-3">
        Add Enrollment
      </Button>
      {enrollments.length === 0 ? (
        <Alert variant="warning">No enrollments available.</Alert>
      ) : (
        <Table striped bordered hover responsive>
          <thead>
            <tr>
              <th>Class</th>
              <th>Student</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {enrollments.map(enrollment => (
              <tr key={enrollment.enrollmentRef}>
                <td>{enrollment.enrolledClass}</td>
                <td>{enrollment.enrolledStudent}</td>
                <td>
                  <Button as={Link} to={`/enrollments/edit/${enrollment.enrollmentRef}`} variant="warning">
                    Edit
                  </Button>
                  <Button onClick={() => deleteEnrollment(enrollment.enrollmentRef)} variant="danger" className="ms-2">
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

export default Enrollments;
