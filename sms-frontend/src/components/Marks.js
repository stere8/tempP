import React, { useEffect, useContext,useState } from 'react';
import axiosInstance from './axiosInstance';
import { Link } from 'react-router-dom';
import { AuthContext } from './AuthContext';
import { BASE_URL } from '../settings';
import { Table, Button, Alert, Spinner, Container } from 'react-bootstrap';

const Marks = () => {
  const { userRole } = useContext(AuthContext);
  const [marks, setMarks] = useState([]);
  const [students, setStudents] = useState([]);
  const [lessons, setLessons] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchMarks = async () => {
      try {
        const [marksResponse, studentsResponse, lessonsResponse] = await Promise.all([
            axiosInstance.get(`${BASE_URL}/api/marks`),
            axiosInstance.get(`${BASE_URL}/api/students`),
            axiosInstance.get(`${BASE_URL}/api/lessons`)
        ]);

        // Ensure marks is an array
        const marksData = Array.isArray(marksResponse.data)
          ? marksResponse.data
          : marksResponse.data.$values || [];
        setMarks(marksData);

        // Ensure students is an array
        const studentsData = Array.isArray(studentsResponse.data)
          ? studentsResponse.data
          : studentsResponse.data.$values || [];
        setStudents(studentsData);

        // Ensure lessons is an array
        const lessonsData = Array.isArray(lessonsResponse.data)
          ? lessonsResponse.data
          : lessonsResponse.data.$values || [];
        setLessons(lessonsData);
      } catch (error) {
        console.error('There was an error fetching the data!', error);
        setError('There was an error fetching the data!');
      } finally {
        setLoading(false);
      }
    };

    fetchMarks();
  }, []);

  const deleteMark = id => {
    axiosInstance.delete(`${BASE_URL}/api/marks/${id}`)
      .then(() => setMarks(prevMarks => prevMarks.filter(mark => mark.markId !== id)))
      .catch(error => console.error('Error deleting mark:', error));
  };

  const findStudentName = studentId => {
    const student = students.find(s => s.studentId === studentId);
    return student ? `${student.firstName} ${student.lastName}` : 'Unknown Student';
  };

  const findLessonName = lessonId => {
    const lesson = lessons.find(l => l.lessonId === lessonId);
    return lesson ? lesson.name : 'Unknown Lesson';
  };

  if (loading) {
    return (
      <Container className="text-center mt-5">
        <Spinner animation="border" role="status">
          <span className="visually-hidden">Loading...</span>
        </Spinner>
        <p>Loading marks...</p>
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
    <div>
      <h1>Marks</h1>
      {userRole === 'Teacher' && (
      <Button as={Link} to="/marks/add" variant="primary">Add Mark</Button>)}
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Student</th>
            <th>Lesson</th>
            <th>Mark</th>
            <th>Date</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {marks.map(mark => (
            <tr key={mark.markId}>
              <td>{findStudentName(mark.studentId)}</td>
              <td>{findLessonName(mark.lessonId)}</td>
              <td>{mark.markValue}</td>
              <td>{new Date(mark.date).toLocaleDateString()}</td>
              <td>
                <Button as={Link} to={`/marks/edit/${mark.markId}`} variant="warning">Edit</Button>
                <Button onClick={() => deleteMark(mark.markId)} variant="danger">Delete</Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  );
};

export default Marks;
