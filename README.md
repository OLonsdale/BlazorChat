BlazorChat is a minimal real‑time chat sample built with .NET 9: a Blazor WebAssembly client and an ASP.NET Core server using SignalR. 

It demonstrates per‑user identity (stable user Id stored in local storage unless disabled), username updates, and unified colour handling: each user gets a deterministic colour from a shared palette, with the option to choose a preferred colour that syncs to all clients.

Presence is handled via an online/offline users list (inactive users are shown separately so past messages retain the correct name/colour). 

There’s also a “New Session” button for quick multi‑tab testing, and a “Disable local storage” toggle to simulate ephemeral sessions. 

To run locally, open the solution in Rider/VS and start the Server project (it hosts both the API/SignalR hub and the WASM client); then open multiple tabs to chat across sessions.