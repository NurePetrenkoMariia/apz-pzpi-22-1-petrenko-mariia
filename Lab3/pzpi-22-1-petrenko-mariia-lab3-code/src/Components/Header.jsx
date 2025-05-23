import './Header.css';
import React from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

function Header() {
  const { i18n } = useTranslation();

  const token = localStorage.getItem('token');
  const role = parseInt(localStorage.getItem('role'));
  const navigate = useNavigate();

  const changeLanguage = (lng) => {
    i18n.changeLanguage(lng);
    localStorage.setItem('language', lng);
  };

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('role');
    navigate('/login');
  };

  return (
    <header className="header">
      <h1>FarmKeeper</h1>
      <nav>
        <ul>
          <li><a href="/farms">{i18n.t('header.main')}</a></li>
          {!token ? (
            <>
              <li><a href="/register">{i18n.t('header.register')}</a></li>
              <li className="header_login"><a href="/login">{i18n.t('header.login')}</a></li>
            </>
          ) : (
            <>
              {role === 3 && (
                <li><a href="/backup">{i18n.t('header.admin')}</a></li>
              )}
              <li>
                <button className="header_logout" onClick={handleLogout}>
                  {i18n.t('header.logout')}
                </button>
              </li>
            </>
          )}
          <li className="language-switcher" style={{ marginLeft: '1rem' }}>
            <a href="#" onClick={() => changeLanguage('uk')}>укр</a> | 
            <a href="#" onClick={() => changeLanguage('en')}> eng</a>
          </li>
        </ul>
      </nav>
    </header>
  );
}

export default Header;
