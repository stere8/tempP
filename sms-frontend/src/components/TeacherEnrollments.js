import React, { useEffect, useState } from 'react';
import axiosInstance from './axiosInstance';
import { Link } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Table, Button, Container, Alert, Spinner } from 'react-bootstrap';

const TeacherEnrollments = () => {
  const [teacherEnrollments, setTeacherEnrollments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchTeacherEnrollments = async () => {
      try {
        const response = await axiosInstance.get(`${BASE_URL}/api/TeacherEnrollments`);
        console.log("TeacherEnrollments response:", response.data);
        // Check if response.data is an array; if not, attempt to extract it from $values or another property.
        const enrollmentsData = Array.isArray(response.data)
          ? response.data
          : response.data.$values || [];
        setTeacherEnrollments(enrollmentsData);
      } catch (error) {
        console.error('Error fetching teacher enrollments:', error);
        setError('Error fetching teacher enrollments');
      } finally {
        setLoading(false);
      }
    };

    fetchTeacherEnrollments();
  }, []);

  const deleteTeacherEnrollment = async (id) => {
    try {
      await axiosInstance.delete(`${BASE_URL}/api/TeacherEnrollments/${id}`);
      setTeacherEnrollments(prevEnrollments => 
        prevEnrollments.filter(enrollment => enrollment.enrollmentRef !== id)
      );
    } catch (error) {
      console.error('Error deleting teacher enrollment:', error);
      setError('Error deleting teacher enrollment');
    }
  };

  if (loading) {
    return (
      <Container className="text-center mt-5">
        <Spinner animation="border" role="status">
          <span className="visually-hidden">Loading...</span>
        </Spinner>
        <p>Loading teacher enrollments...</p>
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
      <h1>Teacher Enrollments</h1>
      <Button as={Link} to="/teacher-enrollments/add" variant="primary">
        Add Teacher Enrollment
      </Button>
      {teacherEnrollments.length === 0 ? (
        <Alert variant="warning" className="mt-3">
          No teacher enrollments available.
        </Alert>
      ) : (
        <Table striped bordered hover>
          <thead>
            <tr>
              <th>Class</th>
              <th>Teacher</th>
              <th>Lesson</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {teacherEnrollments.map(enrollment => (
              <tr key={enrollment.enrollmentRef}>
                <td>{enrollment.assignedClass || 'N/A'}</td>
                <td>{enrollment.enrolledTeacher || 'N/A'}</td>
                <td>{enrollment.assignedLesson || 'N/A'}</td>
                <td>
                  {enrollment.enrollmentRef !== 0 ? (
                    <>
                      <Button as={Link} to={`/teacher-enrollments/edit/${enrollment.enrollmentRef}`} variant="warning">
                        Edit
                      </Button>
                      <Button onClick={() => deleteTeacherEnrollment(enrollment.enrollmentRef)} variant="danger" className="ms-2">
                        Delete
                      </Button>
                    </>
                  ) : (
                    'N/A'
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </Table>
      )}
    </Container>
  );
};

export default TeacherEnrollments;
