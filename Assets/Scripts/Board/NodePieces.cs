using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodePieces : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int value;
    public int damage;
    public HealthType damageOn;
	public Point index;

    [HideInInspector]
    public Vector2 pos;

    [HideInInspector]
    public RectTransform rect;

    bool updataing;
    Image img;

    public void Initialize(int v, Point p, Sprite pice, int dmg, HealthType type)
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        value = v;
        damage = dmg;
		damageOn = type;
		SetIndex(p);
        img.sprite = pice;
    }

    public void SetIndex(Point p) 
    {
        index = p;
        ResetPosition();
        UpdadeName();
    }

    public void ResetPosition() 
    {
        pos = new Vector2((Match3.PieceSize / 2) + (Match3.PieceSize * index.x), -(Match3.PieceSize/2) - (Match3.PieceSize * index.y));
    }

    public void MovePosition(Vector2 move) 
    {
        rect.anchoredPosition += move * Time.deltaTime * 16f;
    }
    
    public void MovePositionTo(Vector2 move) 
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, move, Time.deltaTime * 16f);
    }

    public bool UpdatePice() 
    {
        if(Vector3.Distance(rect.anchoredPosition, pos) > 1) 
        {
            MovePositionTo(pos);
            updataing = true;
            return true;
        }
        else
        {
            rect.anchoredPosition = pos;
            updataing = false;
            return false;
        }
    }

    void UpdadeName() 
    {
        transform.name = "Node [" + index.x + "," + index.y + "]";
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (updataing) 
        {
            return;
        }
        MoviePieces.instace.MovePiece(this);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        MoviePieces.instace.DropPiece();
    }
}
