"use client";

import Image from "next/image";
import styles from "./page.module.css";
import { useRouter } from "next/navigation";
import { FormEvent, useState } from "react";


export default function Home() {
  const router = useRouter();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  var cookie = require('cookie-cutter')

  const handleSubmit = async (e : any) => {
    e.preventDefault();
    const account = { email, password };
    const response = await fetch("http://localhost:5243/Account/signin", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(account),
    });
    const data = await response.json();
    if (data == "400 Bad Request") {
      alert("Email or password is incorrect");
    } else
    if (data === "Email or password is incorrect"){
      alert("Email or password is incorrect");
    }
    else{
      alert("Logged in successfully");
      cookie.set('id', data.Id)
      cookie.set('email', data.Email)
      cookie.set('role', data.Role)
      cookie.set('password',data.Password)
      if(data.Role === 0){router.push("/doctor")}
      else{router.push("/patient")}
    }
  }
  return (
    <main className={styles.main}>
      <div className={styles.formdiv}>
        <h1>Sign in</h1>
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
          <button type="submit">Sign in</button>
        </form>
        <h3>Don't have an account?</h3> <a className={styles.a} href="/signup">Sign up here.</a>
      </div>
    </main>
  );
}
