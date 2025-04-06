import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useParams } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Form, Button, Container } from 'react-bootstrap';
import axiosInstance from './axiosInstance'

const AddEditParent = () => {
  // Parent model: FirstName, LastName, Email, and UserId (for linking the Identity user)
  const [parent, setParent] = useState({ firstName: '', lastName: '', email: '', userId: '' });
  const [users, setUsers] = useState([]); // for the dropdown list of unlinked users
  const navigate = useNavigate();
  const { id } = useParams(); // parent's id from route (for edit)

  // Fetch unlinked Identity users for parents and, if editing, fetch the parent data
  useEffect(() => {
    // Get unlinked Identity users for parents.
    axiosInstance.get(`${BASE_URL}/api/account/parent/unlinked`)
      .then(response => setUsers(response.data))
      .catch(error => console.error('Error fetching unlinked users:', error));

    // If editing an existing parent, load its data.
    if (id) {
      axios.get(`${BASE_URL}/api/parents/${id}`)
        .then(response => setParent(response.data))
        .catch(error => console.error('Error fetching parent data:', error));
    }
  }, [id]);

  const handleChange = e => {
    const { name, value } = e.target;
    setParent(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = e => {
    e.preventDefault();
    if (id) {
      axiosInstance.put(`${BASE_URL}/api/parents/${id}`, parent)
        .then(() => navigate('/parents'))
        .catch(error => console.error('Error updating parent:', error));
    } else {
      axiosInstance.post(`${BASE_URL}/api/parents`, parent)
        .then(() => navigate('/parents'))
        .catch(error => console.error('Error adding parent:', error));
    }
  };

  return (
    <Container>
      <h1>{id ? 'Edit Parent' : 'Add Parent'}</h1>
      <Form onSubmit={handleSubmit}>
        <Form.Group controlId="firstName" className="mb-3">
          <Form.Label>First Name</Form.Label>
          <Form.Control
            type="text"
            name="firstName"
            value={parent.firstName}
            onChange={handleChange}
            required
          />
        </Form.Group>

        <Form.Group controlId="lastName" className="mb-3">
          <Form.Label>Last Name</Form.Label>
          <Form.Control
            type="text"
            name="lastName"
            value={parent.lastName}
            onChange={handleChange}
            required
          />
        </Form.Group>

        <Form.Group controlId="email" className="mb-3">
          <Form.Label>Email</Form.Label>
          <Form.Control
            type="email"
            name="email"
            value={parent.email}
            onChange={handleChange}
            required
          />
        </Form.Group>

        <Form.Group controlId="userId" className="mb-3">
          <Form.Label>Link Identity User (Email)</Form.Label>
          <Form.Control
            as="select"
            name="userId"
            value={parent.userId}
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

export default AddEditParent;
