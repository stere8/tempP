import React, { useState, useEffect } from 'react';
import axiosInstance from './axiosInstance';
import { useNavigate, useParams } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Form, Button, Container } from 'react-bootstrap';

const AddEditParent = () => {
  // Parent model
  const [parent, setParent] = useState({
    firstName: '',
    lastName: '',
    email: '',
    userId: ''
  });

  const [users, setUsers] = useState([]);   // unlinked + possibly the already-linked user
  const navigate = useNavigate();
  const { id } = useParams(); // parent's id from route (for edit)

  useEffect(() => {
    const fetchData = async () => {
      try {
        // For storing info about the user the parent is currently linked to:
        let linkedUser = null;

        // 1) If editing, fetch the parent.
        if (id) {
          const { data: parentData } = await axiosInstance.get(`${BASE_URL}/api/parents/${id}`);
          // You might unify the parent's userId to lowercase if the DB returns uppercase, but not strictly needed:
          setParent(parentData);
          console.log("Fetched parent:", parentData);

          // 2) If parent's userId exists, fetch that user’s info (the "linked" user).
          if (parentData.userId) {
            const { data: userInfo } = await axiosInstance.get(`${BASE_URL}/api/account/userinfo/${parentData.userId}`);
            linkedUser = {
              // unify the ID to one field
              identityUserId: (userInfo.id ?? '').toLowerCase(),
              email: userInfo.email ?? userInfo.userName
            };
            console.log("Fetched account info (linkedUser):", linkedUser);
          }
        }

        // 3) Fetch unlinked users for "parent" type
        const { data: raw } = await axiosInstance.get(`${BASE_URL}/api/account/parent/unlinked`);
        const arrayData = Array.isArray(raw) ? raw : (raw.$values || []);
        console.log("Fetched unlinked users (raw):", arrayData);

        // Normalize them all to { identityUserId, email }
        const unlinkedNormalized = arrayData.map(u => ({
          identityUserId: (u.identityUserId ?? u.id)?.toLowerCase(),
          email: u.email
        }));

        // 4) If we have a linkedUser, ensure they appear in our final list
        if (
          linkedUser &&
          !unlinkedNormalized.some(u => u.identityUserId === linkedUser.identityUserId)
        ) {
          unlinkedNormalized.push(linkedUser);
        }

        setUsers(unlinkedNormalized);
        console.log("Final users for dropdown:", unlinkedNormalized);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, [id]);

  // Keeps the parent data in sync with form inputs
  const handleChange = e => {
    const { name, value } = e.target;
    setParent(prev => ({ ...prev, [name]: value }));
  };

  // Submits either an update or create
  const handleSubmit = e => {
    e.preventDefault();
    if (id) {
      console.log('Updating parent:', parent);
      axiosInstance
        .post(`${BASE_URL}/api/parents/${id}`, parent)
        .then(() => {
          console.log('Parent updated successfully.');
          navigate('/parents');
        })
        .catch(error => {
          console.error('Error updating parent:', error);
          if (error.response) {
            console.error("Response data:", error.response.data);
            console.error("Response status:", error.response.status);
            console.error("Response headers:", error.response.headers);
          }
        });
    } else {
      axiosInstance
        .post(`${BASE_URL}/api/parents`, parent)
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
            {users.map((user) => (
              <option
                key={user.identityUserId}
                value={user.identityUserId}
              >
                {`${user.email} (${user.identityUserId})`}
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
