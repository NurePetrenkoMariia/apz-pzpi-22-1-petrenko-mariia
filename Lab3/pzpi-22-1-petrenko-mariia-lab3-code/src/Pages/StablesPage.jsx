import axios from 'axios';
import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

function StablesPage() {
    const [stables, setStables] = useState([]);
    const { farmId } = useParams();
    const [error, setError] = useState(null);

    const { t } = useTranslation();

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

    if (error) {
        return <p style={{ color: 'red' }}>{error}</p>;
    }
    if (stables.length === 0) {
        return <p>{t('stablesPage.noStables')}</p>;
    }

    return (
        <div className='stables-container'>
            <h2>Стійла ферми</h2>
            <div className='stables-container_list'>
                {stables.map((stable) => (
                    <div key={stable.id} className="stables-container_list_item">
                        <div className='stables-container_list_item_text'>
                            <span className='stables-container_list_item_text_id'>
                                {t('stablesPage.id')}{stable.id}
                            </span>
                            <span className='stables-container_list_item_text_minFeedLevel'>
                                {t('stablesPage.feedLevel')}{stable.minFeedLevel}
                            </span>
                        </div>
                        <button onClick={() => onSelectStable(stable.id)}>{t('stablesPage.goToStable')}</button>
                    </div>
                ))}
            </div>
        </div>
    );

}

export default StablesPage;