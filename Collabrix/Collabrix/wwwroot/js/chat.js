"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
let projectId = document.getElementById("projectId")?.value || null;

// Disable the send button until the connection is established.
document.getElementById("sendButton").disabled = true;

if (!projectId) {
    console.error("Project ID is missing.");
}

// Receive message
connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    li.classList.add("d-block", "message", "message-sent");

    li.innerHTML = `
    <fieldset style="border: none; padding: 0; margin: 0;">
        <legend style="font-weight: bold; margin-bottom: 4px;">${sanitize(user)}</legend>
        <div>${sanitize(message)}</div>
        <span style="font-size: 0.9em; color: gray;">
            at ${new Date().toLocaleString()}
        </span>
    </fieldset>
    `;

    document.getElementById("messagesList").appendChild(li);
    li.scrollIntoView(); // Scroll to the bottom of the chat
});

// Start the connection
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    if (projectId) {
        connection.invoke("JoinProjectGroup", parseInt(projectId)).catch(function (err) {
            console.error("Error joining project group:", err.toString());
        });
    }
}).catch(function (err) {
    console.error("Error establishing connection:", err.toString());
});

// Send message
document.getElementById("sendButton").addEventListener("click", function (event) {
    const message = document.getElementById("messageInput").value;

    if (message.trim() !== "" && projectId) {
        connection.invoke("SendMessage", parseInt(projectId), sanitize(message)).catch(function (err) {
            console.error("Error sending message:", err.toString());
        });
        document.getElementById("messageInput").value = ""; // Clear input after sending
    }
    event.preventDefault();
});

// Function to sanitize inputs to prevent HTML injection
function sanitize(str) {
    var temp = document.createElement("div");
    temp.textContent = str;
    return temp.innerHTML;
}
