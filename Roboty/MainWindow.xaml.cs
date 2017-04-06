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
using System.Threading;
using System.Collections.ObjectModel;
using System.IO;

namespace Roboty 
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        private Robot robot;
        public LineFollower LineF;
        string ip, port = "8000";
        public int countLog;
        private ObservableCollection<Kontener> KontenerLogi = null;
        FileStream doPliku;
        StreamWriter nPlik;
        int licz;




        public MainWindow()
        {
            InitializeComponent();
            robot = new Robot(this);
            this.LineF = new LineFollower();
            SliderDisable();
            robot.zs = 20;
            sliderP.Value = 3.22;
            sliderD.Value = 2.04;
            sliderT.Value = 700;
            countLog = 1;
            KontenerLogi = new ObservableCollection<Kontener>();
            lstPersons.ItemsSource = KontenerLogi;

            doPliku= new FileStream("C:\\Users\\" + Environment.UserName + "\\Desktop\\Logi\\Logi " + DateTime.Now.ToString("HH_mm d/M/yyyy") + ".txt", FileMode.Create, FileAccess.Write);
            nPlik= new StreamWriter(doPliku);

            licz = 0;

            Logi("Uruchomiono aplikacje");

        }

        private void button_Click(object sender, RoutedEventArgs e)//połącz
        {

            Thread oThread = new Thread(new ParameterizedThreadStart(robot.Work));
            oThread.Start(ip + ":" + port);

            Logi("Nawiązywanie połączenia z robotem");

        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e) // ip
        {
            ip = textBox.Text;
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e) // port
        {
            port = textBox1.Text;
        }

        #region ProgressBar_ValueChanged

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           
        }
        private void ProgressBarSensor1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
        }
        private void ProgressBarSensor2(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
        private void ProgressBarSensor3(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
        private void ProgressBarSensor4(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
        private void ProgressBarSensor5(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
        }
        #endregion

        private void button1_Click(object sender, RoutedEventArgs e) // rozłącz
        {
            if (robot.Polaczono == true)
            {
                Logi("Rozłączono z robotem");
                robot.Disc();
                nPlik.Close();
                doPliku.Close();
            }
            else
            {
                Logi("Musisz być połączonym aby rozłączyć");
            }
        }

        private void sliderL_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {      
            robot.engineSL = Convert.ToSByte(sliderL.Value);
        }

        private void sliderR_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {  
            robot.engineSR = Convert.ToSByte(sliderR.Value);
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            robot.ChangedControlType = true;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)//sliders
        {
            robot.ControlType = 1;
            LineF.flag = false;
            SliderDisable();
            SliderEnable();
            Logi("Zmieniono tryb sterowania na slidery");
            //PBS5.Foreground = Brushes.
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)//LF
        {
            robot.ControlType = 2;
            LineF.flag = true;
            SliderDisable();
            SliderEnableLF();
            Logi("Zmieniono tryb sterowania na Line Follower");
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)//klawiatura
        {
            robot.ControlType = 3;
            LineF.flag = false;
            SliderDisable();
            SliderEnableKey();
            Logi("Zmieniono tryb sterowania na klawiature");
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)//brak
        {
            robot.ControlType = 0;
            LineF.flag = false;
            SliderDisable();
            Logi("Wyłączono możliwość sterowania robotem");
        }

        public void UI(UInt16[] temp)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action<UInt16[]>((temp2) =>
                {

                    // wpisanie wartości czujników do texbox

                    textSensor1.Text = Convert.ToString(temp2[1]);
                    textSensor2.Text = Convert.ToString(temp2[2]);
                    textSensor3.Text = Convert.ToString(temp2[3]);
                    textSensor4.Text = Convert.ToString(temp2[4]);
                    textSensor5.Text = Convert.ToString(temp2[5]);
                    // ustawienie progresbar
                    Battery.Value = temp2[0];

                    double akt = temp2[0];
                    byte R = 0, G = 255;
                    
                    try
                    {
                        if (licz == 0)
                        {
                            textBattery.Text = Convert.ToString(temp2[0] + " mV");
                            if (akt > 4700)
                            {
                                if (akt > 5100)
                                {
                                    R = 0;
                                    G = 255;
                                }
                                else
                                {
                                    R = Convert.ToByte(((255 - (akt - 4700)) * 1.568));
                                    G = 255;
                                }
                            }

                            else if (akt < 4700)
                            {
                                if (akt < 4300)
                                {
                                    R = 255;
                                    G = 0;
                                }
                                else
                                {
                                    R = 255;
                                    G = Convert.ToByte(((255 - (4700 - akt)) * 1.568));
                                }
                            }
                            else
                            {
                                R = 255;
                                G = 255;
                            }
                        }
                        else
                        {
                            licz++;
                            if(licz==500)
                            {
                                licz = 0;
                            }
                        }

                    }
                    catch
                    {
                        R = 255;
                        G = 255;
                    }
                    
                    Battery.Foreground= new SolidColorBrush(Color.FromArgb(255, R , G , 0));
                    PBS1.Value = temp2[1];
                    PBS2.Value = temp2[2];
                    PBS3.Value = temp2[3];
                    PBS4.Value = temp2[4];
                    PBS5.Value = temp2[5];
                    

                }), new Object[] {temp});
            }
            catch (Exception)
            {
                Console.WriteLine("exc UI");
            }
        }
  
        private void SliderEnable()
        {      
            sliderL.IsEnabled = true;
            sliderR.IsEnabled = true;
        }

        private void SliderEnableLF()
        {
            sliderP.IsEnabled = true;
            sliderD.IsEnabled = true;
            sliderV.IsEnabled = true;
            sliderT.IsEnabled = true;
        }

        private void SliderEnableKey()
        {
            sliderFactor.IsEnabled = true;
        }

        private void SliderDisable()
        {
            sliderL.IsEnabled = false;
            sliderR.IsEnabled = false;
            sliderFactor.IsEnabled = false;
            sliderP.IsEnabled = false;
            sliderD.IsEnabled = false;
            sliderV.IsEnabled = false;
            sliderT.IsEnabled = false;
            sliderFactor.Value = 0;
            sliderL.Value = 0;
            sliderR.Value = 0;
        }

        private void sliderP_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textBlockP.Text = Convert.ToString(sliderP.Value);
            LineF.P = sliderP.Value;
        }

        private void sliderD_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textBlockD.Text = Convert.ToString(sliderD.Value);
            LineF.D = sliderP.Value;
        }

        private void sliderV_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textBlockV.Text = Convert.ToString(sliderV.Value);
            LineF.speed = Convert.ToSByte(sliderV.Value);
        }

        private void sliderT_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textBlockT.Text = Convert.ToString(sliderT.Value);
            LineF.treshold = Convert.ToInt32(sliderT.Value);
        }

        private void buttonLF_Click(object sender, RoutedEventArgs e)
        {
            if (robot.ControlType == 2) LineF.flag = true;
            Logi("Uruchomiono tryb Line Follower");
        }

        private void lstPersons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }


        public void Logi(string tekst)
        {
            Dispatcher.BeginInvoke(new Action<string>((temp2) =>
            {
                nPlik.WriteLine(countLog + "\t" + DateTime.Now.ToString("HH:mm:ss d/M/yyyy") + "\t" + temp2);
                nPlik.Flush();
                KontenerLogi.Insert(0, new Kontener(countLog, DateTime.Now, temp2));
                countLog++;
            }), new Object[] { tekst });
        }

        public void UISend(string tekst)
        {
            Dispatcher.BeginInvoke(new Action<string>((temp2) =>
            {
                textBlockRamkaWyslana.Text = temp2;
            }), new Object[] { tekst });
        }
        #region KEY KEY
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W && robot.keyW == false) robot.keyW = true;
            else if (e.Key == Key.A && robot.keyA == false) robot.keyA = true;
            else if (e.Key == Key.S && robot.keyD == false) robot.keyS = true;
            else if (e.Key == Key.D && robot.keyD == false) robot.keyD = true;

        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W && robot.keyW == true) robot.keyW = false;
            else if (e.Key == Key.A && robot.keyA == true) robot.keyA = false;
            else if (e.Key == Key.S && robot.keyS == true) robot.keyS = false;
            else if (e.Key == Key.D && robot.keyD == true) robot.keyD = false;
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            { 
                sliderP.Focus();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                sliderP.Focus();
            }
        }

        #endregion

        private void sliderFactor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            robot.factorKey = sliderFactor.Value;
        }

        public void UIRecive(string tekst)
        {
            Dispatcher.BeginInvoke(new Action<string>((temp2) =>
            {
                textBlockRamkaOdebrana.Text = temp2;
            }), new Object[] { tekst });
        }

    }
}