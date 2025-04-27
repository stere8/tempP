import React, { useEffect, useState } from 'react';
import axiosInstance from './axiosInstance';
import { Link } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Table, Button, Container, Alert, Spinner,Badge } from 'react-bootstrap';

const Staff = () => {
  const [staff, setStaff] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchStaff = async () => {
      try {
        const response = await axiosInstance.get(`${BASE_URL}/api/staff`);
        console.log('Staff response:', response.data);
        // Check if response.data is an array; if not, attempt to extract from a $values property.
        const staffData = Array.isArray(response.data)
          ? response.data
          : response.data.$values || [];
        setStaff(staffData);
      } catch (error) {
        console.error('Error fetching staff:', error);
        setError('Error fetching staff data');
      } finally {
        setLoading(false);
      }
    };

    fetchStaff();
  }, []);

  const deleteStaff = async (id) => {
    try {
      await axiosInstance.delete(`${BASE_URL}/api/staff/${id}`);
      setStaff(prevStaff => prevStaff.filter(s => s.staffId !== id));
    } catch (error) {
      console.error('Error deleting staff:', error);
      setError('Error deleting staff record');
    }
  };

  if (loading) {
    return (
      <Container className="text-center mt-5">
        <Spinner animation="border" role="status">
          <span className="visually-hidden">Loading...</span>
        </Spinner>
        <p>Loading staff data...</p>
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
      <h1>Staff</h1>
      <Button as={Link} to="/staff/add" variant="primary" className="mb-3">
        Add Staff
      </Button>
      {staff.length === 0 ? (
        <Alert variant="warning">No staff available.</Alert>
      ) : (
        <Table striped bordered hover responsive>
          <thead>
            <tr>
              <th>First Name</th>
              <th>Last Name</th>
              <th>Email</th>
              <th>Subject Expertise</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {staff.map(s => (
              <tr key={s.staffId}>
                                <td>
                  {s.firstName}
                  {!s.userId && (
                    <Badge
                      pill
                      bg="secondary"
                      className="ms-2"
                      title="No user account linked"
                    >
                      Unlinked
                    </Badge>
                  )}
                </td>                <td>{s.lastName}</td>
                <td>{s.email}</td>
                <td>{s.subjectExpertise}</td>
                <td>
                  <Button as={Link} to={`/staff/edit/${s.staffId}`} variant="warning">
                    Edit
                  </Button>
                  <Button onClick={() => deleteStaff(s.staffId)} variant="danger" className="ms-2">
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

export default Staff;
