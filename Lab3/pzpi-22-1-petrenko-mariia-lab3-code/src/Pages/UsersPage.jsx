import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useTranslation } from 'react-i18next';
import { useParams } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import './UsersPage.css';

function UsersPage() {
    const [users, setUsers] = useState([]);
    const [error, setError] = useState(null);
    const { t } = useTranslation();
    const token = localStorage.getItem('token');
    const { farmId } = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        const fetchUsers = async () => {
            try {
                const response = await axios.get('/api/users', {
                    headers: { Authorization: `Bearer ${token}` }
                });

                const filteredUsers = response.data.filter(user =>
                    (user.farmId?.toLowerCase() === farmId.toLowerCase() ||
                        user.administeredFarmId?.toLowerCase() === farmId.toLowerCase()) &&
                    user.role !== 'Owner'
                );

                setUsers(filteredUsers);
            } catch (err) {
                setError(t('errors.loadUsers') + ': ' + err.message);
            }
        };

        fetchUsers();
    }, [farmId]);

    const handleAdding = () => {
        navigate(`/farms/${farmId}/users/add`);
    };

    const handleEdit = async (user) => {
        navigate(`/farms/${farmId}/users/${user.id}/edit`);
    };

    const handleDelete = async (userId) => {
        if (!window.confirm(t('usersPage.confirmDelete'))) return;

        try {
            await axios.delete(`/api/users/${userId}`, {
                headers: { Authorization: `Bearer ${token}` }
            });

            setUsers(prev => prev.filter(u => u.id !== userId));
        } catch (err) {
            alert(t('errors.deleteUser') + ': ' + err.message);
        }
    };

    return (
        <div className='users-container'>
            <h2>{t('usersPage.title')}</h2>
            <div className='users-container-add-buttons'>
                <button className='users-container-add-worker_button' onClick={() => handleAdding()}>
                    {t('usersPage.addWorker')}
                </button>
            </div>

            {error && <p style={{ color: 'red' }}>{error}</p>}

            {users.length === 0 ? (
                <p className='no-users'>{t('usersPage.noUsers')}</p>
            ) : (
                <div className='users-container_list'>
                    {users.map(user => (
                        <div key={user.id} className='users-container_list_item'>
                            <div className='users-container_list_item_text'>
                                <div className='users-container_list_item_text_div'><strong>{t('usersPage.name')}:</strong> {user.firstName} {user.lastName}</div>
                                <div className='users-container_list_item_text_div'><strong>{t('usersPage.email')}:</strong> {user.email}</div>
                                <div className='users-container_list_item_text_div'><strong>{t('usersPage.phone')}:</strong> {user.phoneNumber}</div>
                                <div className='users-container_list_item_text_div'><strong>{t('usersPage.dateOfBirth')}:</strong> {user.dateOfBirth}</div>
                            </div>

                            <div className='users-container_list-buttons-container'>
                                <button onClick={() => handleEdit(user)}>{t('usersPage.edit')}</button>
                                <button onClick={() => handleDelete(user.id)}>{t('usersPage.delete')}</button>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}

export default UsersPage;