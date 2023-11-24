import * as signalR from "@microsoft/signalr";
const URL = process.env.HUB_ADDRESS ?? "https://localhost:5243/notificationHub";

class Connector {
  private connection: signalR.HubConnection;
  public events: (
    onNotify: (username: string, message: string) => void
  ) => void;
  static instance: Connector;
  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(URL)
      .withAutomaticReconnect()
      .build();
    this.connection.start().catch((err) => document.write(err));
    this.events = (onNotify) => {
      this.connection.on("onNotificationReceived", (username, message) => {
        onNotify(username, message);
      });
    };
  }
  public newNotification = (user: string,messages: string) => {
    this.connection.invoke("Notify", user, messages).catch(function (err) {
        return console.error(err.toString());
    });
  };
  public static getInstance(): Connector {
    if (!Connector.instance) Connector.instance = new Connector();
    return Connector.instance;
  }
}
export default Connector.getInstance;
