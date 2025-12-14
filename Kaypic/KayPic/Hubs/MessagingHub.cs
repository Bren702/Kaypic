using KayPic.Data;
using KayPic.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace KayPic.Hubs
{
    public interface IMessagingHubClient
    {
        Task ReceiveChatMessage(int chatId, int messageId, int senderId, string senderName, string message, DateTime timestamp);
        Task UserJoinedChat(int chatId, string userName);
        Task UserLeftChat(int chatId, string userName);
        Task TypingIndicator(int chatId, string userName, bool isTyping);
        Task MessageDeleted(int chatId, int messageId);
        Task MessageEdited(int chatId, int messageId, string newMessage, DateTime editedAt);
    }

    public class MessagingHub : Hub<IMessagingHubClient>
    {
        private readonly ApplicationDbContext _context;

        public MessagingHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessageToChat(int chatId, int messagingPersonaId, string message)
        {
            // Get the MessagingChatPersona (the user's association with the chat)
            var chatPersona = await _context.MessagingChatPersonas
                .Include(mcp => mcp.mcp_mp)
                .FirstOrDefaultAsync(mcp => mcp.mcp_mc_id == chatId && mcp.mcp_mp_id == messagingPersonaId);

            if (chatPersona == null)
            {
                throw new HubException("User is not a member of this chat");
            }

            // Get team season id from the chat persona
            var teamSeasonId = chatPersona.mcp_ts_id;

            // Create the message
            var newMessage = new MessagingChatPersonaMessage
            {
                mcpm_mcp_id = chatPersona.mcp_id,
                mcpm_ts_id = teamSeasonId,
                mcpm_message = message,
                is_deleted = false,
                created_at = DateTime.UtcNow,
                edited_at = DateTime.UtcNow
            };

            _context.MessagingChatPersonaMessages.Add(newMessage);
            await _context.SaveChangesAsync();

            // Get sender name
            var senderName = $"{chatPersona.mcp_mp.mp_fname} {chatPersona.mcp_mp.mp_lname}";

            // Notify all members of the chat
            await Clients.Group($"chat_{chatId}")
                .ReceiveChatMessage(chatId, newMessage.mcpm_id, messagingPersonaId, senderName, message, newMessage.created_at);
        }

        public async Task JoinChat(int chatId, int messagingPersonaId)
        {
            // Verify the user is a member of this chat
            var chatPersona = await _context.MessagingChatPersonas
                .Include(mcp => mcp.mcp_mp)
                .FirstOrDefaultAsync(mcp => mcp.mcp_mc_id == chatId && mcp.mcp_mp_id == messagingPersonaId);

            if (chatPersona == null)
            {
                throw new HubException("User is not a member of this chat");
            }

            // Add to SignalR group
            await Groups.AddToGroupAsync(Context.ConnectionId, $"chat_{chatId}");

            // Notify other members
            var userName = $"{chatPersona.mcp_mp.mp_fname} {chatPersona.mcp_mp.mp_lname}";
            await Clients.GroupExcept($"chat_{chatId}", Context.ConnectionId)
                .UserJoinedChat(chatId, userName);
        }

        public async Task LeaveChat(int chatId, int messagingPersonaId)
        {
            // Get the user's chat persona
            var chatPersona = await _context.MessagingChatPersonas
                .Include(mcp => mcp.mcp_mp)
                .FirstOrDefaultAsync(mcp => mcp.mcp_mc_id == chatId && mcp.mcp_mp_id == messagingPersonaId);

            if (chatPersona == null)
            {
                throw new HubException("User is not a member of this chat");
            }

            // Remove from SignalR group
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"chat_{chatId}");

            // Notify other members
            var userName = $"{chatPersona.mcp_mp.mp_fname} {chatPersona.mcp_mp.mp_lname}";
            await Clients.Group($"chat_{chatId}")
                .UserLeftChat(chatId, userName);
        }

        public async Task SendTypingIndicator(int chatId, int messagingPersonaId, bool isTyping)
        {
            // Get the user's chat persona
            var chatPersona = await _context.MessagingChatPersonas
                .Include(mcp => mcp.mcp_mp)
                .FirstOrDefaultAsync(mcp => mcp.mcp_mc_id == chatId && mcp.mcp_mp_id == messagingPersonaId);

            if (chatPersona == null)
            {
                throw new HubException("User is not a member of this chat");
            }

            var userName = $"{chatPersona.mcp_mp.mp_fname} {chatPersona.mcp_mp.mp_lname}";

            // Notify other members (not the sender)
            await Clients.GroupExcept($"chat_{chatId}", Context.ConnectionId)
                .TypingIndicator(chatId, userName, isTyping);
        }

        public async Task DeleteMessage(int messageId)
        {
            var message = await _context.MessagingChatPersonaMessages
                .Include(m => m.mcpm_mcp)
                .FirstOrDefaultAsync(m => m.mcpm_id == messageId);

            if (message == null)
            {
                throw new HubException("Message not found");
            }

            // Mark as deleted
            message.is_deleted = true;
            message.edited_at = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Notify all members of the chat
            var chatId = message.mcpm_mcp.mcp_mc_id;
            await Clients.Group($"chat_{chatId}")
                .MessageDeleted(chatId, messageId);
        }

        public async Task EditMessage(int messageId, string newMessage)
        {
            var message = await _context.MessagingChatPersonaMessages
                .Include(m => m.mcpm_mcp)
                .FirstOrDefaultAsync(m => m.mcpm_id == messageId);

            if (message == null)
            {
                throw new HubException("Message not found");
            }

            if (message.is_deleted)
            {
                throw new HubException("Cannot edit a deleted message");
            }

            // Update the message
            message.mcpm_message = newMessage;
            message.edited_at = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Notify all members of the chat
            var chatId = message.mcpm_mcp.mcp_mc_id;
            await Clients.Group($"chat_{chatId}")
                .MessageEdited(chatId, messageId, newMessage, message.edited_at);
        }
    }
}
