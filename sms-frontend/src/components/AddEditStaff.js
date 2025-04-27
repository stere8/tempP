// src/components/AddEditStaff.js

import React, { useState, useEffect } from 'react';
import axiosInstance from './axiosInstance';
import { useNavigate, useParams } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Form, Button, Container } from 'react-bootstrap';

const AddEditStaff = () => {
  // Define the staff state with default values
  const [staff, setStaff] = useState({ 
    firstName: '', 
    lastName: '', 
    email: '', 
    subjectExpertise: '', 
    userId: ''
  });
  // State for holding the list of users to choose from
  const [users, setUsers] = useState([]);
  const navigate = useNavigate();
  const { id } = useParams();

  useEffect(() => {
    const loadData = async () => {
      try {
        let linkedUser = null;
        let fetchedTeacher = null;
  
        // 1) If editing, fetch the teacher data first.
        if (id) {
          const { data: teacherData } = await axiosInstance.get(`${BASE_URL}/api/staff/${id}`);
          fetchedTeacher = teacherData;
          setStaff(teacherData);
          console.log("Fetched teacher:", teacherData);
  
          // 2) If the teacher has a linked userId, fetch that user's info.
          if (teacherData.userId) {
            const { data: userInfo } = await axiosInstance.get(`${BASE_URL}/api/account/userinfo/${teacherData.userId}`);
            linkedUser = {
              // Normalize the ID to lowercase for consistency
              identityUserId: ((userInfo.identityUserId ?? userInfo.id) || "").toLowerCase(),
              email: userInfo.email ?? userInfo.userName
            };
            console.log("Fetched account info (linkedUser):", linkedUser);
          }
        }
  
        // 3) Fetch the unlinked teacher users.
        const { data: raw } = await axiosInstance.get(`${BASE_URL}/api/account/teacher/unlinked`);
        // Some APIs return an array directly; others return an object with a "$values" property.
        const arrayData = Array.isArray(raw) ? raw : (raw.$values || []);
        console.log("Fetched unlinked teacher users (raw):", arrayData);
  
        // 4) Normalize the list so that every object has { identityUserId, email }.
        const unlinkedNormalized = arrayData.map(u => ({
          identityUserId: ((u.identityUserId ?? u.id) || "").toLowerCase(),
          email: u.email
        }));
  
        // 5) If we are editing and have a linked user and it’s not already in the list, add it.
        if (linkedUser && !unlinkedNormalized.some(u => u.identityUserId === linkedUser.identityUserId)) {
          unlinkedNormalized.push(linkedUser);
        }
  
        console.log("Final users for dropdown:", unlinkedNormalized);
  
        // 6) Update state with the normalized user list.
        setUsers(unlinkedNormalized);
      } catch (error) {
        console.error("Error fetching data in AddEditStaff:", error);
      }
    };
  
    loadData();
  }, [id]);
  
  // Update local staff state as form fields change
  const handleChange = e => {
    const { name, value } = e.target;
    setStaff(prevState => ({ ...prevState, [name]: value }));
  };
  
  // Submit the form to either update or add staff
  const handleSubmit = async e => {
    e.preventDefault();
    const payload = { ...staff, userId: staff.userId };
    try {
      if (id) {
        await axiosInstance.put(`${BASE_URL}/api/staff/${id}`, payload);
      } else {
        await axiosInstance.post(`${BASE_URL}/api/staff`, payload);
      }
      navigate('/staff');
    } catch (error) {
      console.error("Error saving staff:", error);
    }
  };
  
  return (
    <Container>
      <h1>{id ? 'Edit Staff' : 'Add Staff'}</h1>
      <Form onSubmit={handleSubmit}>
        <Form.Group controlId="firstName" className="mb-3">
          <Form.Label>First Name</Form.Label>
          <Form.Control
            type="text"
            name="firstName"
            value={staff.firstName}
            onChange={handleChange}
            required
          />
        </Form.Group>
  
        <Form.Group controlId="lastName" className="mb-3">
          <Form.Label>Last Name</Form.Label>
          <Form.Control
            type="text"
            name="lastName"
            value={staff.lastName}
            onChange={handleChange}
            required
          />
        </Form.Group>
  
        <Form.Group controlId="email" className="mb-3">
          <Form.Label>Email</Form.Label>
          <Form.Control
            type="email"
            name="email"
            value={staff.email}
            onChange={handleChange}
            required
          />
        </Form.Group>
  
        <Form.Group controlId="subjectExpertise" className="mb-3">
          <Form.Label>Subject Expertise</Form.Label>
          <Form.Control
            type="text"
            name="subjectExpertise"
            value={staff.subjectExpertise}
            onChange={handleChange}
            required
          />
        </Form.Group>
  
        <Form.Group controlId="userId" className="mb-3">
          <Form.Label>Link User Account</Form.Label>
          <Form.Control
            as="select"
            name="userId"
            value={staff.userId || ''}
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

export default AddEditStaff;
