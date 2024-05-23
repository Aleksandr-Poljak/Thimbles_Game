using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thimbles_Game
{
    /// <summary>
    /// Класс игрового шарика.
    /// </summary>
    public class Ball : GameObjectAbstact
    {
        private string imagePath = string.Empty;
        public override string ImagePath
        {
            get => imagePath; set => imagePath = value;
        }

        public Ball( ) { }
        public Ball( string imagePath ) :this()
        {
            ImagePath = imagePath;
        }

        public override string ToString()
        {
            return $"Object ball: {ImagePath}";
        }

    }
}
