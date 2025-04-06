import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Table, Button } from 'react-bootstrap';

const Enrollments = () => {
    const [enrollments, setEnrollments] = useState([]);

    useEffect(() => {
        const fetchEnrollments = async () => {
            try {
                const link = `${BASE_URL}/enrollments`
                const response = await axios.get(link);
                setEnrollments(response.data);
            } catch (error) {
                console.error('There was an error fetching the enrollments!', error);
            }
        };
    
        fetchEnrollments();
    }, []);

    const deleteEnrollment = id => {
        axios.delete(`${BASE_URL}/enrollments/${id}`)
            .then(() => {
                // Re-fetch the enrollments after successful deletion
                axios.get(`${BASE_URL}/enrollments`)
                    .then(response => setEnrollments(response.data))
                    .catch(error => console.error('Error fetching enrollments:', error));
            })
            .catch(error => console.error('Error deleting enrollment:', error));
    };

    return (
        <div>
            <h1>Enrollments</h1>
            <Button as={Link} to="/enrollments/add" variant="primary">Add Enrollment</Button>
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Class</th>
                        <th>Student</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {enrollments.map(enrollment => (
                        <tr key={enrollment.enrollmentRef}>
                            <td>{enrollment.enrolledClass}</td>
                            <td>{enrollment.enrolledStudent}</td>
                            <td>
                                <Button as={Link} to={`/enrollments/edit/${enrollment.enrollmentRef}`} variant="warning">Edit</Button>
                                <Button onClick={() => deleteEnrollment(enrollment.enrollmentRef)} variant="danger">Delete</Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </div>
    );
};

export default Enrollments;
