import React, { useEffect, useState } from 'react';
import axiosInstance from './axiosInstance';
import { BASE_URL } from '../settings';
import { Form, Button, Container, Alert } from 'react-bootstrap';

const AddEditMark = () => {
  const [students, setStudents] = useState([]);
  const [selectedStudentId, setSelectedStudentId] = useState('');
  const [markValue, setMarkValue] = useState('');
  const [subject, setSubject] = useState('');
  const [date, setDate] = useState('');
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchStudents = async () => {
      try {
        const response = await axiosInstance.get(`${BASE_URL}/api/students`);
        // Log the raw response data to inspect its structure.
        console.log("Raw students data:", response.data);

        // Ensure that the resulting value is an array.
        // If response.data is not an array, try to convert or wrap it in an array.
        let studentsData = response.data.$values;
        if (!Array.isArray(studentsData)) {
          // If it's an object (or something else), check if it has a property (e.g., "data") that is an array.
          if (studentsData && Array.isArray(studentsData.data)) {
            studentsData = studentsData.data;
            console.log("Normalized students data:", studentsData);
          } else {
            // Otherwise, wrap it in an array, though this may not be the intended behavior.
            studentsData = [studentsData];
            console.log("Wrapped students data:", studentsData);
          }
        }
        setStudents(studentsData);
      } catch (err) {
        console.error('Error fetching students:', err);
        setError("Failed to load students");
      }
    };

    fetchStudents();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    // Build your payload according to your backend requirements.
    const payload = {
      studentId: selectedStudentId,
      subject: subject,
      markValue: markValue,
      date: date
    };

    try {
      await axiosInstance.post(`${BASE_URL}/marks/add`, payload);
      // Optionally clear the form or redirect
    } catch (err) {
      console.error("Error saving mark:", err);
      setError("Failed to save mark");
    }
  };

  return (
    <Container>
      <h1>Add Mark</h1>
      {error && <Alert variant="danger">{error}</Alert>}
      <Form onSubmit={handleSubmit}>
        <Form.Group controlId="student">
          <Form.Label>Select Student</Form.Label>
          <Form.Control
            as="select"
            value={selectedStudentId}
            onChange={(e) => setSelectedStudentId(e.target.value)}
            required
          >
            <option value="">Select a student</option>
            {students.map(student => (
              <option key={student.studentId || student.id} value={student.studentId || student.id}>
                {student.firstName} {student.lastName}
              </option>
            ))}
          </Form.Control>
        </Form.Group>
        <Form.Group controlId="subject">
          <Form.Label>Subject</Form.Label>
          <Form.Control
            type="text"
            value={subject}
            onChange={(e) => setSubject(e.target.value)}
            required
          />
        </Form.Group>
        <Form.Group controlId="markValue">
          <Form.Label>Mark Value</Form.Label>
          <Form.Control
            type="number"
            step="any"
            value={markValue}
            onChange={(e) => setMarkValue(e.target.value)}
            required
          />
        </Form.Group>
        <Form.Group controlId="date">
          <Form.Label>Date</Form.Label>
          <Form.Control
            type="date"
            value={date}
            onChange={(e) => setDate(e.target.value)}
            required
          />
        </Form.Group>
        <Button variant="primary" type="submit" className="mt-3">
          Submit
        </Button>
      </Form>
    </Container>
  );
};

export default AddEditMark;
