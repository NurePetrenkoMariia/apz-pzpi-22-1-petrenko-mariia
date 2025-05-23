import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './FarmsPage.css';
import { useParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';

function AssignmentsPage() {
    const [assignments, setAssignments] = useState([]);
    const [error, setError] = useState(null);
    const { t } = useTranslation();
    const navigate = useNavigate();
    const { farmId } = useParams();

    useEffect(() => {
        const fetchAssignments = async () => {
            try {
                const token = localStorage.getItem('token');
                const response = await axios.get('/api/assignments', {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                });
                const farmAssignments = response.data.filter(
                    (assignment) => assignment.farmId === farmId
                );
                setAssignments(farmAssignments);
            } catch (err) {
                setError(t('errors.loadAssignments') + ': ' + err.message);
            }
        };

        fetchAssignments();
    }, [farmId]);

    const handleDelete = async (id) => {
        try {
            const token = localStorage.getItem('token');
            await axios.delete(`/api/assignments/${id}`, {
                headers: { Authorization: `Bearer ${token}` }
            });
            setAssignments(assignments.filter(a => a.id !== id));
        } catch (err) {
            setError(t('errors.deleteFarm') + err.message);
        }
    };

    const handleAdding = () => {
        navigate(`/farms/${farmId}/assignments/add`);
    };

    const getStatusLabel = (status) => {
        if (status === 0) return t('status.notStarted');
        if (status === 1) return t('status.inProgress');
        if (status === 2) return t('status.finished');
        return t('animalsPage.unknown');
    };

    const getPriorityLabel = (priority) => {
        if (priority === 0) return t('priority.high');
        if (priority === 1) return t('priority.medium');
        if (priority === 2) return t('priority.low');
        return t('animalsPage.unknown');
    };

    const handleTaskDistr= () => {
        navigate(`/farms/${farmId}/task-distribution`);
    };

    return (
        <div className='farms-container'>
            <h2>{t('assignmentsPage.title')}</h2>

            <button className='farms-container-add_button' onClick={handleAdding}>
                {t('assignmentsPage.add')}
            </button>
             <button className='farms-container-add_button' onClick={handleTaskDistr}>
                {t('assignmentsPage.taskDistr')}
            </button>


            {error && <p style={{ color: 'red' }}>{error}</p>}

            {assignments.length === 0 ? (
                <p className='no-farms'>{t('assignmentsPage.noAssignments')}</p>
            ) : (
                <div className='farms-container_list'>
                    {assignments.map((assignment) => (
                        <div key={assignment.id} className="farms-container_list_item">
                            <div className='assignments-container_list_item_text'>
                                <div className='assignments-container_list_item_text_value'>
                                   {t('assignmentsPage.name')}:        {assignment.name}
                                </div>
                                <div className='assignments-container_list_item_text_value'>
                                    {t('assignmentsPage.description')}:        {assignment.description}
                                </div>
                                 <div className='assignments-container_list_item_text_value'>
                                    {t('assignmentsPage.numberOfParticipants')}:     {assignment.numberOfParticipants}
                                </div>
                                 <div className='assignments-container_list_item_text_value'>
                                    {t('assignmentsPage.status')}:        {getStatusLabel(assignment.status)}
                                </div>
                                 <div className='assignments-container_list_item_text_value'>
                                    {t('assignmentsPage.priority')}:      {getPriorityLabel(assignment.priority)}
                                </div>
                            </div>
                            <div className='farms-container_list-buttons-container'>
                               
                                <button onClick={() => navigate(`/farms/${farmId}/assignments/${assignment.id}/edit`)}>
                                    {t('farmsPage.edit')}
                                </button>
                                <button onClick={() => handleDelete(assignment.id)}>
                                    {t('farmsPage.delete')}
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );

}

export default AssignmentsPage;