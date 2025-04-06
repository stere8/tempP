import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import { ListGroup, Container } from 'react-bootstrap';
import { BASE_URL } from '../settings';
import axiosInstance from './axiosInstance' 

const Classes = () => {
    const [classes, setClasses] = useState([]);

    useEffect(() => {
        const fetchClasses = async () => {
            try {
                const response = await axiosInstance.get(`${BASE_URL}/classes`);
                console.log (response);
                setClasses(response.data);
            } catch (error) {
                console.error('There was an error fetching the classes!',BASE_URL, error);
            }
        };

        fetchClasses();
    }, []);

    return (
        <Container>
            <h1>Classes</h1>
            <ListGroup>
                {classes.map(classItem => (
                    <ListGroup.Item key={classItem.viewedClass.classId}>
                        <Link to={`/classes/${classItem.viewedClass.classId}`}>{classItem.viewedClass.name}</Link>
                    </ListGroup.Item>
                ))}
            </ListGroup>
        </Container>
    );
};

export default Classes;
