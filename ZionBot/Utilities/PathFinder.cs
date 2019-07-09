using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OtClientBot.Utilities

{


    public class PathFinder : _PathFinder
    {

        public PathFinder() : base(17,13) // Screen Grid
        {

        }

        public bool ThereIsPath(Location loc, bool ConsiderCreatures = true)
        {
            return (FindPath(loc,ConsiderCreatures).Count() > 0 );
        }

        public IEnumerable<byte> FindPath(Location goal, bool ConsiderCreatures = true)//, out List<byte> Directions)
        {
            //Directions = new List<byte>();

            // Populate Grid with speed Values;
            // 0xFF == Unwalkable
            Location StartLocation = Player.Location;
            for (int x = -8; x <= 8; x++)
            {
                for (int y = -6; y <= 6; y++)
                {
                    Location evalLocation = (new Location(StartLocation.X + x, StartLocation.Y + y, StartLocation.Z));
                    
                    // Skip Goal
                    var offset = StartLocation.OffsetTo(goal);
                    bool isGoal = (offset.X == x && offset.Y == y);
                    bool isStair = evalLocation.isStairs();

                    byte tileSpeed = 0xFF;

                    if ((isGoal) || ((evalLocation.isWalkable(ConsiderCreatures) && !isStair)))
                        tileSpeed = Minimap.GetTile(evalLocation).Speed;

                    this.Grid[x + 8, y + 6] = tileSpeed; 
                }
            }

            // The coordinates of the start position inside the grid.
            Point startPos = new Point(8,6);

            // Coordinates of goal inside the grid.
            var Offset = StartLocation.OffsetTo(goal);
            Point goalPos = new Point(8 + Offset.X, 6 + Offset.Y);


            // PathFind
            var Path = base.FindPath(startPos, goalPos);

            return Path;

            // Implement here the directions to send.

            //if (Path.Count<Node>() == 0) return false;

            

            //return false;
        }


        


    }
    



    public class _PathFinder
    {
        public _PathFinder(ushort width, ushort height, byte heuristicEstimate = 2, bool allowDiagonals = true,
            bool penalizeDiagonals = true, float diagonalPenaltyMultiplier = 3, bool considerUnexploredAsUnwalkable = true)
        {
            this.SyncObject = new object();
            this.Grid = new byte[width, height];

            this.DiagonalPenaltyMultiplier = diagonalPenaltyMultiplier;
            this.HeuristicMultiplier = heuristicEstimate;
            this.AllowDiagonals = allowDiagonals;
            this.PenalizeDiagonals = penalizeDiagonals;
            this.ConsiderUnexploredAsUnwalkable = considerUnexploredAsUnwalkable;
        }

        public byte[,] Grid { get; private set; }
        public bool ConsiderUnexploredAsUnwalkable { get; set; }
        public bool AllowDiagonals { get; set; }
        public bool PenalizeDiagonals { get; set; }
        public float DiagonalPenaltyMultiplier { get; set; }
        public byte HeuristicMultiplier { get; set; }
        public readonly ushort MaxGridLength = 0xFFFF;

        protected bool DoStop { get; set; }
        private object SyncObject { get; set; }
        private readonly sbyte[,] Directions = new sbyte[8, 2]
        {
            { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 },
            { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 }
        };

        /// <summary>
        /// A structure to find a path between two points. Contains information like XY position and travel cost.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class Node : IComparable<Node>
        {
            public Node Parent;
            /// <summary>
            /// Position of X axis.
            /// </summary>
            public int X;
            /// <summary>
            /// Position of Y axis.
            /// </summary>
            public int Y;
            /// <summary>
            /// The total travel cost thus far. Is normally referenced to as G.
            /// </summary>
            public double PathTravelCost;
            /// <summary>
            /// The heuristic cost for this node. Is normally referenced to as H.
            /// </summary>
            public double HeuristicCost;
            /// <summary>
            /// The combined travel cost for this node (F=G+H). Is normally referenced to as F.
            /// </summary>
            public double CombinedTravelCost;

            public byte directionFromParent;

            public int CompareTo(Node otherNode)
            {
                if (this.CombinedTravelCost > otherNode.CombinedTravelCost)
                    return 1;
                else if (this.CombinedTravelCost < otherNode.CombinedTravelCost)
                    return -1;
                return 0;
            }


        }

        public void ResetGrid()
        {
            lock (this.SyncObject)
            {
                for (int y = 0; y < this.Grid.GetUpperBound(1); y++)
                {
                    for (int x = 0; x < this.Grid.GetUpperBound(0); x++)
                    {
                        this.Grid[x, y] = (byte)0xFF;
                    }
                }
            }
        }
        public void Stop()
        {
            this.DoStop = true;
        }
        public IEnumerable<byte> FindPath(Point start, Point end, bool stopWhenAdjacent = false)
        {
            if (start.X > this.MaxGridLength || start.X < 0 || start.Y > this.MaxGridLength || start.Y < 0 ||
                end.X > this.MaxGridLength || end.X < 0 || end.Y > this.MaxGridLength || end.Y < 0)
            {
                return Enumerable.Empty<byte>();
            }
            return this.FindPath((byte)start.X, (byte)start.Y, (byte)end.X, (byte)end.Y, stopWhenAdjacent);
        }
        public IEnumerable<byte> FindPath(Location start, Location end, bool stopWhenAdjacent = false)
        {
            if (start.Z != end.Z) throw new Exception("start.Z must be the same as end.Z");

            return this.FindPath(new Point(start.X, start.Y), new Point(end.X, end.Y), stopWhenAdjacent);
        }

        protected IEnumerable<byte> FindPath(int startX, int startY, int endX, int endY, bool stopWhenAdjacent = false)
        {
            lock (this.SyncObject)
            {
                this.DoStop = false;

                int gridWidth = this.Grid.GetUpperBound(0),
                    gridHeight = this.Grid.GetUpperBound(1);
                byte unexploredValue = (byte)0x00,
                    unwalkableValue = (byte)0xFF;
                PriorityQueue<Node> openNodes = new PriorityQueue<Node>();
                List<Node> closedNodes = new List<Node>();
                bool success = false;

                Node currentNode = new Node();
                currentNode.X = startX;
                currentNode.Y = startY;
                currentNode.PathTravelCost = 0;
                currentNode.HeuristicCost = this.HeuristicMultiplier *  (Math.Abs(startX - endX) + Math.Abs(startY - endY)); // ?? Verificar se isso não deveria ser a distancia heuristica
                currentNode.CombinedTravelCost = (double)(currentNode.PathTravelCost + currentNode.HeuristicCost);

                openNodes.Push(currentNode);

                while (openNodes.Count > 0 && !this.DoStop)
                {
                    currentNode = openNodes.Pop();
                    // Check if we reached goal
                    if ((currentNode.X == endX && currentNode.Y == endY) ||
                        (stopWhenAdjacent && Math.Max(Math.Abs(currentNode.X - endX), Math.Abs(currentNode.Y - endY)) <= 1))
                    {
                        closedNodes.Add(currentNode);
                        success = true;
                        break;
                    }

                    // check adjacent nodes
                    for (byte i = 0; i < (this.AllowDiagonals ? 8 : 4); i++)
                    {
                        Node newNode = new Node()
                        {
                            X = (int)(currentNode.X + this.Directions[i, 0]),
                            Y = (int)(currentNode.Y + this.Directions[i, 1])
                        };

                        // check if node is within bounds
                        if (newNode.X < 0 || newNode.X >= gridWidth || newNode.Y < 0 || newNode.Y >= gridHeight) continue;

                        byte speed = this.Grid[newNode.X, newNode.Y];
                        if ((this.ConsiderUnexploredAsUnwalkable && speed == unexploredValue) || speed == unwalkableValue) continue;

                        double totalCost = currentNode.PathTravelCost + (i > 3 ? (double)(speed * (this.PenalizeDiagonals ? this.DiagonalPenaltyMultiplier : 1)) : speed);
                        if (totalCost == currentNode.PathTravelCost) continue;

                        // check if node already exists in open list
                        int index = -1;
                        for (int j = 0; j < openNodes.Count; j++)
                        {
                            if (openNodes[j].X == newNode.X && openNodes[j].Y == newNode.Y)
                            {
                                index = j;
                                break;
                            }
                        }
                        if (index != -1 && openNodes[index].PathTravelCost <= totalCost) continue;

                        // check if node already exists in closed list
                        index = -1;
                        for (int j = 0; j < closedNodes.Count; j++)
                        {
                            if (closedNodes[j].X == newNode.X && closedNodes[j].Y == newNode.Y)
                            {
                                index = j;
                                break;
                            }
                        }
                        if (index != -1 && closedNodes[index].PathTravelCost <= totalCost) continue;

                        newNode.Parent = currentNode;
                        newNode.PathTravelCost = (double)(totalCost > ushort.MaxValue ? ushort.MaxValue : totalCost);
                        newNode.HeuristicCost = this.HeuristicMultiplier * (Math.Abs(newNode.X - endX) + Math.Abs(newNode.Y - endY));//(double)(this.HeuristicEstimate * (Math.Sqrt(((Math.Abs(newNode.X - endX)^2 + Math.Abs(newNode.Y - endY)^2)))));
                        newNode.CombinedTravelCost = (double)(newNode.PathTravelCost + newNode.HeuristicCost);
                        newNode.directionFromParent = i;
                        openNodes.Push(newNode);
                    }

                    closedNodes.Add(currentNode);
                }

                this.DoStop = false;

                // Here we convert the found path on a list of directions.

                Node pathNode = closedNodes.Last<Node>();

                System.Collections.Stack PathStack = new System.Collections.Stack();


                while (true)
                {
                    PathStack.Push(pathNode.directionFromParent);


                    pathNode = pathNode.Parent;

                    if (pathNode == null || pathNode.Parent == null) break;

                }

                List<byte> PathWay = new List<byte>();

                do
                {
                    PathWay.Add((byte)PathStack.Pop());
                } while (PathStack.Count > 0);


                if (success) return PathWay;
                else return Enumerable.Empty<byte>();
            }
        }












        public class PriorityQueue<T>
        {
            #region contructors
            public PriorityQueue()
            {
                this.Comparer = Comparer<T>.Default;
            }
            public PriorityQueue(IComparer<T> comparer)
            {
                this.Comparer = comparer;
            }
            public PriorityQueue(IComparer<T> comparer, int capacity)
            {
                this.Comparer = comparer;
                this.InnerList.Capacity = capacity;
            }
            #endregion

            #region properties
            private List<T> InnerList = new List<T>();
            private IComparer<T> Comparer;
            #endregion

            #region methods
            private void SwitchElements(int i, int j)
            {
                T h = this.InnerList[i];
                this.InnerList[i] = this.InnerList[j];
                this.InnerList[j] = h;
            }
            private int OnCompare(int i, int j)
            {
                return this.Comparer.Compare(this.InnerList[i], this.InnerList[j]);
            }
            /// <summary>
            /// Push an object onto the PQ
            /// </summary>
            /// <param name="O">The new object</param>
            /// <returns>The index in the list where the object is _now_. This will change when objects are taken from or put onto the PQ.</returns>
            public int Push(T item)
            {
                int p = this.InnerList.Count, p2;
                this.InnerList.Add(item);
                while (true)
                {
                    if (p == 0) break;
                    p2 = (p - 1) / 2;
                    if (this.OnCompare(p, p2) < 0)
                    {
                        this.SwitchElements(p, p2);
                        p = p2;
                    }
                    else break;
                }
                return p;
            }
            /// <summary>
            /// Get the smallest object and remove it.
            /// </summary>
            /// <returns>The smallest object</returns>
            public T Pop()
            {
                T result = this.InnerList[0];
                int p = 0, p1, p2, pn;
                this.InnerList[0] = this.InnerList[this.InnerList.Count - 1];
                this.InnerList.RemoveAt(this.InnerList.Count - 1);
                while (true)
                {
                    pn = p;
                    p1 = 2 * p + 1;
                    p2 = 2 * p + 2;
                    if (this.InnerList.Count > p1 && this.OnCompare(p, p1) > 0) p = p1;
                    if (this.InnerList.Count > p2 && this.OnCompare(p, p2) > 0) p = p2;

                    if (p == pn) break;
                    this.SwitchElements(p, pn);
                }

                return result;
            }
            /// <summary>
            /// Notify the PQ that the object at position i has changed
            /// and the PQ needs to restore order.
            /// Since you dont have access to any indexes (except by using the
            /// explicit IList.this) you should not call this function without knowing exactly
            /// what you do.
            /// </summary>
            /// <param name="i">The index of the changed object.</param>
            public void Update(int i)
            {
                int p = i, pn;
                int p1, p2;
                while (true)
                {
                    if (p == 0) break;
                    p2 = (p - 1) / 2;
                    if (this.OnCompare(p, p2) < 0)
                    {
                        this.SwitchElements(p, p2);
                        p = p2;
                    }
                    else break;
                }
                if (p < i) return;
                while (true)
                {
                    pn = p;
                    p1 = 2 * p + 1;
                    p2 = 2 * p + 2;

                    if (this.InnerList.Count > p1 && this.OnCompare(p, p1) > 0) p = p1;
                    if (this.InnerList.Count > p2 && this.OnCompare(p, p2) > 0) p = p2;

                    if (p == pn) break;
                    this.SwitchElements(p, pn);
                }
            }
            /// <summary>
            /// Get the smallest object without removing it.
            /// </summary>
            /// <returns>The smallest object</returns>
            public T Peek()
            {
                if (this.InnerList.Count > 0) return this.InnerList[0];
                return default(T);
            }
            public void Clear()
            {
                this.InnerList.Clear();
            }
            public int Count
            {
                get { return this.InnerList.Count; }
            }
            public void RemoveLocation(T item)
            {
                int index = -1;
                for (int i = 0; i < this.InnerList.Count; i++)
                {
                    if (this.Comparer.Compare(this.InnerList[i], item) == 0) index = i;
                }

                if (index != -1) this.InnerList.RemoveAt(index);
            }
            public T this[int index]
            {
                get { return this.InnerList[index]; }
                set
                {
                    this.InnerList[index] = value;
                    this.Update(index);
                }
            }
            #endregion
        }


        private class NodeComparer : IComparer<Node>
        {
            public int Compare(Node first, Node second)
            {
                if (first.CombinedTravelCost > second.CombinedTravelCost)
                    return 1;
                else if (first.CombinedTravelCost < second.CombinedTravelCost)
                    return -1;
                return 0;
            }
        }



    }



}
