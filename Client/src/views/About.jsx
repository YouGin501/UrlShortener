import { useEffect, useState } from "react";

export default function About() {
    const [content, setContent] = useState("");
    const [editContent, setEditContent] = useState("");
    const [isEditing, setIsEditing] = useState(false);

    const token = localStorage.getItem("token");
    const roles = JSON.parse(localStorage.getItem("roles") || "[]");
    const isAdmin = roles.includes("Admin");

    useEffect(() => {
        fetch("/api/about")
            .then(async res => {
                if (!res.ok) {
                    throw new Error("Failed to load about content");
                }
                return res.text();
            })
            .then(setContent)
            .catch(err => {
                console.error("Error loading about content:", err);
                setContent("Failed to load content.");
            });
    }, []);

    const handleSave = async () => {
        const res = await fetch("/api/about", {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${token}`
            },
            body: JSON.stringify(editContent)
        });

        if (res.ok) {
            setContent(editContent);
            setIsEditing(false);
        } else {
            alert("Failed to save content");
        }
    };

    return (
        <div style={{ padding: "1rem" }}>
            <h2>About</h2>
            {isEditing ? (
                <div>
                    <textarea
                        value={editContent}
                        onChange={(e) => setEditContent(e.target.value)}
                        rows={6}
                        cols={60}
                    />
                    <br />
                    <button onClick={handleSave}>Save</button>
                </div>
            ) : (
                <div>
                    <p>{content}</p>
                    {isAdmin && (
                        <button onClick={() => {
                            setEditContent(content);
                            setIsEditing(true);
                        }}>
                            Edit
                        </button>
                    )}
                </div>
            )}
        </div>
    );
}