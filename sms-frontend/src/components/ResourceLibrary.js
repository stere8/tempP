import React, { useState, useEffect } from "react";
import axios from "axios";

function ResourceLibrary() {
    const [classId, setClassId] = useState(1);
    const [resources, setResources] = useState([]);

    useEffect(() => {
        axios.get(`/api/resource/class/${classId}`)
            .then((res) => setResources(res.data))
            .catch(console.error);
    }, [classId]);

    return (
        <div>
            <h2>Resource Library</h2>
            <select value={classId} onChange={(e) => setClassId(e.target.value)}>
                <option value="1">P3A</option>
                <option value="2">P3B</option>
                <option value="3">P4A</option>
                <option value="4">P4B</option>
            </select>

            <table>
                <thead>
                    <tr>
                        <th>Title</th>
                        <th>Description</th>
                        <th>Download</th>
                    </tr>
                </thead>
                <tbody>
                    {resources.map((res) => (
                        <tr key={res.resourceId}>
                            <td>{res.title}</td>
                            <td>{res.description}</td>
                            <td>
                                <a href={res.filePath} target="_blank" rel="noreferrer">View</a>
                            </td>
                            {/* If teacher/admin, show delete/edit */}
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}

export default ResourceLibrary;
