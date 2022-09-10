using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAPI
{
    public static class Utility
    {
        public static double ConvertDegreeAngleToDouble(string degreeMinuteSecond)
        {
            var degree = degreeMinuteSecond.Split('\u00B0')[0];
            var minutes = degreeMinuteSecond.Split('\u00B0')[1].Split('\u0027')[0];
            var seconds = degreeMinuteSecond.Split('\u00B0')[1].Split('\u0027')[1].Split('\u0027')[0];

            if (degreeMinuteSecond.Contains("S") || degreeMinuteSecond.Contains("W"))
            {
                return -1 * Math.Round(Convert.ToDouble(degree) + (Convert.ToDouble(minutes) / 60) + (Convert.ToDouble(seconds) / 3600), 2);
            }
            else return Math.Round(Convert.ToDouble(degree) + (Convert.ToDouble(minutes) / 60) + (Convert.ToDouble(seconds) / 3600), 2);

            //Decimal degrees = 
            //   whole number of degrees, 
            //   plus minutes divided by 60, 
            //   plus seconds divided by 3600
            //(minutes / 60) + (seconds / 3600) 
        }
    }
}
