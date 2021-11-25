using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingAgent : ChunkUser
{
    [SerializeField]
    private Goal goal;

    public List<Vector2Int> path = new List<Vector2Int>();
    PriorityQueue<Node> frontier = new PriorityQueue<Node>();
    public Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
    public Dictionary<Vector2Int, int> costSoFar = new Dictionary<Vector2Int, int>();
    
    protected override void Update()
    {
        base.Update();

        if (path.Count != 0) MoveToNextPathTile();
    }

    private void MoveToNextPathTile()
    {
        Vector2Int nextTile = path[0];
        transform.position = Vector3.MoveTowards(transform.position, new Vector3((float)nextTile.x, (float)nextTile.y) / GameSettings.CHUNK_SIZE, 2 * Time.deltaTime);
        
        if (Vector2.Distance(GetCurrentTilePos(), nextTile) == 0) path.RemoveAt(0);
    }

    private void DrawLines()
    {
        for (int i = 0; i < path.Count - 2; i++)
        {
            Vector2Int currentTile = path[i];
            Vector2Int nextTile = path[i + 1];
            Debug.DrawLine(
                new Vector3((float)currentTile.x, (float)currentTile.y) / GameSettings.CHUNK_SIZE,
                new Vector3((float)nextTile.x, (float)nextTile.y) / GameSettings.CHUNK_SIZE,
                Color.yellow,
                1
            );
        }
    }

    public override void OnChunkLoad()
    {
        frontier = new PriorityQueue<Node>();
        cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        costSoFar = new Dictionary<Vector2Int, int>();
        FindPath();
    }

    // Todo: Should be turned asynchronous
    public void FindPath()
    {
        if (goal == null) { 
            Debug.LogError("No goal has been defined.");
            return;
        }

        // Node startNode = path.Count != 0 ? new Node(Map.GetInstance().GetTileAt(path[path.Count -1]), path[path.Count -1]) : new Node(Map.GetInstance().GetTileAt(GetCurrentTilePos()), GetCurrentTilePos());
        Node startNode = new Node(Map.GetInstance().GetTileAt(GetCurrentTilePos()), GetCurrentTilePos());
        frontier.Enqueue(startNode, 0);
        cameFrom[GetCurrentTilePos()] = GetCurrentTilePos();
        costSoFar[GetCurrentTilePos()] = 0;

        bool nextNull = false;

        Node lastNode = null;
        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            // Debug.Log("Found path");
            // Debug.Log(Map.GetInstance().GetTileAt(GetCurrentTilePos()));

            if (current.GetTilePos() == goal.GetTilePos())
            {
                goal = null;
                break;
            }

            if (nextNull) break;

            Dictionary<Vector2Int, Tile> neighbourTiles = current.GetNeightbourTiles();
            foreach (Vector2Int nextPos in neighbourTiles.Keys)
            {
                Tile nextTile = neighbourTiles[nextPos];
                // Tile has not loaded.
                if (nextTile == null)
                {
                    nextNull = true;
                    break;
                }

                int newCost = costSoFar[current.GetTilePos()] + nextTile.GetMovementCost();
                if (!costSoFar.ContainsKey(nextPos) || newCost < costSoFar[nextPos])
                {
                    Node nextNode = new Node(nextTile, nextPos);

                    costSoFar[nextNode.GetTilePos()] = newCost;
                    // if (nextTile.GetMovementCost() == 10000) Debug.Log(costSoFar[nextNode.GetTilePos()]);
                    int priority = newCost + Heuristic(nextNode, goal);
                    frontier.Enqueue(nextNode, priority);
                    cameFrom[nextNode.GetTilePos()] = current.GetTilePos();
                }
            }

            lastNode = current;
        }

        List<Vector2Int> newPath = new List<Vector2Int>();
        newPath.Add(lastNode.GetTilePos());
        while (true)
        {
            if (newPath[newPath.Count - 1] == cameFrom[newPath[newPath.Count - 1]]) break;
            newPath.Add(cameFrom[newPath[newPath.Count - 1]]);
        }
        newPath.Reverse();
        path=(newPath);
        DrawLines();
    }

    public void SetGoal(Goal goal)
    {
        this.goal = goal;
        // Reset binary heap.
        // Add beginning tile to binary heap.
    }

    static public int Heuristic(Node a, Goal b)
    {
        return Mathf.Abs(a.GetTilePos().x - b.GetTilePos().x) + Mathf.Abs(a.GetTilePos().y - b.GetTilePos().y);
    }
}

// Todo: Move below to seperate file.
// Todo: Also should be replaced by a binary heap.
public class PriorityQueue<T>
{
    // I'm using an unsorted array for this example, but ideally this
    // would be a binary heap. There's an open issue for adding a binary
    // heap to the standard C# library: https://github.com/dotnet/corefx/issues/574
    //
    // Until then, find a binary heap class:
    // * https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp
    // * http://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx
    // * http://xfleury.github.io/graphsearch.html
    // * http://stackoverflow.com/questions/102398/priority-queue-in-net
    
    private List<Tuple<T, int>> elements = new List<Tuple<T, int>>();

    public int Count
    {
        get { return elements.Count; }
    }
    
    public void Enqueue(T item, int priority)
    {
        elements.Add(Tuple.Create(item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++) {
            if (elements[i].Item2 < elements[bestIndex].Item2) {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Item1;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }
}