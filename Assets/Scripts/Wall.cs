using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public Sprite dmgSprite;
    public int hp = 4;

    private SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	public void DamageWall (int damage) {
        spriteRenderer.sprite = dmgSprite;
        hp -= damage;
        if(hp <= 0)
        {
            gameObject.SetActive(false);
        }
	}
}
