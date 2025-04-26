using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room
{
    Vector3Int position;
    RoomType roomType;
    Tile tile;

    public Vector3Int Position
    {
        get { return position; }
        set { position = value; }
    }

    public Room(Vector3Int position)
    {
        this.position = position;
    }

    public Room(Vector3Int position, RoomType roomType, Tile tile)
    {
        this.position = position;
        this.roomType = roomType;
        this.tile = tile;
    }
}

public enum RoomType
{
    Start,
    Boss,
    Rest,
    Normal
}
