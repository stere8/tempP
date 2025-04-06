import React, { useEffect, useState } from 'react';
import axiosInstance from './axiosInstance';
import { Container, Table, Alert, Button } from 'react-bootstrap';
import { BASE_URL } from '../settings';
import { LinkContainer } from 'react-router-bootstrap';

const AdminParentAssignments = () => {
  const [assignments, setAssignments] = useState([]);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchAssignments = async () => {
      try {
        const response = await axiosInstance.get(`${BASE_URL}/api/ParentStudent`);
        setAssignments(response.data);
      } catch (err) {
        console.error("Error fetching assignments:", err);
        setError("Failed to load assignments");
      }
    };

    fetchAssignments();
  }, []);

  if (error) {
    return <Alert variant="danger">{error}</Alert>;
  }

  return (
    <Container>
      <h1>Parent–Student Assignments</h1>
      {/* Add button to create a new assignment */}
      <LinkContainer to="/parent-student/add">
        <Button variant="primary" className="mb-3">
          Add Assignment
        </Button>
      </LinkContainer>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Parent Name</th>
            <th>Student Name</th>
            <th>Primary?</th>
          </tr>
        </thead>
        <tbody>
          {assignments.map(a => (
            <tr key={`${a.parentId}-${a.studentId}`}>
              <td>{a.parent?.firstName} {a.parent?.lastName}</td>
              <td>{a.student?.firstName} {a.student?.lastName}</td>
              <td>{a.isPrimary ? "Yes" : "No"}</td>
            </tr>
          ))}
        </tbody>
      </Table>
    </Container>
  );
};

export default AdminParentAssignments;
