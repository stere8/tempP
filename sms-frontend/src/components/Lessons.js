import React, { useEffect, useState } from 'react';
import axiosInstance from './axiosInstance';
import { Link } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Table, Button, Alert, Spinner } from 'react-bootstrap';

const Lessons = () => {
  const [lessons, setLessons] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchLessons = async () => {
      try {
        const response = await axiosInstance.get(`${BASE_URL}/api/lessons`);
        console.log(response.data);
        const lessonsData = Array.isArray(response.data)
          ? response.data
          : response.data.$values || [];
        setLessons(lessonsData);
        
      } catch (err) {
        console.error('Error fetching lessons:', err);
        setError('Failed to load lessons');
      } finally {
        setLoading(false);
      }
    };

    fetchLessons();
  }, []);

  const deleteLesson = async (id) => {
    try {
      await axiosInstance.delete(`${BASE_URL}/api/lessons/${id}`);
      setLessons(prevLessons => prevLessons.filter(lesson => lesson.lessonId !== id));
    } catch (error) {
      console.error('Error deleting lesson:', error);
      setError('Failed to delete lesson');
    }
  };

  if (loading) {
    return (
      <div className="text-center mt-5">
        <Spinner animation="border" role="status">
          <span className="visually-hidden">Loading...</span>
        </Spinner>
        <p>Loading lessons...</p>
      </div>
    );
  }

  if (error) {
    return <Alert variant="danger">{error}</Alert>;
  }

  return (
    <div>
      <h1>Lessons</h1>
      <Button as={Link} to="/lessons/add" variant="primary">
        Add Lesson
      </Button>
      {lessons.length === 0 ? (
        <Alert variant="warning" className="mt-3">
          No lessons available.
        </Alert>
      ) : (
        <Table striped bordered hover>
          <thead>
            <tr>
              <th>Name</th>
              <th>Description</th>
              <th>Subject</th>
              <th>Grade Level</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {lessons.map(lesson => (
              <tr key={lesson.lessonId}>
                <td>{lesson.name}</td>
                <td>{lesson.description}</td>
                <td>{lesson.subject}</td>
                <td>{lesson.gradeLevel}</td>
                <td>
                  <Button as={Link} to={`/lessons/edit/${lesson.lessonId}`} variant="warning">
                    Edit
                  </Button>
                  <Button onClick={() => deleteLesson(lesson.lessonId)} variant="danger" className="ms-2">
                    Delete
                  </Button>
                </td>
              </tr>
            ))}
          </tbody>
        </Table>
      )}
    </div>
  );
};

export default Lessons;
