"use client";

//import styles from "./page.module.css";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export default function Page() {
  const Cookies = require("js-cookie");
  const router = useRouter();
  const id = Cookies.get("id");
  const [messasges, setMessages] = useState<any[]>([]);
  var array: any = [];


  useEffect(() => {
    const fetchMessages = async () => {
      const fetchUrl: string =
        "http://localhost:8000/Doctor/notifications?id=" + id;
      const response = await fetch(fetchUrl, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      });
      const data = await response.json();
      if (data === "You have no notifications.") {
        return;
      } else {
        data.split(":").forEach((element: any) => {
          console.log(element);
          array.push(element);
        }, array);
        setMessages(array);
      }
    };
    fetchMessages();
  }, []);

  return (
    <main>
      <h1> Notifications</h1>
      <div>
        {Array.isArray(messasges) && messasges.length > 0 ? (
          messasges.map((message: any) => (
              <p>{message}</p>
          ))
        ) : (
          <p>You have no notifications.</p>
        )}    
      </div>
    </main>
  );
}