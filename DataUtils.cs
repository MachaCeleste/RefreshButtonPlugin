using BepInEx;
using System.IO;
using UnityEngine;

namespace RefreshButtonPlugin
{
    public class DataUtils
    {
        public static Sprite LoadSprite(string file)
        {
            var path = Path.Combine(Paths.PluginPath, MyPluginInfo.PLUGIN_NAME, "Images", $"{file}.png");
            if (!File.Exists(path))
            {
                Plugin.Logger.LogError($"File {path} could not be found");
                return null;
            }
            byte[] imageData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            if (!texture.LoadImage(imageData))
            {
                Plugin.Logger.LogError("Failed to load image as texture2d");
                return null;
            }
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
    }
}