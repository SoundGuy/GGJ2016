using UnityEngine;
using System.Collections;

public class WordGenerator : MonoBehaviour {

    public string[] lines;
    public string[] words;
    public GameObject wordPrefab;

    public Vector2 MinMaxX;
    public Vector2 MinMaxY;
    public float TargetZ;

    // Use this for initialization
    void Start () {
        words = lines[0].Split(' ');

        foreach (string word in words)
        {
            Vector3 GenPosition = new Vector3(Random.Range(MinMaxX.x,MinMaxX.y),
                Random.Range(MinMaxY.x, MinMaxY.y),TargetZ);

            GameObject inst =(GameObject) Instantiate(wordPrefab, GenPosition, Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
