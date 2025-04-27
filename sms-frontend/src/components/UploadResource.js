import React, { useState } from "react";
import axios from "axios";

function UploadResource() {
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [classId, setClassId] = useState(1); // or a dropdown
    const [file, setFile] = useState(null);

    const handleUpload = async (e) => {
        e.preventDefault();
        try {
            const formData = new FormData();
            formData.append("Title", title);
            formData.append("Description", description);
            formData.append("ClassId", classId);
            formData.append("UploadedBy", 1); // In real scenario, get from user session or claims
            formData.append("File", file);

            await axiosInstance.post("/api/resource/upload", formData, {
                headers: {
                    "Content-Type": "multipart/form-data"
                }
            });
            alert("Upload successful!");
        } catch (err) {
            console.error(err);
            alert("Upload failed.");
        }
    };

    return (
        <form onSubmit={handleUpload}>
            <h2>Upload Resource</h2>
            <div>
                <label>Title:</label>
                <input value={title} onChange={(e) => setTitle(e.target.value)} />
            </div>
            <div>
                <label>Description:</label>
                <input value={description} onChange={(e) => setDescription(e.target.value)} />
            </div>
            <div>
                <label>Class:</label>
                <select value={classId} onChange={(e) => setClassId(e.target.value)}>
                    <option value="1">P3A</option>
                    <option value="2">P3B</option>
                    <option value="3">P4A</option>
                    <option value="4">P4B</option>
                </select>
            </div>
            <div>
                <label>File:</label>
                <input type="file" onChange={(e) => setFile(e.target.files[0])} />
            </div>
            <button type="submit">Upload</button>
        </form>
    );
}

export default UploadResource;
