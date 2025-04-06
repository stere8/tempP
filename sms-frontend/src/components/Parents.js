import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Table, Button } from 'react-bootstrap';

const Parents = () => {
  const [parents, setParents] = useState([]);

  useEffect(() => {
    const fetchParents = async () => {
      try {
        const response = await axios.get(`${BASE_URL}/api/parents`);
        console.log(response.data);
        setParents(response.data);
      } catch (error) {
        console.error('Error fetching parents:', error);
      }
    };

    fetchParents();
  }, []);

  const deleteParent = id => {
    axios.delete(`${BASE_URL}/parents/${id}`)
      .then(() => setParents(parents.filter(parent => parent.parentId !== id)))
      .catch(error => console.error('Error deleting parent:', error));
  };

  return (
    <div>
      <h1>Parents</h1>
      <Button as={Link} to="/parents/add" variant="primary">Add Parent</Button>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>First Name</th>
            <th>Last Name</th>
            <th>Email</th>
            <th>User ID</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {parents.map(parent => (
            <tr key={parent.parentId}>
              <td>{parent.firstName}</td>
              <td>{parent.lastName}</td>
              <td>{parent.email}</td>
              <td>{parent.studentParents ? parent.studentParents.length : 0}</td>
              <td>
                <Button as={Link} to={`/parents/edit/${parent.parentId}`} variant="warning">Edit</Button>
                <Button onClick={() => deleteParent(parent.parentId)} variant="danger">Delete</Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  );
};

export default Parents;
