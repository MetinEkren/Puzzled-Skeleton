using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using System.Text.Json;
using System.Linq;

namespace Puzzled
{

    //////////////////////////////////////////////////////////////////////////////////
    // LevelLoader
    //////////////////////////////////////////////////////////////////////////////////
    public class LevelLoader
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Static methods
        ////////////////////////////////////////////////////////////////////////////////////
        public static void Load(string levelPath, ref List<Tile> outTiles, out uint outWidth, out uint outHeight)
        {
            string json = File.ReadAllText(levelPath);
            using (JsonDocument doc = JsonDocument.Parse(json))
            {
                JsonElement root = doc.RootElement;

                // Access the first layer (currently the only layer)
                JsonElement layers = root.GetProperty("layers");
                JsonElement firstLayer = layers[0];

                // Get size
                uint width = firstLayer.GetProperty("width").GetUInt32();
                uint height = firstLayer.GetProperty("height").GetUInt32();
                outWidth = width;
                outHeight = height;

                uint tileSize = (Settings.SpriteSize / Settings.Scale);
                uint tilesX = Assets.TileSheet.Width / tileSize;
                uint tilesY = Assets.TileSheet.Height / tileSize;

                JsonElement data = firstLayer.GetProperty("data");
                uint x = 0, y = 0;
                foreach (JsonElement tile in data.EnumerateArray().Reverse())
                {
                    // Update position
                    Action next = () =>
                    {
                        x++;

                        if (x == width)
                        {
                            x = 0;
                            y++;
                        }
                    };

                    uint tileId = tile.GetUInt32();
                    if (tileId == 0) // No tile
                    {
                        next();
                        continue;
                    }

                    // Convert tileID to UV
                    uint uvX = 0, uvY = 0;
                    {
                        uint id = (uint)(tileId - 1); // Tiled starts at 1

                        uint col = id % tilesX; // column
                        uint row = id / tilesX; // row

                        uvX = col * (Settings.SpriteSize / Settings.Scale);
                        uvY = row * (Settings.SpriteSize / Settings.Scale);
                    }

                    // Note: (width - x) because x is flipped because we .Reverse()
                    outTiles.Add(new Tile(new Maths.Vector2(((width - 1) - x) * (Settings.SpriteSize), y * (Settings.SpriteSize)), new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize), Assets.GetTexture(Assets.TileSheet, uvX, uvY)));
                    next();
                }
            }
        }

    }

}