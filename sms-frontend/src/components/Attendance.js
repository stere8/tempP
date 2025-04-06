import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import { Table, Button } from 'react-bootstrap';
import { BASE_URL } from '../settings';

const Attendance = () => {
    const [attendance, setAttendance] = useState([]);
    const [students, setStudents] = useState([]);
    const [lessons, setLessons] = useState([]);

    useEffect(() => {
        const fetchAttendanceData = async () => {
            try {
                const attendanceResponse = await axios.get(`${BASE_URL}/attendance`);
                const studentsResponse = await axios.get(`${BASE_URL}/students`);
                const lessonsResponse = await axios.get(`${BASE_URL}/lessons`);

                setAttendance(attendanceResponse.data);
                setStudents(studentsResponse.data);
                setLessons(lessonsResponse.data);
            } catch (error) {
                console.error('There was an error fetching the data!', error);
            }
        };

        fetchAttendanceData();
    }, []);

    const deleteAttendance = (id) => {
        axios.delete(`${BASE_URL}/attendance/${id}`)
            .then(() => setAttendance(attendance.filter(record => record.attendanceId !== id)))
            .catch(error => console.error('Error deleting attendance record:', error));
    };

    const getStudentNameById = (id) => {
        const student = students.find(student => student.studentId === id);
        return student ? `${student.firstName} ${student.lastName}` : 'Unknown Student';
    };

    const getLessonNameById = (id) => {
        const lesson = lessons.find(lesson => lesson.lessonId === id);
        return lesson ? lesson.name : 'Unknown Lesson';
    };

    const formatDate = (dateString) => {
        return dateString.split('T')[0]; // Removes the 'T00:00:00' part
    };

    return (
        <div>
            <h1>Attendance Records</h1>
            <Button as={Link} to="/attendance/add" variant="primary">Add Attendance Record</Button>
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Student</th>
                        <th>Lesson</th>
                        <th>Date</th>
                        <th>Status</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {attendance.map(record => (
                        <tr key={record.attendanceId}>
                            <td>{getStudentNameById(record.studentId)}</td>
                            <td>{getLessonNameById(record.lessonId)}</td>
                            <td>{formatDate(record.date)}</td>
                            <td>{record.status}</td>
                            <td>
                                <Button as={Link} to={`/attendance/edit/${record.attendanceId}`} variant="warning">Edit</Button>
                                <Button onClick={() => deleteAttendance(record.attendanceId)} variant="danger">Delete</Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </div>
    );
};

export default Attendance;
