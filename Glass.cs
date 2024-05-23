using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Navigation;

namespace Thimbles_Game
{
    /// <summary>
    /// Класс игрового стакана.
    /// </summary>
    public  class Glass : GameObjectAbstact , INotifyPropertyChanged
    {
        private int id;
        private string imagePath = string.Empty;  
        private Ball? ball = null;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Id
        {
            get => id; private set => id = value;
        }
        public override string ImagePath 
        { 
            get => imagePath;
            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }     
        public Ball? Ball
        {
            get => ball; private set => ball = value;
        }
        public bool HasBall
        {
            get  { return (Ball is not null ? true : false); }
        }

        public Glass() { }
        public Glass(int id, string imagePath, Ball? ball = null): this()
        { 
            Id = id;
            ImagePath = imagePath;
            Ball = ball;
        }

        /// <summary>
        /// Устанавливает мяч в стакан
        /// </summary>
        /// <param name="ball"></param>
        public void SetBall(Ball ball) => Ball = ball;

        /// <summary>
        /// Возвращает мяч из стакана
        /// </summary>
        /// <returns></returns>
        public Ball? RemoveBall()
        { 
            Ball? temp = Ball;
            Ball = null;
            return temp;
        }

        public override string ToString()
        {
            return $"Object glass: Id- {Id} Ball- {HasBall} Image- {ImagePath}";
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
