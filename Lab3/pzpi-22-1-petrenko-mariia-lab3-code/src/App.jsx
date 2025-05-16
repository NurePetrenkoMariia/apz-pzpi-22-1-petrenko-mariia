import './App.css';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Header from './Components/Header';
import Footer from './Components/Footer';
import FarmsPage from './Pages/FarmsPage';
import React from 'react';
import StablesPage from './Pages/StablesPage';
import LoginPage from './Pages/LoginPage';
import RegisterPage from './Pages/RegisterPage';

function App() {

  return (
    <>
      <div className="app_container">
        <BrowserRouter>
          <Header />
          <main>
            <Routes>
              <Route path="/farms" element={<FarmsPage />} />
              <Route path="/farms/:farmId/stables" element={<StablesPage />} />
              <Route path="/login" element={<LoginPage />} />
              <Route path="/register" element={<RegisterPage />} />
            </Routes>
          </main>
          <Footer />
        </BrowserRouter>
      </div>

    </>
  )
}

export default App
