import './Header.css';
import React from 'react';
import { useTranslation } from 'react-i18next';

function Header() {
  const { i18n } = useTranslation();

  const changeLanguage = (lng) => {
    i18n.changeLanguage(lng);
    localStorage.setItem('language', lng); 
  };

  return (
    <header className="header">
      <h1>FarmKeeper</h1>
      <nav>
        <ul>
          <li><a href="/">Головна</a></li>
          <li><a href="#">{i18n.t('header.register')}</a></li>
          <li className='header_login'><a href="#">{i18n.t('header.login')}</a></li>
           <li className="language-switcher" style={{ marginLeft: '1rem' }}>
            <a href="#" onClick={() => changeLanguage('uk')}>укр</a> | 
            <a href="#" onClick={() => changeLanguage('en')}>eng</a>
          </li>
        </ul>
      </nav>
    </header>
  );
}

export default Header;
