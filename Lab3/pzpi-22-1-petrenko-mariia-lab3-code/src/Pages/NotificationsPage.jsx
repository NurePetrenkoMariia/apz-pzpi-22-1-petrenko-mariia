import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useTranslation } from 'react-i18next';
import './UsersPage.css';

function NotificationsPage() {
    const [notifications, setNotifications] = useState([]);
    const [error, setError] = useState('');
    const { t } = useTranslation();
    const token = localStorage.getItem('token');

    useEffect(() => {
        const fetchNotifications = async () => {
            try {
                const response = await axios.get('/api/notifications', {
                    headers: { Authorization: `Bearer ${token}` }
                });
                setNotifications(response.data);
            } catch (err) {
                setError(t('errors.loadNotifications') + ': ' + err.message);
            }
        };

        fetchNotifications();
    }, [token]);

    return (
        <div className='users-container'>
            <h2>{t('notificationsPage.title')}</h2>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            {notifications.length === 0 ? (
                <p className='no-users'>{t('notificationssPage.noNotifications')}</p>
            ) : (
                <div className='users-container_list'>
                    {notifications.map(notification => (
                        <div key={notification.id} className='users-container_list_item'>
                            <div className='users-container_list_item_text'>
                                <div className='users-container_list_item_text_div'>
                                    <strong>
                                        {t('notificationsPage.notificationTitle')}:
                                    </strong>
                                    {notification.title}
                                </div>
                                <div className='users-container_list_item_text_div'>
                                    <strong>
                                        {t('notificationsPage.text')}:
                                    </strong>
                                    {notification.text}
                                </div>
                                <div className='users-container_list_item_text_div'>
                                    <strong>{t('notificationsPage.date')}:</strong>{' '}
                                    {notification.dateTimeCreated.replace('T', ' ')}
                                </div>

                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}

export default NotificationsPage;
