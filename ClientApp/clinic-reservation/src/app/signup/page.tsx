"use client";

import { useState } from "react";
import styles from "./page.module.css";

export default function Page() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [name, setName] = useState("");
    const [specialty, setSpecialty] = useState("");
    const [role, setRole] = useState("");


    const handleSubmit = async (e) => {
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
                    <label htmlFor="role">Specialty</label>
                    <input
                        className={styles.radio}
                        required
                        type="radio"
                        id="doctorRole"
                        name="role"
                        onChange={(e) => setSpecialty(e.target.value)}
                        value={role}
                    />
                    <label htmlFor="doctorRole">Doctor</label>
                    <input
                        className={styles.radio}
                        required
                        type="radio"
                        id="patientRole"
                        name="role"
                        onChange={(e) => setSpecialty(e.target.value)}
                        value={role}
                    />
                    <label htmlFor="patientRole">Patient</label>
                    <button type="submit">Register</button>
                </form>
                <h3>Don't have an account?</h3> <a className={styles.a} href="/signup">Sign up here.</a>
            </div>
        </main>
    );
}




