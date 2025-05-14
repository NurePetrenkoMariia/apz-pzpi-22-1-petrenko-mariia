import './App.css';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Header from './Components/Header';
import Footer from './Components/Footer';
import FarmsPage from './Pages/FarmsPage';
import React from 'react';

function App() {

  return (
    <>
    <div className="app_container">
        <BrowserRouter>
          <Header />
          <main>
            <Routes>
              <Route path="/farms" element={<FarmsPage />} />
        
            </Routes>
          </main>
          <Footer />
        </BrowserRouter>
      </div>
       
    </>
  )
}

export default App
