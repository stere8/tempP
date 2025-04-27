import React, { useState, useEffect } from 'react';
import axiosInstance from './axiosInstance';
import { useNavigate, useParams } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Form, Button, Container, Alert, Spinner, Row, Col } from 'react-bootstrap';

const AddEditAttendance = () => {
  const [attendance, setAttendance] = useState({ 
    studentId: '', 
    lessonId: '', 
    date: new Date().toISOString().split('T')[0], 
    status: 'Present' 
  });
  const [students, setStudents] = useState([]);
  const [lessons, setLessons] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [noStudents, setNoStudents] = useState(false);
  const [noLessons, setNoLessons] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams();

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        setError(null);
        setNoStudents(false);
        setNoLessons(false);

        // Determine if a teacher is logged in by checking localStorage for teacherId.
        // If teacherId is "0", then treat it as not a teacher.
        const teacherId = localStorage.getItem('teacherId');
        let studentsUrl = `${BASE_URL}/api/students`;
        if (teacherId && teacherId !== "0") {
          studentsUrl = `${BASE_URL}/api/students/teacher/${teacherId}`;
        }
        
        const [studentsRes, lessonsRes] = await Promise.all([
          axiosInstance.get(studentsUrl),
          console.log('id id id id '),
          console.log(teacherId),
          axiosInstance.get(`${BASE_URL}/api/lessons`)
        ]);
        
        console.log("Students response:", studentsRes.data);
        // Ensure studentsData is an array.
        const studentsData = Array.isArray(studentsRes.data)
          ? studentsRes.data
          : studentsRes.data.students || [];
        
        if (studentsData.length === 0) {
          setNoStudents(true);
        } else {
          setStudents(studentsData);
        }
        
        // Ensure lessonsData is an array.
        const lessonsData = Array.isArray(lessonsRes.data)
          ? lessonsRes.data
          : lessonsRes.data.lessons || [];
        if (lessonsData.length === 0) {
          setNoLessons(true);
        } else {
          setLessons(lessonsData);
        }
        
        if (id) {
          const attendanceRes = await axiosInstance.get(`${BASE_URL}/api/attendance/${id}`);
          const attendanceData = attendanceRes.data;
          // Format the date for the input field (yyyy-mm-dd)
          attendanceData.date = new Date(attendanceData.date).toISOString().split('T')[0];
          setAttendance(attendanceData);
        }
      } catch (err) {
        setError('Failed to load data');
        console.error('Error:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  const handleChange = e => {
    const { name, value } = e.target;
    setAttendance(prevState => ({ ...prevState, [name]: value }));
  };

  const handleSubmit = async e => {
    e.preventDefault();
    if (noStudents || noLessons) {
      setError('Cannot submit with missing data');
      return;
    }
    
    try {
      if (id) {
        await axiosInstance.put(`${BASE_URL}/api/attendance/${id}`, attendance);
      } else {
        await axiosInstance.post(`${BASE_URL}/api/attendance`, attendance);
      }
      navigate('/attendance');
    } catch (error) {
      console.error('Error submitting attendance:', error);
      setError('Error submitting attendance record');
    }
  };

  if (loading) return (
    <Container className="text-center mt-5">
      <Spinner animation="border" role="status">
        <span className="visually-hidden">Loading...</span>
      </Spinner>
      <p>Loading attendance form...</p>
    </Container>
  );

  if (error) return (
    <Container>
      <Alert variant="danger">{error}</Alert>
      <Button variant="secondary" onClick={() => navigate('/attendance')}>
        Back to Attendance
      </Button>
    </Container>
  );

  return (
    <Container>
      <h1 className="mb-4">{id ? 'Edit Attendance' : 'Add Attendance'}</h1>
      
      {noStudents && (
        <Alert variant="danger" className="mb-4">
          <strong>No students found!</strong> Please ensure:
          <ul className="mt-2">
            <li>Students are properly enrolled</li>
            {localStorage.getItem('teacherId') && localStorage.getItem('teacherId') !== "0" && (
              <li>You have classes assigned to you</li>
            )}
          </ul>
        </Alert>
      )}

      {noLessons && (
        <Alert variant="danger" className="mb-4">
          <strong>No lessons found!</strong> Please ensure lessons are properly set up in the system.
        </Alert>
      )}

      <Form onSubmit={handleSubmit}>
        <Form.Group controlId="studentSelect" className="mb-3">
          <Form.Label>Student</Form.Label>
          {noStudents ? (
            <Alert variant="warning">
              Cannot select student - none available
            </Alert>
          ) : (
            <Form.Control 
              as="select" 
              name="studentId" 
              value={attendance.studentId} 
              onChange={handleChange} 
              required
              disabled={noStudents}
            >
              <option value="">Select a student</option>
              {students.map(student => (
                <option key={student.studentId} value={student.studentId}>
                  {student.firstName} {student.lastName}
                </option>
              ))}
            </Form.Control>
          )}
        </Form.Group>

        <Form.Group controlId="lessonSelect" className="mb-3">
          <Form.Label>Lesson</Form.Label>
          {noLessons ? (
            <Alert variant="warning">
              Cannot select lesson - none available
            </Alert>
          ) : (
            <Form.Control 
              as="select" 
              name="lessonId" 
              value={attendance.lessonId} 
              onChange={handleChange} 
              required
              disabled={noLessons}
            >
              <option value="">Select a lesson</option>
              {lessons.map(lesson => (
                <option key={lesson.lessonId} value={lesson.lessonId}>
                  {lesson.name}
                </option>
              ))}
            </Form.Control>
          )}
        </Form.Group>

        <Row className="mb-3">
          <Col md={6}>
            <Form.Group controlId="date">
              <Form.Label>Date</Form.Label>
              <Form.Control
                type="date"
                name="date"
                value={attendance.date}
                onChange={handleChange}
                required
              />
            </Form.Group>
          </Col>
          <Col md={6}>
            <Form.Group controlId="statusSelect">
              <Form.Label>Status</Form.Label>
              <Form.Control 
                as="select" 
                name="status" 
                value={attendance.status} 
                onChange={handleChange} 
                required
              >
                <option value="Present">Present</option>
                <option value="Absent">Absent</option>
              </Form.Control>
            </Form.Group>
          </Col>
        </Row>

        <div className="d-flex justify-content-between mt-4">
          <Button variant="secondary" onClick={() => navigate('/attendance')}>
            Cancel
          </Button>
          <Button variant="primary" type="submit" disabled={noStudents || noLessons}>
            {id ? 'Update' : 'Add'} Attendance
          </Button>
        </div>
      </Form>
    </Container>
  );
};

export default AddEditAttendance;
