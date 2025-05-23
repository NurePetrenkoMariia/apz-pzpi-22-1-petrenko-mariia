import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './FarmsPage.css';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';


function FarmsPage() {
    const [farms, setFarms] = useState([]);
    const [error, setError] = useState(null);
    const [role, setRole] = useState(null);

    const { t } = useTranslation();
    const navigate = useNavigate();

    const token = localStorage.getItem('token');

    useEffect(() => {
        const storedRole = localStorage.getItem('role');
        if (storedRole !== null) {
            setRole(parseInt(storedRole));
        }
        fetchFarms();
    }, []);


    const fetchFarms = async () => {
        if (!token) {
            setError(t('errors.notAuthorized'));
            return;
        }
        try {
            const response = await axios.get('/api/farms', {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            setFarms(response.data);
        } catch (err) {
            setError(t('errors.loadFarms') + ': ' + err.message);
        }
    };

    const onSelectFarm = (farmId) => {
        navigate(`/farms/${farmId}/stables`);
    };

    const onSelectFarmToTask = (farmId) => {
        navigate(`/farms/${farmId}/assignments`);
    };

    const handleAdding = () => {
        navigate('/farms/add');
    };

    const onSelectFarmToUsers = (farmId) => {
        navigate(`/farms/${farmId}/users`);
    };

    const handleNotifications = () => {
        navigate(`/notifications`);
    }

    const handleDelete = async (id) => {
        const confirm = window.confirm(t('farmsPage.confirmDelete'));
        if (!confirm) return;

        try {
            const token = localStorage.getItem('token');
            await axios.delete(`/api/farms/${id}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });

            setFarms(prevFarms => prevFarms.filter(f => f.id !== id));
        } catch (err) {
            console.error('Delete error:', err);
            alert(t('errors.deleteFarm') + ': ' + err.message);
        }
    };

    return (
        <div className='farms-container'>
            <h2>{t('farmsPage.title')}</h2>

            <button className='farms-container-add_button' onClick={handleNotifications}>
                {t('farmsPage.notifications')}
            </button>
            
            {(role === 0) && (
                <button className='farms-container-add_button' onClick={handleAdding}>
                    {t('farmsPage.add')}
                </button>
            )}

            {error && <p style={{ color: 'red' }}>{error}</p>}

            {farms.length === 0 ? (
                <p className='no-farms'>{t('farmsPage.noFarms')}</p>
            ) : (
                <div className='farms-container_list'>
                    {farms.map((farm) => (
                        <div key={farm.id} className="farms-container_list_item">
                            <div className='farms-container_list_item_text'>
                                <span className='farms-container_list_item_text_name'>
                                    {farm.name}
                                </span>
                                <span className='farms-container_list_item_text_location'>
                                    ({farm.city}, {farm.country})
                                </span>
                            </div>
                            <div className='farms-container_list-buttons-container'>
                                <button onClick={() => onSelectFarm(farm.id)}>
                                    {t('farmsPage.goToFarm')}
                                </button>
                                <button onClick={() => onSelectFarmToTask(farm.id)}>
                                    {t('farmsPage.goToAssignment')}
                                </button>
                                <button onClick={() => onSelectFarmToUsers(farm.id)}>
                                    {t('farmsPage.goToUsers')}
                                </button>

                                {(role === 0 || role === 3) && (
                                    <>
                                        <button onClick={() => navigate(`/farms/${farm.id}/edit`)}>
                                            {t('farmsPage.edit')}
                                        </button>
                                        <button onClick={() => handleDelete(farm.id)}>
                                            {t('farmsPage.delete')}
                                        </button>
                                    </>
                                )}

                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}

export default FarmsPage;
