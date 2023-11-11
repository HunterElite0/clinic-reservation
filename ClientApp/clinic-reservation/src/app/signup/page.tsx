"use client";

import { useState } from "react";
import styles from "./page.module.css";

export default function Page() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [name, setName] = useState("");
    const [specialty, setSpecialty] = useState("");
    const [role, setRole] = useState("");


    const handleSubmit = async (e : any) => {
        e.preventDefault();
    };


    return (
        <main className={styles.main}>
            <div className={styles.formdiv}>
                <h1>Sign Up</h1>
                <form className={styles.form} onSubmit={handleSubmit}>
                    <label htmlFor="email">Email</label>
                    <input
                        required
                        type="text"
                        id="email"
                        onChange={(e) => setEmail(e.target.value)}
                        value={email}
                    />
                    <label htmlFor="password">Password</label>
                    <input
                        required
                        type="password"
                        id="password"
                        onChange={(e) => setPassword(e.target.value)}
                        value={password}
                    />
                    <label htmlFor="name">Name</label>
                    <input
                        required
                        type="text"
                        id="name"
                        onChange={(e) => setName(e.target.value)}
                        value={name}
                    />
                    <div className={styles.radio}>
                    <label htmlFor="role">Role:</label>
                    <label className={styles.radioItem} htmlFor="doctorRole">Doctor</label>
                    <input
                        className={styles.radioItem}
                        required
                        type="radio"
                        id="doctorRole"
                        name="role"
                        onChange={(e) => setSpecialty(e.target.value)}
                        value={role}
                    />
                    <label className={styles.radioItem} htmlFor="patientRole">Patient</label>
                    <input
                        className={styles.radioItem}
                        required
                        type="radio"
                        id="patientRole"
                        name="role"
                        onChange={(e) => setSpecialty(e.target.value)}
                        value={role}
                    />
                    </div>
                    <button type="submit">Register</button>
                </form>
            </div>
        </main>
    );
}




