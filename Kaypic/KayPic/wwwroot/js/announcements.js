// announcements.js - SignalR client for real-time announcements

const announcementConnection = new signalR.HubConnectionBuilder()
    .withUrl("/announcementHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

let currentTeamSeasonId = null;

// Start the connection
async function startAnnouncementConnection() {
    try {
        await announcementConnection.start();
        console.log("SignalR Connected to AnnouncementHub.");
    } catch (err) {
        console.error("Error connecting to AnnouncementHub:", err);
        setTimeout(startAnnouncementConnection, 5000);
    }
}

// Reconnect on disconnect
announcementConnection.onclose(async () => {
    console.log("AnnouncementHub connection closed. Reconnecting...");
    await startAnnouncementConnection();
    if (currentTeamSeasonId) {
        await joinTeamSeasonAnnouncementsGroup(currentTeamSeasonId);
    }
});

// Client methods - receive announcements from server
announcementConnection.on("ReceiveNewAnnouncement", (newsId, title, body, authorName, datePosted) => {
    displayNewAnnouncement(newsId, title, body, authorName, datePosted);
});

announcementConnection.on("AnnouncementUpdated", (newsId, title, body, editedAt) => {
    updateAnnouncementDisplay(newsId, title, body, editedAt);
});

announcementConnection.on("AnnouncementDeleted", (newsId) => {
    removeAnnouncementDisplay(newsId);
});

// Server methods - send messages to server
async function joinTeamSeasonAnnouncementsGroup(teamSeasonId) {
    currentTeamSeasonId = teamSeasonId;
    try {
        await announcementConnection.invoke("JoinTeamSeasonAnnouncementsGroup", teamSeasonId);
        console.log(`Joined team season announcements group ${teamSeasonId}`);
    } catch (err) {
        console.error("Error joining announcements group:", err);
    }
}

async function leaveTeamSeasonAnnouncementsGroup(teamSeasonId) {
    try {
        await announcementConnection.invoke("LeaveTeamSeasonAnnouncementsGroup", teamSeasonId);
        console.log(`Left team season announcements group ${teamSeasonId}`);
        currentTeamSeasonId = null;
    } catch (err) {
        console.error("Error leaving announcements group:", err);
    }
}

async function publishAnnouncement(teamSeasonId, authorMpId, title, body, dateStart, dateEnd, mediaUrl) {
    try {
        await announcementConnection.invoke("PublishAnnouncement", teamSeasonId, authorMpId, title, body, dateStart, dateEnd, mediaUrl || "");
        console.log("Announcement published successfully");
    } catch (err) {
        console.error("Error publishing announcement:", err);
        alert("Erreur lors de la publication de l'annonce: " + err.message);
    }
}

async function updateAnnouncement(newsId, title, body) {
    try {
        await announcementConnection.invoke("UpdateAnnouncement", newsId, title, body);
        console.log("Announcement updated successfully");
    } catch (err) {
        console.error("Error updating announcement:", err);
        alert("Erreur lors de la mise à jour de l'annonce: " + err.message);
    }
}

async function deleteAnnouncement(newsId) {
    try {
        await announcementConnection.invoke("DeleteAnnouncement", newsId);
        console.log("Announcement deleted successfully");
    } catch (err) {
        console.error("Error deleting announcement:", err);
        alert("Erreur lors de la suppression de l'annonce: " + err.message);
    }
}

// Helper functions for UI
function displayNewAnnouncement(newsId, title, body, authorName, datePosted) {
    const announcementsContainer = document.getElementById("announcementsContainer");
    if (!announcementsContainer) return;

    const announcementElement = document.createElement("div");
    announcementElement.className = "card mb-3 announcement-card";
    announcementElement.setAttribute("data-news-id", newsId);

    const formattedDate = new Date(datePosted).toLocaleDateString('fr-FR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });

    announcementElement.innerHTML = `
        <div class="card-body">
            <div class="d-flex justify-content-between align-items-start mb-2">
                <h6 class="card-title announcement-title">📣 ${escapeHtml(title)}</h6>
                <small class="text-muted">${escapeHtml(formattedDate)}</small>
            </div>
            <p class="card-text announcement-body">${escapeHtml(body)}</p>
            <small class="text-muted">Par ${escapeHtml(authorName)}</small>
        </div>
    `;

    // Add animation class
    announcementElement.classList.add("announcement-new");
    
    // Insert at the top of the container
    announcementsContainer.insertBefore(announcementElement, announcementsContainer.firstChild);
    
    // Remove animation class after animation completes
    setTimeout(() => {
        announcementElement.classList.remove("announcement-new");
    }, 500);
}

function updateAnnouncementDisplay(newsId, title, body, editedAt) {
    const announcementElement = document.querySelector(`[data-news-id="${newsId}"]`);
    if (!announcementElement) return;

    const titleElement = announcementElement.querySelector(".announcement-title");
    const bodyElement = announcementElement.querySelector(".announcement-body");

    if (titleElement) {
        titleElement.textContent = `📣 ${title}`;
    }

    if (bodyElement) {
        bodyElement.textContent = body;
        
        const editedBadge = document.createElement("span");
        editedBadge.className = "badge bg-secondary ms-2";
        editedBadge.textContent = "Modifié";
        bodyElement.appendChild(editedBadge);
    }

    // Add highlight effect
    announcementElement.classList.add("announcement-updated");
    setTimeout(() => {
        announcementElement.classList.remove("announcement-updated");
    }, 1000);
}

function removeAnnouncementDisplay(newsId) {
    const announcementElement = document.querySelector(`[data-news-id="${newsId}"]`);
    if (!announcementElement) return;

    // Add fade out animation
    announcementElement.style.transition = "opacity 0.5s";
    announcementElement.style.opacity = "0";

    setTimeout(() => {
        announcementElement.remove();
    }, 500);
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// Initialize on page load
document.addEventListener("DOMContentLoaded", () => {
    startAnnouncementConnection();
});
