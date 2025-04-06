import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useParams } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Form, Button, Container } from 'react-bootstrap';

const AddEditAttendance = () => {
    const [attendance, setAttendance] = useState({ studentId: '', lessonId: '', date: '', status: 'Present' });
    const [students, setStudents] = useState([]);
    const [lessons, setLessons] = useState([]);
    const navigate = useNavigate();
    const { id } = useParams();

    useEffect(() => {
        axios.get(`${BASE_URL}/students`)
            .then(response => setStudents(response.data))
            .catch(error => console.error('Error fetching students:', error));

        axios.get(`${BASE_URL}/lessons`)
            .then(response => setLessons(response.data))
            .catch(error => console.error('Error fetching lessons:', error));

        if (id) {
            axios.get(`${BASE_URL}/attendance/${id}`)
                .then(response => {
                    const attendanceData = response.data;
                    // Format the date to yyyy-mm-dd for the input field
                    attendanceData.date = new Date(attendanceData.date).toISOString().split('T')[0];
                    setAttendance(attendanceData);
                })
                .catch(error => console.error('Error fetching attendance data:', error));
        }
    }, [id]);

    const handleChange = e => {
        const { name, value } = e.target;
        setAttendance(prevState => ({ ...prevState, [name]: value }));
    };

    const handleSubmit = e => {
        e.preventDefault();
        if (id) {
            axios.put(`${BASE_URL}/attendance/${id}`, attendance)
                .then(() => navigate('/attendance'))
                .catch(error => console.error('Error updating attendance:', error));
        } else {
            axios.post(`${BASE_URL}/attendance`, attendance)
                .then(() => navigate('/attendance'))
                .catch(error => console.error('Error adding attendance:', error));
        }
    };

    return (
        <Container>
            <h1>{id ? 'Edit Attendance' : 'Add Attendance'}</h1>
            <Form onSubmit={handleSubmit}>
                <Form.Group controlId="studentSelect">
                    <Form.Label>Student</Form.Label>
                    <Form.Control as="select" name="studentId" value={attendance.studentId} onChange={handleChange} required>
                        <option value="">Select a student</option>
                        {students.map(student => (
                            <option key={student.studentId} value={student.studentId}>
                                {student.firstName} {student.lastName}
                            </option>
                        ))}
                    </Form.Control>
                </Form.Group>
                <Form.Group controlId="lessonSelect">
                    <Form.Label>Lesson</Form.Label>
                    <Form.Control as="select" name="lessonId" value={attendance.lessonId} onChange={handleChange} required>
                        <option value="">Select a lesson</option>
                        {lessons.map(lesson => (
                            <option key={lesson.lessonId} value={lesson.lessonId}>
                                {lesson.name}
                            </option>
                        ))}
                    </Form.Control>
                </Form.Group>
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
                <Form.Group controlId="statusSelect">
                    <Form.Label>Status</Form.Label>
                    <Form.Control as="select" name="status" value={attendance.status} onChange={handleChange} required>
                        <option value="Present">Present</option>
                        <option value="Absent">Absent</option>
                    </Form.Control>
                </Form.Group>
                <Button variant="primary" type="submit">
                    {id ? 'Update' : 'Add'}
                </Button>
            </Form>
        </Container>
    );
};

export default AddEditAttendance;
