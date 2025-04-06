import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Table, Button } from 'react-bootstrap';

const Students = () => {
    const [students, setStudents] = useState([]);

    useEffect(() => {
        const fetchStudents = async () => {
            try {
                const url = `${BASE_URL}/students`
                const response = await axios.get(url);
                console.log(response.data);
                setStudents(response.data);
            } catch (error) {
                console.error('There was an error fetching the students!', BASE_URL, error);
            }
        };

        fetchStudents();
    }, []);

    const deleteStudent = id => {
        axios.delete(`${BASE_URL}/students/${id}`)
            .then(() => setStudents(students.filter(student => student.studentId !== id)))
            .catch(error => console.error('Error deleting student:', error));
    };

    const formatDate = date => {
        return date ? date.split('T')[0] : '';
    };

    return (
        <div>
            <h1>Students</h1>
            <Button as={Link} to="/students/add" variant="primary">Add Student</Button>
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>First Name</th>
                        <th>Last Name</th>
                        <th>Date of Birth</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {students.map(student => (
                        <tr key={student.studentId}>
                            <td>{student.firstName}</td>
                            <td>{student.lastName}</td>
                            <td>{formatDate(student.dateOfBirth)}</td>
                            <td>
                                <Button as={Link} to={`/students/edit/${student.studentId}`} variant="warning">Edit</Button>
                                <Button onClick={() => deleteStudent(student.studentId)} variant="danger">Delete</Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </div>
    );
};

export default Students;
