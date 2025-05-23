import React, { useState } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import './FarmForms.css';


function AddAssignmentPage() {
    const { farmId } = useParams();
    const navigate = useNavigate();
    const { t } = useTranslation();
    const token = localStorage.getItem('token');

    const [assignment, setAssignment] = useState({
        name: '',
        description: '',
        numberOfParticipants: 0,
        status: '',
        priority: ''
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        let newValue;

        if (name === 'numberOfParticipants') {
            newValue = parseInt(value);
        } else if (name === 'status' || name === 'priority') {
            newValue = parseInt(value);
        } else {
            newValue = value;
        }

        setAssignment(prev => ({
            ...prev,
            [name]: newValue
        }));
    };

    const [error, setError] = useState('');
    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        try {
            await axios.post(`/api/assignments/${farmId}`, assignment, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });

            navigate(`/farms/${farmId}/assignments`);
        } catch (err) {

            setError(t('errors.addAssignment') + ': ' + err.message);
        }
    };

    return (
        <div className="farms-edit_form-container">
            <h2>{t('addAssignmentPage.addAssignment')}</h2>

            <form onSubmit={handleSubmit} className="farms-edit_form">
                <div className='farms-edit_form_field'>
                    <label>
                        {t('assignmentsPage.name')}:
                        <input
                            type="text"
                            name="name"
                            value={assignment.name}
                            onChange={handleChange}
                            required
                        />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>
                        {t('assignmentsPage.description')}:
                        <input
                            type="text"
                            name="description"
                            value={assignment.description}
                            onChange={handleChange}
                            required
                        />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>
                        {t('assignmentsPage.numberOfParticipants')}:
                        <input
                            type="number"
                            name="numberOfParticipants"
                            value={assignment.numberOfParticipants}
                            onChange={handleChange}
                            required
                        />
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>
                        {t('assignmentsPage.status')}:
                        <select name="status" value={assignment.status} onChange={handleChange} required>
                            <option value="">{t('status.selectStatus')}</option>
                            <option value="0">{t('status.notStarted')}</option>
                            <option value="1">{t('status.inProgress')}</option>
                            <option value="2">{t('status.finished')}</option>
                        </select>
                    </label>
                </div>
                <div className='farms-edit_form_field'>
                    <label>
                        {t('assignmentsPage.priority')}:
                        <select name="priority" value={assignment.priority} onChange={handleChange} required>
                            <option value="">{t('priority.selectPriority')}</option>
                            <option value="0">{t('priority.high')}</option>
                            <option value="1">{t('priority.medium')}</option>
                            <option value="2">{t('priority.low')}</option>
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

export default AddAssignmentPage;