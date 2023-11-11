// create a page to edit a slot by fetching all available slots and displaying them in a form
// The form should be pre-populated with the current slot information
// The form should be submitted to the edit endpoint
// The edit endpoint should return the updated slot information
// The page should redirect to the patient page
"use client";


import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import styles from "./page.module.css";

export default function EditSlot() {

  var cookie = require("cookie-cutter");
  const router = useRouter();
  const url: string = "http://localhost:5243/Patient/appointments";
  const [account, setAccount] = useState<{ id: number; email: string; role: string }[]>([]);
  const [appointments, setAppointments] = useState<any[]>([]);
  const [slots, setSlots] = useState<any[]>([]);
  const jsonArray: any = [];

  account.push({
    id: cookie.get("id"),
    email: cookie.get("email"),
    role: cookie.get("role"),
  });


  useEffect(() => {
    const fetchSlots = async () => {
      const fetchUrl: string = "http://localhost:5243/Doctor/slots?id=" + cookie.get("doctorId");
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

  // http://localhost:5243/Patient/appointments?AccountId=2&AppointmentId=3&SlotId=5


  const handleSubmit = async (e: any) => {
    e.preventDefault();
    const url : string = "http://localhost:5243/Patient/appointments?AccountId=" + cookie.get("id") + "&AppointmentId=" + cookie.get("appointmentId") + "&SlotId=" + e.target.slot.value;
    const response = await fetch(url, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
    });
    const data = await response.json();
    console.log(data);
    router.push("/patient");
  }

    return (
      <div>
        <h1>Edit Slot</h1>
        <div className={styles.formDiv}>
          <form onSubmit={handleSubmit}>
            <label htmlFor="slot">Select a slot:</label>
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