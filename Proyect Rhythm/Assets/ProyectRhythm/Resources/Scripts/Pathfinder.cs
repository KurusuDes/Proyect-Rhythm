using _Scripts.Tiles;
using RP.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private static readonly Color PathColor = new Color(0.65f, 0.35f, 0.35f);
    private static readonly Color OpenColor = new Color(.4f, .6f, .4f);
    private static readonly Color ClosedColor = new Color(0.35f, 0.4f, 0.5f);
    public static List<BaseHexagon> FindPath(BaseHexagon startNode, BaseHexagon targetNode,bool debugMode = false)
    {
        var toSearch = new List<BaseHexagon>() { startNode };
        var processed = new List<BaseHexagon>();

        while (toSearch.Any())
        {
            var current = toSearch[0];
            foreach (var t in toSearch)
                if (t.F < current.F || t.F == current.F && t.H < current.H) current = t;

            processed.Add(current);
            toSearch.Remove(current);

            if(debugMode)
                current.SetColor(ClosedColor);

            if (current == targetNode)
            {
                var currentPathTile = targetNode;
                var path = new List<BaseHexagon>();
                var count = 100;
                while (currentPathTile != startNode)
                {
                    path.Add(currentPathTile);
                    currentPathTile = currentPathTile.Connection;
                    count--;
                    if (count < 0) throw new Exception();
                }

                if (debugMode)
                {
                    foreach (var tile in path) tile.SetColor(PathColor);
                    startNode.SetColor(PathColor);
                }
                //Debug.Log(path.Count);
                return path;
            }

            foreach (var neighbor in current.Neighbors.Where(t => t.Walkable && !processed.Contains(t)))
            {
                var inSearch = toSearch.Contains(neighbor);

                var costToNeighbor = current.G + current.GetDistance(neighbor);

                if (!inSearch || costToNeighbor < neighbor.G)
                {
                    neighbor.SetG(costToNeighbor);
                    neighbor.SetConnection(current);

                    if (!inSearch)
                    {
                        neighbor.SetH(neighbor.GetDistance(targetNode));
                        toSearch.Add(neighbor);
                        if (debugMode)
                            neighbor.SetColor(OpenColor);
                    }
                }
            }
        }
        return null;
    }
}
