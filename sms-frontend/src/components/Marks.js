import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Table, Button } from 'react-bootstrap';

const Marks = () => {
    const [marks, setMarks] = useState([]);
    const [students, setStudents] = useState([]);
    const [lessons, setLessons] = useState([]);

    useEffect(() => {
        const fetchMarks = async () => {
            try {
                const [marksResponse, studentsResponse, lessonsResponse] = await Promise.all([
                    axios.get(`${BASE_URL}/marks`),
                    axios.get(`${BASE_URL}/students`),
                    axios.get(`${BASE_URL}/lessons`)
                ]);

                setMarks(marksResponse.data);
                setStudents(studentsResponse.data);
                setLessons(lessonsResponse.data);
            } catch (error) {
                console.error('There was an error fetching the data!', error);
            }
        };

        fetchMarks();
    }, []);

    const deleteMark = id => {
        axios.delete(`${BASE_URL}/marks/${id}`)
            .then(() => setMarks(marks.filter(mark => mark.markId !== id)))
            .catch(error => console.error('Error deleting mark:', error));
    };

    const findStudentName = studentId => {
        const student = students.find(s => s.studentId === studentId);
        return student ? `${student.firstName} ${student.lastName}` : 'Unknown Student';
    };

    const findLessonName = lessonId => {
        const lesson = lessons.find(l => l.lessonId === lessonId);
        return lesson ? lesson.name : 'Unknown Lesson';
    };

    return (
        <div>
            <h1>Marks</h1>
            <Button as={Link} to="/marks/add" variant="primary">Add Mark</Button>
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Student</th>
                        <th>Lesson</th>
                        <th>Mark</th>
                        <th>Date</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {marks.map(mark => (
                        <tr key={mark.markId}>
                            <td>{findStudentName(mark.studentId)}</td>
                            <td>{findLessonName(mark.lessonId)}</td>
                            <td>{mark.markValue}</td>
                            <td>{new Date(mark.date).toLocaleDateString()}</td>
                            <td>
                                <Button as={Link} to={`/marks/edit/${mark.markId}`} variant="warning">Edit</Button>
                                <Button onClick={() => deleteMark(mark.markId)} variant="danger">Delete</Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </div>
    );
};

export default Marks;
