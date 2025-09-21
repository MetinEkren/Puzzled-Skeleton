using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Diagnostics;

namespace Puzzled
{

    ////////////////////////////////////////////////////////////////////////////////////
    // Animation
    ////////////////////////////////////////////////////////////////////////////////////
    public class Animation
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // Constructor & Destructor
        ////////////////////////////////////////////////////////////////////////////////////
        public Animation(Texture spriteSheet, uint singleSpriteWidth, float advanceTime)
        {
            m_AdvanceTime = advanceTime;

            Debug.Assert((spriteSheet.Width % singleSpriteWidth == 0), "The spritesheet must be divisible by the singleSpriteWidth.");
            uint spriteCount = spriteSheet.Width / singleSpriteWidth;

            m_Sprites = new List<CroppedTexture>((int)spriteCount);

            // Create cropped textures for each sprite
            for (uint i = 0; i < spriteCount; i++)
                m_Sprites.Add(new CroppedTexture(spriteSheet, new UV(i * singleSpriteWidth, 0, singleSpriteWidth, spriteSheet.Height)));
        }
        ~Animation()
        {
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Methods
        ////////////////////////////////////////////////////////////////////////////////////
        public void Update(float deltaTime)
        {
            m_Timer += deltaTime;

            if (m_Timer >= m_AdvanceTime)
            {
                m_CurrentSprite++;
                if (m_CurrentSprite == m_Sprites.Count)
                    m_CurrentSprite = 0;

                m_Timer = 0.0f;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////
        // Getters
        ////////////////////////////////////////////////////////////////////////////////////
        public uint GetCurrentSpriteID() {  return m_CurrentSprite; }
        public CroppedTexture GetCurrentTexture() {  return m_Sprites[(int)m_CurrentSprite]; }

        ////////////////////////////////////////////////////////////////////////////////////
        // Variables
        ////////////////////////////////////////////////////////////////////////////////////
        private List<CroppedTexture> m_Sprites;
        private float m_AdvanceTime;
        private float m_Timer = 0.0f;
        private uint m_CurrentSprite = 0;

    }

}
