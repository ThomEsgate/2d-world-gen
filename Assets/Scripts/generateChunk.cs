using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Biome { 
    FOREST_THIN, FOREST_THICK, FOREST_PLAINS,
    CAVE_A, CAVE_B,
    SKY_A
}



public class generateChunk : MonoBehaviour
{
    // ((((((((((((((((((((((((********************************************** MAKE A BLOCK OBEJCT THAT INHERITS GAMEOBJECT, SO U CAN GIVE IT IDs
    public GameObject DirtTile;
    public GameObject GrassTile;
    public GameObject GrassDarkTile;
    public GameObject StoneTile;
    public GameObject WoodTile;
    public GameObject SandTile;

    private generateChunks genChunks;

    public int width;
    public int seed;
    public int heightAddition;
    public int oldHeight;

    public float heightMultiplier;
    public float smoothness;
    public int h; //current height - not a local variable so it can be used outside loop

    public Biome chunkBiome;
    public System.Random rand;

    //trees
    private int chance_tree;
    private int tree_max_height;

    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random(seed);//!!!!!!!!!!!
        int biomeChoice = rand.Next(3);
        //int biomeChoice = 1;
        genChunks = FindObjectOfType<generateChunks>();

        //Picking out of 3 possible surface biomes
        switch (biomeChoice){
            case 0: chunkBiome = Biome.FOREST_PLAINS;break;
            case 1: chunkBiome = Biome.FOREST_THIN;  break;
            case 2: chunkBiome = Biome.FOREST_THICK; break;
        }

        genChunks.chunkBiome = chunkBiome;

        //Giving the surface biomes their own stats
        if (chunkBiome == Biome.FOREST_THICK) {
            tree_max_height = 12;
            chance_tree = 5; //1 in 5
            smoothness = 48;
            heightMultiplier = 64;
        }
        else if (chunkBiome == Biome.FOREST_THIN) {
            tree_max_height = 8;
            chance_tree = 16; //1 in 16
            smoothness = 56;
            heightMultiplier = 32;
        }
        else if (chunkBiome == Biome.FOREST_PLAINS) {
            tree_max_height = 0;
            smoothness = 64;
            heightMultiplier = 32;
        }

        Generate();
    }

    
    public void Generate(){ 
        int offset = 0;
        for (int i = 0; i < width; i++){
            
            //Stop problem of chunks in different biomes being at the wrong heights, so add an offset (bodge!)
            if (i == 0) {
                h = Mathf.RoundToInt(Mathf.PerlinNoise(seed, (i + transform.position.x * 0.5f) / smoothness) * heightMultiplier) + offset + heightAddition;
 
                if (genChunks.oldHeight == -999){
                    offset = 0;
                }
                else{
                    offset = genChunks.oldHeight - h;
                }

                Debug.Log("oldHeight: " + genChunks.oldHeight);
                Debug.Log("h: " + h);
                Debug.Log("offset bef: " + offset);
                Debug.Log("oldHeight before: " + genChunks.oldHeight);
            }

            h = Mathf.RoundToInt(Mathf.PerlinNoise(seed, (i + transform.position.x * 0.5f) / smoothness) * heightMultiplier) + offset + heightAddition;

            if (chunkBiome == Biome.FOREST_THICK){
                h += 1;
            }

            for (int j = -64; j < h; j++){
                GameObject selectedTile;
                if (j < h - 1){
                    selectedTile = DirtTile;
                }
                else { 
                    if (chunkBiome == Biome.FOREST_THICK){
                        selectedTile = GrassDarkTile;
                    }
                    else {
                        selectedTile = GrassTile;
                    }

                    if (rand.Next(chance_tree) == 1){
                        makeTree(i, j, tree_max_height);
                    }
                }
                GameObject newTile = Instantiate(selectedTile, new Vector3(i, j), Quaternion.identity);
                newTile.transform.parent = this.gameObject.transform;
                newTile.transform.localPosition = new Vector3(i, j);
            }
            genChunks.oldHeight = h;
        }

        
        Debug.Log("oldHeightAfter: " + genChunks.oldHeight);
        Debug.Log("");
        Debug.Log("");
        Debug.Log("");
    }

    private void makeTree(int i, int j, int tree_max_height){
        //Debug.Log("placing tree!!!!!!");
        int treeHeight = rand.Next(tree_max_height/2, tree_max_height);
        //Debug.Log("treeHeight: " + treeHeight);
        for (int k = 0; k < treeHeight; k++){
            //Debug.Log("placing at: " + i + ", " + (j + k + 1) );
            GameObject tree = Instantiate(WoodTile, new Vector3(i, j + k + 1), Quaternion.identity);
            tree.transform.parent = this.gameObject.transform;
            tree.transform.localPosition = new Vector3(i, j + k + 1);
        }

    }

}
