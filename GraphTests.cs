using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoMM;
using NUnit;
using NUnit.Framework;

namespace Homm.Client
{
    [TestFixture]
    class GraphTests
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(23)]
        [TestCase(451)]
        [TestCase(666)]
        public void ConstructorCustomTest(int seed)
        {
            var cvarcTag = Guid.Parse("00000000-0000-0000-0000-000000000000");
            var ip = "127.0.0.1";
            var port = 18700;
            var client = new HommClient();
            var sensorData = client.Configurate(ip, port, cvarcTag, seed: seed);

            var map = sensorData.Map;
            var graph = new Graph(map);
            Assert.AreEqual(map.Width, graph.Width);
            Assert.AreEqual(map.Height, graph.Height);
            foreach (var objectData in map.Objects)
            {
                var x = objectData.Location.X;
                var y = objectData.Location.Y;
                if (objectData.Wall != null)
                    Assert.AreEqual(null, graph[x, y]);
                else
                    Assert.AreEqual(objectData, graph[x, y].MapObjectData);
            }
            client.Exit();
        }
    }
}
