using System.Collections.Generic;
using System.Drawing;

namespace Dungeon
{
    public class BfsTask
    {
        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
            var visitMap = new HashSet<Point>();
            var queue = new Queue<Point>();
            var result = new Dictionary<Point, SinglyLinkedList<Point>>();

            result.Add(start, new SinglyLinkedList<Point>(start));
            visitMap.Add(start);
            queue.Enqueue(start);

            while (queue.Count != 0)
            {
                var point = queue.Dequeue();

                if (map.Dungeon[point.X, point.Y] != MapCell.Empty) continue;

                for (var dy = -1; dy <= 1; dy++)
                    for (var dx = -1; dx <= 1; dx++)
                        if (dx != 0 && dy != 0) continue;
                        else
                        {
                            var tPoint = new Point { X = point.X + dx, Y = point.Y + dy };
                            if (!visitMap.Contains(tPoint) & map.InBounds(tPoint) & !queue.Contains(tPoint))
                            {
                                queue.Enqueue(tPoint);
                                visitMap.Add(tPoint);
                                result.Add(tPoint, new SinglyLinkedList<Point>(tPoint, result[point]));
                            }
                        }
            }

            foreach (var item in chests)
            {
                if (result.ContainsKey(item)) yield return result[item];
            }

            yield break;
        }
    }
}