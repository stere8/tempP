import React, { useEffect, useState, useContext } from 'react';
import axiosInstance from './axiosInstance';
import { Container, Table, Alert } from 'react-bootstrap';
import { BASE_URL } from '../settings';
import { AuthContext } from './AuthContext';

const MyStudents = () => {
  const [students, setStudents] = useState([]);
  const [error, setError] = useState("");
  const { parentId } = useContext(AuthContext); // Assuming AuthContext stores parentId for Parent users

  useEffect(() => {
    const fetchStudents = async () => {
      try {
        const response = await axiosInstance.get(`${BASE_URL}/api/ParentStudent/parent/${parentId}`);
        setStudents(response.data);
      } catch (err) {
        console.error("Error fetching students for parent:", err);
        setError("Failed to load your students.");
      }
    };

    if (parentId) {
      fetchStudents();
    }
  }, [parentId]);

  if (error) {
    return <Alert variant="danger">{error}</Alert>;
  }

  if (!students.length) {
    return (
      <Container className="mt-4">
        <h3>No students are currently linked to your account.</h3>
      </Container>
    );
  }

  return (
    <Container className="mt-4">
      <h1>My Students</h1>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Student ID</th>
            <th>First Name</th>
            <th>Last Name</th>
          </tr>
        </thead>
        <tbody>
          {students.map((assignment) => (
            <tr key={assignment.studentId}>
              <td>{assignment.studentId}</td>
              <td>{assignment.student.firstName}</td>
              <td>{assignment.student.lastName}</td>
            </tr>
          ))}
        </tbody>
      </Table>
    </Container>
  );
};

export default MyStudents;
