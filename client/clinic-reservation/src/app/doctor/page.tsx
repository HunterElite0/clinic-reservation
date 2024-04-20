"use client";

import styles from "./page.module.css";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { URL } from "../config"
import { authenticate } from "../common/authenticate";

export default function Page() {
  let Cookies = require("js-cookie");
  const router = useRouter();
  const [name, setName] = useState("");
  const [slots, setSlots] = useState<any[]>([]);
  const jsonArray: any = [];
  const role = Cookies.get("role");


  useEffect(() => {
    if (!authenticate(role, "0")) {
      Cookies.remove("id");
      Cookies.remove("email");
      Cookies.remove("role");
      router.push("/")
    }

    setName(Cookies.get("name"));
    const fetchAppointmets = async () => {
      const fetchUrl: string = URL + "/Doctor/slots?id=" + Cookies.get("id");
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
    fetchAppointmets();
  }, []);

  const handleEdit = (sid: any) => {
    Cookies.set("slotId", sid.toString());
    router.push("/doctor/edit");
  };
  const handleCancel = async (sid: number) => {
    const response = await fetch(
      URL + "/Doctor/slots?AccountId=" +
      Cookies.get("id") +
      "&SlotId=" +
      sid,
      {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
        },
      }
    );
    const data = await response.json();
    alert(data);
    window.location.reload();
  };

  const handleNew = () => {
    router.push("/doctor/new");
  };

  const handleLogout = () => {
    Cookies.remove("id");
    Cookies.remove("email");
    Cookies.remove("role");
    router.push("/");
  }

  return (
    <main>
      <h1>Hello {name} (user type: Doctor)</h1>
      <h2>My Slots</h2>
      <button type="button" onClick={(_) => router.push("/doctor/notifications")}>
        <h3>Notifications</h3>
      </button>
      <div>
        <table>
          <thead>
            <tr>
              <th>Slot Date</th>
              <th></th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {Array.isArray(slots) && slots.length > 0 ? (
              slots.map((slot) => (
                <tr key={slot.Id}>
                  <td>{slot.StartTime}</td>
                  <td>{slot.IsBooked ? <a> -- Slot is booked -- </a> : <a> -- Slot is not booked -- </a>}</td>
                  <td>
                    <button
                      onClick={(e: any) => handleEdit(slot.Id)}
                    >
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
      <button type="button" onClick={(_) => handleNew()}>
        Open new slot
      </button>
      <br />
      <button type="button" onClick={(_) => handleLogout()} >Logout</button>
    </main>
  );
}
