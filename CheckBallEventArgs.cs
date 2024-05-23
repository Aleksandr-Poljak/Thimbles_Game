using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thimbles_Game
{
    // Класс аргументов для события  EventHandler<CheckBallEventArgs> CheckBall
    // класса UserControl_Thimbles
    public class CheckBallEventArgs: EventArgs
    {
        public bool HasBall {  get; set; }
    }
}
