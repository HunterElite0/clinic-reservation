"use client"

import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { URL } from "../../config";


export default function Page() {
    const Cookies = require('js-cookie')
    const router = useRouter();
    const [slots, setSlots] = useState<any[]>([]);
    const [doctors, setDoctors] = useState<any[]>([]);
    
    useEffect(() => {
        const getDoctors = async () => {
            //URL +  Doctor/doctors

            const response = await fetch(URL+"/Doctor/doctors", {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                },
            });
            const data = await response.json();
            setDoctors(data);
        }
        getDoctors();
    }, [])

    const getSlots = async (id: any) => {
        //URL +  Doctor/empty-slots?id=1
        const response = await fetch(URL + "Doctor/empty-slots?id=" + id, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
            },
        })
        const data = await response.json();
        if (data === "You have no open slots.") {
            setSlots([]);
            return;
        }
        setSlots(data);
        document.getElementById('slot')?.removeAttribute("disabled");
    } // You have no open slots.



    const handleSubmit = async (e:any) => 
    {
        e.preventDefault();
        const response  = await fetch(URL + 'Patient/appointments?AccountId=' + Cookies.get("id") + '&SlotId=' + e.target.slot.value , {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
        });
        const data = await response.json();
        alert(data);
        if(data === "Account not found" || data === "Slot not found" || data === "Slot is already booked.")
        {return;}
        router.push('/patient')
    }

    return (
        <main>
            <h1>Make appointment</h1>
            <form onSubmit={handleSubmit}>
                <label htmlFor="doctor">Doctor</label>
                <select name="doctor" id="doctor" defaultValue={"Select a doctor"} onChange={(e) => getSlots(e.target.value)}>
                    <option disabled >Select a doctor</option>
                    {doctors.map((doctor) => (
                        <option key={doctor.Account.Id} value={doctor.Account.Id}>
                            {doctor.Name}
                        </option>
                    ))}
                </select>
                <label htmlFor="slot">Slot</label>
                <select disabled name="slot" id="slot" defaultValue={"No available slots"}>
                    {Array.isArray(slots) && slots.length > 0 ? (
                        slots.map((slot) => (
                            <option key={slot.Id} value={slot.Id}>
                                {slot.StartTime}
                            </option>
                        ))
                    ) : (
                        <option disabled>
                            No available slots
                        </option>
                    )}
                </select>
                <button type="submit">Make Appointment</button>
            </form>
        </main>
    )
}