using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class MeshGenerator : MonoBehaviour
{

    public SquareGrid squareGrid;

    public void GenerateMesh(int[,] map, float squareSize)
    {
        squareGrid = new SquareGrid(map,squareSize);
    }
   

	public class  Square
	{
	    public ControlNode TopLeftNode, TopRightNode, BottomLeftNode, BottomRightNode;
	    public Node CenterTopNode, CenterRightNode, CenterBottomNode, CenterLeftNode;

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
	    }
	   
	}
     public class SquareGrid
     {
         public Square[,] squares;

         public SquareGrid(int[,] map, float squareSize)
         {
             int nodeCountX = map.GetLength(0);
             int nodeCountY = map.GetLength(1);
             float mapWidth = nodeCountX*squareSize;
             float mapHeight = nodeCountY*squareSize;

             ControlNode [,] controlNode = new ControlNode[nodeCountX,nodeCountY];

             for (int x = 0; x < nodeCountX; x++)
             {
                 for (int y = 0; y < nodeCountY; y++)
                 {
                     Vector3 pos= new Vector3(-mapWidth/2 + x*squareSize +squareSize/2,0,-mapHeight/2 +y*squareSize +squareSize/2);
                     controlNode[x,y]=new ControlNode(pos,map[x,y]==1,squareSize);
                 }
             }
             squares= new Square[nodeCountX-1,nodeCountY-1];
             for (int x = 0; x < nodeCountX-1; x++)
             {
                 for (int y = 0; y < nodeCountY-1; y++)
                 {
                     squares[x,y]= new Square(controlNode[x,y+1],controlNode[x+1,y+1],controlNode[x+1,y],controlNode[x,y]);
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
        public ControlNode(Vector3 _pos, bool _active, float squareSize) : base(_pos)
        {
            Active = _active;
            Above= new Node(Position+Vector3.forward*squareSize/2f);
            Right = new Node(Position + Vector3.right*squareSize/2f);
        }
    }

    void OnDrawGizmos()
    {
        if (squareGrid != null)
        {
             for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
             {
                 for (int y = 0; y < squareGrid.squares.GetLength(1) ; y++)
                 {
                     Gizmos.color = (squareGrid.squares[x, y].TopLeftNode.Active) ? Color.black : Color.white;
                     Gizmos.DrawCube(squareGrid.squares[x, y].TopLeftNode.Position, Vector3.one * .4f);

                     Gizmos.color = (squareGrid.squares[x, y].TopRightNode.Active) ? Color.black : Color.white;
                     Gizmos.DrawCube(squareGrid.squares[x, y].TopRightNode.Position, Vector3.one * .4f);

                     Gizmos.color = (squareGrid.squares[x, y].BottomRightNode.Active) ? Color.black : Color.white;
                     Gizmos.DrawCube(squareGrid.squares[x, y].BottomRightNode.Position, Vector3.one * .4f);

                     Gizmos.color = (squareGrid.squares[x, y].BottomLeftNode.Active) ? Color.black : Color.white;
                     Gizmos.DrawCube(squareGrid.squares[x, y].BottomLeftNode.Position, Vector3.one * .4f);


                     Gizmos.color = Color.grey;
                     Gizmos.DrawCube(squareGrid.squares[x, y].CenterTopNode.Position, Vector3.one * .15f);
                     Gizmos.DrawCube(squareGrid.squares[x, y].CenterRightNode.Position, Vector3.one * .15f);
                     Gizmos.DrawCube(squareGrid.squares[x, y].CenterBottomNode.Position, Vector3.one * .15f);
                     Gizmos.DrawCube(squareGrid.squares[x, y].CenterLeftNode.Position, Vector3.one * .15f);
                 }
             }
        }
    }
}
