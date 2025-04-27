import React, { useEffect, useState } from 'react';
import axiosInstance from './axiosInstance';
import { Link } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Table, Button, Container, Alert, Spinner,Badge } from 'react-bootstrap';

const Parents = () => {
  const [parents, setParents] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchParents = async () => {
      try {
        const response = await axiosInstance.get(`${BASE_URL}/api/parents`);
        console.log("Parents response:", response.data);
        // Check if the response is an array; if not, try to extract from $values or a similar property.
        const parentsData = Array.isArray(response.data.$value)
          ? response.data
          : response.data.$values || response.data.parents || [];
        setParents(parentsData);
      } catch (error) {
        console.error('Error fetching parents:', error);
        setError('Error fetching parents data');
      } finally {
        setLoading(false);
      }
    };

    fetchParents();
  }, []);

  const deleteParent = async (id) => {
    try {
      await axiosInstance.delete(`${BASE_URL}/api/parents/${id}`);
      setParents(prev => prev.filter(parent => parent.parentId !== id));
    } catch (error) {
      console.error('Error deleting parent:', error);
      setError('Error deleting parent');
    }
  };

  if (loading) {
    return (
      <Container className="text-center mt-5">
        <Spinner animation="border" role="status">
          <span className="visually-hidden">Loading...</span>
        </Spinner>
        <p>Loading parents data...</p>
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
      <h1>Parents</h1>
      <Button as={Link} to="/parents/add" variant="primary" className="mb-3">
        Add Parent
      </Button>
      {parents.length === 0 ? (
        <Alert variant="warning">No parents available.</Alert>
      ) : (
        <Table striped bordered hover responsive>
          <thead>
            <tr>
              <th>First Name</th>
              <th>Last Name</th>
              <th>Email</th>
              <th>User ID</th>
              <th>Student Count</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {parents.map(parent => (
              <tr key={parent.parentId}>
                                <td>
                  {parent.firstName}
                  {!parent.userId && (
                    <Badge
                      pill
                      bg="secondary"
                      className="ms-2"
                      title="No user account linked"
                    >
                      Unlinked
                    </Badge>
                  )}
                </td>                <td>{parent.lastName}</td>
                <td>{parent.email}</td>
                <td>{parent.userId}</td>
                <td>{parent.studentParents ? parent.studentParents.length : 0}</td>
                <td>
                  <Button as={Link} to={`/parents/edit/${parent.parentId}`} variant="warning">
                    Edit
                  </Button>
                  <Button onClick={() => deleteParent(parent.parentId)} variant="danger" className="ms-2">
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

export default Parents;
