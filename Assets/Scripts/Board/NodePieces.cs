using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodePieces : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int value;
    public Point index;

    [HideInInspector]
    public Vector2 pos;

    [HideInInspector]
    public RectTransform rect;

    bool updataing;
    Image img;

    public void Initialize( int v, Point p, Sprite pice)
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        value = v;
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
        pos = new Vector2(32 + (64 * index.x), -32 - (64 * index.y));
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
