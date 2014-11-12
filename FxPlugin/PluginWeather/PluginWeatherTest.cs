using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Weather
{
    [TestFixture]
    public class PluginWeatherTest
    {
        [Test]
        public void GoogleTest()
        {
            GoogleWeather gw = new GoogleWeather();
            string expect = gw.Search("合肥");

            Console.WriteLine(expect);

            expect = gw.Search("南京");

            Console.WriteLine(expect);

            expect = gw.Search("上海");

            Console.WriteLine(expect);

            Assert.IsNotNullOrEmpty(expect);

        }
    }
}
