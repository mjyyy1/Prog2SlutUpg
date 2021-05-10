using System;
using System.Collections.Generic;
using System.Linq;
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

using System.Windows.Threading;

namespace Tap2Kill
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer GameTimer = new DispatcherTimer();

        double speed;
        int intervals;
        Random rand = new Random();

        List<Rectangle> itemRemover = new List<Rectangle>();
        

        ImageBrush BgImage = new ImageBrush();

        int SpiderSkin = 0;
        int i;
        int Hp = 10;

        bool gameActive;

        int score;

        MediaPlayer player = new MediaPlayer();



        public MainWindow()
        {
            InitializeComponent();

            GameTimer.Tick += GameEngine;
            GameTimer.Interval = TimeSpan.FromMilliseconds(20);
            BgImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Sprites/Bg400x800.png"));
            MyCanvas.Background = BgImage;

            
        }

        private void GameEngine(object sender, EventArgs e)
        {
            ScoreTxt.Content = "Score: " + score;
           

            intervals -= 10;

            if (intervals < 1)
            {
                ImageBrush SpiderImg = new ImageBrush();

                SpiderSkin += 1;

                if (SpiderSkin > 5 )
                {
                    SpiderSkin = 1;
                }
                switch (SpiderSkin)//väljer villken färg på spindel som spawnas Just nu finns bara 1 kommer komma mer
                {
                    case 1:
                        SpiderImg.ImageSource = new BitmapImage(new Uri("../../Sprites/SexySpiderRotated.png"));
                        break;
                    case 2:
                        SpiderImg.ImageSource = new BitmapImage(new Uri("../../Sprites/SexySpiderRotated.png"));
                        break;
                    case 3:
                        SpiderImg.ImageSource = new BitmapImage(new Uri("../../Sprites/SexySpiderRotated.png"));
                        break;
                    case 4:
                        SpiderImg.ImageSource = new BitmapImage(new Uri("../../Sprites/SexySpiderRotated.png"));
                        break;
                    case 5:
                        SpiderImg.ImageSource = new BitmapImage(new Uri("../../Sprites/SexySpiderRotated.png"));
                        break;
                }


                Rectangle newSpider = new Rectangle
                {
                    Tag = "Spider",
                    Height = 47,
                    Width = 53,
                    Fill = SpiderImg
                };

                Canvas.SetLeft(newSpider, rand.Next(50, 350));
                Canvas.SetTop(newSpider, 600);

                MyCanvas.Children.Add(newSpider);

                intervals = rand.Next(90, 150);
            }

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {

                if ((string)x.Tag == "Spider")
                {

                    i = rand.Next(-5, 5);

                    Canvas.SetTop(x, Canvas.GetTop(x) - speed);
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - (i * -1));

                }
                if (Canvas.GetTop(x) <10)
                {
                    itemRemover.Add(x);
                    Hp -= 1;
                  
                }

            }

            foreach (Rectangle y in itemRemover)
            {
                MyCanvas.Children.Remove(y);
            }

            if (Hp <= 0)
            {
                gameActive = false;
                GameTimer.Stop();
                RestartBtn.Visibility = Visibility.Visible;
                              
            }

            

            HpTxt.Content = "Hp: " + Hp;

        }



        void RBtnClick(object sender, RoutedEventArgs e)
        {
            Restart();

        }

        void QBtnClick(object sender, RoutedEventArgs e)
        {
            Restart();

        }
        void SBtnClick(object sender, RoutedEventArgs e)
        {
            GameStart();

        }


        private void AtkEnemy(object sender, MouseButtonEventArgs e)
        {
            if (gameActive)
            {

                if (e.OriginalSource is Rectangle)
                {
                    Rectangle activeRec = (Rectangle)e.OriginalSource;

                    player.Open(new Uri("../../Sprites/Hit.mp3", UriKind.RelativeOrAbsolute));
                    player.Volume = 0.1;
                    player.Play();

                    MyCanvas.Children.Remove(activeRec);

                    score += 1;

                    if (score >= 50)
                    {
                        speed = speed + 0.1;
                    }
                    else
                    {
                        speed = speed + 0.05;
                    }

                    
                    Console.WriteLine(speed);

                }

            }
        }

        private void GameStart()
        {
            GameTimer.Start();
           
            score = 0;
            intervals = 90;
            gameActive = true;
            Hp = 10;
            speed = 2;
            RestartBtn.Visibility = Visibility.Collapsed;
            StartBtn.Visibility = Visibility.Collapsed;
            QuitBtn.Visibility = Visibility.Collapsed;
        }

        private void Restart()
        {
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                itemRemover.Add(x);
            }

            foreach (Rectangle y in itemRemover)
            {
                MyCanvas.Children.Remove(y);
            }
            itemRemover.Clear();

            GameStart();



        }
    }
}
