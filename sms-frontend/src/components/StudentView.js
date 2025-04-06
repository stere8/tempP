import React, { useEffect, useState } from 'react';
import axiosInstance from './axiosInstance';
import { useParams } from 'react-router-dom';
import { Container, Card, Table, Alert } from 'react-bootstrap';

const StudentView = () => {
  const { id } = useParams(); // student id (or user id) from URL
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchDashboardData = async () => {
      try {
        // Adjust the endpoint as needed. Here we assume the dashboard endpoint for a student.
        const response = await axiosInstance.get(`/api/dashboard/student/${id}`);
        setData(response.data);
        setLoading(false);
      } catch (err) {
        console.error("Error fetching student dashboard data:", err);
        setError("Failed to load student dashboard data.");
        setLoading(false);
      }
    };

    fetchDashboardData();
  }, [id]);

  if (loading) {
    return (
      <Container className="mt-4">
        <Alert variant="info">Loading student details...</Alert>
      </Container>
    );
  }

  if (error) {
    return (
      <Container className="mt-4">
        <Alert variant="danger">{error}</Alert>
      </Container>
    );
  }

  // If you want to generate a timetable similar to the dashboard:
  const timeSlots = [
    { start: "08:00", end: "09:00" },
    { start: "09:00", end: "10:00" },
    { start: "10:30", end: "11:30" },
    { start: "11:30", end: "12:30" },
    { start: "13:30", end: "14:30" },
    { start: "14:30", end: "15:30" }
  ];
  const days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];

  const generateTimetable = (timetableData) => {
    const timetable = {};
    days.forEach(day => {
      timetable[day] = {};
      timeSlots.forEach(slot => {
        // Compare time strings (backend should return in "HH:mm:ss" format)
        const entry = timetableData.find(
          item =>
            item.dayOfWeek === day &&
            item.startTime === `${slot.start}:00` &&
            item.endTime === `${slot.end}:00`
        );
        timetable[day][slot.start] = entry ? entry.lessonName : "Free Period";
      });
    });
    return timetable;
  };

  // Use the returned data. For example, assume the API returns an object with:
  // { timetable: [...], attendanceSummary: [...], marks: [...], studentInfo: { firstName, lastName, dateOfBirth, className, gradeLevel } }
  const timetable = data.timetable ? generateTimetable(data.timetable) : null;

  return (
    <Container className="mt-4">
      {/* Student Basic Info */}
      {data.studentInfo && (
        <Card className="mb-4">
          <Card.Header>
            {data.studentInfo.firstName} {data.studentInfo.lastName}
          </Card.Header>
          <Card.Body>
            <Card.Text>
              Date of Birth: {new Date(data.studentInfo.dateOfBirth).toLocaleDateString()} <br />
              Class: {data.studentInfo.className} (Grade {data.studentInfo.gradeLevel})
            </Card.Text>
          </Card.Body>
        </Card>
      )}

      {/* Timetable Section */}
      {timetable && (
        <>
          <h2 className="mb-4">Weekly Timetable</h2>
          <Table striped bordered hover className="mb-5">
            <thead>
              <tr>
                <th>Time</th>
                {days.map(day => (
                  <th key={day}>{day}</th>
                ))}
              </tr>
            </thead>
            <tbody>
              {timeSlots.map(slot => (
                <tr key={slot.start}>
                  <td>{slot.start} - {slot.end}</td>
                  {days.map(day => (
                    <td key={day}>{timetable[day][slot.start]}</td>
                  ))}
                </tr>
              ))}
            </tbody>
          </Table>
        </>
      )}

      {/* Attendance Summary */}
      {data.attendanceSummary && data.attendanceSummary.length > 0 ? (
        <>
          <h2 className="mb-4">Attendance Summary</h2>
          <Table striped bordered hover className="mb-5">
            <thead>
              <tr>
                <th>Status</th>
                <th>Count</th>
              </tr>
            </thead>
            <tbody>
              {data.attendanceSummary.map((item, index) => (
                <tr key={index}>
                  <td>{item.status}</td>
                  <td>{item.count}</td>
                </tr>
              ))}
            </tbody>
          </Table>
        </>
      ) : (
        <Alert variant="info" className="mb-5">
          No attendance records available.
        </Alert>
      )}

      {/* Academic Performance */}
      {data.marks && data.marks.length > 0 ? (
        <>
          <h2 className="mb-4">Academic Performance</h2>
          <Table striped bordered hover>
            <thead>
              <tr>
                <th>Subject</th>
                <th>Marks</th>
                <th>Grade</th>
              </tr>
            </thead>
            <tbody>
              {data.marks.map((mark, index) => (
                <tr key={index}>
                  <td>{mark.subject}</td>
                  <td>{mark.markValue}</td>
                  <td>{mark.grade || "--"}</td>
                </tr>
              ))}
            </tbody>
          </Table>
        </>
      ) : (
        <Alert variant="info">
          No academic performance data recorded yet.
        </Alert>
      )}
    </Container>
  );
};

export default StudentView;
