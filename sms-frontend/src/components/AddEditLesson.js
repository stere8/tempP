import React, { useState, useEffect } from 'react';
import axiosInstance from './axiosInstance';
import { useNavigate, useParams } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Form, Button, Container } from 'react-bootstrap';

const AddEditLesson = () => {
    const [lesson, setLesson] = useState({ lessonId: 0, name: '', description: '', subject: '', gradeLevel: '' });
    const navigate = useNavigate();
    const { id } = useParams();

    useEffect(() => {
        if (id) {
            axiosInstance.get(`${BASE_URL}/api/lessons/${id}`)
                .then(response => setLesson(response.data))
                .catch(error => console.error('Error fetching lesson data:', error));
        }
    }, [id]);

    const handleChange = e => {
        const { name, value } = e.target;
        setLesson(prevState => ({ ...prevState, [name]: value }));
    };

    const handleSubmit = e => {
        e.preventDefault();
        if (id) {
            axiosInstance.put(`${BASE_URL}/api/lessons/${id}`, lesson)
                .then(() => navigate('/lessons'))
                .catch(error => console.error('Error updating lesson:', error));
        } else {
            axiosInstance.post(`${BASE_URL}/api/lessons`, lesson)
                .then(() => navigate('/lessons'))
                .catch(error => console.error('Error adding lesson:', error));
        }
    };

    return (
        <Container>
            <h1>{id ? 'Edit Lesson' : 'Add Lesson'}</h1>
            <Form onSubmit={handleSubmit}>
                <Form.Group controlId="name">
                    <Form.Label>Name</Form.Label>
                    <Form.Control
                        type="text"
                        name="name"
                        value={lesson.name}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Form.Group controlId="description">
                    <Form.Label>Description</Form.Label>
                    <Form.Control
                        type="text"
                        name="description"
                        value={lesson.description}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Form.Group controlId="subject">
                    <Form.Label>Subject</Form.Label>
                    <Form.Control
                        type="text"
                        name="subject"
                        value={lesson.subject}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Form.Group controlId="gradeLevel">
    <Form.Label>Grade Level</Form.Label>
    <Form.Control
        type="number"
        name="gradeLevel"
        value={lesson.gradeLevel}
        onChange={handleChange}
        required
        min={0}
        max={12}
    />
</Form.Group>
                <Button variant="primary" type="submit">
                    {id ? 'Update' : 'Add'}
                </Button>
            </Form>
        </Container>
    );
};

export default AddEditLesson;
