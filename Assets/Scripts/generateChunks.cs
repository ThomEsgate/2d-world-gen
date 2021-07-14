using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateChunks : MonoBehaviour {

    public GameObject chunk;
    public GameObject chunk_dark;
    int chunkWidth;
    //Make these constants later
    public int numChunksTotal;
    public int numChunksForest; //Number of chunks to generate of forest    
    public int numChunksForest_dark; //Number of chunks to generate of forest    
    public ArrayList biomeTypeArray = new ArrayList();


    public int lastX;
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

    void GenerateChunkOfType(GameObject chunkType, int lastX_param, int chunkID) //chunkType = biome for 
    {
        lastX = lastX_param;
        GameObject newChunk = Instantiate(chunkType, new Vector3(lastX + chunkWidth, 0f), Quaternion.identity) as GameObject;
        newChunk.GetComponent<generateChunk>().seed = seed;
        newChunk.GetComponent<generateChunk>().rand = rand;
        newChunk.GetComponent<generateChunk>().chunkID = chunkID;
    }

    /*Main method for generating chunks in the world
     * 
     * 
     */
    void Generate(){
        lastX = -chunkWidth; //lastX = last starting position, all subsiquent chunks start fro

        //Generate dark forest
        for (int i = 0; i < numChunksForest_dark; i++){
            GenerateChunkOfType(chunk_dark, lastX, i);
            lastX += chunkWidth;
        }

        //Generate forest
        for (int i = 0; i < numChunksForest; i++){
            GenerateChunkOfType(chunk, lastX, i + numChunksForest_dark); // +numChunksForest_dark because that no. of chunks have already been generated, so you cant start counting the ID from 0 again or there will be 2 chunks with the same ID
            lastX += chunkWidth;
        }
    }


    /*
     * Making a tree, can change the type of wood the tree is made of
     * local x coordinate (with respect to chunk), local y coordinate (with respect to chunk), max height of tree, type of wood
     */
    public void makeTree(int i, int j, int tree_max_height, GameObject WoodTile){
        //Check to see if possible to place tree
        //

        int treeHeight = rand.Next(tree_max_height / 2, tree_max_height);

        for (int k = 0; k < treeHeight; k++) //places blocks to form a tree, k being the height of the block being placed
        {
            //Debug.Log("placing at: " + i + ", " + (j + k + 1) );
            GameObject tree = Instantiate(WoodTile, new Vector3(i, j + k - 1), Quaternion.identity);
            tree.transform.parent = this.gameObject.transform;
            tree.transform.localPosition = new Vector3(i, j + k + 1);
        }

    }



}
