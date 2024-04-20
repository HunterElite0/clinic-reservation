"use client";

import styles from "./page.module.css";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { URL } from "../../config";
import { authenticate } from "../../common/authenticate";

export default function Page() {
  let Cookies = require("js-cookie");
  const router = useRouter();
  const [slotTime, setSlotTime] = useState("");
  let date = new Date(Date.now());
  const role = Cookies.get("role");
  let dateTime =
    date.getFullYear() +
    "-" +
    (date.getMonth() + 1) +
    "-" +
    date.getDate() +
    " " +
    date.getHours() +
    ":" +
    date.getMinutes();

  useEffect(() => {
    if (!authenticate(role, "0")) {
      Cookies.remove("id");
      Cookies.remove("email");
      Cookies.remove("role");
      router.push("/")
      return;
    }
  }, [])

  const handleSubmit = async (e: any) => {
    e.preventDefault();
    setSlotTime(slotTime.replace("T", " "));
    const response = await fetch(URL + "/Doctor/slots?AccountId=" + Cookies.get('id') + "&SlotId=" + Cookies.get('slotId'), {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(slotTime),
    });
    const data = await response.json();
    alert(data);
    router.push("/doctor");
  };

  return (
    <main>
      <h1>Edit your slot</h1>
      <div>
        <form onSubmit={handleSubmit}>
          <label htmlFor="slot">New date and time for your slot: </label>
          <br />
          <input
            type="datetime-local"
            id="slot"
            name="slot"
            min={dateTime}
            value={slotTime}
            onChange={(e) => setSlotTime(e.target.value)}
          />
          <br />
          <button type="submit">Confirm changes</button>
        </form>
      </div>
    </main>
  );

}
