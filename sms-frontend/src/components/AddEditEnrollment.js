import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useParams } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Form, Button } from 'react-bootstrap';

const AddEditEnrollment = () => {
    const [classes, setClasses] = useState([]);
    const [students, setStudents] = useState([]);
    const [classId, setClassId] = useState('');
    const [studentId, setStudentId] = useState('');
    const navigate = useNavigate();  // Correct usage of useNavigate
    const { id } = useParams();

    useEffect(() => {
        axios.get(`${BASE_URL}/classes`)
            .then(response => setClasses(response.data))
            .catch(error => console.error('Error fetching classes:', error));

        axios.get(`${BASE_URL}/students`)
            .then(response => setStudents(response.data))
            .catch(error => console.error('Error fetching students:', error));

        if (id) {
            axios.get(`${BASE_URL}/enrollments/${id}`)
                .then(response => {
                    setClassId(response.data.classId);
                    setStudentId(response.data.studentId);
                })
                .catch(error => console.error('Error fetching enrollment:', error));
        }
    }, [id]);

    const handleSubmit = event => {
        event.preventDefault();

        const enrollmentData = { classId, studentId, enrollmentId: id };

        if (id) {
            axios.put(`${BASE_URL}/enrollments/${id}`, enrollmentData)
                .then(() => navigate('/enrollments'))  // Correct usage of navigate
                .catch(error => console.error('Error updating enrollment:', error));
        } else {
            axios.post(`${BASE_URL}/enrollments`, enrollmentData)
                .then(() => navigate('/enrollments'))  // Correct usage of navigate
                .catch(error => console.error('Error adding enrollment:', error));
        }
    };

    return (
        <div>
            <h1>{id ? 'Edit Enrollment' : 'Add Enrollment'}</h1>
            <Form onSubmit={handleSubmit}>
                <Form.Group>
                    <Form.Label>Class</Form.Label>
                    <Form.Control as="select" value={classId} onChange={e => setClassId(e.target.value)}>
                        <option value="">Select Class</option>
                        {classes.map(cls => (
                            <option key={cls.viewedClass.classId} value={cls.viewedClass.classId}>{cls.viewedClass.name}</option>
                        ))}
                    </Form.Control>
                </Form.Group>
                <Form.Group>
                    <Form.Label>Student</Form.Label>
                    <Form.Control as="select" value={studentId} onChange={e => setStudentId(e.target.value)}>
                        <option value="">Select Student</option>
                        {students.map(student => (
                            <option key={student.studentId} value={student.studentId}>{student.firstName} {student.lastName}</option>
                        ))}
                    </Form.Control>
                </Form.Group>
                <Button variant="primary" type="submit">
                    {id ? 'Update' : 'Add'} Enrollment
                </Button>
            </Form>
        </div>
    );
};

export default AddEditEnrollment;
