import React, { useEffect, useState } from "react";
import axiosInstance from "./axiosInstance";
import { jwtDecode } from "jwt-decode"; // Ensure your version supports named export (or use default export if needed)
import {
  Card,
  Table,
  Container,
  Alert,
  Badge
} from "react-bootstrap";

// Inline view for the Admin role
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

// Inline view for the Teacher role
const TeacherDashboard = ({ data }) => {
  if (!data || data.length === 0) {
    return (
      <Alert variant="warning" className="m-3">
        <h3>No Class Assignments</h3>
        <p className="mb-0">
          You haven't been assigned to any classes yet. Please contact the school administration.
        </p>
      </Alert>
    );
  }

  return (
    <Container className="dashboard-section">
      <h2 className="mb-4">Teaching Schedule</h2>
      {data.map((assignment, index) => (
        <Card key={index} className="mb-3">
          <Card.Body>
            <Card.Title>{assignment.className}</Card.Title>
            <Card.Subtitle className="mb-2 text-muted">
              {assignment.subject} - {assignment.schedule}
            </Card.Subtitle>
          </Card.Body>
        </Card>
      ))}
    </Container>
  );
};

// Inline view for the Parent role
const ParentDashboard = ({ data }) => {
  if (!data || data.length === 0) {
    return (
      <Alert variant="warning" className="m-3">
        <h3>No Students Linked</h3>
        <p className="mb-0">
          Your account isn't associated with any students. Please contact the school office.
        </p>
      </Alert>
    );
  }

  return (
    <Container className="dashboard-section">
      <h2 className="mb-4">Linked Students</h2>
      <div className="row">
        {data.map(student => (
          <div key={student.studentId} className="col-md-4 mb-3">
            <Card>
              <Card.Body>
                <Card.Title>{student.firstName} {student.lastName}</Card.Title>
                <Card.Text>
                  Class: {student.className}<br />
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

const renderTeacherDashBoardView = (data) => {
  // Check if data exists and has keys
  if (!data || Object.keys(data).length === 0) {
    return <div>No data available.</div>;
  }

  const { classes, schedule, subject } = data[0];

  return (
    <div>
      <h2>Teacher Dashboard</h2>

      {/* Display assigned classes */}
      <h3>Assigned Classes</h3>
      {classes && classes.length > 0 ? (
        <ul>
          {classes.map(cls => (
            <li key={cls.classId}>
              {cls.name} - Grade {cls.gradeLevel} ({cls.year})
            </li>
          ))}
        </ul>
      ) : (
        <p>No classes assigned.</p>
      )}

      {/* Display schedule */}
      <h3>Schedule</h3>
      {schedule && schedule.length > 0 ? (
        <table border="1" cellPadding="5" cellSpacing="0">
          <thead>
            <tr>
              <th>Day</th>
              <th>Time</th>
              <th>Lesson</th>
            </tr>
          </thead>
          <tbody>
            {schedule.map(item => {
              // Find the subject details matching the lessonId.
              const subj = subject.find(s => s.lessonId === item.lessonId);
              return (
                <tr key={item.timetableId}>
                  <td>{item.dayOfWeek}</td>
                  <td>{item.endTime} - {item.endTime}</td>
                  <td>{subj ? subj.subject : "Unknown Lesson"}</td>
                  {/* Assuming your timetable items include a teacher's name property;
                      if not, adjust accordingly to pull the teacher's name */}
                </tr>
              );
            })}
          </tbody>
        </table>
      ) : (
        <p>No schedule available.</p>
      )}
    </div>
  );
};

// Inline view for the Student role (integrated directly)
const renderStudentDashboardView = (data) =>
  {
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

  // Generate a timetable object from the timetable data provided in classData.classTimetable
  const generateTimetable = (timetableData) => {
    const timetable = {};
    days.forEach(day => {
      timetable[day] = {};
      timeSlots.forEach(slot => {
        // Ensure that the times are in "HH:mm:ss" format
        const entry = timetableData.find(
          item => item.dayOfWeek === day &&
                  item.startTime === `${slot.start}:00` &&
                  item.endTime === `${slot.end}:00`
        );
        timetable[day][slot.start] = entry ? `${entry.lessonName} (${entry.teachersName})` : "Free Period";
      });
    });
    return timetable;
  };

  // Check for empty profile data; note we expect classData to exist with timetable property.
  const isEmpty =
    !data.classData ||
    (
      (!data.classData.classTimetable || data.classData.classTimetable.length === 0) &&
      (!data.attendanceSummary || data.attendanceSummary.length === 0) &&
      (!data.marks || data.marks.length === 0)
    );

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

  // Generate timetable using the nested property
  const timetable = generateTimetable(data.classData.classTimetable);

  return (
    <Container className="dashboard-section">
      {/* Class Information */}
      <Card className="mb-4">
        <Card.Body>
          <Card.Title>
            {data.classData.viewedClass.name} - Grade {data.classData.viewedClass.gradeLevel}
            <Badge bg="secondary" className="ms-2">{data.classData.viewedClass.year}</Badge>
          </Card.Title>
          <Card.Text>
            Class Teacher:{" "}
            {data.classData.classTeachers?.length > 0
              ? data.classData.classTeachers[0].firstName + " " + data.classData.classTeachers[0].lastName
              : "Not assigned"}
          </Card.Text>
        </Card.Body>
      </Card>

      {/* Timetable Section */}
      {data.classData.classTimetable?.length > 0 && (
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

      {/* Attendance Section */}
      {data.attendanceSummary?.length > 0 ? (
        <>
          <h2 className="mb-4">Attendance Summary</h2>
          <div className="row mb-5">
            {data.attendanceSummary.map((item, index) => (
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
      {data.marks?.length > 0 ? (
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
          <h4>Academic Performance</h4>
          <p className="mb-0">No grades recorded yet</p>
        </Alert>
      )}
    </Container>
  );
};

// Consolidated Dashboard Front Page that handles all roles
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
      setUserId(decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]);
      const roles = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || [];
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
        setError(err.response?.data?.message || "Failed to load dashboard data");
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
          {userRole === "Teacher" && renderTeacherDashBoardView(dashboardData)}
          {userRole === "Parent" && <ParentDashboard data={dashboardData} />}
          {(userRole === "Student" || !userRole) && renderStudentDashboardView(dashboardData)}
        </>
      )}
    </Container>
  );
};

export default DashboardFrontPage;
