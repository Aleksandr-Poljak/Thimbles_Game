using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thimbles_Game
{
    /// <summary>
    /// Абстрактный класс игровых объектов.
    /// </summary>
    public abstract class GameObjectAbstact
    {
        abstract public  string ImagePath {  get; set; }
    }
}
