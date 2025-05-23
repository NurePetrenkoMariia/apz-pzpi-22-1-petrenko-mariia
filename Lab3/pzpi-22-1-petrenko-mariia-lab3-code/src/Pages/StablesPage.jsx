import axios from 'axios';
import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import './StablesPage.css';

function StablesPage() {
    const [stables, setStables] = useState([]);
    const { farmId } = useParams();
    const [error, setError] = useState(null);
    const navigate = useNavigate();
    const { t, i18n } = useTranslation();
    const token = localStorage.getItem('token');

    useEffect(() => {
        const fetchStables = async () => {
            try {
                const response = await axios.get('/api/stables', {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });

                const farmStables = response.data.filter(
                    (stable) => stable.farmId === farmId
                );

                setStables(farmStables);
            } catch (err) {
                setError(t('errors.loadStables') + ': ' + err.message);
            }
        };

        fetchStables();
    }, [farmId]);

    const onSelectStable = (stableId) => {
        navigate(`/farms/${farmId}/stables/${stableId}/animals`);
    };

    const onSelectStableToFeed = (stableId) => {
        navigate(`/farms/${farmId}/stables/${stableId}/feed-level-history`);
    }
    const handleAdding = async () => {
        const minFeedLevel = prompt(t('stablesPage.enterMinFeedLevel'));
        if (!minFeedLevel) return;

        try {
            await axios.post(`/api/stables/${farmId}`,
                { minFeedLevel: parseFloat(minFeedLevel) },
                {
                    headers: { Authorization: `Bearer ${token}` }
                }
            );

            const response = await axios.get('/api/stables', {
                headers: { Authorization: `Bearer ${token}` }
            });
            const farmStables = response.data.filter(stable => stable.farmId === farmId);
            setStables(farmStables);
        } catch (error) {
            alert(t('errors.addStable') + ': ' + error.message);
        }
    };

    const handleEdit = async (stable) => {
        const newLevel = prompt(t('stablesPage.enterNewMinFeedLevel'), stable.minFeedLevel);
        if (newLevel === null) return;

        try {
            await axios.put(`/api/stables/${stable.id}`,
                {
                    id: stable.id,
                    farmId: farmId,
                    minFeedLevel: parseFloat(newLevel)
                },
                {
                    headers: { Authorization: `Bearer ${token}` }
                }
            );

            setStables(prev => prev.map(s =>
                s.id === stable.id ? { ...s, minFeedLevel: parseFloat(newLevel) } : s
            ));
        } catch (error) {
            alert(t('errors.editStable') + ': ' + error.response?.data || error.message);
        }
    };

    const handleDelete = async (stableId) => {
        const confirm = window.confirm(t('stablesPage.confirmDelete'));
        if (!confirm) return;

        try {
            await axios.delete(`/api/stables/${stableId}`, {
                headers: { Authorization: `Bearer ${token}` }
            });

            setStables(prev => prev.filter(s => s.id !== stableId));
        } catch (error) {
            alert(t('errors.deleteStable') + ': ' + error.message);
        }
    };

    return (
        <div className='stables-container'>
            <h2>{t('stablesPage.title')}</h2>
            <button className='stables-container-add_button' onClick={() => handleAdding()}>
                {t('stablesPage.add')}
            </button>

            {error && <p style={{ color: 'red' }}>{error}</p>}

            {stables.length === 0 ? (
                <p className='no-stables'>{t('stablesPage.noStables')}</p>
            ) : (

                <div className='stables-container_list'>
                    {stables.map((stable) => (
                        <div key={stable.id} className="stables-container_list_item">
                            <div className='stables-container_list_item_text'>
                                <span className='stables-container_list_item_text_id'>
                                    {t('stablesPage.id')}:      {stable.id}
                                </span>
                                <div className='stables-container_list_item_text_minFeedLevel'>
                                    {t('stablesPage.feedLevel')}:       {i18n.language === 'en'
                                        ? (stable.minFeedLevel * 0.393701).toFixed(2) + ' in'
                                        : stable.minFeedLevel + ' см'}

                                </div>
                            </div>

                            <div className='stables-container_list-buttons-container'>
                                <button onClick={() => onSelectStable(stable.id)}>
                                    {t('stablesPage.goToStable')}
                                </button>
                                <button onClick={() => onSelectStableToFeed(stable.id)}>
                                    {t('stablesPage.goToFeed')}
                                </button>
                                <button onClick={() => handleEdit(stable)}>
                                    {t('stablesPage.edit')}
                                </button>
                                <button onClick={() => handleDelete(stable.id)}>
                                    {t('stablesPage.delete')}
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );

}

export default StablesPage;