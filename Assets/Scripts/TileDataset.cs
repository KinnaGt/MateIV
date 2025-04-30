using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDataset : MonoBehaviour
{
    [SerializeField]
    public List<TileBase> tileList;

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
