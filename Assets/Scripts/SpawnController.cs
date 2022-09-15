using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnController
{
    public void Spawn(Vector2 gameAreaSize, ref Dictionary<Vector2, List<Vector2>> blastedPoints, ref Dictionary<Vector2, List<Vector2>> spawnPoints)
    {
        Vector2 spawnPosition = Vector2.one, bottomPos = Vector2.zero;
        List<Vector2> pointsOnColumn = new List<Vector2>();

        for (int i = 0; i < blastedPoints.Count; i++)
        {
            pointsOnColumn = blastedPoints.ElementAt(i).Value;
            bottomPos = pointsOnColumn[0];
            List<Vector2> newPoints = new List<Vector2>();
            for (int j = 0; j < pointsOnColumn.Count; j++)
            {
                spawnPosition = new Vector2(pointsOnColumn[j].x, gameAreaSize.y + j);
                newPoints.Add(spawnPosition);
                Debug.Log(i + " , " + j + " spawn pos: " + spawnPosition);
            }

            // Debug.Log(bottomPos + " bottomPos , " + "spawnPosition " + spawnPosition);
            spawnPoints.Add(bottomPos, newPoints);
        }
    }
}