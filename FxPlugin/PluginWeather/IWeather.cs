using System;
using System.Collections.Generic;
using System.Text;

namespace Weather
{
    public interface IWeather
    {
       string Search(string cityName);
    }
}
