using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateChunks : MonoBehaviour {

    public GameObject chunk;
    int chunkWidth;
    public int numChunks;
    int seed;
    public int oldHeight;
    public Biome chunkBiome;

    public int lastHeight;

    public System.Random rand;

    // Start is called before the first frame update
    void Start(){
        chunkWidth = chunk.GetComponent<generateChunk>().width;
        seed = Random.Range(-10000, 10000);
        rand = new System.Random(seed);
        oldHeight = -999;
        Generate();    
    }

    // Update is called once per frame
    void Generate(){
        int lastX = -chunkWidth;
        for (int i = 0; i < numChunks; i++){
            GameObject newChunk = Instantiate(chunk, new Vector3(lastX + chunkWidth, 0f), Quaternion.identity) as GameObject;
            newChunk.GetComponent<generateChunk>().seed = seed;
            newChunk.GetComponent<generateChunk>().rand = rand;
            lastX += chunkWidth;
        }
    }
}
