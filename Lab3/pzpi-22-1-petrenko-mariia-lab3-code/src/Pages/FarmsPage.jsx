import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './FarmsPage.css'

function FarmsPage({ token, onSelectFarm }) {
    const [farms, setFarms] = useState([]);
    const [error, setError] = useState(null);

    useEffect(() => {
        async function fetchFarms() {
            try {
                const response = await axios.get('/api/farms', {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                });
                console.log('API response:', response.data);
                setFarms(response.data);
            } catch (err) {
                setError('Не вдалося завантажити ферми.');
            }
        }

        fetchFarms();
    }, [token]);

    if (farms.length === 0) {
        return <p>У вас ще немає ферм.</p>;
    }

    if (error) {
        return <p style={{ color: 'red' }}>{error}</p>;
    }
    return (
        <div className='farms-container'>
            <h2>Оберіть ферму</h2>
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
                        <button onClick={() => onSelectFarm(farm.id)}>Перейти до ферми</button>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default FarmsPage;
