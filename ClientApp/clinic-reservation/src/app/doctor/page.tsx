"use client";

import styles from "./page.module.css";
import { useRouter } from "next/navigation";
import { FormEvent, useEffect, useState } from "react";

export default function Page() {
  const Cookies = require('js-cookie')
  const router = useRouter();
  const url: string = "http://localhost:5243/Patient/appointments";
  const [appointments, setAppointments] = useState<any[]>([]);
  const [slots, setSlots] = useState<any[]>([]);
  const jsonArray: any = [];


  useEffect(() => {
    const fetchSlots = async () => {
      const fetchUrl: string = "http://localhost:5243/Doctor/slots?id=" + Cookies.get("id");
      const response = await fetch(fetchUrl, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      });
      const data = await response.json();
      if (data === "You have no Slots.") {
        return;
      }
      for (var i = 0; i < data.length; i++) {
        jsonArray.push(data[i]);
      }
      setSlots(jsonArray);
    };
    fetchSlots();
  }, []);

  const handleEdit = (did :any , aid :any) => { 
    Cookies.set("doctorId", did.toString());
    Cookies.set("appointmentId", aid.toString());
    router.push("/patient/edit");
  };
  const handleCancel = async (sid : number) => {
    const fetchUrl: string = url + "?AccountId=" + Cookies.get("id") + "&AppointmentId=" + sid;
    const response = await fetch(fetchUrl, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
    });
    const data = await response.json();
  };


  return (
    <main>
      <h1>Hello User (user type: Doctor)</h1>
      <h2>My Slots</h2>
      <div>
        <table>
          <thead>
            <tr>
              <th>Appointment Date</th>
              <th></th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {Array.isArray(slots) && slots.length > 0 ? (
              slots.map((slot) => (
                <tr key={slot.Id}>
                  <td>{slot.StartTime}</td>
                  <td>
                    <button onClick={(e : any) => handleEdit(slot.Doctor.Id , slot.Id)}>
                      Edit
                    </button>
                  </td>
                  <td>
                    <button onClick={() => handleCancel(slot.Id)}>
                      Cancel
                    </button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={4}>No Slots available</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </main>
  );
}
