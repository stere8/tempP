// AuthContext.js
import React, { createContext, useState, useEffect, useMemo } from 'react';
import { jwtDecode } from 'jwt-decode';
import axiosInstance from "./axiosInstance";

export const AuthContext = createContext({
  userRole: null,
  userId: null,
  studentId: null,
  staffId: null,
  parentId: null,
  setAuthInfo: () => {},
  logout: () => {}
});

export const AuthProvider = ({ children }) => {
  const [authState, setAuthState] = useState({
    userRole: null,
    userId: null,
    studentId: null,
    staffId: null,
    parentId: null
  });

  // This function sets the authentication info from the token
  const setAuthInfo = (token) => {
    try {
      const decoded = jwtDecode(token);
      console.log('setAuthInfo ing');
      console.log(decoded);

      const userRole = Array.isArray(decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']) 
        ? decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'][0] 
        : decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

      // Set auth state
      setAuthState({
        userRole: userRole,
        userId: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
        studentId: decoded['studentId'] || null,
        staffId: decoded['teacherId'] || null,
        parentId: decoded['parentId'] || null
      });

      // Store the values in localStorage for persistence
      localStorage.setItem("userRole", userRole);
      localStorage.setItem("userId", decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']);
      localStorage.setItem("studentId", decoded['studentId'] || null);
      localStorage.setItem("teacherId", decoded['teacherId'] || null);
      localStorage.setItem("parentId", decoded['parentId'] || null);

    } catch (error) {
      console.error("Token decoding failed:", error);
      setAuthState({
        userRole: null,
        userId: null,
        studentId: null,
        staffId: null,
        parentId: null
      });

      // Clear any invalid auth state from localStorage
      localStorage.removeItem("userRole");
      localStorage.removeItem("userId");
      localStorage.removeItem("studentId");
      localStorage.removeItem("teacherId");
      localStorage.removeItem("parentId");
    }
  };

  const logout = () => {
    console.log('Logging out...');

    // Remove token and user data from localStorage
    localStorage.removeItem('authToken');
    localStorage.removeItem("userRole");
    localStorage.removeItem("userId");
    localStorage.removeItem("studentId");
    localStorage.removeItem("staffId");
    localStorage.removeItem("parentId");

    // Reset auth state
    setAuthState({
      userRole: null,
      userId: null,
      studentId: null,
      staffId: null,
      parentId: null
    });
  };

  const contextValue = useMemo(() => ({
    ...authState,
    setAuthInfo,
    logout
  }), [authState]);

  useEffect(() => {
    const token = localStorage.getItem('authToken');
    if (token) {
      setAuthInfo(token);
    }
  }, []);

  return (
    <AuthContext.Provider value={contextValue}>
      {children}
    </AuthContext.Provider>
  );
};
