import React, { useState } from 'react';
import axiosInstance from './axiosInstance';
import { BASE_URL } from '../settings';
import { Form, Button, Container } from 'react-bootstrap';

const AssignStudentToParent = () => {
  const [assignment, setAssignment] = useState({
    parentId: '',
    studentId: '',
    isPrimary: false
  });
  const [message, setMessage] = useState('');

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setAssignment(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axiosInstance.post(`${BASE_URL}/api/ParentStudent/assign`, assignment);
      setMessage('Assignment successful!');
    } catch (error) {
      console.error('Error assigning student to parent:', error);
      setMessage('Assignment failed.');
    }
  };

  return (
    <Container>
      <h2>Assign Student to Parent</h2>
      <Form onSubmit={handleSubmit}>
        <Form.Group controlId="parentId">
          <Form.Label>Parent ID</Form.Label>
          <Form.Control type="number" name="parentId" value={assignment.parentId} onChange={handleChange} required />
        </Form.Group>
        <Form.Group controlId="studentId">
          <Form.Label>Student ID</Form.Label>
          <Form.Control type="number" name="studentId" value={assignment.studentId} onChange={handleChange} required />
        </Form.Group>
        <Form.Group controlId="isPrimary">
          <Form.Check 
            type="checkbox"
            label="Primary Parent"
            name="isPrimary"
            checked={assignment.isPrimary}
            onChange={handleChange}
          />
        </Form.Group>
        <Button variant="primary" type="submit">Assign</Button>
      </Form>
      {message && <p>{message}</p>}
    </Container>
  );
};

export default AssignStudentToParent;
