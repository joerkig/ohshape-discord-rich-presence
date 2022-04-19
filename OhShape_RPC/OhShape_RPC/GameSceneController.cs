using MelonLoader;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace OhShape_RPC
{
    public class GameSceneController : MonoBehaviour
    {
        private void Start() => Load();

        private void Load()
        {
            StartCoroutine(GetSongInformation());
        }

        private IEnumerator GetSongInformation()
        {

            GameInfo gameInfo = Resources.FindObjectsOfTypeAll<GameInfo>().FirstOrDefault();
            MelonLogger.Msg("Trying to set gameInfo");
            if (gameInfo == null)
            {
                MelonLogger.Error("Unable to get game info - quitting!");
                yield break;
            }

            SongManager songManager = FindObjectOfType<SongManager>();
            MelonLogger.Msg("Trying to set songManager");
            if (songManager == null)
            {
                int i = 0;
                while (songManager == null)
                {
                    yield return new WaitForSeconds(0.2f);
                    songManager = FindObjectOfType<SongManager>();
                    MelonLogger.Msg("Retrying to set songManager");
                    i++;

                    if (i > 50)
                    {
                        MelonLogger.Error("Unable to get song manager - quitting!");
                        yield break;
                    }
                }
            }

            AlbumSongs_SO songData = songManager.CurrentSong;
            MelonLogger.Msg("Trying to set songData");
            if (songData == null)
            {
                MelonLogger.Error("Unable to get song data - quitting!");
                yield break;
            }

            // Prepare the data we can output
            string songAuthor = songData.Author;
            string songName = songData.Name;
            string songDifficulty = WallDanceUtils.GetLocalizedText(songData.Levels[songManager.CurrentSongLevel].Difficulty.ToString());
            long songLength = DateTimeOffset.Now.ToUnixTimeSeconds() + ((long)TimeSpan.FromSeconds(songData.AudioTime).TotalSeconds);
            string songAccuracyMultiplier = (1 / WallDanceUtils.CurrentPrecisionMultiplier).ToString("F1");
            string songSpeedMultiplier = WallDanceUtils.CurrentSpeedMultiplier.ToString("F1");
            string songObjectSpeed = Util.ReflectionUtil.GetPrivateField<float>(songManager, "_objectsSpeed").ToString("F0");

            MyMod.songName = songName;
            MyMod.songDifficulty = songDifficulty;
            MyMod.songLength = songLength;

            MelonLogger.Msg("Song info written to disk successfully for song name '" + songName + "' by '" + songAuthor + "'");

            yield return null;
        }
    }
}