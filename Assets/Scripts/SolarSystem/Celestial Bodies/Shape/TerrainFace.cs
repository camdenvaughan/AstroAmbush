﻿using UnityEngine;

public class TerrainFace
{
    private IShapeGenerator _planetShapeGenerator;
    private Mesh mesh;
    private int resolution;
    private Vector3 localUp;

    private Vector3 axisA;
    private Vector3 axisB;

    public TerrainFace(IShapeGenerator planetShapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        this._planetShapeGenerator = planetShapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;
        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int [(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;
        
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                Vector3 pointOnUnitShere = pointOnUnitCube.normalized;
                vertices[i] = _planetShapeGenerator.CalculatePointOnPlanet(pointOnUnitShere);

                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;
                    
                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;
                    triIndex += 6;
                }
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
