using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public enum Biome { //Variants of the NORMAL FOREST Biome
    FOREST_THIN,        // 0
    FOREST_PLAINS,      // 1
    FOREST_THICK,       // 2

    FOREST_THIN_DARK,   // 3
    FOREST_PLAINS_DARK, // 4
    FOREST_THICK_DARK,  // 5
    FOREST_BRAMBLES     // 6
}

/*
 * CHUNK FOR NORMAL FOREST BIOME
 * MAKE A BLOCK OBEJCT THAT INHERITS GAMEOBJECT, SO U CAN GIVE IT IDs ??
 */

public class generateChunk : MonoBehaviour
{
    //Variables needed in every chunk type
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
    public int chunkID; //How far along the chunk is in the world
    public bool isDark; //Determines if you're in the dark forest or not


    //Chunk specific tiles
    public GameObject DirtTile;
    public GameObject GrassTile;
    public GameObject StoneTile;
    public GameObject WoodTile;
    public GameObject SandTile;

    

    //trees
    private int chance_tree;
    private int tree_max_height;

    //1 block holes
    private int holes; //no of tiles to go through and add 1 block holes to - e.g. if its 7, (o = block, x = hole) oxxoxoo

    // Start is called before the first frame update
    void Start(){
       
        genChunks = FindObjectOfType<generateChunks>();
        rand = genChunks.rand;//Use the same random generator in the main generating script
        

        if (!isDark){
            int biomeChoice = rand.Next(3);
            
            //Debug.Log(biomeChoice);
            //int biomeChoice = 2 ;
            //Picking out of possible surface biomes, which determines how they're generated
            switch (biomeChoice){
                case 0: chunkBiome = Biome.FOREST_PLAINS;break;
                case 1: chunkBiome = Biome.FOREST_THIN;  break;
                case 2: chunkBiome = Biome.FOREST_THICK; break;
            }

            genChunks.chunkBiome = chunkBiome;

            //Giving the surface biomes their own stats
            if (chunkBiome == Biome.FOREST_THICK) {
                tree_max_height = 12;
                chance_tree = 8; //1 in 8
                smoothness = 48;
                heightMultiplier = 48;
            }
            else if (chunkBiome == Biome.FOREST_THIN) {
                tree_max_height = 8;
                chance_tree = 16; //1 in 16
                smoothness = 56;
                heightMultiplier = 32;
            }
            else if (chunkBiome == Biome.FOREST_PLAINS) {
                tree_max_height = 0; //No trees
                smoothness = 64;
                    heightMultiplier = 32;
            }
        }
        else{ //Dark Biomes
            int biomeChoice = rand.Next(3,6);
            
            switch (biomeChoice){
                case 3: chunkBiome = Biome.FOREST_PLAINS_DARK;  break;
                case 4: chunkBiome = Biome.FOREST_THIN_DARK;    break;
                case 5: chunkBiome = Biome.FOREST_THICK_DARK;   break;
            }

            //Giving the surface biomes their own stats
            if (chunkBiome == Biome.FOREST_THICK)
            {
                tree_max_height = 12;
                chance_tree = 8; //1 in 8
                smoothness = 48;
                heightMultiplier = 48;
            }
            else if (chunkBiome == Biome.FOREST_THIN)
            {
                tree_max_height = 8;
                chance_tree = 16; //1 in 16
                smoothness = 56;
                heightMultiplier = 32;
            }
            else if (chunkBiome == Biome.FOREST_PLAINS)
            {
                tree_max_height = 0; //No trees
                smoothness = 64;
                heightMultiplier = 32;
            }
        }
        Generate();
    }

    
    public void Generate(){ 
        int offset = 0;
        for (int i = 0; i < width; i++){
            
            //Stop problem of chunks in different biomes being at the wrong heights, so add an offset (bodge!??)
            if (i == 0) {
                h = Mathf.RoundToInt(Mathf.PerlinNoise(seed, (i + transform.position.x * 0.5f) / smoothness) * heightMultiplier) + offset + heightAddition;
 
                if (genChunks.oldHeight == -999){
                    offset = 0;
                }
                else{
                    offset = genChunks.oldHeight - h;
                }
            }

            h = Mathf.RoundToInt(Mathf.PerlinNoise(seed, (i + transform.position.x * 0.5f) / smoothness) * heightMultiplier) + offset + heightAddition;

            //HOLES
            //Determines if the next few blocks along will have the chance for 1 block holes
            if (rand.Next(32) == 0){
                holes = rand.Next(4,8);
            }

            //If holes need to be placed
            if (holes > 0){
                if (rand.Next(1) == 0){
                    h--;
                    holes--;
                }   
            }

            fillBlocks(chunkBiome, rand, i); //Biome Enum, random number generator, how far along in the chunk it is

            //Move the height up again, because it just decreased the height to make a hole, need to put the height up again 
            if (holes >= 0){
                h++;
            }

            genChunks.oldHeight = h;
        }

        //Debug.Log("oldHeightAfter: " + genChunks.oldHeight);
        //Debug.Log("");
        //Debug.Log("");
        //Debug.Log("");
    }

    /*
     * Used for making the overground blocks
     * 
     * @params: Biome Enum, random number generator, how far along in the chunk it is 
     * 
     */
    private void fillBlocks(Biome chunkBiome, System.Random rand, int i){  
        for (int j = -64; j < h; j++)
        {
            GameObject selectedTile;
            if (j >= h - 1){ //ON surface
                selectedTile = GrassTile;

                if (rand.Next(chance_tree) == 1){
                    genChunks.makeTree(i + chunkID * width, j, tree_max_height, WoodTile);
                }
                
            }

            else{
                selectedTile = DirtTile;
            }
            GameObject newTile = Instantiate(selectedTile, new Vector3(i, j), Quaternion.identity);
            newTile.transform.parent = this.gameObject.transform;
            newTile.transform.localPosition = new Vector3(i, j);
        }
    }

    

}
