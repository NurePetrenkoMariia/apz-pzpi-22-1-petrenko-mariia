import './Footer.css';
import React from 'react';
import { useTranslation } from 'react-i18next';

function Footer() {
  const { t } = useTranslation();

  return (
    <footer className="footer">
      <p>&copy; 2025 {t('footer.rights')}</p>
    </footer>
  );
}

export default Footer;