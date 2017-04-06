using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;



namespace Roboty
{
    public class LineFollower
    {
        public double P { get; set; }
        public double D { get; set; }
        public bool flag { get; set; }
        int[] SensorFactor = new int[5];
        int[] ST = new int[5];
        double prvError, error;
        private bool flagOutside;
        public sbyte speed { get; set; }
        public int treshold { get; set; }
        public LineFollower()
        {
            SensorFactor[0] = 6;
            SensorFactor[1] = 2;
            SensorFactor[2] = 0;
            SensorFactor[3] = -2;
            SensorFactor[4] = -6;
            P = 3.22;
            D = 2.04;
            speed = 0;
            treshold=800;//do zrobienia funkcja ustalająca próg

        }

        public sbyte[] Follow(UInt16[] Sensor)
        {
            treshold = Calibrate(Sensor);
            // ustawić klaibracje pod przycisk
            sbyte[] engine = new sbyte[2];
            error = Error(Sensor, prvError);
            prvError = error;
            engine = PD(error, prvError);
            return engine;
        }

        private double Error(UInt16[] tempSensor, double prvError )
        {
            int temp=0;
          //  Console.WriteLine("\n" + "tr:" + treshold);
            for (int i = 1; i <6; i++) // obliczenie błędu
            {
                //Console.WriteLine("\n"+i+"tr:" + treshold);
                if (tempSensor[i] < treshold)
                {
                    ST[i-1] = 1;
                    temp++;
                }
                else ST[i-1] = 0;
                error = error + ST[i-1] * SensorFactor[i-1];
               // Console.WriteLine(tempSensor[i]);
            }
            if (temp != 0 && !flagOutside)
            {
                error = error / temp;
                prvError = error;
            }
            else
            {
                flagOutside = true;
                error = prvError;
                Console.Write("\nPRV!!!!!!!!!");
            }

            if ((ST[2] == 1 && ST[3] == 1) || (ST[4] == 1 && ST[3] == 1)) flagOutside = false;
            //Console.WriteLine("error:" + error);
            return error;
        }

        private sbyte[] PD(double _error, double _prvError)
        {
            double dif = _error - _prvError;
            double value = P * _error + D * dif;
            sbyte[] engine = new sbyte[2];

            try
            {
                engine[0] = Convert.ToSByte(value);
                engine[0] += speed;
            }
            catch
            {
                if (value > 80) engine[0] = 127;
                else if (value < 80) engine[0] = -127;
            }
            try
            {
                engine[1] -= Convert.ToSByte(value);
                engine[1] += speed;
            }
            catch
            {
                if (value > 80) engine[0] = 127;
                else if (value < 80) engine[0] = -127;
            }

            return engine;


        }
        public int Calibrate(UInt16[] tempSensor)
        {
            int  min=2000, max=0;
            for (int i = 0; i <5; i++) // obliczenie błędu
            {
                if (tempSensor[i] < min) min = tempSensor[i];
                if (tempSensor[i] > max) max = tempSensor[i];
            }
            return ((max-min)/2);
        }
        


    }
}