using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Thimbles_Game
{
    /// <summary>
    /// Логика взаимодействия для UserControl_Thimbles.xaml
    /// </summary>
    public partial class UserControl_Thimbles : UserControl
    {
        // Список игровых стаканов
        private List<Glass> _glasses = new();
        
        //Флаг указывает на то, перемешиваются ли стаканы в данный момент времени.
        private bool isMoving = false;
        public bool IsMoving { get => isMoving; private set => isMoving = value; }

        // Флаг укзывает на то ,перемешены и стаканы. Если хотя бы один был поднят, флаг 
        // устанавливается как False.
        private bool isStirred = false;
        public bool  IsStirred { get => isStirred; private set => isStirred = value; }

        // Событие возникающие при первом поднятии стакана ,когда стаканы перемешаны.
        // Отправляет true если ползователь угадал стакан в котором мяч, иначе false.
        private event EventHandler<CheckBallEventArgs>? _checkBall;
        public event EventHandler<CheckBallEventArgs> CheckBall
        {
            add => _checkBall = value;
            remove => _checkBall = value;
        }


        public UserControl_Thimbles()
        {
            InitializeComponent();
            _initGameObjects("Resources/glass.png", "Resources/ball.png");
            _initGameImages();
        }

        /// <summary>
        /// Инициализирует игровые стаканы и мячик
        /// </summary>
        /// <param name="glassImagePath">Путь к изображению стакана</param>
        /// <param name="ballImagePath">Путь к изображению мяча</param>
        private void _initGameObjects(string glassImagePath, string ballImagePath)
        {
            Ball b = new Ball(ballImagePath);
            for (int i = 0; i < 3; i++)
            {
                Glass glass = new Glass(i, glassImagePath);
                if (i == 0) glass.SetBall(b);             
                _glasses.Add(glass);          
            }
        }

        /// <summary>
        /// Устаналвивает привязку изображений стаканов с файломами располженных в объектах стаканов.
        /// В свойстве ImagePath.
        /// </summary>
        private void _initGameImages()
        {
            //Изображение стаканов устанавливается привязкой к свойству объектов-стаканов.
            // (Размещение на гриде выполнено в кода xaml)
            Image_Glass0.SetBinding(Image.SourceProperty, 
                new Binding() 
                { 
                    Source = _glasses[0], Path= new PropertyPath("ImagePath"), Converter= new ImageConverter() 
                }
                );
            Image_Glass1.SetBinding(Image.SourceProperty,
                new Binding()
                {
                    Source = _glasses[1],
                    Path = new PropertyPath("ImagePath"),
                    Converter = new ImageConverter()
                }
                );
            Image_Glass2.SetBinding(Image.SourceProperty,
                new Binding()
                {
                    Source = _glasses[2],
                    Path = new PropertyPath("ImagePath"),
                    Converter = new ImageConverter()
                }
                );
        }

        /// <summary>
        /// Перемещает изображение стакана в центральную позицию сверху. 
        /// Визуально уменьшает стакан (эффект отдаления).
        /// </summary>
        /// <param name="glass">Объект изображения стакана</param>
        private void _putGlassCenterUp(Image glass)
        {
            glass.Margin = new Thickness(8, 8, 8, 8);
            Grid.SetRow(glass, 0);
            Grid.SetColumn(glass, 2);
        }

        /// <summary>
        /// Перемещает изображение стакана в крайнюю левую позицию.
        /// <param name="glass">Объект изображения стакана</param>
        private void _putGlassLeft(Image glass)
        {
            glass.Margin = new Thickness(3, 3, 3, 3);
            Grid.SetRow(glass, 1);
            Grid.SetColumn(glass, 3);
        }

        /// <summary>
        /// Перемещает изображение стакана в центральную позицию.
        /// <param name="glass">Объект изображения стакана</param>
        private void _putGlassCenter(Image glass)
        {
            glass.Margin = new Thickness(3, 3, 3, 3);
            Grid.SetRow(glass, 1);
            Grid.SetColumn(glass, 2);
        }

        /// <summary>
        /// Перемещает изображение стакана в крайнюю правую позицию.
        /// <param name="glass">Объект изображения стакана</param>
        private void _putGlassRight(Image glass)
        {
            glass.Margin = new Thickness(3, 3, 3, 3);
            Grid.SetRow(glass, 1);
            Grid.SetColumn(glass, 1);
        }

        /// <summary>
        /// Визуально перемещает картинки со стаканами.
        /// </summary>
        /// <param name="millisecondsDelay">Задержка при перемещнии стакана</param>
        /// <returns></returns>
        private async Task _stirImageGlassesAnimation(int millisecondsDelay = 1000)
        {
            // Гаранитрует, что стаканы на моменет перемещния не будут подняты и пользователем и
            // .мяча не будет видно
            if (Grid.GetRow(Image_Glass0) == 0) _DownGlass(Image_Glass0);
            if (Grid.GetRow(Image_Glass1) == 0) _DownGlass(Image_Glass1);
            if (Grid.GetRow(Image_Glass2) == 0) _DownGlass(Image_Glass2);

            _putGlassCenterUp(Image_Glass1);
            await Task.Delay(millisecondsDelay);
            _putGlassCenter(Image_Glass2);
            await Task.Delay(millisecondsDelay); ;
            _putGlassLeft(Image_Glass0);
            await Task.Delay(millisecondsDelay);
            _putGlassRight(Image_Glass1);
            await Task.Delay(millisecondsDelay);
            _putGlassCenterUp(Image_Glass2);
            await Task.Delay(millisecondsDelay);
            _putGlassCenter(Image_Glass1);
            await Task.Delay(millisecondsDelay);
            _putGlassRight(Image_Glass0);
            await Task.Delay(millisecondsDelay);
            _putGlassLeft(Image_Glass2);

        }

        /// <summary>
        /// Перемещает объект мяча в выбранный случайно объект стакана
        /// </summary>
        private void _stirBall()
        {
            Ball? ball = null;
            //Извлекает мяч из объекта стакана
            foreach (Glass glass in _glasses)
            {
                if(glass.HasBall)
                {
                    ball = glass.RemoveBall();
                    break;
                }
            }
            //Помещает мяч в случайный объект стакана.
            if (ball is not null) _glasses[RandomNumberGenerator.GetInt32(_glasses.Count)].SetBall(ball);
        }

        /// <summary>
        /// Поднимает стакан вверхнюю позицию и отображает мяч, если он есть в объекте стакана.
        /// Опускате стакан и удаляет изображение мяча
        /// </summary>
        /// <param name="imageGlass"></param>
        /// <returns></returns>
        private async Task _UpAndDownGlass(Image imageGlass)
        {
            
            _upGlass(imageGlass);
            await Task.Delay(1000);
            _DownGlass(imageGlass);
        }

        /// <summary>
        /// Поднмает стакан вверх и отобржает мяч,если он есть внутри объекта-стакана.
        /// </summary>
        /// <param name="imageGlass">
        /// Объект изображения стакана с установленной привязкой к объекту стакана
        /// </param>
        /// <returns></returns>
        private void _upGlass (Image imageGlass)
        {
            Image imageBall = new();  
            // Текущее расположение стакана.
            int row = Grid.GetRow(imageGlass);
            int colum = Grid.GetColumn(imageGlass);

            // Смещение стакана вверх (на одну строку выше)
            Grid.SetRow(imageGlass, row - 1);

            // Получение объекта стакана из привязки изображения стакана
            Glass glass = (Glass)(BindingOperations.GetBinding(imageGlass, Image.SourceProperty).Source);
            // Отрисовка мяча, если он есть в объекте стакана, на той позции ,в которой был стакан
            if (glass.HasBall && glass.Ball is not null)
            {
                int th = 13; // Отступы внутри квадрата грида - для уменьшения размера.
                imageBall.Source = new BitmapImage(new Uri(glass.Ball.ImagePath, UriKind.RelativeOrAbsolute));
                imageBall.Margin = new Thickness(th);
                Grid.SetRow(imageBall, row);
                Grid.SetColumn(imageBall, colum);
                Grid_TableSpace.Children.Add(imageBall);

                // Вызво метода генерации осбытия, сообщающий о том ,что стакан был поднят и стакан содержал в себе мяч.
                _notifyBall(true);

            }
            else { _notifyBall(false); }          
        }

        /// <summary>
        /// Опускает стакан в нижнюю позицию, если стакан поднят. Удаляет объект мяча в нижней позиции
        /// </summary>
        /// <param name="imageGlass">Объект изображения стакана</param>
        private void _DownGlass(Image imageGlass)
        {
            // Если стакан поднят вверхнюю позицию
            if(Grid.GetRow(imageGlass) == 0)
            {
                int column = Grid.GetColumn(imageGlass);
                int rowPos = Grid.GetRow(imageGlass) + 1;

                // Объект мяча для удаления,если он на позиции на которую нужно опутстить стакан
                Image? imgDelete = null; 

                // Поиск объектов на позиции на которую нужно переместить стакан
                foreach (var obj in Grid_TableSpace.Children)
                {
                    if ((obj is Image img) && 
                        Grid.GetColumn(img) == column && Grid.GetRow(img) == rowPos) 
                        imgDelete = img;
                }
                
                Grid.SetRow(imageGlass, rowPos);
                if (imgDelete is not null) Grid_TableSpace.Children.Remove(imgDelete);

            }
            
        }
        /// <summary>
        /// Обработчик события клика мышкой по стакану.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _MouseLeftButtonDownClick_RaiseGlass(object sender, MouseButtonEventArgs e)
        {
            Image imageGlass = (Image)sender;
            _UpAndDownGlass(imageGlass);

            IsStirred = false;
        }

        /// <summary>
        /// Инициирует событие извещающие о первом поднятии , если  стаканы перемешены и 
        /// не поднимались, и наличии в нем мяча
        /// </summary>
        /// <param name="hasBall">Флаг наличия мяча в стакане</param>
        private void _notifyBall(bool hasBall)
        {
            if (IsStirred == true)
            {
                CheckBallEventArgs e = new();
                e.HasBall = hasBall;
                
                _checkBall?.Invoke(this, e);
            }
            
        }

        /// <summary>
        /// Перемещает стаканы и мячик внутри.
        /// </summary>
        /// <param name="Number">Количество перемещений</param>
        /// <param name="millisecondsDelay">Задержка между перемещением стаканов</param>
        public async Task Stir(int Number = 1, int millisecondsDelay = 1000)
        {
            IsMoving = true;
            
            for (int i = 0; i < Number; i++) 
            {
                _stirBall();
                await _stirImageGlassesAnimation(millisecondsDelay); 
            }

            IsMoving = false;
            IsStirred = true;
        }
        
    }
}
