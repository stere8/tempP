// src/components/Login.js
import React, {useContext, useState} from "react";
import axiosInstance from "./axiosInstance";
import { useNavigate } from "react-router-dom";
import { AuthContext } from './AuthContext';

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();
  const token = localStorage.getItem("authToken");
  const { setAuthInfo } = useContext(AuthContext);

  if(token){
    navigate("/dashboard")
  }

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    try {
      const response = await axiosInstance.post("/api/account/login", { email, password });
      console.log("Login successful:", response.data);
      localStorage.setItem("authToken", response.data.token);
      console.log('setAuthInfo ing');
      setAuthInfo(response.data.token);
      navigate("/dashboard");
    } catch (err) {
      console.error(err);
      setError("Login failed. Please check your credentials.");
    }
  };

  return (
    <div>
      <h2>Login</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}
      <form onSubmit={handleSubmit}>
        <div>
          <label>Email:</label>
          <input 
            type="email" 
            value={email} 
            onChange={(e) => setEmail(e.target.value)} 
            required 
          />
        </div>
        <div>
          <label>Password:</label>
          <input 
            type="password" 
            value={password} 
            onChange={(e) => setPassword(e.target.value)} 
            required 
          />
        </div>
        <button type="submit">Login</button>
      </form>
    </div>
  );
};

export default Login;
