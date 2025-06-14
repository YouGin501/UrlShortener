import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

export default function UrlsTable() {
    const [urls, setUrls] = useState ([]);
    const [newUrl, setNewUrl] = useState("");

    const token = localStorage.getItem("token");
    const isAuthorized = !!token;
    const roles = JSON.parse(localStorage.getItem("roles") || "[]");
    const isAdmin = roles.includes("Admin");

    useEffect(() => {
        fetch("/api/shorturls")
            .then(res => res.json())
            .then(data => setUrls(data));
    }, []);

    const handleAdd = async () => {
        const res = await fetch("/api/shorturls", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`
            },
            body: JSON.stringify({ originalUrl: newUrl })
        });

        if (res.ok) {
            const created = await res.json();
            setUrls(prev => [...prev, created]);
        } else {
            const error = await res.text();
            alert(error);
        }
    };

    const handleDelete = async (id) => {
        const res = await fetch(`/api/shorturls/${id}`, {
            method: "DELETE",
            headers: { Authorization: `Bearer ${token}` }
        });

        if (res.status === 204) {
            setUrls(prev => prev.filter(u => u.id !== id));
        } else {
            const error = await res.text();
            alert(error);
        }
    };

    return (
        <div>
            <h2>Shortened URLs</h2>

            {isAuthorized && (
                <div>
                    <input
                        value={newUrl}
                        onChange={(e) => setNewUrl(e.target.value)}
                        placeholder="Enter URL"
                    />
                    <button onClick={handleAdd}>Shorten</button>
                </div>
            )}

            <table>
                <thead>
                    <tr>
                        <th>Original</th>
                        <th>Short</th>
                        {isAuthorized && isAdmin && <th>Actions</th>}
                    </tr>
                </thead>
                <tbody>
                    {urls.map((url) => (
                        <tr key={url.id}>
                            <td>{url.originalUrl}</td>
                            <td>
                                <a href={`/r/${url.shortCode}`} target="_blank" rel="noopener noreferrer">
                                    {url.shortCode}
                                </a>{" "}
                                (<Link to={`/url/${url.id}`}>Info</Link>)
                            </td>
                            {isAuthorized && isAdmin && (
                                <td>
                                    <button onClick={() => handleDelete(url.id)}>Delete</button>
                                </td>
                            )}
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}