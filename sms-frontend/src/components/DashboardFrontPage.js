import React, { useEffect, useState } from "react";
import axiosInstance from "./axiosInstance";
import {jwtDecode} from "jwt-decode"; // Make sure you're using the correct import if it's default export
import {
  Card,
  Table,
  Container,
  Alert,
  Badge
} from "react-bootstrap";

//
// ────────────────────────────────────────────────────────────────── Admin Dashboard
//
const AdminDashboard = ({ data }) => (
  <Container className="dashboard-section">
    <h2 className="mb-4">Admin Overview</h2>
    <div className="row">
      <div className="col-md-4 mb-3">
        <Card>
          <Card.Body>
            <Card.Title>Students</Card.Title>
            <Card.Text className="display-4">{data.totalStudents}</Card.Text>
          </Card.Body>
        </Card>
      </div>
      <div className="col-md-4 mb-3">
        <Card>
          <Card.Body>
            <Card.Title>Teachers</Card.Title>
            <Card.Text className="display-4">{data.totalTeachers}</Card.Text>
          </Card.Body>
        </Card>
      </div>
      <div className="col-md-4 mb-3">
        <Card>
          <Card.Body>
            <Card.Title>Parents</Card.Title>
            <Card.Text className="display-4">{data.totalParents}</Card.Text>
          </Card.Body>
        </Card>
      </div>
    </div>
  </Container>
);

//
// ────────────────────────────────────────────────────────────────── Teacher Dashboard
//
const TeacherDashboard = ({ data }) => {
  // If data is empty or invalid
  if (!data || data.length === 0) {
    return (
      <Alert variant="warning" className="m-3">
        <h3>No Class Assignments</h3>
        <p className="mb-0">
          You haven&apos;t been assigned to any classes yet. Please contact the school administration.
        </p>
      </Alert>
    );
  }

  // We expect something like data = [{ classes: [...], schedule: [...], subject: [...] }, ...]
  const { classes, schedule, subject } = data[0] || {};

  return (
    <Container className="dashboard-section">
      <h2 className="mb-4">Teacher Dashboard</h2>

      {/* Display assigned classes */}
      <h3>Assigned Classes</h3>
      {classes && classes.length > 0 ? (
        <ul>
          {classes.map((cls) => (
            <li key={cls.classId}>
              {cls.name} - Grade {cls.gradeLevel} ({cls.year})
            </li>
          ))}
        </ul>
      ) : (
        <p>No classes assigned.</p>
      )}

      {/* Display schedule */}
      <h3 className="mt-4">Schedule</h3>
      {schedule && schedule.length > 0 ? (
        <Table bordered hover>
          <thead>
            <tr>
              <th>Day</th>
              <th>Time</th>
              <th>Lesson</th>
            </tr>
          </thead>
          <tbody>
            {schedule.map((item) => {
              // Find the subject details for each lesson
              const subj = subject.find((s) => s.lessonId === item.lessonId);

              return (
                <tr key={item.timetableId}>
                  <td>{item.dayOfWeek}</td>
                  <td>
                    {item.startTime} - {item.endTime}
                  </td>
                  <td>{subj ? subj.subject : "Unknown Lesson"}</td>
                </tr>
              );
            })}
          </tbody>
        </Table>
      ) : (
        <p>No schedule available.</p>
      )}
    </Container>
  );
};

//
// ────────────────────────────────────────────────────────────────── Parent Dashboard
//
const ParentDashboard = ({ data }) => {
  if (!data || data.length === 0) {
    return (
      <Alert variant="warning" className="m-3">
        <h3>No Students Linked</h3>
        <p className="mb-0">
          Your account isn&apos;t associated with any students. Please contact the school office.
        </p>
      </Alert>
    );
  }

  return (
    <Container className="dashboard-section">
      <h2 className="mb-4">Linked Students</h2>
      <div className="row">
        {data.map((student) => (
          <div key={student.studentId} className="col-md-4 mb-3">
            <Card>
              <Card.Body>
                <Card.Title>
                  {student.firstName} {student.lastName}
                </Card.Title>
                <Card.Text>
                  Class: {student.className}
                  <br />
                  Grade: {student.gradeLevel}
                </Card.Text>
              </Card.Body>
            </Card>
          </div>
        ))}
      </div>
    </Container>
  );
};

