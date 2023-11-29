"use client";


import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import styles from "./page.module.css";

export default function EditSlot() {
  const Cookies = require('js-cookie')
  const router = useRouter();
  const url: string = "http://api:8080/Patient/appointments";
  const [appointments, setAppointments] = useState<any[]>([]);
  const [slots, setSlots] = useState<any[]>([]);
  const jsonArray: any = [];


  useEffect(() => {
    const fetchSlots = async () => {
      const fetchUrl: string = "http://api:8080/Doctor/empty-slots?id=" + Cookies.get("doctorId");
      const response = await fetch(fetchUrl, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      });
      const data = await response.json();
      if (data === "You have no open slots.") {
        return;
      }
      for (var i = 0; i < data.length; i++) {
        jsonArray.push(data[i]);
      }
      setSlots(jsonArray);
    };
    fetchSlots();
  }, []);

  // http://api:8080/Patient/appointments?AccountId=2&AppointmentId=3&SlotId=5


  const handleSubmit = async (e: any) => {
    e.preventDefault();
    const url : string = "http://api:8080/Patient/appointments?AccountId=" + Cookies.get("id") + "&AppointmentId=" + Cookies.get("appointmentId") + "&SlotId=" + e.target.slot.value;
    const response = await fetch(url, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
    });
    const data = await response.json();
    Cookies.remove('appointmentId');
    Cookies.remove('doctorId');
    router.push("/patient");
  }

    return (
      <div>
        <h1>Edit Slot</h1>
        <div className={styles.formDiv}>
          <form onSubmit={handleSubmit}>
            <label htmlFor="slot">Select a differnt slot:</label>
            <select name="slot" id="slot">
              {slots.map((slot) => (
                <option key={slot.Id} value={slot.Id}>
                  {slot.StartTime}
                </option>
              ))}
            </select>
            <button type="submit">Submit</button>
          </form>
        </div>
      </div>

    )
  }