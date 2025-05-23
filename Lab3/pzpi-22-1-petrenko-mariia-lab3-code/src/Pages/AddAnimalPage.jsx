import React, { useState } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import "./FarmForms.css";

const AddAnimalPage = () => {
    const { farmId, stableId } = useParams();
    const navigate = useNavigate();
    const { t } = useTranslation();
    const token = localStorage.getItem('token');

    const [animal, setAnimal] = useState({
        species: '',
        name: '',
        breed: '',
        dateOfBirth: '',
        sex: ''
    });

    const [error, setError] = useState('');

    const handleChange = (e) => {
        const { name, value } = e.target;
        setAnimal((prev) => ({
            ...prev,
            [name]: name === 'sex' ? parseInt(value, 10) : value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        try {
            await axios.post(`/api/animals/${stableId}`, animal, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });

            navigate(`/farms/${farmId}/stables/${stableId}/animals`);
        } catch (err) {

            setError(t('errors.addAnimal') + ': ' + err.message);
        }
    };

    return (
        <div className="farms-edit_form-container">
            <h2>{t('addAnimalPage.addAnimal')}</h2>

            <form onSubmit={handleSubmit} className="farms-edit_form">
                <div className='farms-edit_form_field'>
                    <label>
                        {t('addAnimalPage.name')}:
                        <input
                            type="text"
                            name="name"
                            value={animal.name}
                            onChange={handleChange}
                            required
                        />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>
                        {t('addAnimalPage.species')}:
                        <input
                            type="text"
                            name="species"
                            value={animal.species}
                            onChange={handleChange}
                            required
                        />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>
                        {t('addAnimalPage.breed')}:
                        <input
                            type="text"
                            name="breed"
                            value={animal.breed}
                            onChange={handleChange}
                            required
                        />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>
                        {t('addAnimalPage.dateOfBirth')}:
                        <input
                            type="date"
                            name="dateOfBirth"
                            value={animal.dateOfBirth}
                            onChange={handleChange}
                            required
                        />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>
                        {t('addAnimalPage.sex')}:
                        <select name="sex" value={animal.sex} onChange={handleChange} required>
                            <option value="">{t('sex.selectSex')}</option>
                            <option value="0">{t('sex.female')}</option>
                            <option value="1">{t('sex.male')}</option>
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
};

export default AddAnimalPage;


