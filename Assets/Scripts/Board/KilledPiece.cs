using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class KilledPiece : MonoBehaviour
{
    [Header("Settings")]
	[SerializeField] private float speed = 16f;
    [SerializeField] private float gravity = 32f;
	[Header("Debug")]
	[SerializeField, ReadOnly] private bool falling;
	[SerializeField, ReadOnly] private Vector2 moveDir;
	[SerializeField, ReadOnly] private RectTransform rect;
	[SerializeField, ReadOnly] private Image img;

    public bool Falling => falling;

    public void Initialize(Sprite pice, Vector2 start)
    {
        falling = true;

        moveDir = Vector2.up;
        moveDir.x = Random.Range(-1.0f, 1.0f);
        moveDir *= speed / 2;

        img = GetComponent<Image>();
        img.raycastTarget = false;
        rect = GetComponent<RectTransform>();
        img.sprite = pice;
        rect.anchoredPosition = start;
    }

    // Update is called once per frame
    void Update()
    {
        if (!falling)
        {
            return;
        }

        moveDir.y -= Time.deltaTime * gravity;
        moveDir.x = Mathf.Lerp(moveDir.x, 0, Time.deltaTime);
        rect.anchoredPosition += moveDir * Time.deltaTime * speed;

        if (rect.position.x < -Match3.PieceSize || rect.position.x > Screen.width + Match3.PieceSize || rect.position.y < -Match3.PieceSize|| rect.position.y > Screen.height + Match3.PieceSize)
        {
            falling = false;
        }
    }
}
