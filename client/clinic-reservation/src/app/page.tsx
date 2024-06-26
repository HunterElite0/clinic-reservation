"use client";

import styles from "./page.module.css";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { URL } from "./config"

export default function Home() {
  const router = useRouter();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  let Cookies = require('js-cookie')

  const handleSubmit = async (e : any) => {
    e.preventDefault();
    setEmail(email.replace(/\s/g, ''));
    setPassword(password.replace(/\s/g, ''));
    const account = { email, password };
    // curl -X POST http://app:8080/Account/signin -H 'Content-Type: application/json' \  -d '{"email": "string", "password": "string", "role": 0}'
    const response = await fetch(URL + "/Account/signin", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "Allow-Cross-Origin": "*",
        "Access-Control-Allow-Origin": "*",
        "Access-Control-Allow-Methods": "GET, POST, PUT, DELETE, OPTIONS",
        "Access-Control-Allow-Headers": "Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With",
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
      Cookies.set('id', data.Id)
      Cookies.set('role', data.Role)
      Cookies.set('name', data.Name)
      if(data.Role === 0){router.push("/doctor")}
      else if(data.Role === 1){router.push("/patient")}
      else{alert("Invalid User")}
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
