"use client"

import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export default function Page() {
    var cookie = require("cookie-cutter");
    const router = useRouter();
    const [account, setAccount] = useState<
        { id: number; email: string; role: string }[]>([]);
    const [slots, setSlots] = useState<any[]>([]);
    const [doctors, setDoctors] = useState<any[]>([]);
    const jsonArray: any = [];

    account.push({
        id: cookie.get("id"),
        email: cookie.get("email"),
        role: cookie.get("role"),
    });

    useEffect(() => {
        const getDoctors = async () => {
            // http://localhost:5243/Doctor/doctors

            const response = await fetch("http://localhost:5243/Doctor/doctors", {
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
        // http://localhost:5243/Doctor/empty-slots?id=1
        const response = await fetch("http://localhost:5243/Doctor/empty-slots?id=" + id, {
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
    } // You have no open slots.

    document.getElementById('slot')?.removeAttribute("disabled");


    return (
        <main>
            <h1>Make appointment</h1>
            <form>
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
            </form>
        </main>
    )
}