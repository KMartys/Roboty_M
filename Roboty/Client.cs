using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;

//[003e129501ee0060019501f302]

namespace Roboty
{
    public class Client
    {
        IPAddress ip;
        Socket sck;
        byte[] bytes = new byte[28];
        public int flag=10;


        public Client(object oData)
        {
            string data = oData.ToString();
            Char delimiter = ':';
            Console.Write(oData);
            string[] Foo = data.Split(delimiter);
            try
            {
               ip = IPAddress.Parse(Foo[0]);
            }
            catch
            {

            }
            Int32 port = Int32.Parse(Foo[1]);
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                IPEndPoint localEndPoint = new IPEndPoint(ip, port);
            }
            catch
            {
                flag = 1;
            }
            if (flag != 1)
            {
                try
                {
                    sck.Connect(ip, port);
                    flag = 2;

                }
                catch
                {
                    flag = 3;
                }
            }
        }

        public void Disconnect()
        {
           
            sck.Shutdown(SocketShutdown.Both);
            sck.Close();

           // throw new System.NotImplementedException();
        }

        public string Send(int L, int R)
        {
            string tempL = L.ToString("X2");
            string tempR = R.ToString("X2");
            string frame = "[" + "1" + "1" + tempL + tempR + "]";
            byte[] u_data = Encoding.ASCII.GetBytes(frame);
            sck.Send(u_data);
            return frame;
        }


        public string Send(byte led1, byte led2, SByte L, SByte R)
        {
            string tempL = L.ToString("X2");
            string tempR = R.ToString("X2");
            string tempLed1 = led1.ToString("X1");
            string tempLed2 = led2.ToString("X1");
            string frame = "[" + tempLed1 + tempLed2 + tempL + tempR + "]";
            byte[] u_data = Encoding.ASCII.GetBytes(frame);
            sck.Send(u_data);
            return frame;
        }

        public string recive()
        {

            int i = sck.Receive(bytes);
            string fr = Encoding.ASCII.GetString(bytes);
            return fr;
        }

        public UInt16[] Decode(string frame)
        {
            string frame2;
            string frame2a;
            string frame2b;
            UInt16[] temp = new UInt16[6];
            int z = 0; // indeksy tablicy 
            for (int i = 3; i < 25; i = i + 4)
            {
                frame2 = frame.Substring(i, 4);
                frame2a = frame2.Substring(2);
                frame2b = frame2.Substring(0, 2);
                frame2 = frame2a + frame2b;

                try
                {
                    temp[z] = UInt16.Parse(frame2, System.Globalization.NumberStyles.HexNumber);
                    z++;
                }
                catch
                {
                    //TODO
                }
            }
            return temp;
        }

    }
}