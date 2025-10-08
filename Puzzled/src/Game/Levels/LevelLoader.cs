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
                JsonElement layers = root.GetProperty("layers");

                // Get size
                uint width = root.GetProperty("width").GetUInt32();
                uint height = root.GetProperty("height").GetUInt32();
                outWidth = width;
                outHeight = height;

                uint tileSize = (Settings.SpriteSize / Settings.Scale);
                uint tilesX = Assets.TileSheet.Width / tileSize;
                uint tilesY = Assets.TileSheet.Height / tileSize;

                // Tile layer
                {
                    // TODO: Make it load the tile layer based on some layer name? or set a fixed format.
                    JsonElement data = layers[0].GetProperty("data");
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
                            uint id = tileId - 1; // Tiled starts at 1

                            uint col = id % tilesX; // Column
                            uint row = id / tilesX; // Row

                            uvX = col * (Settings.SpriteSize / Settings.Scale);
                            uvY = row * (Settings.SpriteSize / Settings.Scale);
                        }

                        // Note: (width - x) because x is flipped because we .Reverse()
                        Maths.Vector2 position = new Maths.Vector2(((width - 1) - x) * (Settings.SpriteSize), y * (Settings.SpriteSize));
                        Maths.Vector2 size = new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize);

                        outTiles.Add(new Tile(position, size, Assets.GetTexture(Assets.TileSheet, uvX, uvY), (TileType)tileId));
                        next();
                    }
                }

                // Object layer
                {
                    // TODO: ...
                }
            }
        }

    }

}