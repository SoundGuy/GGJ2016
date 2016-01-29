using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveImageUp : MonoBehaviour {

    public float spread = 10f;
    public float speed = 100f;
    public float fadespeed = 0.01f;
    // Use this for initialization
    void Start () {

        float xDiff = Random.Range(-1f * spread, spread);
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 pos = rect.position;
        pos.x += xDiff;
        rect.position = pos;

        DestroyObject(this, 10f);

    }
	
	// Update is called once per frame
	void Update () {
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 pos = rect.position;
        pos.y += speed * Time.deltaTime;
        rect.position = pos;

        Image img = GetComponent<Image>();
        Color col = img.color;
        col.a -= fadespeed * Time.deltaTime;

        img.color = col;
    }
}
