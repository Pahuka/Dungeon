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
            var chestsMap = new HashSet<Point>();
            var path = new SinglyLinkedList<Point>(start);
            var queue = new Queue<Point>();
            var point = new Point();

            foreach (var item in chests)
                chestsMap.Add(item);

            queue.Enqueue(start);

            while (queue.Count != 0 || chestsMap.Count != 0)
            {

                if (queue.Count == 0)
                {
                    for (var dy = -1; dy <= 1; dy++)
                        for (var dx = -1; dx <= 1; dx++)
                            if (dx != 0 && dy != 0) continue;
                            else queue.Enqueue(new Point { X = point.X + dx, Y = point.Y + dy });
                }
                point = queue.Dequeue();

                if (point.X < 0 || point.X >= map.Dungeon.GetLength(0) || point.Y < 0
                    || point.Y >= map.Dungeon.GetLength(1)) continue;
                if (map.Dungeon[point.X, point.Y] != MapCell.Empty) continue;
                if (!path.Contains(point))
                {
                    //if (path.Previous != null)
                    //{
                    //    var t = path.Previous;
                    //    path = new SinglyLinkedList<Point>(point, t);
                    //}
                    path = new SinglyLinkedList<Point>(point, path);
                }
                if (chestsMap.Contains(point))
                {
                    chestsMap.Remove(point);
                    yield return path;
                }


            }

            yield break;
        }
    }
}