using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
    public class BfsTask
    {
        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
            var chestMap = new HashSet<Point>();
            var visitMap = new HashSet<Point>();
            var queue = new Queue<Point>();
            var point = new Point();
            var result = new HashSet<SinglyLinkedList<Point>>();

            foreach (var item in chests)
            {
                chestMap.Add(item);
            }

            queue.Enqueue(start);

            while (queue.Count != 0)
            {
                point = queue.Dequeue();

                if (!map.InBounds(point) || map.Dungeon[point.X, point.Y] != MapCell.Empty) continue;

                if (!visitMap.Contains(point))
                {
                    visitMap.Add(point);
                    if (result.Count == 0) result.Add(new SinglyLinkedList<Point>(point));
                    else
                    {
                        foreach (var item in visitMap)
                        {
                            if (item == new Point(point.X - 1, point.Y))
                                result.Add(new SinglyLinkedList<Point>(point, result
                                    .SingleOrDefault(x => x.Value == new Point(point.X - 1, point.Y))));
                            if (item == new Point(point.X, point.Y - 1))
                                result.Add(new SinglyLinkedList<Point>(point, result
                                    .SingleOrDefault(x => x.Value == new Point(point.X, point.Y - 1))));
                            if (item == new Point(point.X + 1, point.Y))
                                result.Add(new SinglyLinkedList<Point>(point, result
                                    .SingleOrDefault(x => x.Value == new Point(point.X + 1, point.Y))));
                            if (item == new Point(point.X, point.Y + 1))
                                result.Add(new SinglyLinkedList<Point>(point, result
                                    .SingleOrDefault(x => x.Value == new Point(point.X, point.Y + 1))));
                        }

                        if (chestMap.Contains(point)) yield return result.Last();
                    }
                }

                for (var dy = -1; dy <= 1; dy++)
                    for (var dx = -1; dx <= 1; dx++)
                        if (dx != 0 && dy != 0) continue;
                        else
                        {
                            var tPoint = new Point { X = point.X + dx, Y = point.Y + dy };
                            if (!visitMap.Contains(tPoint))
                                queue.Enqueue(new Point { X = point.X + dx, Y = point.Y + dy });
                        }
            }

            yield break;
        }
    }
}