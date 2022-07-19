namespace Entrogic
{
    public class PathVertex
    {
        public int x, y; // 顶点的X,Y坐标
        public PathVertex parent = null; // 父节点（前驱节点）,默认无父节点
        public int F, G, H; // F = G + H
        public PathVertex(int x, int y) {
            this.x = x;
            this.y = y;
        }
        public PathVertex() {

        }
    }

    public class AstarTestClass
    {
        public static int HeuristicsCostEstimate(PathVertex reachableV, PathVertex goalV) {
            int disX = reachableV.x - goalV.x;
            int disY = reachableV.y - goalV.y;
            return Math.Abs(disX) + Math.Abs(disY);
        }

        public static int GetG(PathVertex v)// 当前点与父节点为上下左右的关系，距离为1（不考虑上左，上右，下左，下右）
        {
            return v.parent != null ? v.parent.G + 1 : 0;
        }

        // 从openList中查找F值最小的顶点
        public static PathVertex GetVexOfMinFFromOpenList(List<PathVertex> openList) {
            int min = openList[0].F;
            PathVertex vexOfMinF = openList[0];
            for (int i = 1; i < openList.Count; ++i) {
                if (openList[i].F < min) {
                    min = openList[i].F;
                    vexOfMinF = openList[i];
                }
            }
            return vexOfMinF;
        }

        public static bool IsInList(int x, int y, List<PathVertex> list) {
            foreach (PathVertex v in list) {
                if (v.x == x && v.y == y) {
                    return true;
                }
            }
            return false;
        }


        public static PathVertex GetVertexFromList(int x, int y, List<PathVertex> list) {
            foreach (PathVertex v in list) {
                if (v.x == x && v.y == y) {
                    return v;
                }
            }
            return new PathVertex(0, 0);//上面一定有返回值
        }

        public static bool IsPathable(int x, int y) =>
            IsSafeTile(x, y) && IsSafeTile(x, y - 1) &&
            ((IsSafeTile(x - 1, y) && IsSafeTile(x - 1, y - 1)) ||
            (IsSafeTile(x + 1, y) && IsSafeTile(x + 1, y - 1)));

        public static bool IsSafeTile(int x, int y) {
            if (!WorldGen.InWorld(x, y))
                return false;

            var tile = Framing.GetTileSafely(x, y);
            return !WorldGen.SolidTile(tile) || tile.IsActuated || !tile.HasTile;
        }

        public List<PathVertex> AStar(PathVertex startVertex, PathVertex endVertex, int findLimits) {
            List<PathVertex> openList = new();
            List<PathVertex> closeList = new();

            short[,] direction = new short[4, 2] {
                { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 }
            };

            //初始化closeList
            //closeList.Add(startVertex);

            //初始化openList,将起点添加进去
            openList.Add(startVertex);

            int times = 0;

            // 当openList不为空时
            while (openList.Count > 0 && times <= findLimits) {
                // 遍历openList,查找F值最小的顶点
                PathVertex minVertex = GetVexOfMinFFromOpenList(openList);

                if (minVertex.x == endVertex.x && minVertex.y == endVertex.y) {
                    endVertex.parent = minVertex.parent;
                    break;
                }

                // 将F值最小的节点移入close表中，并将其作为当前要处理的节点
                openList.Remove(minVertex);
                closeList.Add(minVertex);

                // 对当前要处理节点的可到达节点进行检查,
                for (int i = 0; i < 4; ++i) {
                    int nextX = minVertex.x + direction[i, 0];
                    int nextY = minVertex.y + direction[i, 1];
                    // 不是障碍物并且不在close列表中
                    if (IsPathable(nextX, nextY) && !IsInList(nextX, nextY, closeList)) {
                        //判断是否在openList中
                        if (!IsInList(nextX, nextY, openList)) { // 不在openList中，则加入，并将当前处理节点作为它的父节点，并计算其F,G,H
                            PathVertex vex = new() {
                                x = nextX,
                                y = nextY,
                                parent = minVertex
                            };
                            vex.G = GetG(vex);
                            vex.H = HeuristicsCostEstimate(vex, endVertex);
                            vex.F = vex.G + vex.H;
                            openList.Add(vex);
                        }
                        else {
                            //从openList中获取该节点
                            PathVertex vex = GetVertexFromList(nextX, nextY, openList);
                            if (minVertex.G + 1 < vex.G) { // 如果从当前处理顶点到该顶点使得G更小
                                vex.parent = minVertex;
                                vex.G = minVertex.G + 1;
                                vex.F = vex.G + vex.H;
                            }
                        }
                    }
                }
                times++;
            }

            List<PathVertex> theWay = new();
            PathVertex v = endVertex;
            while (v.parent != null) {
                theWay.Add(v);
                v = v.parent;
            }
            return theWay;
        }
    }

    public class AStarPathfinding
    {

        public static List<PathVertex> Pathfinding(Point start, Point end, int findLimits) {
            //Console.WriteLine("start {0},{1}", start.x, start.y);
            //Console.WriteLine("end {0},{1}", end.x, end.y);
            //Console.WriteLine("{0} {1 }", mapData.Rank, mapData.GetLength(0));

            PathVertex startVertex = new(start.X, start.Y);
            PathVertex endVertex = new(end.X, end.Y);

            startVertex.F = startVertex.G = startVertex.H = 0;

            AstarTestClass aStarTest = new();

            List<PathVertex> theWay = aStarTest.AStar(startVertex, endVertex, findLimits);

            //for (int i = theWay.Count() - 1; i >= 0; --i) {
            //    Console.Write("{0},{1} ->", theWay[i].x, theWay[i].y);
            //}

            return theWay;
        }
    }
}