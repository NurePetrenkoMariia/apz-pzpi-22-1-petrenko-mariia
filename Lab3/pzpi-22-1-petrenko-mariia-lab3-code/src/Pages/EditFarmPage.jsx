import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import './FarmForms.css';

function EditFarmPage() {
    const { id } = useParams();
    const [form, setForm] = useState(null);
    const [error, setError] = useState(null);
    const { t } = useTranslation();
    const token = localStorage.getItem('token');
    const navigate = useNavigate();

    useEffect(() => {
        const fetchFarm = async () => {
            try {
                const response = await axios.get(`/api/farms/${id}`, {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                });

                const { name, city, country, street } = response.data;
                setForm({ name, city, country, street });
            } catch (err) {
                setError(err.message);
            }
        };

        fetchFarm();
    }, [id, token]);


    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            console.log('Submitting farm update:', form);
            await axios.put(`/api/farms/${id}`, form, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            navigate('/farms');
        } catch (err) {
            setError(t('errors.updateFarm') + ': ' + err.message);
        }
    };

    if (error) {
        return <p style={{ color: 'red' }}>{error}</p>;
    }

    if (!form) {
        return <p>{t('loading')}</p>;
    }

    return (
        
<div className="farms-edit_form-container">
     <h2>{t('editFarmPage.editFarm')}</h2>
        <form onSubmit={handleSubmit} className="farms-edit_form">
            <div className='farms-edit_form_field'>
                <label>
                    {t('editFarmPage.name')}:
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
                    {t('editFarmPage.city')}:
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
                    {t('editFarmPage.country')}:
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
                    {t('editFarmPage.street')}:
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
            <button type="submit">{t('editFarmPage.save')}</button>
            </div>
        </form>
        </div>
    );
}

export default EditFarmPage;
