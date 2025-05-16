import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './FarmsPage.css';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';


function FarmsPage() {
    const [farms, setFarms] = useState([]);
    const [error, setError] = useState(null);
    const { t } = useTranslation();
    const navigate = useNavigate();

    useEffect(() => {
        async function fetchFarms() {
            const token = localStorage.getItem('token');
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
                console.log('API response:', response.data);
                setFarms(response.data);
            } catch (err) {
                setError(t('errors.loadFarms'));
            }
        }

        fetchFarms();
    }, []);

    const onSelectFarm = (farmId) => {
        navigate(`/farms/${farmId}/stables`);
    };

    if (farms.length === 0) {
        return <p className='no-farms'>{t('farmsPage.noFarms')}</p>;
    }

    if (error) {
        return <p style={{ color: 'red' }}>{error}</p>;
    }
    return (
        <div className='farms-container'>
            <h2>{t('farmsPage.title')}</h2>
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
                        <button onClick={() => onSelectFarm(farm.id)}>{t('farmsPage.goToFarm')}</button>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default FarmsPage;
