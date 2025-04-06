import React, { useState, useEffect } from 'react';
import axiosInstance from './axiosInstance'
import { useNavigate, useParams } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Form, Button, Container } from 'react-bootstrap';

const AddEditStudent = () => {
  const [student, setStudent] = useState({
    firstName: '',
    lastName: '',
    dateOfBirth: '',
    userId: ''
  });
  const [users, setUsers] = useState([]);
  const navigate = useNavigate();
  const { id } = useParams();

  useEffect(() => {
    // Wrap everything in an async function
    const loadData = async () => {
      try {
        let fetchedStudent = null;

        // If editing, fetch the student first
        if (id) {
          const studentResp = await axiosInstance.get(`${BASE_URL}/students/${id}`);
          fetchedStudent = studentResp.data;
          // format the date
          if (fetchedStudent.dateOfBirth) {
            fetchedStudent.dateOfBirth = fetchedStudent.dateOfBirth.split('T')[0];
          }
          setStudent(fetchedStudent);
        }

        // Now fetch unlinked users
        const unlinkedResp = await axiosInstance.get(`${BASE_URL}/api/account/student/unlinked`);
        let fetchedUsers = unlinkedResp.data; // Array of { id, email }

        // If editing & the student has a userId, ensure that user is in the list
        if (id && fetchedStudent && fetchedStudent.userId) {
          // Check if that user is already in the unlinked list
          const exists = fetchedUsers.some(u => u.id === fetchedStudent.userId);
          if (!exists) {
            // We can guess the student's user email if we have it
            // If the student object has `user` or `email`, use that; otherwise use a placeholder
            const userEmail = fetchedStudent.user?.email || 'Assigned User';
            fetchedUsers.push({ id: fetchedStudent.userId, email: userEmail });
          }
        }

        setUsers(fetchedUsers);
      } catch (error) {
        console.error('Error loading data:', error);
      }
    };

    loadData();
  }, [id]);

  const handleChange = e => {
    const { name, value } = e.target;
    setStudent(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async e => {
    e.preventDefault();
    const payload = { ...student };

    try {
      if (id) {
        await axiosInstance.put(`${BASE_URL}/students/${id}`, payload);
      } else {
        await axiosInstance.post(`${BASE_URL}/students`, payload);
      }
      navigate('/students');
    } catch (error) {
      console.error('Error saving student:', error);
    }
  };

  return (
    <Container>
      <h1>{id ? 'Edit Student' : 'Add Student'}</h1>
      <Form onSubmit={handleSubmit}>
        <Form.Group controlId="firstName" className="mb-3">
          <Form.Label>First Name</Form.Label>
          <Form.Control
            type="text"
            name="firstName"
            value={student.firstName}
            onChange={handleChange}
            required
          />
        </Form.Group>

        <Form.Group controlId="lastName" className="mb-3">
          <Form.Label>Last Name</Form.Label>
          <Form.Control
            type="text"
            name="lastName"
            value={student.lastName}
            onChange={handleChange}
            required
          />
        </Form.Group>

        <Form.Group controlId="dateOfBirth" className="mb-3">
          <Form.Label>Date of Birth</Form.Label>
          <Form.Control
            type="date"
            name="dateOfBirth"
            value={student.dateOfBirth}
            onChange={handleChange}
            required
          />
        </Form.Group>

        <Form.Group controlId="userId" className="mb-3">
          <Form.Label>Link User Account</Form.Label>
          <Form.Control
            as="select"
            name="userId"
            value={student.userId || ''} // ensure it's a string or ''
            onChange={handleChange}
          >
            <option value="">Select a user</option>
            {users.map(user => (
              <option key={user.id} value={user.id}>
                {user.email}
              </option>
            ))}
          </Form.Control>
        </Form.Group>

        <Button variant="primary" type="submit">
          {id ? 'Update' : 'Add'}
        </Button>
      </Form>
    </Container>
  );
};

export default AddEditStudent;
