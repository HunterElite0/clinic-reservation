"use client";

import { useState } from "react";
import styles from "./page.module.css";
import { useRouter } from "next/navigation";

export default function Page() {
    const router = useRouter();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [name, setName] = useState("");
    const [speciality, setSpeciality] = useState("");
    const [role, setRole] = useState("");


    const handleSubmit = async (e: any) => {
        e.preventDefault();
        
        setName(name.replace(/\s/g, ''));
        const response = await fetch("http://localhost:5243/Account/signup", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                "name": name,
                "speciality": speciality,
                "account": {
                    "email": email,
                    "password": password,
                    "role": role === "doctor" ? 0 : 1
                }
            }),
        });
        const data = await response.json();
        if (data === "Email already exists") {
            alert("Email already exists");
            return;
        }
        else if (data === "Error creating account") {
            alert("Error creating account");
            return;
        }
        alert("Account created successfully");
        router.push("/");
    };

    const roleChange = (e: any) => {
        if (e.target.value === "doctor") {
            document.getElementById("ifDoctor")!.style.display = "block";
        } else {
            document.getElementById("ifDoctor")!.style.display = "none";
        }
    }

    const twoCalls = (e: any) => {
        setRole(e.target.value);
        roleChange(e);
    }
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
                        <label className={styles.radioItem} htmlFor="patientRole">Patient</label>
                        <input
                            className={styles.radioItem}
                            required
                            type="radio"
                            id="patientRole"
                            name="role"
                            onChange={(e) => twoCalls(e)}
                            value="patient"
                        />
                        <label className={styles.radioItem} htmlFor="doctorRole">Doctor</label>
                        <input
                            className={styles.radioItem}
                            required
                            type="radio"
                            id="doctorRole"
                            name="role"
                            onChange={(e) => twoCalls(e)}
                            value="doctor"
                        />
                    </div>
                    <div className={styles.specialityDiv} id="ifDoctor">
                        <label htmlFor="speciality">Specialty</label>
                        <br />
                        <input
                            id="speciality"
                            type="text"
                            name="speciality"
                            onChange={(e) => setSpeciality(e.target.value)}
                            value={speciality} />
                    </div>
                    <br />
                    <button type="submit">Register</button>
                </form>
            </div>
        </main>
    );
}




