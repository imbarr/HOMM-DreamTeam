using System;
using System.Linq;
using HoMM.Sensors;
using HoMM;
using HoMM.ClientClasses;
using System.Collections.Generic;

namespace Homm.Client
{
    static class Program
    {
        // Вставьте сюда свой личный CvarcTag для того, чтобы учавствовать в онлайн соревнованиях.
        public static readonly Guid CvarcTag = Guid.Parse("dfe38d95-21a7-4b20-a527-ac6971263b20");


        public static void Main(string[] args)
        {
            //195.151.21.246
            if (args.Length == 0)
                args = new[] { "127.0.0.1", "18700" };
            var ip = args[0];
            var port = int.Parse(args[1]);

            var client = new HommClient();

            client.OnSensorDataReceived += Print;
            client.OnInfo += OnInfo;

            var sensorData = client.Configurate(
                ip, port, CvarcTag,

                timeLimit: 90, // Продолжительность матча в секундах (исключая время, которое "думает" ваша программа). 

                operationalTimeLimit: 20, // Суммарное время в секундах, которое разрешается "думать" вашей программе. 
                                            // Вы можете увеличить это время для отладки, чтобы ваш клиент не был отключен, 
                                            // пока вы разглядываете программу в режиме дебаггинга.

                seed: 4,
                // Seed карты. Используйте этот параметр, чтобы получать одну и ту же карту и отлаживаться на ней.
                // Иногда меняйте этот параметр, потому что ваш код должен хорошо работать на любой карте.

                spectacularView: true, // Вы можете отключить графон, заменив параметр на false.

                debugMap: false,
                // Вы можете использовать отладочную простую карту, чтобы лучше понять, как устроен игоровой мир.

                level: HommLevel.Level1, // Здесь можно выбрать уровень. На уровне два на карте присутствует оппонент.

                isOnLeftSide: true
            // Вы можете указать, с какой стороны будет находиться замок героя при игре на втором уровне.
            // Помните, что на сервере выбор стороны осуществляется случайным образом, поэтому ваш код
            // должен работать одинаково хорошо в обоих случаях.
            );

            var commands = new Queue<Direction>();

            while (true)
            {
                var graph = new Graph(sensorData.Map);
                var currentNode = graph[sensorData.Location.X, sensorData.Location.Y];

                if (commands.Count > 0)
                {
                    sensorData = client.Move(commands.Dequeue());
                    currentNode = graph[sensorData.Location.X, sensorData.Location.Y];
                    var forHire = currentNode.MaximumHireNumber(sensorData.MyTreasury);
                    if(forHire > 0)
                        sensorData = client.HireUnits(forHire);
                    continue;
                }
                
                var gpi = Routing.GetGraphPathInfo(currentNode, n => n.MapObjectData.NeutralArmy != null);
                var path = Routing.FindPathToClosest(currentNode, gpi, n =>
                    n.IsValidTarget(currentNode.MapObjectData.Hero, gpi[n].TravelTime, 7.0));
                if (path == null)
                {
                    path = Routing.FindPathToClosest(currentNode, gpi, n =>
                        n.IsValidDwelling(sensorData.MyTreasury, gpi[n].TravelTime, 7.0, 3));
                }
                if (path == null)
                {
                    path = Routing.FindPathToClosest(currentNode, gpi, n =>
                        n.IsValidTarget(currentNode.MapObjectData.Hero, gpi[n].TravelTime, double.MaxValue));
                }
                if (path == null)
                {
                    path = Routing.FindPathToClosest(currentNode, gpi, n =>
                        n.IsValidDwelling(sensorData.MyTreasury, gpi[n].TravelTime, double.MaxValue, 3));
                }

                commands = Convertation.ToDirectionQueue(path.Reverse());
                Console.WriteLine(path.First());
            }
        }


        public static bool IsValidTarget(Node node)
        {
            return node.MapObjectData.ResourcePile != null
                || node.MapObjectData.Dwelling != null && node.MapObjectData.Dwelling.Owner != "Left"
                || node.MapObjectData.Mine != null && node.MapObjectData.Mine.Owner != "Left";
        }
        static void Print(HommSensorData data)
        {
            Console.WriteLine("---------------------------------");

            Console.WriteLine($"You are here: ({data.Location.X},{data.Location.Y})");

            Console.WriteLine($"You have {data.MyTreasury.Select(z => z.Value + " " + z.Key).Aggregate((a, b) => a + ", " + b)}");

            var location = data.Location.ToLocation();

            Console.Write("W: ");
            Console.WriteLine(GetObjectAt(data.Map, location.NeighborAt(Direction.Up)));

            Console.Write("E: ");
            Console.WriteLine(GetObjectAt(data.Map, location.NeighborAt(Direction.RightUp)));

            Console.Write("D: ");
            Console.WriteLine(GetObjectAt(data.Map, location.NeighborAt(Direction.RightDown)));

            Console.Write("S: ");
            Console.WriteLine(GetObjectAt(data.Map, location.NeighborAt(Direction.Down)));

            Console.Write("A: ");
            Console.WriteLine(GetObjectAt(data.Map, location.NeighborAt(Direction.LeftDown)));

            Console.Write("Q: ");
            Console.WriteLine(GetObjectAt(data.Map, location.NeighborAt(Direction.LeftUp)));
        }

        static string GetObjectAt(MapData map, Location location)
        {
            if (location.X < 0 || location.X >= map.Width || location.Y < 0 || location.Y >= map.Height)
                return "Outside";
            return map.Objects.
                Where(x => x.Location.X == location.X && x.Location.Y == location.Y)
                .FirstOrDefault()?.ToString() ?? "Nothing";
        }


        static void OnInfo(string infoMessage)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(infoMessage);
            Console.ResetColor();
        }
    }
}