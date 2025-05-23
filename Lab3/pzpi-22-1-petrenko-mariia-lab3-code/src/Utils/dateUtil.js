export function formatDateTime(dateString, language) {
  const date = new Date(dateString);
  return new Intl.DateTimeFormat(language === 'en' ? 'en-US' : 'uk-UA', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: 'numeric',
    minute: '2-digit',
    hour12: language === 'en',
  }).format(date);
}