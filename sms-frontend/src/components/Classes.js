import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { ListGroup, Container, Alert, Spinner } from 'react-bootstrap';
import { BASE_URL } from '../settings';
import axiosInstance from './axiosInstance';

const Classes = () => {
  const [classes, setClasses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchClasses = async () => {
      try {
        setLoading(true);
        setError(null);

        const response = await axiosInstance.get(`${BASE_URL}/api/classes`);
        console.log("Classes response:", response.data);

        // The data has a top-level $values array:
        // e.g. { $id: "1", $values: [ { $id: "2", viewedClass: { ... }, ... }, ... ] }
        const classesData = Array.isArray(response.data)
          ? response.data
          : response.data.$values || [];

        setClasses(classesData);
      } catch (err) {
        console.error('Error fetching classes:', err);
        setError('Failed to load classes');
      } finally {
        setLoading(false);
      }
    };

    fetchClasses();
  }, []);

  if (loading) {
    return (
      <Container className="text-center mt-5">
        <Spinner animation="border" role="status">
          <span className="visually-hidden">Loading...</span>
        </Spinner>
        <p>Loading classes...</p>
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
      <h1>Classes</h1>
      {classes.length === 0 ? (
        <Alert variant="warning" className="mt-3">
          No classes available.
        </Alert>
      ) : (
        <ListGroup>
          {classes.map((classItem) => {
            // classItem.viewedClass holds the actual class data
            const c = classItem.viewedClass; 
            // or rename for clarity: const { classId, name } = classItem.viewedClass;
            return (
              <ListGroup.Item key={c.classId}>
                <Link to={`/classes/${c.classId}`}>{c.name}</Link> 
                {/* e.g. "P3A" */}
              </ListGroup.Item>
            );
          })}
        </ListGroup>
      )}
    </Container>
  );
};

export default Classes;
