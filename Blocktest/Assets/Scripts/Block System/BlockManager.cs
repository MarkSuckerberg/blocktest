// System required for [Serializable] attribute.
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BlockManager : MonoBehaviour {

    /// Array to store all blocks created in Start()
    [HideInInspector] public Block[] allBlocks;
    /// List used to store the names of blocks, the index is the corresponding ID.
    [HideInInspector] public List<string> blockNames;

    /// Dropdown used for item selection
    [SerializeField] Dropdown selectionDropdown;

    /// Tilemap for foreground objects
    [SerializeField] Tilemap foregroundTilemap;
    /// Tilemap for background (non-dense) objects
    [SerializeField] Tilemap backgroundTilemap;


    private void Awake()
    {
        // Add this to the global variable
        Globals.blockManager = this;
        Globals.foregroundTilemap = foregroundTilemap;
        Globals.backgroundTilemap = backgroundTilemap;

        // This mess gets all subtypes of Block and puts the types in a list.
        Type[] allBlockTypes = (
                        from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                        from assemblyType in domainAssembly.GetTypes()
                        where assemblyType.IsSubclassOf(typeof(Block))
                        select assemblyType).ToArray();

        allBlocks = new Block[allBlockTypes.Length];

        // For loops to populate main allBlocks array.
        for (int i = 0; i < allBlockTypes.Length; i++)
        {
            Type newBlockType = allBlockTypes[i];
            Block newBlock = (Block)Activator.CreateInstance(newBlockType);
            Block newBlockTwo = new Block();
            newBlock.Initialize();
            if(newBlock.blockID == -1) {
                newBlock.blockID = i;
            }
            if(allBlocks[newBlock.blockID] != null) {
                Debug.LogWarning("Block " + newBlock + " conflicts with block " + allBlocks[newBlock.blockID] + "! (Block ID: " + newBlock.blockID + ")");
            } else if(newBlock.blockID > allBlocks.Length || newBlock.blockID < 0) {
                Debug.LogWarning("Block " + newBlock + " has invalid ID " + newBlock.blockID + "! (Max ID " + allBlocks.Length + ")");
            }
            blockNames.Add(newBlock.blockName);
            allBlocks[newBlock.blockID] = newBlock;
        }
        selectionDropdown.AddOptions(blockNames);
        WorldGen.GenerateMainMap(); // TODO: Move this to some sort of global initialization method
    }
}
