import './App.css';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Header from './Components/Header';
import Footer from './Components/Footer';
import FarmsPage from './Pages/FarmsPage';
import React from 'react';
import StablesPage from './Pages/StablesPage';
import LoginPage from './Pages/LoginPage';
import RegisterPage from './Pages/RegisterPage';
import AnimalsPage from './Pages/AnimalsPage';
import EditFarmPage from './Pages/EditFarmPage';
import AddFarmPage from './Pages/AddFarmPage';
import AddAnimalPage from './Pages/AddAnimalPage';
import EditAnimalPage from './Pages/EditAnimalPage';

function App() {

  return (
    <>
      <div className="app_container">
        <BrowserRouter>
          <Header />
          <main>
            <Routes>
              <Route path="/farms" element={<FarmsPage />} />
              <Route path="/farms/add" element={<AddFarmPage />} />
              <Route path="/farms/:id/edit" element={<EditFarmPage />} />
              <Route path="/farms/:farmId/stables" element={<StablesPage />} />
              <Route path="/farms/:farmId/stables/:stableId/animals" element={<AnimalsPage />} />
              <Route path="/farms/:farmId/stables/:stableId/animals/add" element={<AddAnimalPage />} />
              <Route path="/farms/:farmId/stables/:stableId/animals/:animalId/edit" element={<EditAnimalPage />} />

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
