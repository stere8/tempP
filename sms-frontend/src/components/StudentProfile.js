import React, { useEffect, useState } from 'react';
import axiosInstance from './axiosInstance ';
import { notifyError } from './NotificationService';

const StudentProfile = ({ studentId }) => {
  const [student, setStudent] = useState(null);
  const [parentMessages, setParentMessages] = useState([]);

  useEffect(() => {
    // Fetch student details
    axiosInstance.get(`/api/students/${studentId}`)
      .then(response => setStudent(response.data))
      .catch(() => notifyError("Failed to load student details."));

    // Fetch parent messages
    axiosInstance.get(`/api/parent-message/${studentId}`)
      .then(response => setParentMessages(response.data))
      .catch(() => notifyError("Failed to load parent messages."));
  }, [studentId]);

  return (
    <div className="student-profile">
      {student && (
        <div className="student-info">
          <h2>{student.name}</h2>
          <p>Class: {student.className}</p>
          <p>Contact: {student.contactInfo}</p>
          {/* More student details... */}
        </div>
      )}
      <div className="parent-messages">
        <h3>Parent Messages</h3>
        {parentMessages.length ? (
          parentMessages.map(msg => (
            <div key={msg.id} className="message">
              <strong>{msg.ParentName}</strong>: {msg.Message}
            </div>
          ))
        ) : (
          <p>No messages yet.</p>
        )}
      </div>
    </div>
  );
};

export default StudentProfile;
