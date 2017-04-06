using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;



//--------------------------------TODO-------------------------------
//sterowanie klawiaturą
//gui







namespace Roboty
{
    public class Robot : LineFollower
    {
        #region zmienne
        private MainWindow window;

        public UInt16[] sensor { get; set; } = new UInt16[6];
        private UInt16[] sensor2 = new UInt16[6];
        public sbyte engineSL { get; set; }
        public sbyte engineSR { get; set; }
        public sbyte engineKL { get; set; }
        public sbyte engineKR { get; set; }
        public byte led1 { get; set; }
        public byte led2 { get; set; }
        public byte ControlType { get; set; }
        public bool ChangedControlType { get; set; }
        private Client client;
        public bool Polaczono { get; set; }
        public double factorKey { get; set; }

        public bool keyW { get; set; }
        public bool keyS { get; set; }
        public bool keyA { get; set; }
        public bool keyD { get; set; }

        //sbyte engineL, engineR;
        private sbyte[] engine = new sbyte[2];

        //private string addres;
        int id;
        public byte zs { get; set; }

        #endregion

        public Robot(MainWindow window)
        {
            
            this.window = window;
            engine[0] = 0;
            engine[1] = 0;
            led1 = 1;
            led2 = 1;
            zs = 0;
            Polaczono = false;
            
        }

        public void Work(object addres)
        {
            client = new Client(addres); // utworznie obiektu oraz polaczenie z robotem


            Console.Write(client.flag);

            if( client.flag == 1)
            {
                Polaczono = false;
                window.Logi("Błędny adres IP lub Port");
            }
            else if (client.flag == 2)
            {
                Polaczono = true;
                window.Logi("Połączono z robotem");
            }
            else if (client.flag == 3)
            {
                Polaczono = false;
                window.Logi("Nieudane połączenie z robotem");
            }

            if (client.flag == 2)
            {
                string temp = addres.ToString();
                temp = temp.Substring(10, 1);
                id = int.Parse(temp);

                Thread.Sleep(1000);

                string frame;
                while (true)
                {
                    engine = Sterowanie();
                    window.UISend(client.Send(led1, led2, engine[0], engine[1]));

                    Thread.Sleep(40);
                    frame= client.recive();
                    sensor = client.Decode(frame);

                    window.UIRecive(frame);

                    window.UI(sensor);
                }
            }

        }

        private sbyte[] Sterowanie()
        {
            
            sbyte[] temp = new sbyte[2];
            if (ChangedControlType)
            {
                // czyszczenie LF
            }
            else
            {

            }

            //0-brak
            //1-sliders
            //2-LF
            //3-klawiatura
            switch (ControlType)
                {
                    case 0:
                        temp[0] = 0;
                        temp[1] = 0;

                        break;
                    case 1:
                        temp[0] = engineSL;
                        temp[1] = engineSR;

                    break;
                    case 2://wywolanie LF
                    //Console.WriteLine("LF1");
                    //if (flag == true)
                      //  {
                            sensor2 = sensor;
                            temp = window.LineF.Follow(sensor2);
                       // }
                    break;
                    case 3://klawiatura
                        if(keyW==true && keyA == false  && keyS == false && keyD == false)
                    {
                        temp[0] = Convert.ToSByte(factorKey * 2);
                        temp[1] = Convert.ToSByte(factorKey * 2); 
                    }
                        else if(keyW == false && keyA == false && keyS == true && keyD == false)
                    {
                        temp[0] = Convert.ToSByte(factorKey * -2);
                        temp[1] = Convert.ToSByte(factorKey * -2);
                    }
                    else if (keyW == true && keyA == true && keyS == false && keyD == false)
                    {
                        temp[0] = Convert.ToSByte(factorKey);
                        temp[1] = Convert.ToSByte(factorKey * 2);
                    }
                    else if (keyW == true && keyA == false && keyS == false && keyD == true)
                    {
                        temp[0] = Convert.ToSByte(factorKey * 2);
                        temp[1] = Convert.ToSByte(factorKey);
                    }
                    else if (keyW == false && keyA == true && keyS == true && keyD == false)
                    {
                        temp[0] = Convert.ToSByte(factorKey * -1);
                        temp[1] = Convert.ToSByte(factorKey * -2);
                    }
                    else if (keyW == false && keyA == false && keyS == true && keyD == true)
                    {
                        temp[0] = Convert.ToSByte(factorKey * -2);
                        temp[1] = Convert.ToSByte(factorKey * -1);
                    }
                    else if (keyW == false && keyA == true && keyS == false && keyD == false)
                    {
                        temp[0] = 0;
                        temp[1] = Convert.ToSByte(factorKey);
                    }
                    else if (keyW == false && keyA == false && keyS == false && keyD == true)
                    {
                        temp[0] = Convert.ToSByte(factorKey);
                        temp[1] = 0;
                    }
                    else
                    {
                        temp[0] = 0;
                        temp[1] = 0;
                    }

                    break;
                    default:
                        temp[0] = 0;
                        temp[1] = 0;
                        break;
                }

            return temp;
        }

        public void Disc()
        {
            client.Disconnect();
        }

    }
}
