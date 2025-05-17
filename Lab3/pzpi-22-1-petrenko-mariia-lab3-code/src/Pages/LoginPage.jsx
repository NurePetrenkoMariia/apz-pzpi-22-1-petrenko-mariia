import './LoginPage.css';
import { Link } from 'react-router-dom';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useTranslation } from 'react-i18next';

function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState('');
  const { t } = useTranslation();
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post('/api/auth/login', {
        email,
        password
      });

       const { token, userId } = response.data;
      localStorage.setItem('token', token);
      localStorage.setItem('userId', userId);
      const decoded = parseToken(token);


      if (decoded.role === "Owner" || decoded.role === "DatabaseAdmin") {
        navigate("/farms");
      } else if (decoded.role === "Admin") {
        navigate("/stables");
      } else {
        navigate("/");
      }
    } catch (err) {
      setError(t('errors.login') + ': ' + err.message);
    }
  };

  function parseToken(token) {
    try {
      const base64Url = token.split(".")[1];
      const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
      const jsonPayload = decodeURIComponent(
        atob(base64).split("").map(function (c) {
          return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
        }).join("")
      );

      return JSON.parse(jsonPayload);
    } catch (e) {
      return {};
    }
  }
  return (
    <>
      <div className="login-form-container">
        <h2>{t('loginPage.title')}</h2>
        <form onSubmit={handleLogin}>
          <div className="login-form-field">
            <label htmlFor="email">{t('loginPage.email')}</label>
            <input
              type="text"
              name="email"
              onChange={e => setEmail(e.target.value)} required
            />
          </div>
          <div className="login-form-field">
            <label htmlFor="password">{t('loginPage.password')}</label>
            <input
              type="password"
              name="password"
              onChange={e => setPassword(e.target.value)} required
            />
          </div>
          <div className="login-form-container-button">
            <button type="submit">{t('loginPage.login')}</button>
          </div>
          {error && <p>{error}</p>}
        </form>
        <p className="register-link">{t('loginPage.account')}
          <Link to="/register">
            <strong>{t('loginPage.register')}</strong>
          </Link>
        </p>
      </div>

    </>
  )

}

export default LoginPage;