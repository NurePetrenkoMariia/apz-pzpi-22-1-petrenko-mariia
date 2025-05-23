import React, { useState } from 'react';
import axios from 'axios';
import { useTranslation } from 'react-i18next';
import './BackupPage.css';

function BackupPage() {
    const { t } = useTranslation();
    const [restoring, setRestoring] = useState(false);
    const [message, setMessage] = useState('');
    const [file, setFile] = useState(null);
    const token = localStorage.getItem('token');

    const handleDownloadBackup = async () => {
        try {
            const response = await axios.get('/api/backup/download-backup', {
                responseType: 'blob',
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', 'FarmKeeperDatabaseBackup.bak');
            document.body.appendChild(link);
            link.click();
            link.remove();
            setMessage(t('backupPage.downloadSuccess'));
        } catch (error) {
            setMessage(`${t('backupPage.downloadError')}: ${error.message}`);
        }
    };

    const handleFileChange = (event) => {
        setFile(event.target.files[0]);
        setMessage('');
    };

    const handleRestore = async () => {
        if (!file) {
            setMessage(t('backupPage.noFileSelected'));
            return;
        }

        setRestoring(true);
        setMessage('');
        const formData = new FormData();
        formData.append('backupFile', file);

        try {
            const response = await axios.post('/api/backup/restore-database', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                    'Authorization': `Bearer ${token}`
                },
            });
            setMessage(`${t('backupPage.restoreSuccess')}: ${response.data}`);
        } catch (error) {
            setMessage(`${t('backupPage.restoreError')}: ${error.response?.data || error.message}`);
        } finally {
            setRestoring(false);
        }
    };

    return (
        <div className="backup-page-container">
            <h2>{t('backupPage.title')}</h2>
            <h3>{t('backupPage.backup')}</h3>

            <button className="users-container-add-worker_button" onClick={handleDownloadBackup}>
                {t('backupPage.downloadButton')}
            </button>

            <h3>{t('backupPage.restoreTitle')}</h3>

            <input
                type="file"
                accept=".bak"
                onChange={handleFileChange}
                className="file-input"
            />

            <button
                className="users-container-add-worker_button"
                onClick={handleRestore}
                disabled={restoring}
            >
                {t('backupPage.restoreButton')}
            </button>

            {message && <p className="backup-message">{message}</p>}
        </div>
    );
};

export default BackupPage;
