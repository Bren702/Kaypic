using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace KayPic.Hubs
{
    public class LearningHub : Hub<ILearningHubClient>
    {
        private static readonly HashSet<string> GroupNames = new HashSet<string>();

        private static readonly ConcurrentDictionary<string, List<string>> GroupUsers = new ConcurrentDictionary<string, List<string>>();
        public async Task SendToIndividual(string connectionId, string message)
        {
            // Envoie un message privé à un utilisateur spécifique
            await Clients.Client(connectionId).ReceiveMessage($"Message de l'utilisateur {Context.ConnectionId} : {message}");
        }

        public async Task CreateGroup(string groupName)
        {
            // Vérifie si le groupe n'existe pas déjà
            if (!GroupNames.Contains(groupName))
            {
                // Ajoute le nom du groupe et initialise la liste des utilisateurs dans ce groupe
                GroupNames.Add(groupName);
                GroupUsers[groupName] = new List<string>();
                // Notifie l'appelant que le groupe a été créé avec succès
                await Clients.Caller.Notify($"Le groupe {groupName} a été créé avec succès !");
            }
            else
            {
                // Notifie l'appelant que le groupe existe déjà
                await Clients.Caller.Notify($"Le groupe {groupName} existe déjà.");
            }
        }

        // Méthode pour ajouter un utilisateur à un groupe existant
        public async Task AddUserToGroup(string groupName)
        {
            if (GroupNames.Contains(groupName))
            {
                // Ajoute l'utilisateur au groupe spécifié
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                if (!GroupUsers[groupName].Contains(Context.ConnectionId))
                {
                    // Enregistre l'ID de connexion de l'utilisateur dans le groupe
                    GroupUsers[groupName].Add(Context.ConnectionId);
                }
                // Notifie l'utilisateur qu'il a rejoint le groupe
                await Clients.Caller.Notify($"Vous avez rejoint le groupe {groupName}");
                // Informe les autres utilisateurs du groupe qu'un nouvel utilisateur a rejoint
                await Clients.Group(groupName).ReceiveMessage($"L'utilisateur {Context.ConnectionId} a rejoint le groupe {groupName}");
            }
            else
            {
                // Informe l'utilisateur que le groupe n'existe pas
                await Clients.Caller.Notify($"Le groupe {groupName} n'existe pas.");
            }
        }

        public async Task SendToGroup(string groupName, string message)
        {
            if (GroupNames.Contains(groupName))
            {
                // Envoie un message à tous les membres du groupe spécifié
                await Clients.Group(groupName).ReceiveMessage($"Message pour le groupe {groupName} : {message}");
            }
            else
            {
                // Notifie l'appelant que le groupe n'existe pas
                await Clients.Caller.Notify($"Le groupe {groupName} n'existe pas.");
            }
        }

        public async Task RemoveUserFromGroup(string groupName)
        {
            if (GroupNames.Contains(groupName))
            {
                // Retire l'utilisateur du groupe spécifié
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
                if (GroupUsers[groupName].Contains(Context.ConnectionId))
                {
                    // Supprime l'utilisateur de la liste des membres du groupe
                    GroupUsers[groupName].Remove(Context.ConnectionId);
                }
                // Notifie l'utilisateur qu'il a quitté le groupe
                await Clients.Caller.Notify($"Vous avez quitté le groupe {groupName}");
                // Informe les autres membres du groupe que l'utilisateur est parti
                await Clients.Group(groupName).ReceiveMessage($"L'utilisateur {Context.ConnectionId} a quitté le groupe {groupName}");
            }
            else
            {
                // Notifie l'utilisateur que le groupe n'existe pas
                await Clients.Caller.Notify($"Le groupe {groupName} n'existe pas.");
            }
        }

        // Méthode pour obtenir la liste des utilisateurs dans un groupe et l'envoyer à l'utilisateur
        public async Task GetGroupUsers(string groupName)
        {
            if (GroupUsers.ContainsKey(groupName))
            {
                // Récupère la liste des utilisateurs du groupe et l'envoie à l'appelant
                var users = GroupUsers[groupName];
                await Clients.Caller.ReceiveGroupUsers(groupName, users); // Utilise ReceiveGroupUsers pour envoyer la liste des utilisateurs
            }
            else
            {
                // Notifie l'appelant que le groupe n'existe pas ou est vide
                await Clients.Caller.Notify($"Le groupe {groupName} n'existe pas ou n'a pas d'utilisateurs.");
            }
        }
    }
}
