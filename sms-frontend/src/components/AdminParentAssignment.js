import React, { useEffect, useState } from 'react';
import axiosInstance from './axiosInstance';
import { Container, Table, Alert, Button, Modal } from 'react-bootstrap';
import { BASE_URL } from '../settings';
import { LinkContainer } from 'react-router-bootstrap';

const AdminParentAssignments = () => {
  const [assignments, setAssignments] = useState([]);
  const [error, setError] = useState("");
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [assignmentToDelete, setAssignmentToDelete] = useState(null);
  const [isDeleting, setIsDeleting] = useState(false);

  useEffect(() => {
    fetchAssignments();
  }, []);

  const fetchAssignments = async () => {
    try {
      const response = await axiosInstance.get(`${BASE_URL}/api/ParentStudent`);
      const assignmentsData = response.data.$values || [];
      setAssignments(assignmentsData);
    } catch (err) {
      console.error("Error fetching assignments:", err);
      setError("Failed to load assignments");
    }
  };

  const handleDeleteClick = (assignment) => {
    setAssignmentToDelete(assignment);
    setShowDeleteModal(true);
  };

  const handleConfirmDelete = async () => {
    if (!assignmentToDelete) return;

    setIsDeleting(true);
    try {
      await axiosInstance.delete(
        `${BASE_URL}/api/ParentStudent/${assignmentToDelete.parentId}/${assignmentToDelete.studentId}`
      );
      setAssignments(assignments.filter(a => 
        !(a.parentId === assignmentToDelete.parentId && a.studentId === assignmentToDelete.studentId)
      ));
      setShowDeleteModal(false);
    } catch (err) {
      console.error("Error deleting assignment:", err);
      setError("Failed to delete assignment");
    } finally {
      setIsDeleting(false);
    }
  };

  if (error) {
    return <Alert variant="danger">{error}</Alert>;
  }

  return (
    <Container>
      <h1>Parent–Student Assignments</h1>
      <LinkContainer to="/parent-student/add">
        <Button variant="primary" className="mb-3">
          Add Assignment
        </Button>
      </LinkContainer>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Parent Name</th>
            <th>Student Name</th>
            <th>Primary?</th>
            <th>Parent Email</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {assignments.map(assignment => (
            <tr key={`${assignment.parentId}-${assignment.studentId}`}>
              <td>
                {assignment.parent?.firstName} {assignment.parent?.lastName}
              </td>
              <td>
                {assignment.student?.firstName} {assignment.student?.lastName}
              </td>
              <td>{assignment.isPrimary ? "Yes" : "No"}</td>
              <td>{assignment.parent?.email}</td>
              <td>
                <Button 
                  variant="danger" 
                  size="sm"
                  onClick={() => handleDeleteClick(assignment)}
                >
                  Delete
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

      {/* Delete Confirmation Modal */}
      <Modal show={showDeleteModal} onHide={() => setShowDeleteModal(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Confirm Deletion</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          Are you sure you want to delete the assignment between {assignmentToDelete?.parent?.firstName} {assignmentToDelete?.parent?.lastName} and {assignmentToDelete?.student?.firstName} {assignmentToDelete?.student?.lastName}?
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowDeleteModal(false)}>
            Cancel
          </Button>
          <Button 
            variant="danger" 
            onClick={handleConfirmDelete}
            disabled={isDeleting}
          >
            {isDeleting ? "Deleting..." : "Delete"}
          </Button>
        </Modal.Footer>
      </Modal>
    </Container>
  );
};

export default AdminParentAssignments;