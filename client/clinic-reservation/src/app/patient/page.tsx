"use client";

import styles from "./page.module.css";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export default function Page() {
  const Cookies = require('js-cookie')
  const router = useRouter();
  const url: string = "http://api:8080/Patient/appointments";
  const [name, setName] = useState('');
  const [appointments, setAppointments] = useState<any[]>([]);
  const [slots, setSlots] = useState<any[]>([]);
  const jsonArray: any = [];

  useEffect(() => {
    setName(Cookies.get("name"));
    const fetchAppointmets = async () => {
      const fetchUrl: string = url + "?id=" + Cookies.get("id");
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

  const handleEdit = (did: any, aid: any) => {
    Cookies.set("doctorId", did.toString());
    Cookies.set("appointmentId", aid.toString());
    router.push("/patient/edit");
  };
  const handleCancel = async (sid: number) => {
    const fetchUrl: string = url + "?AccountId=" + Cookies.get("id") + "&AppointmentId=" + sid;
    const response = await fetch(fetchUrl, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
    });
    const data = await response.json();
    alert(data)
    window.location.reload();
  };

  const handleLogout = () => {
    Cookies.remove('id');
    Cookies.remove('email');
    Cookies.remove('role');
    router.push('/');
  }

  return (
    <main>
      <h1>Hello {name} (user type: Patient)</h1>
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
                    <button onClick={(e: any) => handleEdit(appointment.Slot.Doctor.Id, appointment.Id)}>
                      Edit
                    </button>
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
        <div>
          <button type="button" onClick={() => router.push('/patient/add')}>Make Appointment</button>
        </div>
      </div>
      <button type="button" onClick={(_) => handleLogout()} >Logout</button>
    </main>
  );
}
