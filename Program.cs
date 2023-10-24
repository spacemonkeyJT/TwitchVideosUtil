using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace TwitchVideosUtil
{
    internal class Program
    {
        private class UsersResponse
        {
            public User[] data;
        }

        private class User
        {
            public string id;
            public string login;
            public string display_name;
            public string type;
            public string broadcaster_type;
            public string description;
            public string profile_image_url;
            public string offline_image_url;
            public int view_count;
            public DateTime created_at;
        }

        private class VideosResponse
        {
            public Video[] data;
            public Pagination pagination;
        }

        private class Video
        {
            public string id;
            public string stream_id;
            public string user_id;
            public string user_login;
            public string user_name;
            public string title;
            public string description;
            public DateTime created_at;
            public DateTime published_at;
            public string url;
            public string thumbnail_url;
            public string viewable;
            public int view_count;
            public string language;
            public string type;
            public string duration;
        }

        private class Pagination
        {
            public string cursor;
        }

        private const string clientID = "uxj8hdpst8v4lutkr842b3lxz8tp0o";

        private readonly string command;
        private readonly string token;

        static void Main(string[] args)
        {
            try
            {
                if (args.Length > 1)
                {
                    var command = args[0].ToLowerInvariant();
                    var token = args[1];

                    new Program(command, token).Run(args.Skip(2).ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Environment.Exit(1);
            }
        }

        private Program(string command, string token)
        {
            this.command = command;
            this.token = token;
        }

        private void Run(string[] args)
        {
            if (command == "getvideolist")
            {
                if (args.Length == 1)
                {
                    GetVideoList(args[0]);
                }
            }
        }

        private void GetVideoList(string username)
        {
            var user = GetUser(username);
            var videos = GetVideos(user.id);
            var titles = videos.Select(r => $"{r.created_at:yyMMdd} - {r.title}");
            foreach (var title in titles)
            {
                Console.WriteLine(title);
            }
        }

        private List<Video> GetVideos(string userID)
        {
            var result = new List<Video>();
            string cursor = null;
            do
            {
                var res = Get<VideosResponse>($"videos?user_id={userID}&type=archive&after={cursor}");
                result.AddRange(res.data);
                cursor = res.pagination.cursor;
            }
            while (cursor != null);
            return result;
        }

        private User GetUser(string username)
        {
            return Get<UsersResponse>($"users?login={username}").data[0];
        }

        private T Get<T>(string path)
        {
            var req = WebRequest.Create($"https://api.twitch.tv/helix/{path}");
            req.Headers.Add($"Authorization: Bearer {token}");
            req.Headers.Add($"Client-Id: {clientID}");
            req.Method = "GET";
            var res = req.GetResponse();
            using (var reader = new StreamReader(res.GetResponseStream()))
            {
                string json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }
    }
}
