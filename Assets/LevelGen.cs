using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

[System.Serializable]
public class ChunkVert
{
    public bool[] sides;
    public bool added;
    public int rotation;

    public int height;

    public ChunkVert()
    {
        sides = new bool[4];
        added = false;
        rotation = 0;
        height = 0;
    }

    public void SetupForRealChunk()
    {
        int count = 0;
        for (int index = 0; index < 4; index++)
        {
            if (sides[index])
                count++;
        }
        if (count == 4)
        {
            rotation = Random.Range(0, 5);
        }
        else if (count == 3)
        {
            if (sides[0] == false)
                rotation = 0;
            else if (sides[1] == false)
                rotation = 1;
            else if (sides[2] == false)
                rotation = 2;
            else if (sides[3] == false)
                rotation = 3;
        }
        else if (count == 2)
        {
            if (sides[0] == true && sides[2] == true)
            {
                rotation = Random.Range(0, 2) * 2;
            }
            else if (sides[1] == true && sides[3] == true)
            {
                rotation = 1 + Random.Range(0, 2) * 2;
            }
            else if (sides[0] == true && sides[1] == true)
            {
                rotation = 0;
            }
            else if (sides[1] == true && sides[2] == true)
            {
                rotation = 1;
            }
            else if (sides[2] == true && sides[3] == true)
            {
                rotation = 2;
            }
            else if (sides[3] == true && sides[0] == true)
            {
                rotation = 3;
            }
        }
        else if (count == 1)
        {
            if (sides[0] == true)
                rotation = 0;
            else if (sides[1] == true)
                rotation = 1;
            else if (sides[2] == true)
                rotation = 2;
            else if (sides[3] == true)
                rotation = 3;
        }

    }

}

[System.Serializable]
public struct ChunkEdge
{
    public int x;
    public int z;
    public bool horizontal;

    public ChunkEdge(int x, int z, bool horizontal)
    {
        this.x = x;
        this.z = z;
        this.horizontal = horizontal;
    }

    void Set(int x1, int z1, int x2, int z2)
    {
        if (x1 == x2)
            horizontal = true;
        else
            horizontal = false;
        x = Mathf.Min(x1, x2);
        z = Mathf.Min(z1, z2);
    }
}


public class LevelGen : NetworkBehaviour
{
    public int size = 5;
    public int tileSize = 3;
    public int height = 3;

    ChunkVert[,] verts;

    void Start()
    {
        GenGraph();
        DrawLevel();

    }

    public void GenGraph()
    {
        verts = new ChunkVert[size, size];
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                verts[z, x] = new ChunkVert();
            }
        }

        List<ChunkEdge> availableEdges = new List<ChunkEdge>();
        AddVert(size / 2, size / 2, ref availableEdges);
        while (availableEdges.Count > 0)
        {
            ChunkEdge randomEdge = availableEdges[Random.Range(0, availableEdges.Count)];
            SetSides(randomEdge);
            int randVertX = randomEdge.x;
            int randVertZ = randomEdge.z;
            if (verts[randVertZ, randVertX].added == true)
            {
                if (randomEdge.horizontal)
                    randVertX = randVertX + 1;
                else
                    randVertZ = randVertZ + 1;
            }
            AddVert(randVertX, randVertZ, ref availableEdges);
        }

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                verts[z, x].SetupForRealChunk();
            }
        }
        
    }

    void AddVert(int x, int z, ref List<ChunkEdge> edges)
    {
        ChunkEdge up = new ChunkEdge(x, z, false);
        ChunkEdge down = new ChunkEdge(x, z - 1, false);
        ChunkEdge right = new ChunkEdge(x, z, true);
        ChunkEdge left = new ChunkEdge(x - 1, z, true);
        AddOrRemoveEdge(up, ref edges);
        AddOrRemoveEdge(down, ref edges);
        AddOrRemoveEdge(right, ref edges);
        AddOrRemoveEdge(left, ref edges);
        verts[z, x].added = true;
    }

    void AddOrRemoveEdge(ChunkEdge edge, ref List<ChunkEdge> edges)
    {
        int size = verts.GetLength(0);
        if (edge.horizontal == true && (edge.x < 0 || edge.x >= size - 1))
            return;
        if (edge.horizontal == false && (edge.z < 0 || edge.z >= size - 1))
            return;
        if (edges.Contains(edge))
            edges.Remove(edge);
        else
            edges.Add(edge);
    }

    void SetSides(ChunkEdge edge)
    {
        if (edge.horizontal == true)
        {
            verts[edge.z, edge.x].sides[1] = true;
            verts[edge.z, edge.x + 1].sides[3] = true;
        }
        else {
            verts[edge.z, edge.x].sides[0] = true;
            verts[edge.z + 1, edge.x].sides[2] = true;
        }
    }

    public void DrawLevel()
    {
        int tileGridSize = 2 * size + 1;
        bool[,] realTiles = new bool[tileGridSize,tileGridSize];
        for (int z = 0; z < tileGridSize; z++)
        {
            for (int x = 0; x < tileGridSize; x++)
            {
                realTiles[z, x] = true;
            }
        }

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                int zReal = z * 2 + 1;
                int xReal = x * 2 + 1;
                realTiles[zReal, xReal] = false;
                
                realTiles[zReal + 1, xReal] = !verts[z, x].sides[0];
                realTiles[zReal, xReal+1] = !verts[z, x].sides[1];
                realTiles[zReal - 1, xReal] = !verts[z, x].sides[2];
                realTiles[zReal, xReal -1] = !verts[z, x].sides[3];
            }
        }

        int zPos = 0;
        for (int z = 0; z < tileGridSize; z++)
        {
            int xPos = 0;
            for (int x = 0; x < tileGridSize; x++)
            {
                if (realTiles[z, x])
                {
                    GameObject temp = (GameObject)GameObject.CreatePrimitive(PrimitiveType.Cube);
                    temp.transform.position = new Vector3(xPos, height / 2, zPos);
                    Vector3 newScale = Vector3.one;
                    if (x % 2 == 1)
                        newScale.x = tileSize;
                    if (z % 2 == 1)
                        newScale.z = tileSize;
                    newScale.y = height;
                    temp.transform.localScale = newScale;
                }
                xPos += (1 + tileSize) / 2;
            }
            zPos += (1 + tileSize) / 2;
        }
        GameObject plane = (GameObject)GameObject.CreatePrimitive(PrimitiveType.Quad);
        float realWorldSize = tileGridSize * (1 + tileSize) / 2 - .5f;
        plane.transform.position = new Vector3(realWorldSize / 2, 0, realWorldSize / 2);
        plane.transform.localScale = Vector3.one * realWorldSize;
        plane.transform.rotation = Quaternion.Euler(90, 0, 0);
        
        plane.GetComponent<MeshRenderer>().material.color = Color.gray;
    }
}

