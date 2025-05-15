import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

import translationUA from './translate/uk.json';
import translationEN from './translate/en.json';

const savedLanguage = localStorage.getItem('language') || 'uk';

const resources = {
  uk: { translation: translationUA },
  en: { translation: translationEN },
};

i18n
  .use(initReactI18next)
  .init({
    resources,
    lng: savedLanguage,
    fallbackLng: 'uk',
    interpolation: {
      escapeValue: false,
    },
  });

export default i18n;
