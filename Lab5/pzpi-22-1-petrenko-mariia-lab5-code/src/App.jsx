import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import React from "react";
import './App.css'
import WelcomePage from './pages/WelcomePage'

function App() {

  return (
    <>
      <Router>
        <Routes>
          <Route path="/" element={<WelcomePage />} />
        </Routes>
      </Router>
    </>
  )
}

export default App
