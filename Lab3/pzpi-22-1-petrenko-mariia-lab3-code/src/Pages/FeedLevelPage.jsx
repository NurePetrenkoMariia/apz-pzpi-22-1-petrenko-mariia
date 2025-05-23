import axios from 'axios';
import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import i18n from 'i18next';
import './StablesPage.css';
import { formatDateTime } from '../Utils/dateUtil';

function FeedLevelPage() {

    const [records, setRecords] = useState([]);
    const { stableId } = useParams();
    const [error, setError] = useState(null);
    const { t } = useTranslation();
    const token = localStorage.getItem('token');

    useEffect(() => {
        const fetchRecords = async () => {
            try {
                const response = await axios.get('/api/FeedLevelHistory', {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });
                console.log('All records from backend:', response.data);

                const allRecords = response.data;
                const filtered = allRecords.filter(r => r.stableId === stableId);

                setRecords(filtered);
            } catch (err) {
                setError(t('errors.loadRecords') + ': ' + err.message);
            }
        };

        console.log('Stable ID from params:', stableId);

        fetchRecords();
    }, [stableId]);

    return (
        <div className='stables-container'>
            <h2>{t('feedLevelPage.title')}</h2>
            {error && <p style={{ color: 'red' }}>{error}</p>}

            {records.length === 0 ? (
                <p className='no-stables'>{t('feedLevelPage.noHistory')}</p>
            ) : (

                <div className='stables-container_list'>
                    {records.map((record) => (
                        <div key={record.id} className="stables-container_list_item">
                            <div className='stables-container_list_item_text'>
                                <span className='stables-container_list_item_text_id'>
                                    {t('feedLevelPage.stableId')}:      {record.stableId}
                                </span>
                                <div className='stables-container_list_item_text_id'>
                                    {t('feedLevelPage.feedLevel')}:       {
                                        i18n.language === 'en'
                                            ? (record.feedLevel * 0.393701).toFixed(2) + ' in'
                                            : record.feedLevel + ' см'
                                    }

                                </div>
                                <div className='stables-container_list_item_text_id'>
                                    {t('feedLevelPage.time')}:       {formatDateTime(record.timestamp, i18n.language)}
                                </div>
                                <div className='stables-container_list_item_text_id'>
                                    {t('feedLevelPage.predictedTime')}:       {record.predictedTimeToEmpty}
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}
export default FeedLevelPage;