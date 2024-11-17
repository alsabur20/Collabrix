"use strict";

$(document).ready(function () {
    scrollToBottom();

    // SignalR connection setup
    const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
    let projectId = $("#projectId").val() || null;

    // Disable the send button until the connection is established.
    $("#sendButton").prop("disabled", true);

    if (!projectId) {
        console.error("Project ID is missing.");
    }

    // Receive message
    connection.on("ReceiveMessage", function (user, message, sentByUserId) {
        const currentUserId = parseInt($("#currentUserId").val());

        // Create the message element dynamically
        const messageElement = $("<div>").addClass("message");
        if (sentByUserId === currentUserId) {
            messageElement.addClass("message-sent");
        }

        // Message structure
        messageElement.html(`
            <div class="avatar me-2">
                <span class="avatar-title rounded-circle border border-white">
                    ${getInitials(user)}
                </span>
            </div>
            <div class="message-content ${sentByUserId === currentUserId ? 'text-end' : ''}">
                <h6 class="mb-1">${sentByUserId === currentUserId ? 'You' : user}</h6>
                <p>${sanitize(message)}</p>
                <small class="text-muted">${new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</small>
            </div>
        `);

        // Append message and scroll to the bottom
        $("#chat-messages-container").append(messageElement);
        scrollToBottom();
    });

    // Start the connection
    connection.start().then(function () {
        $("#sendButton").prop("disabled", false);
        if (projectId) {
            connection.invoke("JoinProjectGroup", parseInt(projectId)).catch(function (err) {
                console.error("Error joining project group:", err.toString());
            });
        }
    }).catch(function (err) {
        console.error("Error establishing connection:", err.toString());
    });

    // Initialize emojioneArea
    $("#messageInput").emojioneArea({
        pickerPosition: "top",
        filtersPosition: "bottom",
        tones: false,
        autocomplete: false,
        searchPosition: "bottom",
        hidePickerOnBlur: false
    });

    // Send message on Enter key or button click    
    $("#sendButton").on("click", sendMessage);

    $("#messageInput")[0].emojioneArea.on("keydown", function (editor, event) {
        if (event.key === "Enter" && !event.shiftKey) {
            event.preventDefault();
            sendMessage();
        }
    });

    function sendMessage() {
        const message = $("#messageInput")[0].emojioneArea.getText().trim(); // Get the text with emojis

        if (message && projectId) {
            connection.invoke("SendMessage", parseInt(projectId), sanitize(message))
                .then(() => {
                    $("#messageInput")[0].emojioneArea.setText(""); // Clear input
                    scrollToBottom();
                })
                .catch(err => console.error("Error sending message:", err.toString()));
        }
    }

    // Utility functions
    function sanitize(str) {
        const temp = $("<div>").text(str);
        return temp.html();
    }

    function getInitials(name) {
        return name.split(" ").map(n => n[0]).join("");
    }

    function scrollToBottom() {
        const chatContainer = $("#chat-messages-container");
        if (chatContainer.length) {
            chatContainer.scrollTop(chatContainer[0].scrollHeight);
        }
    }
});
