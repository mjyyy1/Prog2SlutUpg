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

        double speed;//Variable för hastigheten på spindlarna
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
            InitializeComponent();//skapar bakgrunden

            GameTimer.Tick += GameEngine;
            GameTimer.Interval = TimeSpan.FromMilliseconds(20);
            BgImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Sprites/Bg400x800.png"));//refererar till bakgrunden
            MyCanvas.Background = BgImage;

            
        }

        private void GameEngine(object sender, EventArgs e)


        {
            ScoreTxt.Content = "Score: " + score;//visar poängen
           

            intervals -= 10;

            if (intervals < 1)
            {
                ImageBrush SpiderImg = new ImageBrush();

                SpiderSkin += 1;

                if (SpiderSkin > 3)
                {
                    
                    SpiderSkin = 1;
                }

                switch (SpiderSkin)//väljer villken färg på spindel som spawnas Just nu finns bara 1 kommer komma mer
                {
                    

                    case 1:
                        SpiderImg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Sprites/SexySpiderRotated.png"));
                        break;
                    case 2:
                        SpiderImg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Sprites/BlueSpider2.png"));
                        break;
                    case 3:
                        SpiderImg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Sprites/NinjaSpooder2.png"));
                        break;

                    default:
                        SpiderImg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Sprites/SexySpiderRotated.png"));   
                        break;


                }

                Rectangle newSpider = new Rectangle// skapar en rektangle med en bild på en spindel
                {
                    Tag = "Spider",
                    Height = 47,
                    Width = 53,
                    Fill = SpiderImg
                };

                Canvas.SetLeft(newSpider, rand.Next(50, 350));//sätter ett random x värde för spindelns spawn position
                Canvas.SetTop(newSpider, 600);//sätter y värdet för spawn positionen

                MyCanvas.Children.Add(newSpider);//visar spindel på canvasen

                intervals = rand.Next(90, 150);//en intervall tills nästa spindel ska spawnas
            }

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {

                if ((string)x.Tag == "Spider")
                {

                    i = rand.Next(-5, 5);

                    Canvas.SetTop(x, Canvas.GetTop(x) - speed);
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - (i * -1));

                }
                if (Canvas.GetTop(x) <10)//lägger till spindeln för att ta bort den när den når toppen och tar bort 1 hp
                {
                    itemRemover.Add(x);
                    Hp -= 1;
                  
                }

            }

            Remover();//kallar på metoden Remover() som tarbort spindlarna

            if (Hp <= 0)//när hp går under 0 så avslutas spelet och restart knappen visas
            {
                gameActive = false;
                GameTimer.Stop();
                RestartBtn.Visibility = Visibility.Visible;//gör så att knapparna visas
                QuitBtn.Visibility = Visibility.Visible;
                              
            }

            

            HpTxt.Content = "Hp: " + Hp;//skriver ut hur många liv man har kvar

        }



        void RBtnClick(object sender, RoutedEventArgs e)//kallar på Restart metoden när man trycker på Restart Knappen
        {
            Restart();

        }
        void QBtnClick(object sender, RoutedEventArgs e)//kallar på Quit metoden när man trycker på Quit Knappen
        {
            Quit();
        }

        
        void SBtnClick(object sender, RoutedEventArgs e)//kallar på GameStart metoden när man trycker på Start Knappen
        {
            GameStart();

        }


        private void AtkEnemy(object sender, MouseButtonEventArgs e)//den här spelar ett ljud och tar bort den aktiva rektrangeln
        {
            if (gameActive)//körs bara om gameActive är true
            {

                if (e.OriginalSource is Rectangle)//kollar om det man trycker på är en Rectangle
                {
                    Rectangle activeRec = (Rectangle)e.OriginalSource;

                    player.Open(new Uri("../../Sprites/Hit.mp3", UriKind.RelativeOrAbsolute));//refererar till ljud filen
                    player.Volume = 0.1;
                    player.Play();//spelar ljudfilen

                    MyCanvas.Children.Remove(activeRec);//tar bort den spindeln man tryckt på

                    score += 1;

                    if (score >= 50)// här ökar hastigheten som spindlarna rör sig efter att score är 50
                    {
                        speed = speed + 0.1;
                    }
                    else// här ökar hastigheten innan score är 50
                    {
                        speed = speed + 0.05;
                    }

                   

                }

            }
        }

        private void GameStart()//startar spelet och resettar alla värden till sina normala värden
        {
            GameTimer.Start();
           
            score = 0;
            intervals = 90;
            gameActive = true;
            Hp = 10;
            speed = 2;
            RestartBtn.Visibility = Visibility.Collapsed;//gör så att knapparna inte syns
            StartBtn.Visibility = Visibility.Collapsed;
            QuitBtn.Visibility = Visibility.Collapsed;
        }

        private void Restart()//startar om spelet
        {
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())//lägger in alla spindlar som är på skärmen i en lista
            {
                itemRemover.Add(x);
            }

            Remover();//kallar på metoden Remover() som tarbort spindlarna

            itemRemover.Clear();

            GameStart();



        }
         
        private void Remover()
        {
            foreach (Rectangle y in itemRemover)//här går man igenom listan med spindlar och tarbort dem
            {
                MyCanvas.Children.Remove(y);
            }
        }
        private void Quit()//stänger av applikationen 
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
