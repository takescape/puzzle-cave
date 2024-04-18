using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MoviePieces : MonoBehaviour
{
    public static MoviePieces instace;

    [SerializeField] private int dragThreshold = 32;
    [SerializeField] private float minTimeToStopMove = .2f;
	[Header("Audio")]
	[SerializeField] private string pieceSwipe = "pieces_swipe";

	Match3 game;

    NodePieces moving;
    Point newIndex;
    Vector2 mouseStart;

    public void Awake()
    {
        instace = this;
    }

    void Start()
    {
        game = GetComponent<Match3>();
    }

    void Update()
	{
		if (CheckIfIsPlayerTurn()) return;

		if (moving != null) 
        {
            Vector2 dir = ((Vector2)Input.mousePosition - mouseStart);
            Vector2 nDir = dir.normalized;
            Vector2 aDir = new(Mathf.Abs(dir.x), Mathf.Abs(dir.y));

            newIndex = Point.Clone(moving.index);
            Point add = Point.Zero;
            if (dir.magnitude > dragThreshold) //If our mouse is 32 pixel away from the starting point of the mose
            {
                //Make add either (1, 0) | (-1, 0) | (0, 1) | (0, -1) depending on the direction of the mouse point  
                if(aDir.x > aDir.y) 
                {
                    add = (new Point((nDir.x > 0) ? 1 : -1, 0));
                }
                else if (aDir.y > aDir.x) 
                {
                    add = (new Point(0, (nDir.y > 0) ? -1 : 1));
                }
            }
            newIndex.Add(add);

            Vector2 pos = game.GetPositionFromPoint(moving.index);
            if (!newIndex.Equals(moving.index)) 
            {
                pos += Point.Mult(new Point(add.x, -add.y), 16).ToVector();
            }

            moving.MovePositionTo(pos);
        }
    }

    public void MovePiece(NodePieces piece)
	{
		if (CheckIfIsPlayerTurn()) return;

		if (moving != null) 
        {
            return;
        }

        moving = piece;
        mouseStart = Input.mousePosition;
    }

    public void DropPiece()
	{
		if (CheckIfIsPlayerTurn()) return;

		if (moving == null)
        {
            return;
        }

        AudioManager.Instance.PlaySoundOneShot(pieceSwipe, 4);
        if (!newIndex.Equals(moving.index))
		{
			game.FlipPieces(moving.index, newIndex, true);
        }
        else
        {
            game.ResetPiece(moving);
        }
        moving = null;
    }

    private bool CheckIfIsPlayerTurn()
	{
		if (!TurnManager.IsPlayerTurn)
		{
			if (moving != null)
				game.ResetPiece(moving);

			moving = null;
            mouseStart = Vector2.zero;

            return true;
		}

		if (TurnManager.IsPlayerTurn && TurnManager.TurnTime < minTimeToStopMove)
		{
			if (moving != null)
				game.ResetPiece(moving);

			moving = null;
			mouseStart = Vector2.zero;

			return true;
		}

		return false;
	}
}
