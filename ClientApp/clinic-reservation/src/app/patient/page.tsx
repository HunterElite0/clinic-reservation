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

    const handleEdit = (id: number) => {
        // Implement your edit logic here
        console.log(`Edit button clicked for appointment with id ${id}`);
    };
    const handleCancel = (id: number) => {
        // Implement your cancel logic here
        console.log(`Cancel button clicked for appointment with id ${id}`);
    }

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
               appointments.map(appointment => (
                 <tr key={appointment.Id}>
                   <td>{appointment.Slot.StartTime}</td>
                   <td>Dr.{appointment.Slot.Doctor.Name}</td>
                   <td>
                     <button onClick={() => handleEdit(appointment.Id)}>Edit</button>
                   </td>
                   <td>
                     <button onClick={() => handleCancel(appointment.Id)}>Cancel</button>
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