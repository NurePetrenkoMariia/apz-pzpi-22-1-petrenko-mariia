import React, { useState } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import "./FarmForms.css";

function AddUserPage() {
    const { farmId } = useParams();
    const navigate = useNavigate();
    const { t } = useTranslation();
    const token = localStorage.getItem('token');

    const [user, setUser] = useState({
        firstName: '',
        lastName: '',
        dateOfBirth: '',
        phoneNumber: '',
        email: '',
        password: '',
        role: ''
    });

    const [error, setError] = useState('');

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
            const requestBody = {
                firstName: user.firstName,
                lastName: user.lastName,
                dateOfBirth: user.dateOfBirth,
                phoneNumber: user.phoneNumber,
                email: user.email,
                passwordHash: user.password,
                role: parseInt(user.role),
                farmId: farmId
            };

            await axios.post('/api/users/create', requestBody, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });

            navigate(`/farms/${farmId}/users`);
        } catch (err) {
            setError(t('errors.addUser') + ': ' + (err.response?.data || err.message));
        }
    };

    return (
        <div className="farms-edit_form-container">
            <h2>{t('addUserPage.addUser')}</h2>

            <form onSubmit={handleSubmit} className="farms-edit_form">
                <div className='farms-edit_form_field'>
                    <label>
                        {t('addUserPage.firstName')}:
                        <input
                            type="text"
                            name="firstName"
                            value={user.firstName}
                            onChange={handleChange}
                            required
                        />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>
                        {t('addUserPage.lastName')}:
                        <input
                            type="text"
                            name="lastName"
                            value={user.lastName}
                            onChange={handleChange}
                            required
                        />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>
                        {t('usersPage.dateOfBirth')}:
                        <input
                            type="date"
                            name="dateOfBirth"
                            value={user.dateOfBirth}
                            onChange={handleChange}
                            required
                        />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>
                        {t('usersPage.phoneNumber')}:
                        <input
                            type="text"
                            name="phoneNumber"
                            value={user.phoneNumber}
                            onChange={handleChange}
                            required
                        />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>
                        {t('usersPage.email')}:
                        <input
                            type="email"
                            name="email"
                            value={user.email}
                            onChange={handleChange}
                            required
                        />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>
                        {t('usersPage.password')}:
                        <input
                            type="password"
                            name="password"
                            value={user.password}
                            onChange={handleChange}
                            required
                        />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>
                        {t('addUserPage.role')}:
                        <select name="role" value={user.role} onChange={handleChange} required>
                            <option value="">{t('addUserPage.selectRole')}</option>
                            <option value={2}>{t('roles.worker')}</option>
                            <option value={1}>{t('roles.admin')}</option>
                        </select>

                    </label>
                </div>

                <div className="farms-edit_form_button">
                    <button type="submit">{t('addAnimalPage.add')}</button>
                </div>
                {error && <p style={{ color: 'red' }}>{error}</p>}
            </form>
        </div>
    );
}

export default AddUserPage;