import { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function Login() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    const handleLogin = async () => {
        const response = await fetch("/api/auth/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, password })
        });

        if (response.ok) {
            const data = await response.json();
            localStorage.setItem("token", data.token);
            localStorage.setItem("username", data.username);
            localStorage.setItem("roles", JSON.stringify(data.roles));
            navigate("/urls");
        } else {
            alert("Login failed");
        }
    };

    return (
        <div>
            <h2>Login</h2>
            <input value={username} onChange={(e) => setUsername(e.target.value)} placeholder="Username" />
            <input value={password} onChange={(e) => setPassword(e.target.value)} placeholder="Password" type="password" />
            <button onClick={handleLogin}>Login</button>
        </div>
    );
}