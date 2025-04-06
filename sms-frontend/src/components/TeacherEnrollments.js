import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Table, Button } from 'react-bootstrap';

const TeacherEnrollments = () => {
    const [teacherEnrollments, setTeacherEnrollments] = useState([]);

    const fetchTeacherEnrollments = () => {
        axios.get(`${BASE_URL}/TeacherEnrollments`)
            .then(response => {
                console.log(response); // Logging the data
                setTeacherEnrollments(response.data);
            })
            .catch(error => console.error('Error fetching teacher enrollments:', error));
    };

    useEffect(() => {
        fetchTeacherEnrollments();
    }, []);

    const deleteTeacherEnrollment = id => {
        axios.delete(`${BASE_URL}/TeacherEnrollments/${id}`)
            .then(() => fetchTeacherEnrollments()) // Refetch after delete
            .catch(error => console.error('Error deleting teacher enrollment:', error));
    };

    return (
        <div>
            <h1>Teacher Enrollments</h1>
            <Button as={Link} to="/teacher-enrollments/add" variant="primary">Add Teacher Enrollment</Button>
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Class</th>
                        <th>Teacher</th>
                        <th>Lesson</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {teacherEnrollments.map(enrollment => (
                        <tr key={enrollment.enrollmentRef}>
                            <td>{enrollment.assignedClass || 'N/A'}</td>
                            <td>{enrollment.enrolledTeacher || 'N/A'}</td>
                            <td>{enrollment.assignedLesson || 'N/A'}</td>
                            <td>
                                {enrollment.enrollmentRef !== 0 ? (
                                    <>
                                        <Button as={Link} to={`/teacher-enrollments/edit/${enrollment.enrollmentRef}`} variant="warning">Edit</Button>
                                        <Button onClick={() => deleteTeacherEnrollment(enrollment.enrollmentRef)} variant="danger">Delete</Button>
                                    </>
                                ) : (
                                    'N/A'
                                )}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </div>
    );
};

export default TeacherEnrollments;
