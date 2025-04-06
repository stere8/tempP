import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Table, Button } from 'react-bootstrap';

const Lessons = () => {
    const [lessons, setLessons] = useState([]);

    useEffect(() => {
        axios.get(`${BASE_URL}/lessons`)
            .then(response => setLessons(response.data))
            .catch(error => console.error('Error fetching lessons:', error));
    }, []);

    const deleteLesson = id => {
        axios.delete(`${BASE_URL}/lessons/${id}`)
            .then(() => setLessons(lessons.filter(lesson => lesson.lessonId !== id)))
            .catch(error => console.error('Error deleting lesson:', error));
    };

    return (
        <div>
            <h1>Lessons</h1>
            <Button as={Link} to="/lessons/add" variant="primary">Add Lesson</Button>
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Subject</th>
                        <th>Grade Level</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {lessons.map(lesson => (
                        <tr key={lesson.lessonId}>
                            <td>{lesson.name}</td>
                            <td>{lesson.description}</td>
                            <td>{lesson.subject}</td>
                            <td>{lesson.gradeLevel}</td>
                            <td>
                                <Button as={Link} to={`/lessons/edit/${lesson.lessonId}`} variant="warning">Edit</Button>
                                <Button onClick={() => deleteLesson(lesson.lessonId)} variant="danger">Delete</Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </div>
    );
};

export default Lessons;