using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite DmgSprite;
    public int Hp = 4;

    private SpriteRenderer _spriteRenderer;

	// Use this for initialization
	void Awake ()
	{
	    _spriteRenderer = GetComponent<SpriteRenderer>();
	}

    public void DamageWall(int loss)
    {
        _spriteRenderer.sprite = DmgSprite;
        Hp -= loss;
        if (Hp <= 0)
            gameObject.SetActive(false);
    }
}
