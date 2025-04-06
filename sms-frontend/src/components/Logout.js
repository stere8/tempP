import { useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from './AuthContext';

const Logout = () => {
  const { logout } = useContext(AuthContext);
  const navigate = useNavigate();

  useEffect(() => {
    console.log('Logout component mounted - logging out');
    logout();
    navigate('/login', { replace: true });
  }, []); // Run only once

  return null;
};

export default Logout;
