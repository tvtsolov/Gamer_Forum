using Forum_System.Models;

namespace Forum_System.Helpers
{
    public class AvatarHelper
    {
        const string avatarDir = "wwwroot/images/avatars";


        public string GetAvatar(string username)
        {
            bool commenterHasAvatar = Directory.EnumerateFiles(avatarDir, $"{username}*").Any();

            string commenterAvatarName = "default.jpg";

            if (commenterHasAvatar)
            {
                var directory = new DirectoryInfo(@"wwwroot/images/avatars");
                var fileInfo = directory.GetFiles("*" + username + "*.*");
                commenterAvatarName = fileInfo[0].Name;
            }

            return commenterAvatarName;
        }
    }
}
