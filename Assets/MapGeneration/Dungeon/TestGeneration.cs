using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestGeneration : MonoBehaviour
{

    private const int NODE = 1;
    private const int EMPTY = 0;

    public int randomFillPercent;

    public string seed;
    public bool useRandomSeed;

    int[,] grid;
    private Dictionary<Node, LinkedList<Node>> graph;

    public int width, height;
    // Use this for initialization
    void Start()
    {
        grid = new int[width, height];
        for (int x = 0; x < width; x++)
        {

            for (int y = 0; y < height; y++)
            {
                grid[x, y] = EMPTY;
            }
        }
        int xR = Random.Range(0, width);
        int yR = Random.Range(0, height);
        grid[xR, yR] = NODE;
        Node root = new Node(xR, yR);
        graph = new Dictionary<Node, LinkedList<Node>>();
        graph.Add(root, createRandomNodes());

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            createRandomNodes();

    }

    public struct Node
    {
        int xCoordinate;
        int yCoordinate;

        public Node(int x, int y)
        {
            xCoordinate = x;
            yCoordinate = y;
        }
    }

    public LinkedList<Node> createRandomNodes()
    {
        LinkedList<Node> edges = new LinkedList<Node>();
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    grid[x, y] = EMPTY;
                }
                else
                {
                    if (pseudoRandom.Next(0, 100) < randomFillPercent)
                    {
                        grid[x, y] = NODE;
                        Node n = new Node(x, y);
                        edges.AddLast(n);

                    }
                    else
                    {
                        grid[x, y] = EMPTY;
                    }
                }
            }
        }
        return edges;
    }



    void OnDrawGizmos()
    {
        if (grid != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (grid[x, y] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
                    Gizmos.DrawCube(pos, Vector3.one);

                }
            }
        }
    }

		
	}
	
	


