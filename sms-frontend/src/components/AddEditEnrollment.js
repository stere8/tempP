import React, { useState, useEffect } from 'react';
import axiosInstance from './axiosInstance';
import { useNavigate, useParams, Link } from 'react-router-dom';
import { BASE_URL } from '../settings';
import { Form, Button } from 'react-bootstrap';

const AddEditEnrollment = () => {
  const [classes, setClasses] = useState([]);
  const [students, setStudents] = useState([]);
  const [classId, setClassId] = useState('');
  const [studentId, setStudentId] = useState('');
  const navigate = useNavigate();
  const { id } = useParams();

  useEffect(() => {
    // Fetch classes and students data in parallel
    const fetchData = async () => {
      try {
        const [classesRes, studentsRes] = await Promise.all([
          axiosInstance.get(`${BASE_URL}/api/classes`),
          axiosInstance.get(`${BASE_URL}/api/students`)
        ]);

        // For classes: if needed, normalize data (depends on your API response)
        const classesData = Array.isArray(classesRes.data)
          ? classesRes.data
          : classesRes.data.$values || [];
        setClasses(classesData);

        // For students: ensure we have an array for mapping.
        const studentsData = Array.isArray(studentsRes.data)
          ? studentsRes.data
          : studentsRes.data.$values || [];
        setStudents(studentsData);

        if (id) {
          // Fetch existing enrollment record if editing
          const enrollmentRes = await axiosInstance.get(`${BASE_URL}/api/enrollments/${id}`);
          setClassId(enrollmentRes.data.classId);
          setStudentId(enrollmentRes.data.studentId);
        }
      } catch (error) {
        console.error('Error fetching enrollment data:', error);
      }
    };

    fetchData();
  }, [id]);

  const handleSubmit = event => {
    event.preventDefault();

    const enrollmentData = { classId, studentId, enrollmentId: id };

    if (id) {
      axiosInstance.put(`${BASE_URL}/api/enrollments/${id}`, enrollmentData)
        .then(() => navigate('/enrollments'))
        .catch(error => console.error('Error updating enrollment:', error));
    } else {
      axiosInstance.post(`${BASE_URL}/api/enrollments`, enrollmentData)
        .then(() => navigate('/enrollments'))
        .catch(error => console.error('Error adding enrollment:', error));
    }
  };

  return (
    <div className="p-4">
      <h1>{id ? 'Edit Enrollment' : 'Add Enrollment'}</h1>
      <Form onSubmit={handleSubmit}>
        <Form.Group className="mb-3">
          <Form.Label>Class</Form.Label>
          <Form.Control 
            as="select" 
            value={classId} 
            onChange={e => setClassId(e.target.value)}
          >
            <option value="">Select Class</option>
            {classes.map(cls => (
              // Assuming each class object has a nested property "viewedClass"
              // that holds the class details (adjust if necessary)
              <option key={cls.viewedClass.classId} value={cls.viewedClass.classId}>
                {cls.viewedClass.name}
              </option>
            ))}
          </Form.Control>
        </Form.Group>
        <Form.Group className="mb-3">
          <Form.Label>Student</Form.Label>
          <Form.Control 
            as="select" 
            value={studentId} 
            onChange={e => setStudentId(e.target.value)}
          >
            <option value="">Select Student</option>
            {students.map(student => (
              <option key={student.studentId} value={student.studentId}>
                {student.firstName} {student.lastName}
              </option>
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
