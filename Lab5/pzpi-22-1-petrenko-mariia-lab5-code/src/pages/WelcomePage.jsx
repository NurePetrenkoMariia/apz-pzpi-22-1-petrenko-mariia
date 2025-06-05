import React from "react";
import './WelcomePage.css';


const DownloadPage = () => {
  const handleDownload = () => {
    const link = document.createElement("a");
    link.href = "/full-project.zip";
    link.download = "FarmKeeperInstaller.zip";
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };
  return (
    <div className="download-container">
      <h2 className="download-title">Завантажити інсталятор FarmKeeper</h2>
      <p className="download-description">
        Натисніть кнопку нижче, щоб отримати ZIP-файл з інсталятором
      </p>
      <p className="download-description">
        Після успішного завантаження розархівуйте ZIP-файл та запустіть run_system.py
      </p>
      <button className="download-button" onClick={handleDownload}>
        Завантажити ZIP
      </button>

    </div>
  );
};

export default DownloadPage;