using MelonLoader;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace OhShape_RPC 
{
    public static class BuildInfo
    {
        public const string Name = "OhShape_RPC";          // Name of the Mod.  (MUST BE SET)
        public const string ShortName = "OhShape_RPC";
        public const string Description = "Adds Discord Rich Presence"; // Description for the Mod.  (Set as null if none)
        public const string Author = "joerkig";                 // Author of the Mod.  (Set as null if none)
        public const string Company = null;                     // Company that made the Mod.  (Set as null if none)
        public const string Version = "0.0.1";                  // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = "https://github.com/joerkig/ohshape-discord-rich-presence";                // Download Link for the Mod.  (Set as null if none)
    }
    public class MyMod : MelonMod {
        public Discord.Discord discord;
        public Discord.Activity activity = new Discord.Activity
        {
            Timestamps =
                {
                Start = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
            },
            Assets =
                {
                LargeImage = "icon",
                LargeText = "By joerkig#1337",
            },
            Instance = true,
        };
        string sceneData;

        internal static string songName;
        internal static string songDifficulty;
        internal static long songLength;

        internal static GameSceneController _gscInstance;

        public override void OnApplicationStart() {

            discord = new Discord.Discord(939893035116949534, (System.UInt64)Discord.CreateFlags.Default);

        }
        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {

            if (sceneName != "GameScene") {
                sceneData = "menu";
                var activityManager = discord.GetActivityManager();
                activity.State = "";
                activity.Details = "In Menu";
                //activity.Timestamps.End = 0;
                activityManager.UpdateActivity(activity, (res) => {
                    if (res == Discord.Result.Ok) {
                    }
                });
            } else
            {
                sceneData = "game";
            }

            if (_gscInstance == null)
            {
                _gscInstance = new GameObject(BuildInfo.ShortName).AddComponent<GameSceneController>();
            }
        }
        public override void OnUpdate() {

            // UpdateActivity(discord, "State", "Details");
            if (sceneData == "game")
            {
                var activityManager = discord.GetActivityManager();
                activity.Details = songName;
                activity.State = songDifficulty;
                //activity.Timestamps.End = songLength;
                activityManager.UpdateActivity(activity, (res) => {
                    if (res == Discord.Result.Ok)
                    {
                    }
                });
            }

            discord.RunCallbacks();

        }
        static void UpdateActivityMenu(Discord.Discord discord) {
            MelonLogger.Msg("UpdateActivityMenu");
            var activityManager = discord.GetActivityManager();
            var lobbyManager = discord.GetLobbyManager();
            var activity = new Discord.Activity {
                State = "",
                Details = "Menu",
                Timestamps =
                {
                Start = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
            },
                Assets =
                {
                LargeImage = "icon",
                LargeText = "By Knuckles#4442",
            },
                Instance = true,
            };
            activityManager.UpdateActivity(activity, result => {
            });
        }
    }
}
