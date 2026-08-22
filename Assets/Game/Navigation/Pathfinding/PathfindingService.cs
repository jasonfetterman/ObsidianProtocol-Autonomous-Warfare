using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Pathfinding
{
    public sealed class PathfindingService : MonoBehaviour
    {
        private sealed class Node
        {
            public Vector2Int Coordinates;
            public Vector3 WorldPosition;
            public bool Walkable = true;
            public float TraversalCost = 1f;

            public float GCost = float.PositiveInfinity;
            public float HCost;
            public Node Parent;

            public float FCost => GCost + HCost;
        }

        [SerializeField] private PathfindingDefinition definition;

        private Node[,] nodes;

        public PathfindingDefinition Definition => definition;

        private static readonly Vector2Int[] Directions =
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, -1)
        };

        private void Awake()
        {
            BuildGrid();
        }

        public void BuildGrid()
        {
            if (definition == null)
            {
                Debug.LogError(
                    "PathfindingService requires a PathfindingDefinition.",
                    this);

                nodes = null;
                return;
            }

            nodes = new Node[
                definition.GridWidth,
                definition.GridHeight];

            Vector3 origin = transform.position;

            for (int x = 0; x < definition.GridWidth; x++)
            {
                for (int y = 0; y < definition.GridHeight; y++)
                {
                    Vector3 worldPosition =
                        origin +
                        new Vector3(
                            (x + 0.5f) * definition.NodeSize,
                            0f,
                            (y + 0.5f) * definition.NodeSize);

                    nodes[x, y] = new Node
                    {
                        Coordinates = new Vector2Int(x, y),
                        WorldPosition = worldPosition
                    };
                }
            }
        }

        public bool TryBuildPath(
            Vector3 start,
            Vector3 destination,
            List<Vector3> path)
        {
            if (path == null)
            {
                return false;
            }

            path.Clear();

            if (definition == null)
            {
                return false;
            }

            if (nodes == null)
            {
                BuildGrid();
            }

            if (nodes == null)
            {
                return false;
            }

            Node startNode = GetClosestNode(start);
            Node destinationNode = GetClosestNode(destination);

            if (startNode == null || destinationNode == null)
            {
                return false;
            }

            if (!startNode.Walkable || !destinationNode.Walkable)
            {
                return false;
            }

            ResetSearchState();

            var openSet = new List<Node>();
            var closedSet = new HashSet<Node>();

            startNode.GCost = 0f;
            startNode.HCost =
                Heuristic(startNode, destinationNode);

            openSet.Add(startNode);

            int iterations = 0;

            while (openSet.Count > 0 &&
                   iterations < definition.MaximumIterations)
            {
                iterations++;

                Node current = GetLowestCostNode(openSet);

                if (current == destinationNode)
                {
                    BuildResultPath(
                        start,
                        destination,
                        destinationNode,
                        path);

                    return true;
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (Node neighbour in GetNeighbours(current))
                {
                    if (neighbour == null ||
                        !neighbour.Walkable ||
                        closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    float movementCost =
                        Distance(current, neighbour) *
                        Mathf.Max(0.01f, neighbour.TraversalCost);

                    float tentativeG =
                        current.GCost + movementCost;

                    if (tentativeG < neighbour.GCost)
                    {
                        neighbour.Parent = current;
                        neighbour.GCost = tentativeG;
                        neighbour.HCost =
                            Heuristic(neighbour, destinationNode);

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }

            return false;
        }

        public void SetNodeWalkable(
            Vector2Int coordinates,
            bool walkable)
        {
            Node node = GetNode(coordinates);

            if (node != null)
            {
                node.Walkable = walkable;
            }
        }

        public void SetNodeTraversalCost(
            Vector2Int coordinates,
            float traversalCost)
        {
            Node node = GetNode(coordinates);

            if (node != null)
            {
                node.TraversalCost =
                    Mathf.Max(0.01f, traversalCost);
            }
        }

        private Node GetNode(Vector2Int coordinates)
        {
            if (nodes == null ||
                coordinates.x < 0 ||
                coordinates.y < 0 ||
                coordinates.x >= nodes.GetLength(0) ||
                coordinates.y >= nodes.GetLength(1))
            {
                return null;
            }

            return nodes[
                coordinates.x,
                coordinates.y];
        }

        private Node GetClosestNode(Vector3 worldPosition)
        {
            if (nodes == null)
            {
                return null;
            }

            float bestDistance = float.PositiveInfinity;
            Node closest = null;

            for (int x = 0; x < nodes.GetLength(0); x++)
            {
                for (int y = 0; y < nodes.GetLength(1); y++)
                {
                    Node node = nodes[x, y];

                    float distance =
                        (node.WorldPosition - worldPosition).sqrMagnitude;

                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        closest = node;
                    }
                }
            }

            return closest;
        }

        private IEnumerable<Node> GetNeighbours(Node node)
        {
            foreach (Vector2Int direction in Directions)
            {
                Vector2Int coordinates =
                    node.Coordinates + direction;

                Node neighbour = GetNode(coordinates);

                if (neighbour != null)
                {
                    yield return neighbour;
                }
            }
        }

        private static float Distance(Node a, Node b)
        {
            return Vector3.Distance(
                a.WorldPosition,
                b.WorldPosition);
        }

        private static float Heuristic(Node a, Node b)
        {
            return Vector3.Distance(
                a.WorldPosition,
                b.WorldPosition);
        }

        private static Node GetLowestCostNode(
            List<Node> openSet)
        {
            Node best = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                Node candidate = openSet[i];

                if (candidate.FCost < best.FCost ||
                    (Mathf.Approximately(
                        candidate.FCost,
                        best.FCost) &&
                     candidate.HCost < best.HCost))
                {
                    best = candidate;
                }
            }

            return best;
        }

        private static void BuildResultPath(
            Vector3 start,
            Vector3 destination,
            Node destinationNode,
            List<Vector3> path)
        {
            var reversePath = new List<Vector3>();

            Node current = destinationNode;

            while (current != null)
            {
                reversePath.Add(current.WorldPosition);
                current = current.Parent;
            }

            reversePath.Reverse();

            path.Add(start);

            for (int i = 0; i < reversePath.Count; i++)
            {
                path.Add(reversePath[i]);
            }

            if (path.Count == 0 ||
                Vector3.Distance(
                    path[path.Count - 1],
                    destination) > 0.01f)
            {
                path.Add(destination);
            }
        }

        private void ResetSearchState()
        {
            if (nodes == null)
            {
                return;
            }

            for (int x = 0; x < nodes.GetLength(0); x++)
            {
                for (int y = 0; y < nodes.GetLength(1); y++)
                {
                    nodes[x, y].GCost =
                        float.PositiveInfinity;

                    nodes[x, y].HCost = 0f;
                    nodes[x, y].Parent = null;
                }
            }
        }
    }
}

