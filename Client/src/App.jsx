import React from "react";
import { Routes, Route, Link } from "react-router-dom";

import Welcome from "./views/Welcome";
import Login from "./views/Login";
import UrlsTable from "./views/UrlsTable";
import UrlInfo from "./views/UrlInfo";
import About from "./views/About";

function App() {
    return (
        <div>
            <nav style={{ padding: "1rem", background: "#f0f0f0" }}>
                <Link to="/" style={{ marginRight: "1rem" }}>Home</Link>
                <Link to="/login" style={{ marginRight: "1rem" }}>Login</Link>
                <Link to="/urls" style={{ marginRight: "1rem" }}>URLs</Link>
                <Link to="/about">About</Link>
            </nav>

            <Routes>
                <Route path="/" element={<Welcome />} />
                <Route path="/login" element={<Login />} />
                <Route path="/urls" element={<UrlsTable />} />
                <Route path="/url/:id" element={<UrlInfo />} />
                <Route path="/about" element={<About />} />
            </Routes>
        </div>
    );
}

export default App;