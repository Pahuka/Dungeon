using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace Dungeon
{
	public class DungeonTask
	{
        public static List<Tuple<Point, Point>> PathBigrams(List<Point> items)
        {
            var result = new List<Tuple<Point, Point>>();

            if (items.Count == 0) return null;
            else
            {
                var tempValue = items[0];
                int index = 0;
                foreach (var point in items)
                {
                    index++;
                    if (index >= 2 & tempValue != point)
                    {
                        result.Add(Tuple.Create(tempValue, point));
                        index = 1;
                        //tempValue = point;
                    }
                    tempValue = point;
                }
            }

            return result;
        }

        public static MoveDirection[] FindShortestPath(Map map)
		{
            var startPath = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
            var endPath = BfsTask.FindPaths(map, map.Exit, map.Chests);
            var pathToExit = BfsTask.FindPaths(map, map.InitialPosition, new Point[] { map.Exit }).FirstOrDefault();
            var pointList = new List<Point>();

            if (pathToExit == null) return new MoveDirection[0];
            if (startPath.Count() != 0 && endPath.Count() != 0)
            {
                pointList.AddRange(endPath
                    .OrderBy(x => x.Length)
                    .First().Reverse());
                if (startPath.Where(x => x.Value == pointList.Last()).Count() == 0) return new MoveDirection[0];
                pointList.AddRange(startPath
                    .Where(x => pointList.Contains(x.Value))
                    .SelectMany(x => x));
                pointList.Reverse();
            }
            else
            {
                pointList.AddRange(pathToExit.Reverse().ToList());
            }

            var bigramList = PathBigrams(pointList);
            var resultPath = new MoveDirection[bigramList.Count];

            for (int i = 0; i < bigramList.Count; i++)
            {
                if (bigramList[i].Item1.X == bigramList[i].Item2.X)
                    resultPath[i] = bigramList[i].Item1.Y < bigramList[i].Item2.Y 
                        ? MoveDirection.Down : MoveDirection.Up;
                else resultPath[i] = bigramList[i].Item1.X < bigramList[i].Item2.X
                        ? MoveDirection.Right : MoveDirection.Left;
            }

            return resultPath;
		}
	}
}
