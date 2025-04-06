import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Navbar from './components/NavigationBar';
import Attendance from './components/Attendance';
import Classes from './components/Classes';
import ClassDetails from './components/ClassDetails';
import Enrollments from './components/Enrollments';
import Lessons from './components/Lessons';
import Marks from './components/Marks';
import Staff from './components/Staff';
import Students from './components/Students';
import Timetable from './components/Timetable';
import AddEditStudent from './components/AddEditStudent';
import StudentView from './components/StudentView'; 
import AddEditStaff from './components/AddEditStaff';
import AddEditEnrollment from './components/AddEditEnrollment';
import AddEditTeacherEnrollment from './components/AddEditTeacherEnrollment';
import TeacherEnrollments from './components/TeacherEnrollments';
import AddEditLesson from './components/AddEditLesson';
import AddEditAttendance from './components/AddEditAttendance';
import AddEditMark from './components/AddEditMark';
import DashboardFrontPage from './components/DashboardFrontPage'; // Adjust the path if needed
import Login from './components/Login';
import Register from './components/Register';
import Parents from "./components/Parents";
import AddEditParent from "./components/AddEditParent";
import AdminParentAssignments from './components/AdminParentAssignment';
import AddParentStudentAssignment from './components/AddParentStudentAssignment';
import MyStudents from './components/MyStudents'
import Logout from "./components/Logout";

function App() {
    return (
        <Router>
            <Navbar />
            <Routes>
                {/* Dashboard Route */}
                <Route path="/dashboard" element={<DashboardFrontPage />} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/logout" element={<Logout />} />

                {/* Attendance Routes */}
                <Route path="/attendance" element={<Attendance />} />
                <Route path="/attendance/add" element={<AddEditAttendance />} />
                <Route path="/attendance/edit/:id" element={<AddEditAttendance />} />

                {/* Classes Routes */}
                <Route path="/classes" element={<Classes />} />
                <Route path="/classes/:id" element={<ClassDetails />} />

                {/* Enrollment Routes */}
                <Route path="/enrollments" element={<Enrollments />} />
                <Route path="/enrollments/add" element={<AddEditEnrollment />} />
                <Route path="/enrollments/edit/:id" element={<AddEditEnrollment />} />

                {/* Teacher Enrollment Routes */}
                <Route path="/teacher-enrollments" element={<TeacherEnrollments />} />
                <Route path="/teacher-enrollments/add" element={<AddEditTeacherEnrollment />} />
                <Route path="/teacher-enrollments/edit/:id" element={<AddEditTeacherEnrollment />} />

                {/* Lessons Routes */}
                <Route path="/lessons" element={<Lessons />} />
                <Route path="/lessons/add" element={<AddEditLesson />} />
                <Route path="/lessons/edit/:id" element={<AddEditLesson />} />

                {/* Marks Routes */}
                <Route path="/marks" element={<Marks />} />
                <Route path="/marks/add" element={<AddEditMark />} />
                <Route path="/marks/edit/:id" element={<AddEditMark />} />

                {/* Staff Routes */}
                <Route path="/staff" element={<Staff />} />
                <Route path="/staff/add" element={<AddEditStaff />} />
                <Route path="/staff/edit/:id" element={<AddEditStaff />} />

                {/* Students Routes */}
                <Route path="/students" element={<Students />} />
                <Route path="/students/add" element={<AddEditStudent />} />
                <Route path="/students/edit/:id" element={<AddEditStudent />} />
                <Route path="/students/view/:id" element={<StudentView />} />


                <Route path="/parent-students" element={<AdminParentAssignments />} />
                <Route path="/parents/students" element={<MyStudents />} />
                <Route path="/parent-student/add" element={<AddParentStudentAssignment />} />

                {/* Students Routes */}
                <Route path="/Parents" element={<Parents />} />
                <Route path="/Parents/add" element={<AddEditParent />} />
                <Route path="/Parents/edit/:id" element={<AddEditParent />} />

                {/* Timetable Route */}
                <Route path="/timetable" element={<Timetable />} />
            </Routes>
        </Router>
    );
}

export default App;