// messaging.js - SignalR client for real-time messaging

const messagingConnection = new signalR.HubConnectionBuilder()
    .withUrl("/messagingHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

let currentChatId = null;
let currentMessagingPersonaId = null;
let typingTimeout = null;

// Start the connection
async function startMessagingConnection() {
    try {
        await messagingConnection.start();
        console.log("SignalR Connected to MessagingHub.");
    } catch (err) {
        console.error("Error connecting to MessagingHub:", err);
        setTimeout(startMessagingConnection, 5000);
    }
}

// Reconnect on disconnect
messagingConnection.onclose(async () => {
    console.log("MessagingHub connection closed. Reconnecting...");
    await startMessagingConnection();
    if (currentChatId && currentMessagingPersonaId) {
        await joinChat(currentChatId, currentMessagingPersonaId);
    }
});

// Client methods - receive messages from server
messagingConnection.on("ReceiveChatMessage", (chatId, messageId, senderId, senderName, message, timestamp) => {
    if (chatId === currentChatId) {
        displayMessage(messageId, senderId, senderName, message, timestamp, false);
        scrollToBottom();
    }
});

messagingConnection.on("UserJoinedChat", (chatId, userName) => {
    if (chatId === currentChatId) {
        displaySystemMessage(`${userName} a rejoint le chat`);
    }
});

messagingConnection.on("UserLeftChat", (chatId, userName) => {
    if (chatId === currentChatId) {
        displaySystemMessage(`${userName} a quitté le chat`);
    }
});

messagingConnection.on("TypingIndicator", (chatId, userName, isTyping) => {
    if (chatId === currentChatId) {
        const typingIndicator = document.getElementById("typingIndicator");
        if (typingIndicator) {
            if (isTyping) {
                typingIndicator.textContent = `${userName} est en train d'écrire...`;
                typingIndicator.style.display = "block";
            } else {
                typingIndicator.style.display = "none";
            }
        }
    }
});

messagingConnection.on("MessageDeleted", (chatId, messageId) => {
    if (chatId === currentChatId) {
        const messageElement = document.querySelector(`[data-message-id="${messageId}"]`);
        if (messageElement) {
            const messageTextElement = messageElement.querySelector(".message-text");
            if (messageTextElement) {
                messageTextElement.innerHTML = '<em class="text-muted">Message supprimé</em>';
            }
        }
    }
});

messagingConnection.on("MessageEdited", (chatId, messageId, newMessage, editedAt) => {
    if (chatId === currentChatId) {
        const messageElement = document.querySelector(`[data-message-id="${messageId}"]`);
        if (messageElement) {
            const messageTextElement = messageElement.querySelector(".message-text");
            if (messageTextElement) {
                messageTextElement.textContent = newMessage;
                const editedBadge = '<span class="badge bg-secondary ms-2">Modifié</span>';
                messageTextElement.innerHTML += editedBadge;
            }
        }
    }
});

// Server methods - send messages to server
async function joinChat(chatId, messagingPersonaId) {
    currentChatId = chatId;
    currentMessagingPersonaId = messagingPersonaId;
    try {
        await messagingConnection.invoke("JoinChat", chatId, messagingPersonaId);
        console.log(`Joined chat ${chatId}`);
    } catch (err) {
        console.error("Error joining chat:", err);
    }
}

async function leaveChat(chatId, messagingPersonaId) {
    try {
        await messagingConnection.invoke("LeaveChat", chatId, messagingPersonaId);
        console.log(`Left chat ${chatId}`);
    } catch (err) {
        console.error("Error leaving chat:", err);
    }
}

async function sendMessage(chatId, messagingPersonaId, message) {
    if (!message || message.trim() === "") return;
    
    try {
        await messagingConnection.invoke("SendMessageToChat", chatId, messagingPersonaId, message);
    } catch (err) {
        console.error("Error sending message:", err);
        alert("Erreur lors de l'envoi du message: " + err.message);
    }
}

async function sendTypingIndicator(chatId, messagingPersonaId, isTyping) {
    try {
        await messagingConnection.invoke("SendTypingIndicator", chatId, messagingPersonaId, isTyping);
    } catch (err) {
        console.error("Error sending typing indicator:", err);
    }
}

async function deleteMessage(messageId) {
    try {
        await messagingConnection.invoke("DeleteMessage", messageId);
    } catch (err) {
        console.error("Error deleting message:", err);
        alert("Erreur lors de la suppression du message: " + err.message);
    }
}

async function editMessage(messageId, newMessage) {
    if (!newMessage || newMessage.trim() === "") return;
    
    try {
        await messagingConnection.invoke("EditMessage", messageId, newMessage);
    } catch (err) {
        console.error("Error editing message:", err);
        alert("Erreur lors de la modification du message: " + err.message);
    }
}

// Helper functions for UI
function displayMessage(messageId, senderId, senderName, message, timestamp, isDeleted) {
    const messagesContainer = document.getElementById("messagesContainer");
    if (!messagesContainer) return;

    const messageElement = document.createElement("div");
    messageElement.className = "message mb-2";
    messageElement.setAttribute("data-message-id", messageId);
    messageElement.setAttribute("data-sender-id", senderId);

    const isSelf = senderId === currentMessagingPersonaId;
    const alignClass = isSelf ? "text-end" : "text-start";

    const formattedTime = new Date(timestamp).toLocaleTimeString('fr-FR', { 
        hour: '2-digit', 
        minute: '2-digit' 
    });

    const messageContent = isDeleted 
        ? '<em class="text-muted">Message supprimé</em>'
        : message;

    messageElement.innerHTML = `
        <div class="${alignClass}">
            <strong class="message-sender">${senderName}</strong>
            <small class="text-muted ms-2">${formattedTime}</small>
            <div class="message-text">${messageContent}</div>
        </div>
    `;

    messagesContainer.appendChild(messageElement);
}

function displaySystemMessage(message) {
    const messagesContainer = document.getElementById("messagesContainer");
    if (!messagesContainer) return;

    const messageElement = document.createElement("div");
    messageElement.className = "system-message text-center text-muted my-2";
    messageElement.innerHTML = `<small><em>${message}</em></small>`;

    messagesContainer.appendChild(messageElement);
}

function scrollToBottom() {
    const messagesContainer = document.getElementById("messagesContainer");
    if (messagesContainer) {
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
    }
}

function handleTypingInput() {
    if (!currentChatId || !currentMessagingPersonaId) return;

    // Send typing indicator
    sendTypingIndicator(currentChatId, currentMessagingPersonaId, true);

    // Clear existing timeout
    if (typingTimeout) {
        clearTimeout(typingTimeout);
    }

    // Set timeout to stop typing indicator after 2 seconds of inactivity
    typingTimeout = setTimeout(() => {
        sendTypingIndicator(currentChatId, currentMessagingPersonaId, false);
    }, 2000);
}

// Initialize on page load
document.addEventListener("DOMContentLoaded", () => {
    startMessagingConnection();

    // Setup message input and send button
    const messageInput = document.getElementById("messageInput");
    const sendButton = document.getElementById("sendMessageButton");

    if (messageInput) {
        messageInput.addEventListener("input", handleTypingInput);
        
        messageInput.addEventListener("keypress", (e) => {
            if (e.key === "Enter" && !e.shiftKey) {
                e.preventDefault();
                sendButton?.click();
            }
        });
    }

    if (sendButton) {
        sendButton.addEventListener("click", async () => {
            if (!messageInput) return;
            
            const message = messageInput.value.trim();
            if (message && currentChatId && currentMessagingPersonaId) {
                await sendMessage(currentChatId, currentMessagingPersonaId, message);
                messageInput.value = "";
                
                // Stop typing indicator
                if (typingTimeout) {
                    clearTimeout(typingTimeout);
                }
                await sendTypingIndicator(currentChatId, currentMessagingPersonaId, false);
            }
        });
    }
});
