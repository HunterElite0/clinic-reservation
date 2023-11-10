'use client';

import Image from "next/image";
import styles from "./page.module.css";
import { useRouter } from "next/navigation";
import { FormEvent, useEffect, useState } from "react";



export default function Page() {
    var cookie = require('cookie-cutter')
    const router = useRouter();
    const [account, setAccount] = useState("");
    const [appointments, setAppointments] = useState([])
    const jsonArray: any = [];


    useEffect(() => {
        const fetchAppointmets = async () => {
            const url: string = "http://localhost:5243/Patient/appointments?id=" + cookie.get('id');
            const response = await fetch(url, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            });
            const data = await response.json();
            if(data === "You have no appointments.") {return}
            for (var i = 0; i < data.length; i++) {
                jsonArray.push(data[i]);
            }
            setAppointments(jsonArray)
        }
        fetchAppointmets();
    }, []);


    return (
        <main>
            <h1>Hello User (user type: Patient)</h1>
            <h2>My Appointments</h2>
            <div>
                <table>
                    <tr>
                        <th>Appointment</th>
                        <th>Doctor</th>
                        <th></th>
                        <th></th>
                    </tr>
                    {appointments.map((appointment) => (
                        <tr>
                            <td>{appointment.Slot.StartTime}</td>
                            <td>{appointment.Doctor}</td>
                            <td><button>Edit</button></td>
                            <td><button>Cancel</button></td>
                        </tr>
                    ))}
                </table>
            </div>
        </main>
    );
}