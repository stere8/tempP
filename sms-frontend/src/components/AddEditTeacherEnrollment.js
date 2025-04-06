import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useParams } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Form, Button } from 'react-bootstrap';
import axiosInstance from './axiosInstance' 

const AddEditTeacherEnrollment = () => {
    const [teachers, setTeachers] = useState([]);
    const [classes, setClasses] = useState([]);
    const [lessons, setLessons] = useState([]);
    const [teacherId, setTeacherId] = useState('');
    const [classId, setClassId] = useState('');
    const [lessonId, setLessonId] = useState('');
    const { id } = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        axiosInstance.get(`${BASE_URL}/staff`)
            .then(response => setTeachers(response.data))
            .catch(error => console.error('Error fetching teachers:', error));

            axiosInstance.get(`${BASE_URL}/classes`)
            .then(response => setClasses(response.data))
            .catch(error => console.error('Error fetching classes:', error));

            axiosInstance.get(`${BASE_URL}/lessons`)
            .then(response => setLessons(response.data))
            .catch(error => console.error('Error fetching lessons:', error));

        if (id && id !== '0') {
            axiosInstance.get(`${BASE_URL}/teacherenrollments/${id}`)
                .then(response => {
                    setTeacherId(response.data.staffId);
                    setClassId(response.data.classId);
                    setLessonId(response.data.lessonId);
                })
                .catch(error => console.error('Error fetching enrollment:', error));
        }
    }, [id]);

    const handleSubmit = event => {
        event.preventDefault();
        const enrollment = { staffId: teacherId, classId: classId, lessonId: lessonId,TeacherEnrollmentId : id };

        if (id && id !== '0') {
            axiosInstance.put(`${BASE_URL}/teacherenrollments/${id}`, enrollment)
                .then(() => navigate('/teacher-enrollments'))
                .catch(error => console.error('Error updating enrollment:', error));
        } else {
            axiosInstance.post(`${BASE_URL}/teacherenrollments`, enrollment)
                .then(() => navigate('/teacher-enrollments'))
                .catch(error => console.error('Error creating enrollment:', error));
        }
    };

    return (
        <div>
            <h1>{id && id !== '0' ? 'Edit Teacher Enrollment' : 'Add Teacher Enrollment'}</h1>
            <Form onSubmit={handleSubmit}>
                <Form.Group controlId="teacherSelect">
                    <Form.Label>Teacher</Form.Label>
                    <Form.Control as="select" value={teacherId} onChange={e => setTeacherId(e.target.value)} required>
                        <option value="">Select a teacher</option>
                        {teachers.map(teacher => (
                            <option key={teacher.staffId} value={teacher.staffId}>
                                {teacher.firstName} {teacher.lastName}
                            </option>
                        ))}
                    </Form.Control>
                </Form.Group>
                <Form.Group controlId="classSelect">
                    <Form.Label>Class</Form.Label>
                    <Form.Control as="select" value={classId} onChange={e => setClassId(e.target.value)} required>
                        <option value="">Select a class</option>
                        {classes.map(classItem => (
                            <option key={classItem.viewedClass.classId} value={classItem.viewedClass.classId}>
                                {classItem.viewedClass.name}
                            </option>
                        ))}
                    </Form.Control>
                </Form.Group>
                <Form.Group controlId="lessonSelect">
                    <Form.Label>Lesson</Form.Label>
                    <Form.Control 
                    as="select" 
                    value={lessonId} onChange={e => setLessonId(e.target.value)} required>
                        <option value="">Select a lesson</option>
                        {lessons.map(lesson => (
                            <option key={lesson.lessonId} value={lesson.lessonId}>
                                {lesson.name}
                            </option>
                        ))}
                    </Form.Control>
                </Form.Group>
                <Button variant="primary" type="submit">
                    {id && id !== '0' ? 'Update Enrollment' : 'Add Enrollment'}
                </Button>
            </Form>
        </div>
    );
};

export default AddEditTeacherEnrollment;
