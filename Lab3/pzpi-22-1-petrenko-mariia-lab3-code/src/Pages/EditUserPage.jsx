import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import "./FarmForms.css";

function EditUserPage() {
    const { farmId, userId } = useParams();
    const navigate = useNavigate();
    const { t } = useTranslation();
    const token = localStorage.getItem('token');

    const [user, setUser] = useState({
        firstName: '',
        lastName: '',
        dateOfBirth: '',
        phoneNumber: '',
        email: '',
        role: ''
    });

    const [error, setError] = useState('');

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const response = await axios.get(`/api/users/${userId}`, {
                    headers: { Authorization: `Bearer ${token}` }
                });

                const userData = response.data;
                setUser({
                    firstName: userData.firstName,
                    lastName: userData.lastName,
                    dateOfBirth: userData.dateOfBirth,
                    phoneNumber: userData.phoneNumber,
                    email: userData.email,
                    role: userData.role
                });
            } catch (err) {
                setError(t('errors.fetchUser') + ': ' + err.message);
            }
        };

        fetchUser();
    }, [userId, token, t]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setUser((prev) => ({
            ...prev,
            [name]: name === 'role' ? parseInt(value, 10) : value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        try {
            const updateDto = {
                firstName: user.firstName,
                lastName: user.lastName,
                dateOfBirth: user.dateOfBirth,
                phoneNumber: user.phoneNumber,
                email: user.email,
                role: parseInt(user.role),
                farmId: farmId
            };

            await axios.put(`/api/users/${userId}`, updateDto, {
                headers: { Authorization: `Bearer ${token}` }
            });

            navigate(`/farms/${farmId}/users`);
        } catch (err) {
            setError(t('errors.updateUser') + ': ' + err.message);
        }
    };

    return (
        <div className="farms-edit_form-container">
            <h2>{t('editUserPage.editUser')}</h2>

            <form onSubmit={handleSubmit} className="farms-edit_form">
                <div className='farms-edit_form_field'>
                    <label>{t('addUserPage.firstName')}:
                        <input type="text" name="firstName" value={user.firstName} onChange={handleChange} required />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>{t('addUserPage.lastName')}:
                        <input type="text" name="lastName" value={user.lastName} onChange={handleChange} required />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>{t('usersPage.dateOfBirth')}:
                        <input type="date" name="dateOfBirth" value={user.dateOfBirth} onChange={handleChange} required />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>{t('usersPage.phone')}:
                        <input type="text" name="phoneNumber" value={user.phoneNumber} onChange={handleChange} required />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>{t('usersPage.email')}:
                        <input type="email" name="email" value={user.email} onChange={handleChange} required />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>{t('addUserPage.role')}:
                        <select name="role" value={user.role} onChange={handleChange} required>
                            <option value="">{t('addUserPage.selectRole')}</option>
                            <option value={2}>{t('roles.worker')}</option>
                            <option value={1}>{t('roles.admin')}</option>
                        </select>
                    </label>
                </div>
                <div className="farms-edit_form_button">
                    <button type="submit">{t('editUserPage.saveChanges')}</button>
                </div>
                {error && <p style={{ color: 'red' }}>{error}</p>}
            </form>
        </div>
    );
}

export default EditUserPage;
