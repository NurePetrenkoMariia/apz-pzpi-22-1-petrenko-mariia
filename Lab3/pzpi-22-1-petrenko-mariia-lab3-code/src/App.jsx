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
import AssignmentsPage from './Pages/AssignmentsPage';
import AddAssignmentPage from './Pages/AddAssignmentPage';
import EditAssignmentPage from './Pages/EditAssignmentPage';
import UsersPage from './Pages/UsersPage';
import AddUserPage from './Pages/AddUserPage';
import EditUserPage from './Pages/EditUserPage';
import NotificationsPage from './Pages/NotificationsPage';
import FeedLevelPage from './Pages/FeedLevelPage';
import TaskDistributionPage from './Pages/TaskDistributionPage';
import BackupPage from './Pages/BackupPage';

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
              <Route path="/farms/:farmId/assignments" element={<AssignmentsPage />} />
              <Route path="/farms/:farmId/assignments/add" element={<AddAssignmentPage />} />
              <Route path="/farms/:farmId/assignments/:assignmentId/edit" element={<EditAssignmentPage />} />
              <Route path="/farms/:farmId/users" element={<UsersPage />} />
              <Route path="/farms/:farmId/users/add" element={<AddUserPage />} />
              <Route path="/farms/:farmId/users/:userId/edit" element={<EditUserPage />} />
              <Route path="/notifications" element={<NotificationsPage />} />
              <Route path="/farms/:farmId/stables/:stableId/feed-level-history/" element={<FeedLevelPage />} />
              <Route path="/login" element={<LoginPage />} />
              <Route path="/register" element={<RegisterPage />} />
              <Route path="/backup" element={<BackupPage />} />
              <Route path="/farms/:farmId/task-distribution" element={<TaskDistributionPage />} />
            </Routes>
          </main>
          <Footer />
        </BrowserRouter>
      </div>

    </>
  )
}

export default App
