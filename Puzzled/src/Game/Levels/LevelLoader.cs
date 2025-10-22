using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

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
        public static void Load(string levelPath, ref List<Tile> outTiles, ref Dictionary<uint, DynamicObject> dynamicObjects, out uint outWidth, out uint outHeight)
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

                            uvX = col * tileSize;
                            uvY = row * tileSize;
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
                    JsonElement data = layers[1].GetProperty("objects");

                    uint x = 0, y = 0;

                    foreach (JsonElement obj in data.EnumerateArray().Reverse())
                    {
                        // Get position
                        x = obj.GetProperty("x").GetUInt32();
                        y = (height * (Settings.SpriteSize / Settings.Scale)) - obj.GetProperty("y").GetUInt32();

                        string objType = obj.GetProperty("type").GetString();

                        // Note: (width - x) because x is flipped because we .Reverse()
                        Maths.Vector2 position = new Maths.Vector2((x * Settings.Scale), (y * Settings.Scale));
                        Maths.Vector2 size = new Maths.Vector2(Settings.SpriteSize, Settings.SpriteSize);

                        uint m_ID;
                        uint m_ConnectionID;
                        switch(objType)
                        {
                            case "Box":
                                m_ID = obj.GetProperty("id").GetUInt32();
                                dynamicObjects.Add(m_ID, new Box(position));
                                break;

                            case "Button":
                                m_ID = obj.GetProperty("id").GetUInt32();
                                m_ConnectionID = obj.GetProperty("properties")[0].GetProperty("value").GetUInt32();
                                dynamicObjects.Add(m_ID, new Button(position, m_ConnectionID));
                                break;

                            case "ButtonDoor":
                                m_ID = obj.GetProperty("id").GetUInt32();
                                dynamicObjects.Add(m_ID, new Door(position, DoorType.ButtonDoor));
                                break;

                            case "DoorKey":
                                m_ID = obj.GetProperty("id").GetUInt32();
                                dynamicObjects.Add(m_ID, new DoorKey(position));
                                break;

                            case "KeyDoor":
                                m_ID = obj.GetProperty("id").GetUInt32();
                                dynamicObjects.Add(m_ID, new Door(position, DoorType.KeyDoor));
                                break;

                            case "Ladder":
                                m_ID = obj.GetProperty("id").GetUInt32();
                                dynamicObjects.Add(m_ID, new Ladder(position));
                                break;

                            case "Lava":
                                m_ID = obj.GetProperty("id").GetUInt32();
                                dynamicObjects.Add(m_ID, new Lava(position));
                                break;

                            case "Spike":
                                m_ID = obj.GetProperty("id").GetUInt32();
                                dynamicObjects.Add(m_ID, new Spike(position));
                                break;

                            default:
                                Logger.Error($"INVALID OBJECT - {objType}");
                                break;

                        }
                    }
                }
            }
        }

    }

}