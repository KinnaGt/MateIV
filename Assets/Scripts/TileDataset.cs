using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//Used to store the tile dataset and convert binary codes to tiles
public class TileDataset : MonoBehaviour
{
    [SerializeField]
    public List<TileBase> tileList;

    public TileBase bossTile;
    public TileBase homeTile;

    public Tile codeToTile(int binaryCode)
    {
        int code = getIntFromBinary(binaryCode);
        return (Tile)tileList[binaryCode];
    }

    public int getIntFromBinary(int code)
    {
        int result = 0;
        for (int i = 0; i < 4; i++)
        {
            result += ((code >> i) & 1) << i;
        }
        return result;
    }
}
