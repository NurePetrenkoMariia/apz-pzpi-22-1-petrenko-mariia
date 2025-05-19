import axios from 'axios';
import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import './AnimalsPage.css';
import { useNavigate } from 'react-router-dom';

function Animals() {
    const [animals, setAnimals] = useState([]);
    const { stableId, farmId } = useParams();
    const [error, setError] = useState(null);
    const { t } = useTranslation();
    const token = localStorage.getItem('token');
    const navigate = useNavigate();

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

    const handleDelete = async (id) => {
        const confirm = window.confirm(t('animalsPage.confirmDelete'));
        if (!confirm) return;

        try {
            const token = localStorage.getItem('token');
            await axios.delete(`/api/animals/${id}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });

            setAnimals(prevAnimals => prevAnimals.filter(animal => animal.id !== id));
        } catch (err) {
            console.error('Delete animal error:', err);
            alert(t('errors.deleteAnimal') + ': ' + err.message);
        }
    };
    const getSexLabel = (sex) => {
        if (sex === 0) return t('sex.female');
        if (sex === 1) return t('sex.male');
        return t('animalsPage.unknown');
    };

    const handleAdding = () => {
        navigate(`/farms/${farmId}/stables/${stableId}/animals/add`);
    };

    return (
        <div className='animals-container'>
            <h2>{t('animalsPage.title')}</h2>
            <button className='animals-container-add_button' onClick={() => handleAdding()}>
                {t('animalsPage.add')}
            </button>
            {error && <p style={{ color: 'red' }}>{error}</p>}

            {animals.length === 0 ? (
                <p className='no-animals'>{t('animalsPage.noAnimals')}</p>
            ) : (
                <div className='animals-container_list'>
                    {animals.map((animal) => (
                        <div key={animal.id} className="animals-container_list_item">
                            <div className='animals-container_list_item_text'>
                                <div className='animals-container_list_item_text_species'>
                                    {t('animalsPage.species')}:     {animal.species}
                                </div>
                                <div className='animals-container_list_item_text_name'>
                                    {t('animalsPage.name')}:        {animal.name}
                                </div>
                                <div className='animals-container_list_item_text_breed'>
                                    {t('animalsPage.breed')}:       {animal.breed}
                                </div>
                                <div className='animals-container_list_item_text_dateOfBirth'>
                                    {t('animalsPage.dateOfBirth')}:     {animal.dateOfBirth}
                                </div>
                                <div className='animals-container_list_item_text_sex'>
                                    {t('animalsPage.sex')}:     {getSexLabel(animal.sex)}
                                </div>

                            </div>
                            <div className='farms-container_list-buttons-container'>
                                <button onClick={() => navigate(`/farms/${farmId}/stables/${stableId}/animals/${animal.id}/edit`)}>
                                    {t('farmsPage.edit')}
                                </button>
                                <button onClick={() => handleDelete(animal.id)}>
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

export default Animals;