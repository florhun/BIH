using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleMove : MonoBehaviour
{
    public PolygonCollider2D holeColider;
    public PolygonCollider2D groundColider;
    public MeshCollider meshCollider;
    public float initScale = .5f;
    public TouchInput move;
    Mesh generatedMesh;


    private float screenWidth, screenHeight;
    public void Awake()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;
    }

    private void FixedUpdate()
    {
        if(transform.hasChanged == true)
        {
            transform.hasChanged = false;
            holeColider.transform.position = new Vector2(transform.position.x, transform.position.z);
            holeColider.transform.localScale = transform.localScale * initScale;
            MakeHole();
            MakeMesh();
        }

    }

    private void MakeHole()
    {
        Vector2[] PointPositions = holeColider.GetPath(0);
        for (int i = 0; i < PointPositions.Length; i++)
        {
            PointPositions[i] += (Vector2)holeColider.transform.TransformPoint(PointPositions[i]);
        }

        groundColider.pathCount = 2;
        groundColider.SetPath(1, PointPositions);
    }

    private void MakeMesh()
    {
        if (generatedMesh != null) Destroy(generatedMesh);
        generatedMesh = groundColider.CreateMesh(true, true);
        meshCollider.sharedMesh = generatedMesh;
    }
   
}
