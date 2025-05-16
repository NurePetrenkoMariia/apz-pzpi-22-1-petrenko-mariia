import './RegisterPage.css';
import { Link } from 'react-router-dom';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useTranslation } from 'react-i18next';

function RegisterPage() {

    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [dateOfBirth, setDateOfBirth] = useState("");
    const [phoneNumber, setPhoneNumber] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState('');
    const { t } = useTranslation();
    const navigate = useNavigate();

    const handleRegister = async (e) => {
        e.preventDefault();
        try {
            await axios.post('/api/auth/register', {
                firstName,
                lastName,
                dateOfBirth,
                phoneNumber,
                email,
                passwordHash: password,
            }, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            navigate('/login');
        } catch (err) {
            setError(t('errors.register') + ': ' + err.message);
        }
    };
    return (
        <>
            <div className="register-form-container">
                <h2>{t('registerPage.title')}</h2>
                <form onSubmit={handleRegister}>
                    <div className="register-form-field">
                        <label htmlFor="firstName">{t('registerPage.firstName')}</label>
                        <input type="text"
                            name="firstName"
                            onChange={e => setFirstName(e.target.value)} required
                        />
                    </div>
                    <div className="register-form-field">
                        <label htmlFor="lastName">{t('registerPage.lastName')}</label>
                        <input type="text"
                            name="lastName"
                            onChange={e => setLastName(e.target.value)} required
                        />
                    </div>
                    <div className="register-form-field">
                        <label htmlFor="dateOfBirth">{t('registerPage.dateOfBirth')}</label>
                        <input type="date"
                            name="dateOfBirth"
                            onChange={e => setDateOfBirth(e.target.value)} required
                        />
                    </div>
                    <div className="register-form-field">
                        <label htmlFor="phoneNumber">{t('registerPage.phoneNumber')}</label>
                        <input type="tel"
                            name="phoneNumber"
                            onChange={e => setPhoneNumber(e.target.value)} required
                        />
                    </div>
                    <div className="register-form-field">
                        <label htmlFor="email">{t('registerPage.email')}</label>
                        <input type="text" name="email" onChange={e => setEmail(e.target.value)} required />
                    </div>
                    <div className="register-form-field">
                        <label htmlFor="password">{t('registerPage.password')}</label>
                        <input type="password" name="password" onChange={e => setPassword(e.target.value)} required />
                    </div>
                    <div className="register-form-container-button">
                        <button type="submit">{t('registerPage.register')}</button>
                    </div>
                    {error && <p>{error}</p>}
                </form>
                <p className="register-link">{t('registerPage.account')}
                    <Link to="/login">
                        <strong>{t('registerPage.login')}</strong>
                    </Link>
                </p>
            </div>
        </>
    )
}

export default RegisterPage;