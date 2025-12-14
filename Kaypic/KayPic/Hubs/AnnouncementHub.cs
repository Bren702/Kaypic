using KayPic.Data;
using KayPic.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace KayPic.Hubs
{
    public interface IAnnouncementHubClient
    {
        Task ReceiveNewAnnouncement(int newsId, string title, string body, string authorName, DateTime datePosted);
        Task AnnouncementUpdated(int newsId, string title, string body, DateTime editedAt);
        Task AnnouncementDeleted(int newsId);
    }

    public class AnnouncementHub : Hub<IAnnouncementHubClient>
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task PublishAnnouncement(int teamSeasonId, int authorMpId, string title, string body, DateTime dateStart, DateTime dateEnd, string mediaUrl)
        {
            // Get the author
            var author = await _context.MessagingPersonas
                .FirstOrDefaultAsync(mp => mp.mp_id == authorMpId && mp.mp_team_season_id == teamSeasonId);

            if (author == null)
            {
                throw new HubException("Author not found in this team season");
            }

            // Create the news announcement
            var news = new News
            {
                news_ts_id = teamSeasonId,
                author_mp_id = authorMpId,
                news_title = title,
                news_body = body,
                news_status = Status.active,
                news_media_category = 'N', // N for news
                news_url = mediaUrl ?? string.Empty,
                date_posted = DateTime.UtcNow,
                date_start = dateStart,
                date_end = dateEnd
            };

            _context.News.Add(news);
            await _context.SaveChangesAsync();

            var authorName = $"{author.mp_fname} {author.mp_lname}";

            // Notify all members of the team season
            await Clients.Group($"teamseason_{teamSeasonId}")
                .ReceiveNewAnnouncement(news.news_id, title, body, authorName, news.date_posted);
        }

        public async Task JoinTeamSeasonAnnouncementsGroup(int teamSeasonId)
        {
            // Add the user to the team season announcements group
            await Groups.AddToGroupAsync(Context.ConnectionId, $"teamseason_{teamSeasonId}");
        }

        public async Task LeaveTeamSeasonAnnouncementsGroup(int teamSeasonId)
        {
            // Remove the user from the team season announcements group
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"teamseason_{teamSeasonId}");
        }

        public async Task UpdateAnnouncement(int newsId, string title, string body)
        {
            var news = await _context.News
                .FirstOrDefaultAsync(n => n.news_id == newsId);

            if (news == null)
            {
                throw new HubException("Announcement not found");
            }

            if (news.news_status != Status.active)
            {
                throw new HubException("Cannot update an inactive announcement");
            }

            // Update the announcement
            news.news_title = title;
            news.news_body = body;
            var editedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Notify all members of the team season
            await Clients.Group($"teamseason_{news.news_ts_id}")
                .AnnouncementUpdated(newsId, title, body, editedAt);
        }

        public async Task DeleteAnnouncement(int newsId)
        {
            var news = await _context.News
                .FirstOrDefaultAsync(n => n.news_id == newsId);

            if (news == null)
            {
                throw new HubException("Announcement not found");
            }

            // Set status to inactive
            news.news_status = Status.inactive;
            await _context.SaveChangesAsync();

            // Notify all members of the team season
            await Clients.Group($"teamseason_{news.news_ts_id}")
                .AnnouncementDeleted(newsId);
        }
    }
}
