import React, { useContext, useEffect, useState } from 'react';
import axios from 'axios';
import { Link, useNavigate } from 'react-router-dom';
import { Table, Button, Alert, Spinner, Card, Container } from 'react-bootstrap';
import { BASE_URL } from '../settings';
import axiosInstance from './axiosInstance';
import { AuthContext } from './AuthContext';

const Attendance = () => {
  const [attendance, setAttendance] = useState([]);
  const [students, setStudents] = useState([]);
  const [lessons, setLessons] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();
  const { userRole } = useContext(AuthContext);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [attendanceRes, studentsRes, lessonsRes] = await Promise.all([
          axiosInstance.get(`${BASE_URL}/api/attendance`),
          axiosInstance.get(`${BASE_URL}/api/students`),
          axiosInstance.get(`${BASE_URL}/api/lessons`)
        ]);

        setAttendance(attendanceRes.data || []);
        setStudents(studentsRes.data || []);
        setLessons(lessonsRes.data || []);
      } catch (err) {
        setError('Failed to load data');
        console.error('Error:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  const deleteAttendance = async (id) => {
    try {
      await axiosInstance.delete(`${BASE_URL}/api/attendance/${id}`);
      setAttendance(attendance.filter(record => record.attendanceId !== id));
    } catch (error) {
      setError('Failed to delete record');
      console.error('Error:', error);
    }
  };

  const getStudentNameById = (id) => {
    const student = students.find(s => s.studentId === id);
    return student ? `${student.firstName} ${student.lastName}` : 'Unknown';
  };

  const getLessonNameById = (id) => {
    const lesson = lessons.find(l => l.lessonId === id);
    return lesson ? lesson.name : 'Unknown';
  };

  const formatDate = (dateString) => {
    return dateString ? new Date(dateString).toLocaleDateString() : '';
  };

  if (loading) return <Spinner animation="border" />;
  if (error) return <Alert variant="danger">{error}</Alert>;

  return (
    <Container className="p-4">
      <h1 className="mb-4">Attendance Records</h1>
      {/* Render the Add New Record button only if the user is not a Teacher */}
      {userRole === 'Teacher' && (
        <Button as={Link} to="/attendance/add" variant="primary" className="mb-4">
          Add New Record
        </Button>
      )}
      
      {attendance.length > 0 ? (
        <Table striped bordered hover responsive>
          <thead className="thead-dark">
            <tr>
              <th>Student</th>
              <th>Lesson</th>
              <th>Date</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {attendance.map(record => (
              <tr key={record.attendanceId}>
                <td>{getStudentNameById(record.studentId)}</td>
                <td>{getLessonNameById(record.lessonId)}</td>
                <td>{formatDate(record.date)}</td>
                <td>{record.status}</td>
                <td>
                  <Button 
                    variant="warning" 
                    size="sm" 
                    onClick={() => navigate(`/attendance/edit/${record.attendanceId}`)}
                    className="me-2"
                  >
                    Edit
                  </Button>
                  <Button 
                    variant="danger" 
                    size="sm"
                    onClick={() => deleteAttendance(record.attendanceId)}
                  >
                    Delete
                  </Button>
                </td>
              </tr>
            ))}
          </tbody>
        </Table>
      ) : (
        <Card>
          <Card.Body>
            <Card.Text>No attendance records found.</Card.Text>
          </Card.Body>
        </Card>
      )}
    </Container>
  );
};

export default Attendance;
