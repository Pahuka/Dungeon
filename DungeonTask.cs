using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
    public class DungeonTask
    {
        public static MoveDirection[] FindShortestPath(Map map)
        {
            var startPath = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
            var endPath = startPath
                .Select(chest => Tuple.Create(chest, BfsTask.FindPaths(map, chest.Value, new[] { map.Exit })
                .FirstOrDefault())).MinElement();
            var pathToExit = BfsTask.FindPaths(map, map.InitialPosition, new Point[] { map.Exit }).FirstOrDefault();

            if (pathToExit == null) return new MoveDirection[0];
            if (endPath == null) return pathToExit.Reverse().ToList().ParseDirection().ToArray();

            return endPath.Item1.Reverse().ToList().ParseDirection()
                .Concat(endPath.Item2.Reverse().ToList().ParseDirection()).ToArray();
        }
    }

    public static class ExtentionMetods
    {
        public static Tuple<SinglyLinkedList<Point>, SinglyLinkedList<Point>>
            MinElement(this IEnumerable<Tuple<SinglyLinkedList<Point>, SinglyLinkedList<Point>>> items)
        {
            if (items.Count() == 0 || items.First().Item2 == null) return null;

            var minValue = int.MaxValue;
            var minElement = items.First();

            foreach (var item in items)
            {
                if ((item.Item1.Length + item.Item2.Length) < minValue)
                {
                    minValue = item.Item1.Length + item.Item2.Length;
                    minElement = item;
                }
            }
            return minElement;
        }

        public static MoveDirection GetDirection(Point one, Point two)
        {
            if (one.X == two.X)
                return one.Y < two.Y ? MoveDirection.Down : MoveDirection.Up;
            else return one.X < two.X ? MoveDirection.Right : MoveDirection.Left;
        }

        public static MoveDirection[] ParseDirection(this List<Point> items)
        {
            var result = new List<MoveDirection>();
            if (items == null) return new MoveDirection[0];
            var index = 0;
            var tempValue = items.First();

            foreach (var point in items)
            {
                index++;
                if (index >= 2 & tempValue != point)
                {
                    result.Add(GetDirection(tempValue, point));
                    index = 1;
                }
                tempValue = point;
            }
            return result.ToArray();
        }
    }
}