using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralMap : MonoBehaviour
{
    #region Variables
    [SerializeField]
    int seed = 12345; // Semilla del generador

    [SerializeField]
    int rooms = 20;

    LCG generator = new LCG(12345); // Semilla

    [SerializeField]
    Tilemap tilemap;

    [SerializeField]
    Tile tileBase;

    [SerializeField]
    List<Room> roomList = new List<Room>();

    [SerializeField]
    int maxTries = 5;

    List<Room> expansionRooms = new List<Room>(); // Lista de habitaciones desde donde expandir

    [SerializeField]
    TileDataset tileDataset; // Dataset de tiles
    #endregion

    #region Startup
    void Start()
    {
        generator = new LCG(seed);
        InitializeTilemap();
        GenerateRooms();
    }

    void InitializeTilemap()
    {
        tilemap.ClearAllTiles();
        Vector3Int center = GetTilemapCenter(tilemap);
        Room startRoom = new(center, RoomType.Start, tileBase);
        roomList.Add(startRoom);
        expansionRooms.Add(startRoom); // Agrega el start a expansión
        tilemap.SetTile(center, tileBase);
    }

    #endregion

    #region Update Test methods
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tilemap.ClearAllTiles();
            roomList.Clear();
            expansionRooms.Clear();

            InitializeTilemap();
            GenerateRooms();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            ConnectRooms();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //TODO
        }
    }

    #endregion

    #region Room Generation

    void GenerateRooms()
    {
        int createdRooms = 1; // Ya hay una (start room)
        while (createdRooms < rooms && expansionRooms.Count > 0)
        {
            Room baseRoom = expansionRooms[generator.Next(expansionRooms.Count)];
            Vector3Int basePosition = baseRoom.Position;

            int direction = generator.GetDirection();
            Vector3Int offset = GetDirectionOffset(direction);
            Vector3Int newPosition = basePosition + offset;

            if (!roomList.Exists(room => room.Position == newPosition))
            {
                Room newRoom = new(newPosition, RoomType.Normal, tileBase);
                roomList.Add(newRoom);
                expansionRooms.Add(newRoom);
                tilemap.SetTile(newPosition, tileBase);
                createdRooms++;
            }
            else
            {
                // Si no puede expandir, elimina esa baseRoom de la lista
                expansionRooms.Remove(baseRoom);
            }
        }
    }

    Vector3Int? GetNextPosition()
    {
        Vector3Int currentPosition = GetRandomRoomPosition();

        for (int i = 0; i < maxTries; i++)
        {
            int direction = generator.GetDirection();
            Vector3Int offset = GetDirectionOffset(direction);
            Vector3Int newPosition = currentPosition + offset;

            if (!roomList.Exists(room => room.Position == newPosition))
            {
                return newPosition;
            }
        }

        return null; // No encontró posición libre
    }

    #endregion


    #region Neighbor Connection
    void ConnectRooms()
    {
        foreach (Room room in roomList)
        {
            Vector3Int currentPosition = room.Position;
            int neighbors = CheckNeighbors(currentPosition);
            Tile tile = tileDataset.codeToTile(neighbors); // Obtiene el tile correspondiente al código binario
            if (tile == null)
                continue; // Si no hay tile, no se conecta nada
            tilemap.SetTile(currentPosition, tile); // Coloca la habitación en el tilemap
        }
    }

    int CheckNeighbors(Vector3Int currentPosition)
    {
        int code = 0;

        if (roomList.Exists(room => room.Position == currentPosition + new Vector3Int(0, 1, 0)))
            code |= 1;
        if (roomList.Exists(room => room.Position == currentPosition + new Vector3Int(1, 0, 0)))
            code |= 2;
        if (roomList.Exists(room => room.Position == currentPosition + new Vector3Int(0, -1, 0)))
            code |= 4;
        if (roomList.Exists(room => room.Position == currentPosition + new Vector3Int(-1, 0, 0)))
            code |= 8;
        return code;
    }
    #endregion

    #region Camera Options

    #endregion
    #region Helper Methods
    Vector3Int GetDirectionOffset(int direction)
    {
        return direction switch
        {
            0 => new Vector3Int(0, 1, 0),
            1 => new Vector3Int(0, -1, 0),
            2 => new Vector3Int(-1, 0, 0),
            3 => new Vector3Int(1, 0, 0),
            _ => Vector3Int.zero, // Por defecto no se mueve
        };
    }

    Vector3Int GetTilemapCenter(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;
        int centerX = (bounds.xMin + bounds.xMax) / 2;
        int centerY = (bounds.yMin + bounds.yMax) / 2;

        return new Vector3Int(centerX, centerY, 0);
    }

    Vector3Int GetRandomRoomPosition()
    {
        int randomIndex = generator.Next(roomList.Count);
        if (randomIndex < 0 || randomIndex >= roomList.Count)
        {
            randomIndex = 0; // fallback de seguridad
        }
        return roomList[randomIndex].Position;
    }
    #endregion
}
