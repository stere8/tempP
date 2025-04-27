// src/components/AddEditStudent.js

import React, { useState, useEffect } from 'react';
import axiosInstance from './axiosInstance';
import { useNavigate, useParams } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Form, Button, Container } from 'react-bootstrap';

const AddEditStudent = () => {
  // Student state
  const [student, setStudent] = useState({
    firstName: '',
    lastName: '',
    dateOfBirth: '',
    userId: ''
  });

  // Dropdown user list state
  const [users, setUsers] = useState([]);
  const navigate = useNavigate();
  const { id } = useParams();

  useEffect(() => {
    const loadData = async () => {
      try {
        let linkedUser = null;
        let fetchedStudent = null;

        // 1) If editing, fetch the student data first.
        if (id) {
          const { data: studentData } = await axiosInstance.get(`${BASE_URL}/api/students/${id}`);
          fetchedStudent = studentData;
          // Format the date (assumes ISO string, e.g., "2024-04-07T00:00:00")
          if (fetchedStudent.dateOfBirth) {
            fetchedStudent.dateOfBirth = fetchedStudent.dateOfBirth.split('T')[0];
          }
          setStudent(fetchedStudent);
          console.log("Fetched student:", fetchedStudent);

          // 2) If the student has a linked userId, fetch that user’s info.
          if (fetchedStudent.userId) {
            const { data: userInfo } = await axiosInstance.get(`${BASE_URL}/api/account/userinfo/${fetchedStudent.userId}`);
            linkedUser = {
              // Normalize the identifier (check for identityUserId property or fallback to id)
              identityUserId: ((userInfo.identityUserId ?? userInfo.id) || "").toLowerCase(),
              email: userInfo.email ?? userInfo.userName
            };
            console.log("Fetched account info (linkedUser):", linkedUser);
          }
        }

        // 3) Fetch unlinked student users.
        const { data: raw } = await axiosInstance.get(`${BASE_URL}/api/account/student/unlinked`);
        // Handle response that might be an array or wrapped in a $values property.
        const arrayData = Array.isArray(raw) ? raw : (raw.$values || []);
        console.log("Fetched unlinked student users (raw):", arrayData);

        // 4) Normalize the list: for each user, ensure we have { identityUserId, email }
        const unlinkedNormalized = arrayData.map(u => ({
          identityUserId: (((u.identityUserId ?? u.id) || "").toLowerCase()),
          email: u.email
        }));

        // 5) If we're editing and the student has a linked user, ensure that user is included in the list.
        if (linkedUser && !unlinkedNormalized.some(u => u.identityUserId === linkedUser.identityUserId)) {
          unlinkedNormalized.push(linkedUser);
        }

        console.log("Final users for dropdown:", unlinkedNormalized);
        // 6) Update state with the normalized user list.
        setUsers(unlinkedNormalized);
      } catch (error) {
        console.error("Error fetching data in AddEditStudent:", error);
      }
    };

    loadData();
  }, [id]);

  // Handle form field changes
  const handleChange = e => {
    const { name, value } = e.target;
    setStudent(prev => ({ ...prev, [name]: value }));
  };

  // Handle submission: Add or update student
  const handleSubmit = async e => {
    e.preventDefault();
    const payload = { ...student };
    try {
      if (id) {
        await axiosInstance.put(`${BASE_URL}/api/students/${id}`, payload);
      } else {
        await axiosInstance.post(`${BASE_URL}/api/students`, payload);
      }
      navigate('/students');
    } catch (error) {
      console.error("Error saving student:", error);
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
            value={student.userId || ''}
            onChange={handleChange}
          >
            <option value="">Select a user</option>
            {users.map(user => (
              <option key={user.identityUserId} value={user.identityUserId}>
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
