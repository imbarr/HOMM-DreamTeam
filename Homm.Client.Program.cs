﻿using System;
using System.Linq;
using HoMM.Sensors;
using HoMM;
using HoMM.ClientClasses;
using System.Collections.Generic;

namespace Homm.Client
{
    class Program
    {
        // Вставьте сюда свой личный CvarcTag для того, чтобы учавствовать в онлайн соревнованиях.
        public static readonly Guid CvarcTag = Guid.Parse("00000000-0000-0000-0000-000000000000");


        public static void Main(string[] args)
        {
            if (args.Length == 0)
                args = new[] { "127.0.0.1", "18700" };
            var ip = args[0];
            var port = int.Parse(args[1]);

            var client = new HommClient();

            client.OnSensorDataReceived += Print;
            client.OnInfo += OnInfo;

            var sensorData = client.Configurate(
                ip, port, CvarcTag,

                timeLimit: 90,              // Продолжительность матча в секундах (исключая время, которое "думает" ваша программа). 

                operationalTimeLimit: 20,   // Суммарное время в секундах, которое разрешается "думать" вашей программе. 
                                            // Вы можете увеличить это время для отладки, чтобы ваш клиент не был отключен, 
                                            // пока вы разглядываете программу в режиме дебаггинга.

                seed: 0,                    // Seed карты. Используйте этот параметр, чтобы получать одну и ту же карту и отлаживаться на ней.
                                            // Иногда меняйте этот параметр, потому что ваш код должен хорошо работать на любой карте.

                spectacularView: true,      // Вы можете отключить графон, заменив параметр на false.

                debugMap: false,            // Вы можете использовать отладочную простую карту, чтобы лучше понять, как устроен игоровой мир.

                level: HommLevel.Level1,    // Здесь можно выбрать уровень. На уровне два на карте присутствует оппонент.

                isOnLeftSide: true          // Вы можете указать, с какой стороны будет находиться замок героя при игре на втором уровне.
                                            // Помните, что на сервере выбор стороны осуществляется случайным образом, поэтому ваш код
                                            // должен работать одинаково хорошо в обоих случаях.
            );

            var path = new[] { Direction.RightDown, Direction.RightUp, Direction.RightDown, Direction.RightUp, Direction.LeftDown, Direction.Down, Direction.RightDown, Direction.RightDown, Direction.RightUp };
            sensorData = client.HireUnits(1);
            foreach (var e in path)
                sensorData = client.Move(e);
            sensorData = client.Move(Direction.RightDown);
            client.Exit();
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