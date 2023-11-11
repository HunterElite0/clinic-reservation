"use client";

import Image from "next/image";
import styles from "./page.module.css";
import { useRouter } from "next/navigation";
import { FormEvent, useEffect, useState } from "react";
import Popup from "reactjs-popup";

export default function Page() {
  var cookie = require("cookie-cutter");
  const router = useRouter();
  const url: string = "http://localhost:5243/Patient/appointments";
  const [account, setAccount] = useState<
    { id: number; email: string; role: string }[]
  >([]);
  const [appointments, setAppointments] = useState<any[]>([]);
  const jsonArray: any = [];

  account.push({
    id: cookie.get("id"),
    email: cookie.get("email"),
    role: cookie.get("role"),
  });

  useEffect(() => {
    const fetchAppointmets = async () => {
      const fetchUrl: string = url + "?id=" + cookie.get("id");
      const response = await fetch(fetchUrl, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      });
      const data = await response.json();
      if (data === "You have no appointments.") {
        return;
      }
      for (var i = 0; i < data.length; i++) {
        jsonArray.push(data[i]);
      }
      setAppointments(jsonArray);
    };
    fetchAppointmets();
  }, []);

  const handleEdit = (id: number) => {};
  const handleCancel = async (id: number) => {
    const fetchUrl: string = url + "/Patient/appointments";
    const requestBody = { AccountId: cookie.get("id"), AppointmentId: id };
    await fetch(fetchUrl, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(requestBody),
    });
    alert("Appointment canceled successfully");
  };

  return (
    <main>
      <h1>Hello User (user type: Patient)</h1>
      <h2>My Appointments</h2>
      <div>
        <table>
          <thead>
            <tr>
              <th>Appointment Date</th>
              <th>Doctor</th>
              <th></th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {Array.isArray(appointments) && appointments.length > 0 ? (
              appointments.map((appointment) => (
                <tr key={appointment.Id}>
                  <td>{appointment.Slot.StartTime}</td>
                  <td>Dr.{appointment.Slot.Doctor.Name}</td>
                  <td>
                    <Popup
                      trigger={<button>Edit </button>}
                      modal
                    >
                      <div>
                        <form>
                          <label htmlFor="doctor">Doctor</label>
                          <input
                            disabled
                            type="text"
                            id="doctor"
                            value={appointment.Slot.Doctor.Name}
                          />
                          <label htmlFor="slot">Slot</label>
                        </form>
                        </div>
                    </Popup>
                  </td>
                  <td>
                    <button onClick={() => handleCancel(appointment.Id)}>
                      Cancel
                    </button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={4}>No appointments available</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </main>
  );
}
