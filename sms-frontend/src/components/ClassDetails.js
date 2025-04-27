import React, { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { Card, Container, Table, Button, Form, Alert } from "react-bootstrap";
import axiosInstance from "./axiosInstance"; // Adjust path as needed
import { BASE_URL } from "../settings";

const ClassDetails = () => {
  const { id } = useParams(); // Class ID from route param
  const [classDetails, setClassDetails] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  
  // For sending a message
  const [messageText, setMessageText] = useState("");
  const [messageStatus, setMessageStatus] = useState("");

  useEffect(() => {
    const fetchClassDetails = async () => {
      try {
        const response = await axiosInstance.get(`${BASE_URL}/api/classes/${id}`);
        setClassDetails(response.data);
      } catch (err) {
        console.error("Error fetching class details:", err);
        setError("Unable to load class details.");
      } finally {
        setLoading(false);
      }
    };

    fetchClassDetails();
  }, [id]);

  // Timetable generation
  const timeSlots = [
    { start: "08:00", end: "09:00" },
    { start: "09:00", end: "10:00" },
    { start: "10:30", end: "11:30" },
    { start: "11:30", end: "12:30" },
    { start: "13:30", end: "14:30" },
    { start: "14:30", end: "15:30" },
  ];
  const days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];

  const generateTimetable = () => {
    if (!classDetails || !classDetails.classTimetable) return {};
    
    // Ensure we have an array; check if classTimetable is already an array or has a $values property.
    const timetableArray = Array.isArray(classDetails.classTimetable)
      ? classDetails.classTimetable
      : classDetails.classTimetable.$values || [];

    const timetable = {};
    days.forEach((day) => {
      timetable[day] = {};
      timeSlots.forEach((slot) => {
        const entry = timetableArray.find(
          (item) =>
            item.dayOfWeek === day &&
            item.startTime === `${slot.start}:00` &&
            item.endTime === `${slot.end}:00`
        );
        timetable[day][slot.start] = entry ? entry.lessonName : "N/A";
      });
    });
    return timetable;
  };

  // Send a message to the entire class
  const handleSendMessage = async () => {
    if (!messageText.trim()) {
      setMessageStatus("Please enter a message.");
      return;
    }
    try {
      await axiosInstance.post(`${BASE_URL}/api/classes/${id}/message`, {
        text: messageText,
      });
      setMessageStatus("Message sent successfully!");
      setMessageText("");
    } catch (err) {
      console.error("Error sending message:", err);
      setMessageStatus("Failed to send message.");
    }
  };

  if (loading) {
    return (
      <Container>
        <h3>Loading class details...</h3>
      </Container>
    );
  }

  if (error) {
    return (
      <Container>
        <Alert variant="danger">{error}</Alert>
      </Container>
    );
  }

  if (!classDetails) {
    return (
      <Container>
        <Alert variant="warning">No class details available.</Alert>
      </Container>
    );
  }

  const timetable = generateTimetable();

  return (
    <Container>
      <Card className="mb-4">
        <Card.Header>
          {classDetails.viewedClass.name} (Grade:{" "}
          {classDetails.viewedClass.gradeLevel}, Year:{" "}
          {classDetails.viewedClass.year})
        </Card.Header>
        <Card.Body>
          <Card.Title>Teachers</Card.Title>
          <ul>
            {classDetails.classTeachers && classDetails.classTeachers.length > 0 ? (
              classDetails.classTeachers.map((teacher) => (
                <li key={teacher.staffId}>
                  {teacher.firstName} {teacher.lastName} (
                  {teacher.subjectExpertise || "N/A"})
                </li>
              ))
            ) : (
              <li>N/A</li>
            )}
          </ul>

          <Card.Title className="mt-3">Students</Card.Title>
          <ul>
            {classDetails.classStudents && classDetails.classStudents.length > 0 ? (
              classDetails.classStudents.map((student) => (
                <li key={student.studentId}>
                  {student.firstName} {student.lastName}
                </li>
              ))
            ) : (
              <li>N/A</li>
            )}
          </ul>

          {/* Send Message Section */}
          <Card.Title className="mt-4">Send Message to Class</Card.Title>
          <Form.Group controlId="messageText" className="mb-2">
            <Form.Control
              as="textarea"
              rows={3}
              value={messageText}
              onChange={(e) => setMessageText(e.target.value)}
              placeholder="Type your message here..."
            />
          </Form.Group>
          <Button variant="primary" onClick={handleSendMessage}>
            Send Message
          </Button>
          {messageStatus && <p className="mt-2">{messageStatus}</p>}

          <Card.Title className="mt-4">Timetable</Card.Title>
          <Table bordered hover responsive>
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
                  <td>
                    {slot.start} - {slot.end}
                  </td>
                  {days.map((day) => (
                    <td key={day}>
                      {timetable[day] ? timetable[day][slot.start] : "N/A"}
                    </td>
                  ))}
                </tr>
              ))}
            </tbody>
          </Table>
        </Card.Body>
      </Card>
    </Container>
  );
};

export default ClassDetails;
