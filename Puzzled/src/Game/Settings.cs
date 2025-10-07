using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Puzzled
{

    //////////////////////////////////////////////////////////////////////////////////
    // Assets
    //////////////////////////////////////////////////////////////////////////////////
    public class Settings
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Audio
        ////////////////////////////////////////////////////////////////////////////////////
        private static uint s_MasterVolume = ((Environment.GetEnvironmentVariable("VULKAN_SDK") == null) ? 50u : 1u); // % // TODO: This is for Jorben's PC, ignore
        public static uint MasterVolume { get { return s_MasterVolume; }
            set 
            {
                // Note: Currently all audio files must manually be added... // FUTURE TODO: ...
                s_MasterVolume = value;
                Assets.IntroMusic.Volume = value;
                Assets.MainMenuMusic.Volume = value;
            }
        }
        
        ////////////////////////////////////////////////////////////////////////////////////
        // Renderer
        ////////////////////////////////////////////////////////////////////////////////////
        public const uint Scale = 3;
        public const uint SpriteSize = 16 * Scale; // 16x16 pixels

        ////////////////////////////////////////////////////////////////////////////////////
        // Map
        ////////////////////////////////////////////////////////////////////////////////////
        public const uint ChunkSize = 4; // 4x4 tiles
        public const uint MaxChunks = 100; // 100x100 chunks, this is for stopping checks and crashes when moving outside of uint range, it can easily be increased.

        ////////////////////////////////////////////////////////////////////////////////////
        // Physics // TODO: tweak these
        ////////////////////////////////////////////////////////////////////////////////////
        public const float Gravity = 200.0f * Scale;
        public const float GroundFriction = Gravity;
        public const float PlayerRunningVelocity = 35.0f * Scale;
        public const float PlayerJumpingVelocity = 90.0f * Scale;
        public const float PlayerTerminalVelocity = PlayerJumpingVelocity; // Note: For downwards, so it doesn't keep accelerating

    }

}