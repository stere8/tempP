// AuthContext.js
import React, { createContext, useState, useEffect, useMemo } from 'react';
import {jwtDecode} from 'jwt-decode';
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

  const setAuthInfo = (token) => {
    try {
      const decoded = jwtDecode(token);
      console.log('setAuthInfo ing');
      console.log(decoded);
      setAuthState({
        userRole: Array.isArray(decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']) ? decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'][0] : decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
        userId: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
        studentId: decoded['studentId'] || null,
        staffId: decoded['teacherId'] || null,
        parentId: decoded['parentId'] || null
      });
    } catch (error) {
      console.error("Token decoding failed:", error);
      setAuthState({
        userRole: null,
        userId: null,
        studentId: null,
        staffId: null,
        parentId: null
      });
    }
  };

  const logout = () => {
    console.log('logouting'); // This should appear in the console.
    localStorage.removeItem('authToken');
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
