import axios from 'axios';
import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

function Animals() {
    const [animals, setAnimals] = useState([]);
    const { stableId } = useParams();
    const [error, setError] = useState(null);
    const { t } = useTranslation();
    const token = localStorage.getItem('token');

    useEffect(() => {
        const fetchAnimals = async () => {
            setError('');
            try {
                const response = await axios.get('/api/animals', {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });
                const filteredAnimals = response.data.filter(
                    animal => animal.stableId === stableId
                );
                setAnimals(filteredAnimals);
            } catch (err) {
                setError(t('errors.loadAnimals') + ': ' + err.message);
            }
        };

        if (stableId) {
            fetchAnimals();
        }
    }, [stableId]);

    if (error) {
        return <p style={{ color: 'red' }}>{error}</p>;
    }

    if (animals.length === 0) {
        return <p>{t('stablesPage.noStables')}</p>;
    }

    return (
        <div className='animals-container'>
            <h2>{t('animalsPage.title')}</h2>
            <div className='animals-container_list'>
                {animals.map((animal) => (
                    <div key={animal.id} className="animals-container_list_item">
                        <div className='animals-container_list_item_text'>
                            <span className='animals-container_list_item_text_species'>
                                {t('animalsPage.species')}{animal.species}
                            </span>
                            <span className='animals-container_list_item_text_name'>
                                {t('animalsPage.name')}{animal.name}
                            </span>
                            <span className='animals-container_list_item_text_breed'>
                                {t('animalsPage.breed')}{animal.breed}
                            </span>
                            <span className='animals-container_list_item_text_dateOfBirth'>
                                {t('animalsPage.dateOfBirth')}{animal.dateOfBirth}
                            </span>
                            <span className='animals-container_list_item_text_sex'>
                                {t('animalsPage.sex')}{animal.sex}
                            </span>
                        </div>
                        <button onClick={() => onSelectAnimal(animal.id)}>{t('stablesPage.goToStable')}</button>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default Animals;