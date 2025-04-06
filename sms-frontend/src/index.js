import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import { AuthProvider } from './components/AuthContext';
import 'bootstrap/dist/css/bootstrap.min.css';


ReactDOM.render(
  <React.StrictMode>
    <AuthProvider>
      <App />
    </AuthProvider>
  </React.StrictMode>,
  document.getElementById('root')
);
