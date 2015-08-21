using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public class MeshGenerator : MonoBehaviour
{
    List<Vector3> vertices;
    List<int> triangles;
    public SquareGrid squareGrid;
    public MeshFilter walls;

    private Dictionary<int, List<Triangle>> trianglDictionary=new Dictionary<int, List<Triangle>>(); 
    List<List<int>> outlines= new List<List<int>>();
    HashSet<int> checkedVertices= new HashSet<int>(); 

    public void GenerateMesh(int[,] map, float squareSize)
    {
        trianglDictionary.Clear();
        outlines.Clear();
        checkedVertices.Clear();


        squareGrid = new SquareGrid(map, squareSize);
        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
        {
            for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
            {
                TriangulateSquare(squareGrid.squares[x, y]);

            }
        }

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        CreateWallMesh();

    }

    void CreateWallMesh()
    {

        CalculateMeshOutlines();

        List<Vector3> wallVertices = new List<Vector3>();
        List<int> wallTriangles = new List<int>();
        Mesh wallMesh = new Mesh();
        float wallHeight = 5;

        foreach (List<int> outline in outlines)
        {
            for (int i = 0; i < outline.Count - 1; i++)
            {
                int startIndex = wallVertices.Count;
                wallVertices.Add(vertices[outline[i]]); //left vertex
                wallVertices.Add(vertices[outline[i+1]]); //right vertex
                wallVertices.Add(vertices[outline[i]]-Vector3.up *wallHeight); //bottom left vertex
                wallVertices.Add(vertices[outline[i + 1]] - Vector3.up * wallHeight); //bottom right vertex

                wallTriangles.Add(startIndex+0);
                wallTriangles.Add(startIndex+2);
                wallTriangles.Add(startIndex+3);

                wallTriangles.Add(startIndex + 3);
                wallTriangles.Add(startIndex + 1);
                wallTriangles.Add(startIndex + 0);
                walls.mesh = wallMesh;
            }
        }
        wallMesh.vertices = wallVertices.ToArray();
        wallMesh.triangles = wallTriangles.ToArray();

    }
    void TriangulateSquare(Square square)
    {
        switch (square.Configuration)
        {
            case 0:
                break;

            // 1 points:
            case 1:
                MeshFromPoints(square.CenterLeftNode, square.CenterBottomNode, square.BottomLeftNode);
                break;
            case 2:
                MeshFromPoints(square.BottomRightNode, square.CenterBottomNode, square.CenterRightNode);
                break;
            case 4:
                MeshFromPoints(square.TopRightNode, square.CenterRightNode, square.CenterTopNode);
                break;
            case 8:
                MeshFromPoints(square.TopLeftNode, square.CenterTopNode, square.CenterLeftNode);
                break;

            // 2 points:
            case 3:
                MeshFromPoints(square.CenterRightNode, square.BottomRightNode, square.BottomLeftNode, square.CenterLeftNode);
                break;
            case 6:
                MeshFromPoints(square.CenterTopNode, square.TopRightNode, square.BottomRightNode, square.CenterBottomNode);
                break;
            case 9:
                MeshFromPoints(square.TopLeftNode, square.CenterTopNode, square.CenterBottomNode, square.BottomLeftNode);
                break;
            case 12:
                MeshFromPoints(square.TopLeftNode, square.TopRightNode, square.CenterRightNode, square.CenterLeftNode);
                break;
            case 5:
                MeshFromPoints(square.CenterTopNode, square.TopRightNode, square.CenterRightNode, square.CenterBottomNode, square.BottomLeftNode, square.CenterLeftNode);
                break;
            case 10:
                MeshFromPoints(square.TopLeftNode, square.CenterTopNode, square.CenterRightNode, square.BottomRightNode, square.CenterBottomNode, square.CenterLeftNode);
                break;

            // 3 point:
            case 7:
                MeshFromPoints(square.CenterTopNode, square.TopRightNode, square.BottomRightNode, square.BottomLeftNode, square.CenterLeftNode);
                break;
            case 11:
                MeshFromPoints(square.TopLeftNode, square.CenterTopNode, square.CenterRightNode, square.BottomRightNode, square.BottomLeftNode);
                break;
            case 13:
                MeshFromPoints(square.TopLeftNode, square.TopRightNode, square.CenterRightNode, square.CenterBottomNode, square.BottomLeftNode);
                break;
            case 14:
                MeshFromPoints(square.TopLeftNode, square.TopRightNode, square.BottomRightNode, square.CenterBottomNode, square.CenterLeftNode);
                break;

            // 4 point:
            case 15:
                MeshFromPoints(square.TopLeftNode, square.TopRightNode, square.BottomRightNode, square.BottomLeftNode);
                checkedVertices.Add(square.TopLeftNode.VertexIndex);
                checkedVertices.Add(square.TopRightNode.VertexIndex);
                checkedVertices.Add(square.BottomLeftNode.VertexIndex);
                checkedVertices.Add(square.BottomRightNode.VertexIndex);
                break;
        }

    }

    void MeshFromPoints(params Node[] points)
    {
        AssignVertices(points);
        if (points.Length >= 3)
        {
            CreateTriangle(points[0], points[1], points[2]);
        }
        if (points.Length >= 4)
        {
            CreateTriangle(points[0], points[2], points[3]);
        }
        if (points.Length >= 5)
        {
            CreateTriangle(points[0], points[3], points[4]);
        }
        if (points.Length >= 6)
        {
            CreateTriangle(points[0], points[4], points[5]);
        }
    }

    void AssignVertices(Node[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].VertexIndex == -1)
            {
                points[i].VertexIndex = vertices.Count;
                vertices.Add(points[i].Position);
            }
        }
    }

    void CreateTriangle(Node a, Node b, Node c)
    {
        triangles.Add(a.VertexIndex);
        triangles.Add(b.VertexIndex);
        triangles.Add(c.VertexIndex);

        Triangle triangle = new Triangle(a.VertexIndex,b.VertexIndex,c.VertexIndex);
        AddTriangleToDictionary(triangle.vertexIndexA,triangle);
        AddTriangleToDictionary(triangle.vertexIndexB, triangle);
        AddTriangleToDictionary(triangle.vertexIndexC, triangle);
    }

    void AddTriangleToDictionary(int vertexIndexKey, Triangle triangle)
    {
        if (trianglDictionary.ContainsKey(vertexIndexKey))
        {
            trianglDictionary[vertexIndexKey].Add(triangle);
        }
        else
        {
            List<Triangle> triangleList = new List<Triangle>();
            triangleList.Add(triangle);
            trianglDictionary.Add(vertexIndexKey,triangleList);
        }
    }

    void FollowOutline(int vertexIndex,int outlineIndex )
    {
        outlines[outlineIndex].Add(vertexIndex);
        checkedVertices.Add(vertexIndex);
        int nextVertexIndex = GetConnectedOutlineVertex(vertexIndex);

        if (nextVertexIndex != -1)
        {
            FollowOutline(nextVertexIndex,outlineIndex);
        }
    }
    void CalculateMeshOutlines()
    {
        for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
        {
            if (!checkedVertices.Contains(vertexIndex))
            {
                int newOutlineVertex = GetConnectedOutlineVertex(vertexIndex);
                if (newOutlineVertex != -1)
                {
                    checkedVertices.Add(vertexIndex);

                    List<int> newOutline= new List<int>();
                    outlines.Add(newOutline);
                    FollowOutline(newOutlineVertex, outlines.Count - 1);
                    outlines[outlines.Count-1].Add(vertexIndex);
                }
            }
        }
    }
    int GetConnectedOutlineVertex(int vertexIndex)
    {
        List<Triangle> trianglesContainingVertex = trianglDictionary[vertexIndex];

        for (int i = 0; i < trianglesContainingVertex.Count; i++)
        {
            Triangle triangle = trianglesContainingVertex[i];

            for (int j = 0; j < 3; j++)
            {
                int vertexB = triangle[j];

                if (vertexB != vertexIndex && !checkedVertices.Contains(vertexB))
                {
                    if (IsOutLineEdge(vertexIndex, vertexB))
                    {
                        return vertexB;
                    }
                } 
            }
        }
        return -1;
    }
    bool IsOutLineEdge(int vertexA, int vertexB)
    {
        List<Triangle> trianglesContainingVertexA = trianglDictionary[vertexA];
        int sharedTriangleCount = 0;

        for (int i = 0; i < trianglesContainingVertexA.Count; i++)
        {
            if (trianglesContainingVertexA[i].Contains(vertexB))
            {
                sharedTriangleCount++;
                if (sharedTriangleCount > 1)
                {
                    break;
                }
            }
        }
        return sharedTriangleCount == 1;
    }

    public class Square
    {
        public ControlNode TopLeftNode, TopRightNode, BottomLeftNode, BottomRightNode;
        public Node CenterTopNode, CenterRightNode, CenterBottomNode, CenterLeftNode;
        public int Configuration;

        public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft)
        {
            TopLeftNode = _topLeft;
            TopRightNode = _topRight;
            BottomRightNode = _bottomRight;
            BottomLeftNode = _bottomLeft;

            CenterTopNode = TopLeftNode.Right;
            CenterRightNode = BottomRightNode.Above;
            CenterBottomNode = BottomLeftNode.Right;
            CenterLeftNode = BottomLeftNode.Above;

            if (TopLeftNode.Active)
            {
                Configuration += 8;
            }
            if (TopRightNode.Active)
            {
                Configuration += 4;
            }
            if (BottomRightNode.Active)
            {
                Configuration += 2;
            }
            if (BottomLeftNode.Active)
            {
                Configuration += 1;
            }
        }

    }
    public class SquareGrid
    {
        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNode[,] controlNode = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector3 pos = new Vector3(-mapWidth / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2);
                    controlNode[x, y] = new ControlNode(pos, map[x, y] == 1, squareSize);
                }
            }
            squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for (int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
                    squares[x, y] = new Square(controlNode[x, y + 1], controlNode[x + 1, y + 1], controlNode[x + 1, y], controlNode[x, y]);
                }
            }

        }

    }

    public class Node
    {
        public Vector3 Position;
        public int VertexIndex = -1;

        public Node(Vector3 _pos)
        {
            Position = _pos;
        }
    }

    public class ControlNode : Node
    {
        public bool Active;
        public Node Above, Right;
        public ControlNode(Vector3 _pos, bool _active, float squareSize)
            : base(_pos)
        {
            Active = _active;
            Above = new Node(Position + Vector3.forward * squareSize / 2f);
            Right = new Node(Position + Vector3.right * squareSize / 2f);
        }
    }

    struct Triangle
    {
        public int vertexIndexA;
        public int vertexIndexB;
        public int vertexIndexC;
        private int[] vertices;

        public Triangle(int a, int b, int c)
        {
            vertexIndexA = a;
            vertexIndexB = b;
            vertexIndexC = c;

            vertices = new int[3];
            vertices[0] = a;
            vertices[1] = b;
            vertices[2] = c;

        }

        public int this[int i]
        {
            get { return vertices[i]; }
        }
        public bool Contains(int vertexIndex)
        {
            return vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC;
        }
    }

}
