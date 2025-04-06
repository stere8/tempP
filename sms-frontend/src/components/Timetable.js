import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { BASE_URL } from '../settings';
import { Table, Dropdown, Button } from 'react-bootstrap';

const Timetable = () => {
    const [classes, setClasses] = useState([]);
    const [selectedClass, setSelectedClass] = useState(null);
    const [selectedClassName, setSelectedClassName] = useState('');
    const [timetable, setTimetable] = useState([]);
    const [lessons, setLessons] = useState([]);
    const timeSlots = ['08:00 - 09:00', '09:00 - 10:00', '10:30 - 11:30', '11:30 - 12:30', '13:30 - 14:30', '14:30 - 15:30'];
    const days = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'];

    useEffect(() => {
        const fetchClasses = async () => {
            try {
                const response = await axios.get(`${BASE_URL}/classes`);
                setClasses(response.data);
            } catch (error) {
                console.error('There was an error fetching the classes!', error);
            }
        };

        fetchClasses();
    }, []);

    useEffect(() => {
        if (selectedClass) {
            fetchTimetable();
        }
    }, [selectedClass]);

    useEffect(() => {
        if (selectedClass) {
            const fetchLessons = async () => {
                try {
                    const gradeLevel = classes.find(c => c.viewedClass.classId === selectedClass)?.viewedClass.gradeLevel;
                    const response = await axios.get(`${BASE_URL}/lessons/grade/${gradeLevel}`);
                    setLessons(response.data);
                } catch (error) {
                    console.error('There was an error fetching the lessons!', error);
                }
            };

            fetchLessons();
        }
    }, [selectedClass, classes]);

    const fetchTimetable = async () => {
        try {
            const response = await axios.get(`${BASE_URL}/timetable/class/${selectedClass}`);
            const timetableData = createDefaultTimetable(response.data);
            setTimetable(timetableData);
        } catch (error) {
            console.error('There was an error fetching the timetable!', error);
        }
    };

    const createDefaultTimetable = (data) => {
        let defaultTimetable = [];

        days.forEach(day => {
            timeSlots.forEach(slot => {
                const [startTime, endTime] = slot.split(' - ').map(time => time + ':00');
                const existingEntry = data.find(item => item.dayOfWeek === day && item.startTime === startTime && item.endTime === endTime);
                defaultTimetable.push({
                    day,
                    slot,
                    lessonId: existingEntry ? existingEntry.lessonId : null,
                    id: existingEntry ? existingEntry.timetableId : null,  // Assuming each timetable entry has an ID
                });
            });
        });

        return defaultTimetable;
    };

    const handleClassSelect = (classId, className) => {
        setSelectedClass(classId);
        setSelectedClassName(className);
    };

    const handleLessonChange = (day, slot, lessonId) => {
        const updatedTimetable = timetable.map(t =>
            t.day === day && t.slot === slot ? { ...t, lessonId } : t
        );
        setTimetable(updatedTimetable);
    };

    const handleSave = (entry) => {
        const timetableEntry = {
            classId: selectedClass,
            lessonId: entry.lessonId,
            dayOfWeek: entry.day,
            startTime: entry.slot.split(' - ')[0] + ':00',
            endTime: entry.slot.split(' - ')[1] + ':00'
        };


        console.log('Saving timetable entry:', timetableEntry);

        if (entry.id) {
            timetableEntry.timetableId = entry.id;
            axios.put(`${BASE_URL}/timetable/${entry.id}`, timetableEntry)
                .then(() => {
                    console.log('Timetable entry updated successfully');
                    fetchTimetable();
                })
                .catch(error => {
                    console.error('Error updating timetable entry:', error);
                    if (error.response) {
                        console.error('Server response:', error.response.data);
                    }
                });
        } else {
            axios.post(`${BASE_URL}/timetable`, timetableEntry)
                .then((response) => {
                    console.log('Timetable entry created successfully');
                    const updatedTimetable = timetable.map(t => 
                        t.day === entry.day && t.slot === entry.slot 
                            ? { ...t, id: response.data.timetableId } 
                            : t
                    );
                    setTimetable(updatedTimetable);
                })
                .catch(error => {
                    console.error('Error creating timetable entry:', error);
                    if (error.response) {
                        console.error('Server response:', error.response.data);
                    }
                });
        }
    };

    return (
        <div>
            <h1>Timetable</h1>
            <Dropdown>
                <Dropdown.Toggle variant="success" id="dropdown-basic">
                    Select Class
                </Dropdown.Toggle>
                <Dropdown.Menu>
                    {classes.map(c => (
                        <Dropdown.Item 
                            key={c.viewedClass.classId} 
                            onClick={() => handleClassSelect(c.viewedClass.classId, c.viewedClass.name)}
                        >
                            {c.viewedClass.name}
                        </Dropdown.Item>
                    ))}
                </Dropdown.Menu>
            </Dropdown>

            {selectedClassName && <h2>Selected Class: {selectedClassName}</h2>}

            {selectedClass && (
                <Table striped bordered hover>
                    <thead>
                        <tr>
                            <th>Day</th>
                            <th>Time Slot</th>
                            <th>Lesson</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {timetable.map((time) => (
                            <tr key={`${time.day}-${time.slot}`}>
                                <td>{time.day}</td>
                                <td>{time.slot}</td>
                                <td>
                                    <LessonDropdown 
                                        lessons={lessons} 
                                        selectedLesson={time.lessonId} 
                                        onChange={(lessonId) => handleLessonChange(time.day, time.slot, lessonId)} 
                                    />
                                </td>
                                <td>
                                    <Button variant="primary" onClick={() => handleSave(time)}>Save</Button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </Table>
            )}
        </div>
    );
};

const LessonDropdown = ({ lessons, selectedLesson, onChange }) => {
    return (
        <select value={selectedLesson || ''} onChange={(e) => onChange(e.target.value)}>
            <option value="">{selectedLesson ? 'N/A' : 'Select Lesson'}</option>
            {lessons.map(lesson => (
                <option key={lesson.lessonId} value={lesson.lessonId}>
                    {lesson.name}
                </option>
            ))}
        </select>
    );
};

export default Timetable;