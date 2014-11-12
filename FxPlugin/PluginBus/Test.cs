using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace PluginBus
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void TestLine()
        {
            Bus bus = new Bus();
            Trace.Write(bus.Execute("158"));
        }

        [Test]
        public void TestStation()
        {
            Bus bus = new Bus();
            Trace.Write(bus.Execute("黄科路口"));
        }

        [Test]
        public void TestStops()
        {
            Bus bus = new Bus();
            Trace.Write(bus.Execute("逍遥津 火车站"));
        }

    }
}
