using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Match3 : MonoBehaviour
{
    public static float PieceSize = 128;

    public Board boardLayout;
	public float pieceSize = 128f;
	public int[] pieceScores;

	[Header("UI Elements")]
    public Sprite[] pices;
	public RectTransform gameBoard;
	public RectTransform KilledBoard; 

	[Header("Prefabs")]
    public GameObject nodePice;
    public GameObject KilledPiece;

    int width = 9;
    int height = 7;
    int[] fills;
    Node[,] board;

    List<NodePieces> update;
    List<FlippedPieces> flipped;
    List<NodePieces> dead;
    List<KilledPiece> killed;

    System.Random random;

    void Start()
    {
        PieceSize = pieceSize;
		StartGame();
    }

    void Update()
    {
        List<NodePieces> finishedUpdating = new();
        for (int i = 0; i <update.Count; i++)
        {
            NodePieces piece = update[i];
            if(!piece.UpdatePice())
            {
                finishedUpdating.Add(piece);
            }

        }

        for (int i = 0; i < finishedUpdating.Count; i++)
        {
            NodePieces piece = finishedUpdating[i];
            FlippedPieces flip = GetFlipped(piece);
            NodePieces flippedPiece = null;

            int x = (int)piece.index.x;
            fills[x] = Mathf.Clamp(fills[x] - 1, 0, width);

            List<Point> connected = IsConnected(piece.index, true);
            bool wasFllipped = (flip != null);
            
            if (wasFllipped) //If we flipped to make this update
            {
                flippedPiece = flip.GetOtherPieces(piece);
                AddPoints(ref connected, IsConnected(flippedPiece.index, true));
            }
            if(connected.Count == 0)  //If we didn't make a match
            {
                if (wasFllipped) //If we flipped
                {
                    FlipPieces(piece.index, flippedPiece.index, false); //Flip back
                }
            }
            else //If we made a match
            {
                foreach (Point ptn in connected) //Remove the node pieces connected
                {
                    KillPiece(ptn);
                    Node node = GetNodeAtPoint(ptn);
                    NodePieces nodePieces = node.GetPiece();
                    if(nodePieces != null) 
                    {
                        nodePieces.gameObject.SetActive(false);
                        dead.Add(nodePieces);
                    }

                    node.SetPiece(null);
                }

                ApplyGravityToBoard();
            }

            flipped.Remove(flip); //Remove the flip after update
            update.Remove(piece);
        }
    }

    void ApplyGravityToBoard() 
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = (height - 1); y >= 0; y--)
            {
                Point p = new Point(x, y);
                Node node = GetNodeAtPoint(p);
                int val = GetValueAtPoint(p);
                if(val != 0) //If it is not hole, do nothing 
                {
                    continue;
                }

                for (int ny = (y-1); ny >= -1; ny--)
                {
                    Point next = new Point(x, ny);
                    int nextVal = GetValueAtPoint(next);
                    if(nextVal == 0) 
                    {
                        continue;
                    }

                    if (nextVal != -1) //If we did not hit an end, but its not 0 then use this to fill the current hole
                    {
                        Node got = GetNodeAtPoint(next);
                        NodePieces piece = got.GetPiece();

                        //Set the hole
                        node.SetPiece(piece);
                        update.Add(piece);

                        //Replace the hole
                        got.SetPiece(null);
                    }
                    else //Hit an end 
                    {
                        //Fill in the hole
                        int newVal = FillPiece();
                        NodePieces piece;
                        Point fallPnt = new Point(x, -1 - fills[x]);
                        if (dead.Count > 0)
                        {
                            NodePieces revived = dead[0];
                            revived.gameObject.SetActive(true);
                            piece = revived;

                            dead.RemoveAt(0);
                        }
                        else
                        {
                            GameObject obj = Instantiate(nodePice, gameBoard);
                            NodePieces n = obj.GetComponent<NodePieces>();
                            
                            piece = n;
                        }

                        piece.Initialize(newVal, p, pices[newVal - 1], pieceScores[newVal - 1]);
                        piece.rect.anchoredPosition = GetPositionFromPoint(fallPnt);

                        Node hole = GetNodeAtPoint(p);
                        hole.SetPiece(piece);
                        ResetPiece(piece);

                        fills[x]++;
                    }
                    break;
                }
            }
        }
    }

    FlippedPieces GetFlipped(NodePieces p) 
    {
        FlippedPieces flip = null;
        for (int i = 0; i < flipped.Count; i++)
        {
            if (flipped[i].GetOtherPieces(p) != null)
            {
                flip = flipped[i];
                break;
            }
        }
        return flip;
    }

    void StartGame() 
    {

        fills = new int[width];
        string seed = GetRandomSeed();
        random = new System.Random(seed.GetHashCode());
        update = new List<NodePieces>();
        flipped = new List<FlippedPieces>();
        dead = new List<NodePieces>();
        killed = new List<KilledPiece>();

        InicializeBoard();
        VerifyBoard();
        InstantiateBoard();
    }

    void InicializeBoard() 
    {
        board = new Node[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0;  x < width; x++)
            {
                board[x, y] = new Node((boardLayout.rows[y].row[x]) ? -1 : FillPiece(), new Point(x, y));
            }
        }
    }

    void VerifyBoard() 
    {
        List<int> remove;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0;  y < height;  y++)
            {
                Point p = new(x, y);
                int val = GetValueAtPoint(p);
                if(val <= 0) 
                {
                    continue;
                }

                remove = new List<int>();
                while (IsConnected(p, true).Count > 0)
                {
                    val = GetValueAtPoint(p);
                    if (!remove.Contains(val)) 
                    {
                        remove.Add(val);
                    }

                    SetValueAtPoint(p, NewValue(ref remove));
                }
            }
        }
    }

    void InstantiateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Node node = GetNodeAtPoint(new Point(x, y));

                int val = node.value;
                if (val <= 0) 
                {
                    continue;
                }

                GameObject p = Instantiate(nodePice, gameBoard);
                NodePieces piece = p.GetComponent<NodePieces>();
                RectTransform rect = p.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2((PieceSize / 2) + (PieceSize * x), -(PieceSize/2) - (PieceSize * y));
                piece.Initialize(val, new Point(x, y), pices[val - 1], pieceScores[val - 1]);
                node.SetPiece(piece);
            }
        }
    }

    public void ResetPiece(NodePieces piece)
    {
        piece.ResetPosition();
        update.Add(piece);
    }

    public void FlipPieces(Point one, Point two, bool main) 
    {
        if (GetValueAtPoint(one) < 0) 
        {
            return;
        }

        Node nodeOne = GetNodeAtPoint(one);
        NodePieces pieceOne = nodeOne.GetPiece();
        if (GetValueAtPoint(two) > 0) 
        {
            Node nodeTwo = GetNodeAtPoint(two);
            NodePieces pieceTwo = nodeTwo.GetPiece();
            nodeOne.SetPiece(pieceTwo);
            nodeTwo.SetPiece(pieceOne);

            if (main)
            {
                flipped.Add(new FlippedPieces(pieceOne, pieceTwo));
            }

            update.Add(pieceOne);
            update.Add(pieceTwo);
        }
        else 
        {
            ResetPiece(pieceOne);
        }
    }

    void KillPiece(Point p)
    {
        List<KilledPiece> avaliable = new();

        for (int i = 0; i < killed.Count; i++)
        {
            if (!killed[i].falling)
            {
                avaliable.Add(killed[i]);
            }
        }

        KilledPiece set = null;
        if (avaliable.Count > 0)
        {
            set = avaliable[0];
        }
        else
        {
            GameObject kill = GameObject.Instantiate(KilledPiece, KilledBoard);
            KilledPiece kPiece = kill.GetComponent<KilledPiece>();

            set = kPiece;
            killed.Add(kPiece);
        }

        int val = GetValueAtPoint(p) - 1;
        Debug.Log($"piece {val+1} score {pieceScores[val]}");
        GameManager.AddScore(pieceScores[val]);
        if (set != null && val >= 0 && val < pices.Length)
        {
            set.Initialize(pices[val], GetPositionFromPoint(p));
        }
    }

    List<Point> IsConnected(Point p, bool main) 
    {
        List<Point> connected = new();
        int val = GetValueAtPoint(p);

        Point[] directions = { Point.Up, Point.Right, Point.Down, Point.Left };

        foreach (Point dir in directions) //Checkin if there is 2 or same sharpes in the directions
        {
            List<Point> line = new();

            int same = 0;
            for (int i = 1; i < 3; i++)
            {
                Point check = Point.Add(p, Point.Mult(dir, i));
                if(GetValueAtPoint(check) == val)
                {
                    line.Add(check);
                    same++;
                }
            }

            if(same > 1) //If there are more than 1 sharpe in the  direction them we know it is a match
            {
                AddPoints(ref connected, line); //Add these points to the overarching connected list
            }
        }

        for (int i = 0; i < 2; i++) //Checking if we are in the middle of two of the same shapes
        {
            List<Point> line = new();

            int same = 0;
            Point[] check = { Point.Add(p, directions[i]), Point.Add(p, directions[i + 2]) };
            foreach (Point next in check) //Check both sides of the pice, if they are the same value, add them to the list 
            {
                if (GetValueAtPoint(next) == val)
                {
                    line.Add(next);
                    same++;
                }
            }

            if (same > 1)
            {
                AddPoints(ref connected, line);
            }
        }

        for (int i = 0; i < 4; i++) //Check for a 2x2
        {
            List<Point> square = new();

            int same = 0;
            int next = i + 1;

            if (next >= 4)
            {
                next -= 4;
            }

            Point[] check = { Point.Add(p, directions[i]), Point.Add(p, directions[next]), Point.Add(directions[i], directions[next]) };
            foreach (Point pnt in check) //Check all sides of the pice, if they are the same value, add them to the list 
            {
                if (GetValueAtPoint(pnt) == val)
                {
                    square.Add(pnt);
                    same++;
                }
            }

            if(same > 2)
            {
                AddPoints(ref connected, square);
            }

        }

        if (main) //Cheks for other mtcher along the current match
        {
            for (int i = 0; i < connected.Count; i++)
            {
                AddPoints(ref connected, IsConnected(connected[i], false));
            }
        }

        /*  UNNESSARY | REMOVE THIS!
        if (connected.Count > 0)
        {
            connected.Add(p);
        }*/

        return connected;
    }

    void AddPoints(ref List<Point> points, List<Point> add)
    {
        foreach (Point p in add)
        {
            bool doAdd = true;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Equals(p)) 
                {
                    doAdd = false;
                    break;
                }
            }

            if (doAdd) 
            {
                points.Add(p);
            }
        }
    }

    int FillPiece()
    {
        int val = 1;
        val = (random.Next(0, 100) / (100 / pices.Length)) + 1;
		return val;
    }

    int GetValueAtPoint(Point p)
    {
        if (p.x < 0 || p.x >= width || p.y < 0 || p.y >= height)
        {
            return -1;
        }
        return board[p.x, p.y].value;
    }

    void SetValueAtPoint(Point p, int v) 
    {
        board[p.x, p.y].value = v;
    }

    Node GetNodeAtPoint(Point p) 
    {
        return board[p.x, p.y];
    }

    int NewValue(ref List<int> remove) 
    {
        List<int> available = new List<int>();
        for (int i = 0; i <pices.Length; i++)
        {
            available.Add(i + 1);
        }

        foreach (int i in remove)
        {
            available.Remove(i);
        }

        if (available.Count <= 0) 
        {
            return 0;
        }

        return available[random.Next(0, available.Count)];
    }

    string GetRandomSeed() 
    {
        string seed = "", accepTableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopkrstuvwxyz1234567890!@#$%&*()^";
        for (int i = 0; i < 20; i++)
        {
            seed += accepTableChars[Random.Range(0, accepTableChars.Length)];
        }

        return seed;
    }

    public Vector2 GetPositionFromPoint(Point p)
    {
        return new Vector2((PieceSize / 2) + (PieceSize * p.x), -(PieceSize / 2) - (PieceSize * p.y));
    }

}

[System.Serializable]
public class Node 
{
    public int value;// 0 = Balnk, 1 = Peça-1, 2 = Peça-2, 3 = Peça-3, 4 = Peça-4, 5 = Peça-5, -1 = Hole 
	public Point index;
    NodePieces piece;

    public Node(int v, Point i) 
    {
        value = v;
        index = i;
    }

    public void SetPiece(NodePieces p) 
    {
        piece = p;
        value = (piece == null) ? 0 : piece.value;
        if(piece == null) 
        {
            return;
        }
        piece.SetIndex(index);

    }

    public NodePieces GetPiece() 
    {
        return piece;
    }
}

[System.Serializable]
public class FlippedPieces 
{
    public NodePieces one;
    public NodePieces two;

    public FlippedPieces(NodePieces o, NodePieces t) 
    {
        one = o; two = t;
    }

    public NodePieces GetOtherPieces(NodePieces p) 
    {
        if(p == one) 
        {
            return two;
        }
        else if(p == two) 
        {
            return one;
        }
        else 
        {
            return null ;
        }
    }

}
