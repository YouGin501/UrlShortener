import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";

export default function UrlInfo() {
    const { id } = useParams();
    const [url, setUrl] = useState(null);

    useEffect(() => {
        fetch(`/api/shorturls/${id}`, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem("token")}`
            }
        })
            .then(res => res.json())
            .then(data => setUrl(data));
    }, [id]);

    if (!url) return <p>Loading...</p>;

    return (
        <div>
            <h2>URL Info</h2>
            <p><b>Original:</b> {url.originalUrl}</p>
            <p><b>Short Code:</b> {url.shortCode}</p>
            <p><b>Created:</b> {new Date(url.createdDate).toLocaleString()}</p>
        </div>
    );
}