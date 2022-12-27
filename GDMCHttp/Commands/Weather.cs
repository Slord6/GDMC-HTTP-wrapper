using System;
using System.Collections.Generic;
using System.Text;

namespace GDMCHttp.Commands
{
    public class Weather : Command
    {
        public Weather(WeatherType type) : base("weather", new string[] {type.ToString().ToLower()})
        {

        }
    }
}