//
// ────────────────────────────────────────────────────────────────── Student Dashboard
//
const StudentDashboard = ({ data }) => {
  // Helpful debug
  console.log("Student Dashboard Data:", data);

  // Define time slots and days for the timetable
  const timeSlots = [
    { start: "08:00", end: "09:00" },
    { start: "09:00", end: "10:00" },
    { start: "10:30", end: "11:30" },
    { start: "11:30", end: "12:30" },
    { start: "13:30", end: "14:30" },
    { start: "14:30", end: "15:30" }
  ];
  const days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];

  const timetableData = data?.classData?.classTimetable?.$values || [];
  const attendance = data?.attendanceSummary || [];
  const marks = data?.marks || [];
  const viewedClass = data?.classData?.viewedClass;
  const classTeachers = data?.classData?.classTeachers || [];

  // Empty checks
  const isEmpty =
    timetableData.length === 0 &&
    attendance.length === 0 &&
    marks.length === 0;

  if (isEmpty) {
    return (
      <Alert variant="warning" className="m-3">
        <h3>Academic Profile Pending</h3>
        <p className="mb-0">
          Your educational profile is being set up. Please visit administration if this persists.
        </p>
      </Alert>
    );
  }

  // Build timetable keyed by day + startTime
  const generateTimetable = (rawTimetable) => {
    const result = {};
    days.forEach((day) => {
      result[day] = {};
      timeSlots.forEach((slot) => {
        // Match item by dayOfWeek, start/end times
        const entry = rawTimetable.find(
          (item) =>
            item.dayOfWeek === day &&
            item.startTime === `${slot.start}:00` &&
            item.endTime === `${slot.end}:00`
        );
        // Fill each cell with either lesson or 'Free Period'
        result[day][slot.start] = entry
          ? `${entry.lessonName} (${entry.teachersName})`
          : "Free Period";
      });
    });
    return result;
  };

  const timetable = generateTimetable(timetableData);

  return (
    <Container className="dashboard-section">
      {/* Class Information */}
      {viewedClass && (
        <Card className="mb-4">
          <Card.Body>
            <Card.Title>
              {viewedClass.name} - Grade {viewedClass.gradeLevel}
              <Badge bg="secondary" className="ms-2">
                {viewedClass.year}
              </Badge>
            </Card.Title>
            <Card.Text>
              Class Teacher:&nbsp;
              {classTeachers.length > 0
                ? `${classTeachers[0].firstName} ${classTeachers[0].lastName}`
                : "Not assigned"}
            </Card.Text>
          </Card.Body>
        </Card>
      )}

      {/* Timetable Section */}
      {timetableData.length > 0 && (
        <>
          <h2 className="mb-4">Weekly Timetable</h2>
          <Table striped bordered hover className="mb-5">
            <thead>
              <tr>
                <th>Time</th>
                {days.map((day) => (
                  <th key={day}>{day}</th>
                ))}
              </tr>
            </thead>
            <tbody>
              {timeSlots.map((slot) => (
                <tr key={slot.start}>
                  <td>{slot.start} - {slot.end}</td>
                  {days.map((day) => (
                    <td key={day}>{timetable[day][slot.start]}</td>
                  ))}
                </tr>
              ))}
            </tbody>
          </Table>
        </>
      )}

      {/* Attendance Section */}
      {attendance.length > 0 ? (
        <>
          <h2 className="mb-4">Attendance Summary</h2>
          <div className="row mb-5">
            {attendance.map((item, index) => (
              <div key={index} className="col-md-3 mb-3">
                <Card>
                  <Card.Body>
                    <Card.Title>{item.status}</Card.Title>
                    <Card.Text className="display-4">{item.count}</Card.Text>
                  </Card.Body>
                </Card>
              </div>
            ))}
          </div>
        </>
      ) : (
        <Alert variant="info" className="mb-5">
          <h4>Attendance Records</h4>
          <p className="mb-0">No attendance records available for this period</p>
        </Alert>
      )}

      {/* Marks Section */}
      {marks.length > 0 ? (
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
              {marks.map((mark, index) => (
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
          <h4>Academic Performance</h4>
          <p className="mb-0">No grades recorded yet</p>
        </Alert>
      )}
    </Container>
  );
};

//
// ────────────────────────────────────────────────────────────────── Main Dashboard
//
const DashboardFrontPage = () => {
  const [userRole, setUserRole] = useState(null);
  const [userId, setUserId] = useState(null);
  const [dashboardData, setDashboardData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  // Decode JWT to set user role and ID
  useEffect(() => {
    const token = localStorage.getItem("authToken");
    if (!token) {
      setError("Not authenticated");
      setLoading(false);
      return;
    }

    try {
      const decoded = jwtDecode(token);
      setUserId(
        decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]
      );
      const roles =
        decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
        [];
      // roles can be string or array
      setUserRole(Array.isArray(roles) ? roles[0] : roles);
    } catch (err) {
      console.error("Token decoding failed:", err);
      setError("Invalid authentication token");
      setLoading(false);
    }
  }, []);

  // Fetch dashboard data based on role and userId
  useEffect(() => {
    if (!userRole || !userId) return;

    const endpoints = {
      Admin: "/api/dashboard/admin",
      Teacher: `/api/dashboard/teacher/${userId}`,
      Parent: `/api/dashboard/parent/${userId}`,
      Student: `/api/dashboard/student/${userId}`
    };

    const endpoint = endpoints[userRole] || endpoints.Student;

    axiosInstance
      .get(endpoint)
      .then((res) => {
        console.log("Dashboard data:", res.data);
        setDashboardData(res.data);
        setLoading(false);
      })
      .catch((err) => {
        console.error("API Error:", err);
        setError(
          err.response?.data?.message || "Failed to load dashboard data"
        );
        setLoading(false);
      });
  }, [userRole, userId]);

  return (
    <Container className="dashboard-container">
      <h1 className="my-4 text-center">{userRole} Dashboard</h1>

      {loading && (
        <div className="text-center my-5">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
          <p className="mt-2">Loading dashboard...</p>
        </div>
      )}

      {error && (
        <Alert variant="danger" className="my-4">
          <h3>Data Loading Issue</h3>
          <p>{error}</p>
          <p className="mb-0">
            Please try again or contact support if the problem continues.
          </p>
        </Alert>
      )}

      {!loading && !error && (
        <>
          {userRole === "Admin" && <AdminDashboard data={dashboardData} />}
          {userRole === "Teacher" && <TeacherDashboard data={dashboardData} />}
          {userRole === "Parent" && <ParentDashboard data={dashboardData} />}
          {(userRole === "Student" || !userRole) && (
            <StudentDashboard data={dashboardData} />
          )}
        </>
      )}
    </Container>
  );
};

export default DashboardFrontPage;
