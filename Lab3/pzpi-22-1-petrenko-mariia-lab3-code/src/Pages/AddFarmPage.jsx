import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import './FarmForms.css';

function AddFarmPage() {
    const [form, setForm] = useState({
        name: '',
        city: '',
        country: '',
        street: ''
    });
    const [error, setError] = useState(null);
    const { t } = useTranslation();
    const token = localStorage.getItem('token');
    const navigate = useNavigate();

    const ownerId = localStorage.getItem('userId'); 
    console.log('ownerId:', ownerId);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!ownerId) {
            setError(t('errors.userNotFound'));
            return;
        }

        try {
            console.log('Submitting new farm:', form);
            await axios.post(`/api/farms/${ownerId}`, form, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            navigate('/farms');
        } catch (err) {
            setError(t('errors.addFarm') + ': ' + err.message);
        }
    };

    return (
        <div className="farms-edit_form-container">
            <h2>{t('addFarmPage.addFarm')}</h2>
            <form onSubmit={handleSubmit} className="farms-edit_form">
                <div className='farms-edit_form_field'>
                    <label>
                        {t('addFarmPage.name')}:
                        <input
                            type="text"
                            name="name"
                            value={form.name}
                            onChange={handleChange}
                            required
                        />
                    </label>
                </div>
                 <div className='farms-edit_form_field'>
                    <label>
                        {t('addFarmPage.country')}:
                        <input
                            type="text"
                            name="country"
                            value={form.country}
                            onChange={handleChange}
                            required
                        />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>
                        {t('addFarmPage.city')}:
                        <input
                            type="text"
                            name="city"
                            value={form.city}
                            onChange={handleChange}
                            required
                        />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>
                        {t('addFarmPage.street')}:
                        <input
                            type="text"
                            name="street"
                            value={form.street}
                            onChange={handleChange}
                            required
                        />
                    </label>
                </div>
                <div className="farms-edit_form_button">
                    <button type="submit">{t('addFarmPage.add')}</button>
                </div>
                {error && <p style={{ color: 'red' }}>{error}</p>}
            </form>
        </div>
    );
}

export default AddFarmPage;
