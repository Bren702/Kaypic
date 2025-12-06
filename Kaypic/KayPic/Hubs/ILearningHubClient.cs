namespace KayPic.Hubs
{
    public interface ILearningHubClient
    {
        Task ReceiveMessage(string message);
        Task Notify(string notification);
        Task ReceiveGroupUsers(string groupName, List<string> users);
    }
}
