using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class KilledPiece : MonoBehaviour
{
    [Header("Settings")]
	[SerializeField] private float speed = 16f;
    [SerializeField] private float gravity = 32f;
    [SerializeField, Range(0f, 1f)] private float transparency = .9f;
	[Header("Debug")]
	[SerializeField, ReadOnly] private bool falling;
	[SerializeField, ReadOnly] private Vector2 moveDir;
	[SerializeField, ReadOnly] private RectTransform rect;
	[SerializeField, ReadOnly] private Image img;
	[SerializeField, ReadOnly] private float randomRot;

	public bool Falling => falling;

    public void Initialize(Sprite pice, Vector2 start, float size)
    {
        falling = true;

        moveDir = Vector2.up;
        moveDir.x = Random.Range(-1.0f, 1.0f);
        moveDir *= speed / 2;
        randomRot = Random.Range(.8f, 1.3f);

		img = GetComponent<Image>();
        img.raycastTarget = false;
        rect = GetComponent<RectTransform>();
        img.sprite = pice;
        rect.anchoredPosition = start;
		rect.sizeDelta = new Vector2(size, size);
        Color newC = img.color;
        newC.a = transparency;
        img.color = newC;
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
        rect.Rotate(new Vector3(0, 0, randomRot));

        if (rect.position.x < -Match3.PieceSize || rect.position.x > Screen.width + Match3.PieceSize || rect.position.y < -Match3.PieceSize|| rect.position.y > Screen.height + Match3.PieceSize)
        {
            falling = false;
		}
    }
}
