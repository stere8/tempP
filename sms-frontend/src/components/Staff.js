import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Table, Button } from 'react-bootstrap';

const Staff = () => {
    const [staff, setStaff] = useState([]);

    useEffect(() => {
        axios.get(`${BASE_URL}/staff`)
            .then(response => setStaff(response.data))
            .catch(error => console.error('Error fetching staff:', error));
    }, []);

    const deleteStaff = id => {
        axios.delete(`${BASE_URL}/staff/${id}`)
            .then(() => setStaff(staff.filter(staff => staff.staffId !== id)))
            .catch(error => console.error('Error deleting staff:', error));
    };

    return (
        <div>
            <h1>Staff</h1>
            <Button as={Link} to="/staff/add" variant="primary">Add Staff</Button>
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>First Name</th>
                        <th>Last Name</th>
                        <th>Email</th>
                        <th>Subject Expertise</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {staff.map(staff => (
                        <tr key={staff.staffId}>
                            <td>{staff.firstName}</td>
                            <td>{staff.lastName}</td>
                            <td>{staff.email}</td>
                            <td>{staff.subjectExpertise}</td>
                            <td>
                                <Button as={Link} to={`/staff/edit/${staff.staffId}`} variant="warning">Edit</Button>
                                <Button onClick={() => deleteStaff(staff.staffId)} variant="danger">Delete</Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </div>
    );
};

export default Staff;
