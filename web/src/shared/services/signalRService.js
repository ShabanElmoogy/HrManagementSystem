import * as signalR from "@microsoft/signalr";

class SignalRService {
  constructor(hubUrl) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl, {
        accessTokenFactory: () => {
          const token = sessionStorage.getItem('token'); // Make sure this matches your token key
          return token;
        }
      })
      .withAutomaticReconnect()
      .build();
  }

  async start() {
    try {
      console.log("SignalR Starting...", this.connection.state);
      if (this.connection.state === signalR.HubConnectionState.Disconnected) {
        await this.connection.start();
        console.log("SignalR Connected");
      }
    } catch (err) {
      console.error("SignalR Connection Error:", err);
    }
  }

  stop() {
    if (this.connection.state === signalR.HubConnectionState.Connected) {
      this.connection
        .stop()
        .catch((err) => console.error("Error stopping SignalR:", err));
    }
  }

  on(eventName, callback) {
    this.connection.on(eventName, callback);
  }

  off(eventName, callback) {
    this.connection.off(eventName, callback);
  }
}

// Create a single instance of the service
const lsBaseApi = (localStorage.getItem("baseApiUrl") && localStorage.getItem("baseApiUrl").replace(/\/$/, '')) || null;
const envApiBase = (import.meta.env.VITE_API_URL && import.meta.env.VITE_API_URL.replace(/\/$/, '')) || null;
const envHubUrl = (import.meta.env.VITE_SIGNALR_HUB_URL && import.meta.env.VITE_SIGNALR_HUB_URL.trim()) || null;
const hubUrl = (lsBaseApi && `${lsBaseApi}/hubs/company`) || envHubUrl || (envApiBase && `${envApiBase}/hubs/company`) || "https://localhost:7037/hubs/company";
const signalRService = new SignalRService(hubUrl);

export default signalRService;