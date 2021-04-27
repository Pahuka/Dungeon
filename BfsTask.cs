using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
    public class BfsTask
    {
        public static void PointGeneration(HashSet<Point> visit, Map map, Queue<Point> qVisit, Point point)
        {
            for (var dy = -1; dy <= 1; dy++)
                for (var dx = -1; dx <= 1; dx++)
                    if (dx != 0 && dy != 0) continue;
                    else
                    {
                        var tPoint = new Point { X = point.X + dx, Y = point.Y + dy };
                        if (!visit.Contains(tPoint) & map.InBounds(tPoint) & !qVisit.Contains(tPoint))
                            qVisit.Enqueue(new Point { X = point.X + dx, Y = point.Y + dy });
                    }
        }

        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
            var chestMap = new HashSet<Point>();
            var visitMap = new HashSet<Point>();
            var queue = new Queue<Point>();
            var point = new Point();
            var result = new HashSet<SinglyLinkedList<Point>>();

            foreach (var item in chests)
                chestMap.Add(item);

            queue.Enqueue(start);

            while (queue.Count != 0)
            {
                point = queue.Dequeue();

                if (map.Dungeon[point.X, point.Y] != MapCell.Empty) continue;

                if (!visitMap.Contains(point))
                {
                    visitMap.Add(point);

                    if (result.Count == 0) result.Add(new SinglyLinkedList<Point>(point));
                    else
                    {
                        if (visitMap.Contains(new Point(point.X - 1, point.Y)))
                            result.Add(new SinglyLinkedList<Point>(point, result
                                .First(x => x.Value == new Point(point.X - 1, point.Y))));
                        if (visitMap.Contains(new Point(point.X, point.Y - 1)))
                            result.Add(new SinglyLinkedList<Point>(point, result
                                .First(x => x.Value == new Point(point.X, point.Y - 1))));
                        if (visitMap.Contains(new Point(point.X + 1, point.Y)))
                            result.Add(new SinglyLinkedList<Point>(point, result
                                .First(x => x.Value == new Point(point.X + 1, point.Y))));
                        if (visitMap.Contains(new Point(point.X, point.Y + 1)))
                            result.Add(new SinglyLinkedList<Point>(point, result
                                .First(x => x.Value == new Point(point.X, point.Y + 1))));

                        if (chestMap.Contains(point))
                        {
                            chestMap.Remove(point);
                            yield return result.Last();
                        }
                    }
                }

                if (chestMap.Count != 0) PointGeneration(visitMap, map, queue, point);
            }

            yield break;
        }
    }
}