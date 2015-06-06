using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using QuickGraph;
using QuickGraph.Algorithms.Observers;
using QuickGraph.Algorithms.ShortestPath;
using QuickGraph.Serialization;
using QuickGraph.Algorithms;

namespace LongestSkiPath
{
    internal class SkiingResolver
    {
        private AdjacencyGraph<Point, TaggedEdge<Point, int>> _grapth;
        private readonly Point _rootNodePoint;
        private readonly Point _endNodePoint;
        private IEnumerable<TaggedEdge<Point, int>> _path;
        private int _maxSteepness = 0;

        public SkiingResolver(int width, int height, string data)
        {
            var mapArray = new Point[width, height];
            _grapth = new AdjacencyGraph<Point, TaggedEdge<Point, int>>();

            var sr = new StringReader(data);
            for (int j = 0; j < width; j++)
            {
                var lineString = sr.ReadLine();
                Debug.Assert(lineString != null, "lineString != null");
                var line = lineString.Split(' ');
                for (int i = 0; i < height; i++)
                {
                    var number = Convert.ToInt32(line[i]);
                    var p = new Point(i, j, number);
                    mapArray[i, j] = p;
                    _grapth.AddVertex(p);
                    if (i > 0)
                    {
                        if (mapArray[i - 1, j] > mapArray[i, j])
                        {
                            _grapth.AddEdge(new TaggedEdge<Point, int>(mapArray[i - 1, j], mapArray[i, j], 1));
                        }
                        else if (mapArray[i - 1, j] < mapArray[i, j])
                        {
                            _grapth.AddEdge(new TaggedEdge<Point, int>(mapArray[i, j], mapArray[i - 1, j], 1));
                        }
                    }


                    if (j > 0)
                    {
                        if (mapArray[i, j - 1] > mapArray[i, j])
                        {
                            _grapth.AddEdge(new TaggedEdge<Point, int>(mapArray[i, j - 1], mapArray[i, j], 1));
                        }
                        else if (mapArray[i, j - 1] < mapArray[i, j])
                        {
                            _grapth.AddEdge(new TaggedEdge<Point, int>(mapArray[i, j], mapArray[i, j - 1], 1));
                        }
                    }


                }
            }

            _rootNodePoint = new Point(-1, -1, -1);
            _grapth.AddVertex(_rootNodePoint);

            foreach (var node in _grapth.Roots())
            {
                if (!node.Equals(_rootNodePoint))
                {
                    _grapth.AddEdge(new TaggedEdge<Point, int>(_rootNodePoint, node, -1));
                }
            }

            _endNodePoint = new Point(-2, -2, -2);
            _grapth.AddVertex(_endNodePoint);

            foreach (var node in _grapth.Sinks())
            {
                if (!node.Equals(_endNodePoint))
                {
                    _grapth.AddEdge(new TaggedEdge<Point, int>(node, _endNodePoint, -1));
                }
            }
        }

        public override string ToString()
        {
            if (_path == null)
            {
                throw new Exception("Run calculate first!");
            }
            else
            {
                var sb = new StringBuilder();
                foreach (var taggedEdge in _path)
                {
                    sb.Append("[(" + taggedEdge.Source.X + "," + taggedEdge.Source.Y + ") --> (" + taggedEdge.Target.X + "," + taggedEdge.Target.Y + ")]\n");
                }

                return sb.ToString();
            }
        }

        public bool Calculate()
        {
            var result = false;

            var tryGetPath = _grapth.ShortestPathsBellmanFord(edge => -1, _rootNodePoint);
            result = tryGetPath(_endNodePoint, out _path);
            if (result)
            {
                _maxSteepness = _path.First().Target.Height - _path.Last().Source.Height;
                Console.Write("First - " + _path.Count() + " -- steep " + _maxSteepness);
                CalculateAllOptions(_path, _maxSteepness);
            }

            return result;
        }

        private void CalculateAllOptions(IEnumerable<TaggedEdge<Point, int>> path, int maxSteepness)
        {
            foreach (var edge in path.Where(edge => edge.Tag > 0))
            {
                Console.Write(".");
                _grapth.RemoveEdge(edge);

                var tryGetPath = _grapth.ShortestPathsBellmanFord(node => -1, _rootNodePoint);
                IEnumerable<TaggedEdge<Point, int>> pathTemp;
                var result = tryGetPath(_endNodePoint, out pathTemp);

                _grapth.AddEdge(edge);

                if (result)
                {

                    if (pathTemp.Count() >= path.Count())
                    {

                        int steepness = pathTemp.First().Target.Height - pathTemp.Last().Source.Height;

                        if (steepness > maxSteepness)
                        {
                            Console.WriteLine("new record! " + (pathTemp.Count() - 1));
                            _path = pathTemp;
                            _maxSteepness = steepness;
                            CalculateAllOptions(pathTemp, steepness);
                        }
                    }
                }

            }
        }

        private void CheckWeCanReturnValues()
        {
            if (_path == null)
            {
                throw new Exception("Run calculate first!");
            }

            if (_path.Count() <= 1)
            {
                throw new Exception("Path not found!");
            }
        }

        public int GetLongestSkiingPathLength()
        {
            CheckWeCanReturnValues();
            return _path.Count() - 1;
        }

        public int GetLongestSkiingPathSteepness()
        {
            CheckWeCanReturnValues();
            return _maxSteepness;
        }
    }
}
