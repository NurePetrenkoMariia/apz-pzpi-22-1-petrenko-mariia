import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';


const TaskDistributionPage = () => {
    const [tasks, setTasks] = useState([]);
    const [assigning, setAssigning] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const { farmId } = useParams();
    const { t } = useTranslation();


    useEffect(() => {
        fetchTasks();
    }, []);

    const fetchTasks = async () => {
        try {
            const res = await axios.get('/api/UserTask', {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem('token')}`
                }
            });
            console.log('All records from backend:', res.data);
            setTasks(res.data);
        } catch (err) {
            setError('Помилка при завантаженні завдань');
        }
    };

    const handleDistribute = async () => {
        setAssigning(true);
        setError('');
        setSuccess('');
        try {
            await axios.post(`/api/TaskAssignment/assign-tasks/${farmId}`, null, {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem('token')}`
                }
            });
            await fetchTasks();
            setSuccess(t('taskDistrPage.success'));
        } catch (err) {
            setError(t('taskDistrPage.error'));
        } finally {
            setAssigning(false);
        }
    };

    return (
        <div className='user-task-container'>
            <h2>{t('taskDistrPage.title')}</h2>
            <button
                onClick={handleDistribute}
                disabled={assigning}
                style={{
                    width: '154px',
                    borderRadius: '8px',
                    border: 'none',
                    backgroundColor: 'black',
                    color: 'white',
                    padding: '5px',
                }}
            >
                {t('taskDistrPage.distributing')}
            </button>

            {error && <p style={{ color: 'red' }}>{error}</p>}
            {success && <p style={{ color: 'green' }}>{success}</p>}

            <table border="1" cellPadding="10" style={{ marginTop: '20px', width: '100%' }}>
                <thead>
                    <tr>
                        <th>{t('taskDistrPage.userId')}</th>
                        <th>{t('taskDistrPage.user')}</th>
                        <th>{t('usersPage.email')}</th>
                        <th>{t('taskDistrPage.taskName')}</th>
                        <th>{t('taskDistrPage.taskDescr')}</th>
                    </tr>
                </thead>
                <tbody>
                    {tasks.length === 0 ? (
                        <tr><td colSpan="4" style={{ textAlign: 'center' }}>{t('taskDistrPage.no-task')}</td></tr>
                    ) : (
                        tasks.map((task) => (
                            <tr key={task.id}>
                                <td>{task.user?.id || '—'}</td>
                                <td>{task.user?.firstName || '—'} {task.user.lastName}</td>
                                <td>{task.user?.email}</td>
                                <td>{task.assignment?.name}</td>
                                <td>{task.assignment?.description}</td>
                            </tr>
                        ))
                    )}
                </tbody>
            </table>
        </div>
    );
};

export default TaskDistributionPage;
