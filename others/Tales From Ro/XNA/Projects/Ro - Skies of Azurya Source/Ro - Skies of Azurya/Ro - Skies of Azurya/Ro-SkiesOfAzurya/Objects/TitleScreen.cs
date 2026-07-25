using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SkiesOfAzurya.Objects
{
    [Serializable]
    public class TitleScreen
    {
        public Dictionary<string, Model> TitleModels = new Dictionary<string, Model>();
        public Model TitleModel { get; set; }
        public Model BackgroundModel { get; set; }
        public Texture2D BackgroundTitleTexture { get; set; }
    }
}
