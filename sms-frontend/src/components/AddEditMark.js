import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useParams } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Form, Button, Container } from 'react-bootstrap';

const AddEditMark = () => {
    const [mark, setMark] = useState({ studentId: '', lessonId: '', markValue: '', date: '' });
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
            axios.get(`${BASE_URL}/marks/${id}`)
                .then(response => {
                    const markData = response.data;
                    markData.date = markData.date.split('T')[0]; // Format date as yyyy-mm-dd
                    setMark(markData);
                })
                .catch(error => console.error('Error fetching mark data:', error));
        }
    }, [id]);

    const handleChange = e => {
        const { name, value } = e.target;
        setMark(prevState => ({ ...prevState, [name]: value }));
    };

    const handleSubmit = e => {
        e.preventDefault();
        if (id) {
            axios.put(`${BASE_URL}/marks/${id}`, mark)
                .then(() => navigate('/marks'))
                .catch(error => console.error('Error updating mark:', error));
        } else {
            axios.post(`${BASE_URL}/marks`, mark)
                .then(() => navigate('/marks'))
                .catch(error => console.error('Error adding mark:', error));
        }
    };

    return (
        <Container>
            <h1>{id ? 'Edit Mark' : 'Add Mark'}</h1>
            <Form onSubmit={handleSubmit}>
                <Form.Group controlId="studentSelect">
                    <Form.Label>Student</Form.Label>
                    <Form.Control as="select" name="studentId" value={mark.studentId} onChange={handleChange} required>
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
                    <Form.Control as="select" name="lessonId" value={mark.lessonId} onChange={handleChange} required>
                        <option value="">Select a lesson</option>
                        {lessons.map(lesson => (
                            <option key={lesson.lessonId} value={lesson.lessonId}>
                                {lesson.name}
                            </option>
                        ))}
                    </Form.Control>
                </Form.Group>
                <Form.Group controlId="markValue">
                    <Form.Label>Mark</Form.Label>
                    <Form.Control
                        type="number"
                        name="markValue"
                        value={mark.markValue}
                        onChange={handleChange}
                        required
                        step="0.01"
                    />
                </Form.Group>
                <Form.Group controlId="date">
                    <Form.Label>Date</Form.Label>
                    <Form.Control
                        type="date"
                        name="date"
                        value={mark.date}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Button variant="primary" type="submit">
                    {id ? 'Update' : 'Add'}
                </Button>
            </Form>
        </Container>
    );
};

export default AddEditMark;
