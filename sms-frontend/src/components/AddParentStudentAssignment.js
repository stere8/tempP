import React, { useState, useEffect } from 'react';
import { Form, Button, Container, Alert } from 'react-bootstrap';
import axiosInstance from './axiosInstance';
import { BASE_URL } from '../settings';
import { useNavigate } from 'react-router-dom';

const AddParentStudentAssignment = () => {
  const [parents, setParents] = useState([]);
  const [students, setStudents] = useState([]);
  const [assignment, setAssignment] = useState({
    parentId: '',
    studentId: '',
    isPrimary: false,
  });
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    // Fetch available parents and students
    const fetchData = async () => {
      try {
        const parentsResp = await axiosInstance.get(`${BASE_URL}/api/parents`);
        console.log("Fetched parents:", parentsResp.data);
        setParents(parentsResp.data.$values || []);
        const studentsResp = await axiosInstance.get(`${BASE_URL}/api/students`);
        console.log("Fetched students:", studentsResp.data);
        setStudents(studentsResp.data.$values || []);
      } catch (err) {
        console.error("Error fetching data:", err);
        setError("Failed to load parents and students data");
      }
    };

    fetchData();
  }, []);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setAssignment(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await axiosInstance.post(`${BASE_URL}/api/ParentStudent/assign`, assignment);
      setSuccess('Assignment created successfully');
      setError('');
      // Optionally, reset the form:
      setAssignment({ parentId: '', studentId: '', isPrimary: false });
      // Redirect to the assignments view, if needed:
      navigate('/parent-student');
    } catch (err) {
      console.error("Error creating assignment:", err);
      setError("Failed to create assignment");
      setSuccess('');
    }
  };

  return (
    <Container>
      <h1>Add Parent–Student Assignment</h1>
      {error && <Alert variant="danger">{error}</Alert>}
      {success && <Alert variant="success">{success}</Alert>}
      <Form onSubmit={handleSubmit}>
        <Form.Group controlId="parentId" className="mb-3">
          <Form.Label>Select Parent</Form.Label>
          <Form.Control
            as="select"
            name="parentId"
            value={assignment.parentId}
            onChange={handleChange}
            required
          >
            <option value="">Select a parent</option>
            {parents.map((parent) => (
              <option key={parent.parentId} value={parent.parentId}>
                {parent.firstName} {parent.lastName}
              </option>
            ))}
          </Form.Control>
        </Form.Group>

        <Form.Group controlId="studentId" className="mb-3">
          <Form.Label>Select Student</Form.Label>
          <Form.Control
            as="select"
            name="studentId"
            value={assignment.studentId}
            onChange={handleChange}
            required
          >
            <option value="">Select a student</option>
            {students.map((student) => (
              <option key={student.studentId} value={student.studentId}>
                {student.firstName} {student.lastName}
              </option>
            ))}
          </Form.Control>
        </Form.Group>

        <Form.Group controlId="isPrimary" className="mb-3">
          <Form.Check
            type="checkbox"
            label="Primary Assignment"
            name="isPrimary"
            checked={assignment.isPrimary}
            onChange={handleChange}
          />
        </Form.Group>

        <Button variant="primary" type="submit">
          Add Assignment
        </Button>
      </Form>
    </Container>
  );
};

export default AddParentStudentAssignment;
