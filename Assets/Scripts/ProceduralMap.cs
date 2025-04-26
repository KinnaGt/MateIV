using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralMap : MonoBehaviour
{
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

    void Start()
    {
        generator = new LCG(seed); // Inicializa el generador con la semilla
        InitializeTilemap();
        GenerateRooms();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tilemap.ClearAllTiles(); // Limpia el Tilemap antes de empezar
            roomList.Clear(); // Limpia la lista de habitaciones
            InitializeTilemap();
            GenerateRooms();
        }
    }

    void InitializeTilemap()
    {
        tilemap.ClearAllTiles(); // Limpia el Tilemap antes de empezar
        Vector3Int center = GetTilemapCenter(tilemap);
        Room startRoom = new Room(center, RoomType.Start, tileBase);
        roomList.Add(startRoom);
        tilemap.SetTile(center, tileBase); // Coloca la habitación inicial en el Tilemap
    }

    Vector3Int GetTilemapCenter(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;
        int centerX = (bounds.xMin + bounds.xMax) / 2;
        int centerY = (bounds.yMin + bounds.yMax) / 2;

        return new Vector3Int(centerX, centerY, 0);
    }

    void GenerateRooms()
    {
        for (int i = 0; i < rooms; i++)
        {
            Vector3Int newPosition = GetNextPosition();
            Room newRoom = new Room(newPosition, RoomType.Normal, tileBase);
            roomList.Add(newRoom);
            tilemap.SetTile(newPosition, tileBase); // Coloca la habitación en el Tilemap
        }
    }

    Vector3Int GetNextPosition()
    {
        int direction = generator.GetDirection();
        Vector3Int currentPosition = roomList[roomList.Count - 1].Position;

        Vector3Int newPosition = currentPosition;

        Vector3Int offset = GetDirectionOffset(direction);
        int tries = 0;
        while (roomList.Exists(room => room.Position == newPosition))
        {
            newPosition = newPosition + offset;
            Debug.Log($"Nueva posición: {newPosition}");
            tries++;

            if (tries >= maxTries)
            {
                Debug.LogWarning(
                    "No se pudo encontrar una nueva posición después de varios intentos."
                );
                break; // Sale del bucle si no se encuentra una nueva posición
            }
        }

        return newPosition;
    }

    Vector3Int GetDirectionOffset(int direction)
    {
        switch (direction)
        {
            case 0:
                return new Vector3Int(0, 1, 0);
            case 1:
                return new Vector3Int(0, -1, 0);
            case 2:
                return new Vector3Int(-1, 0, 0);
            case 3:
                return new Vector3Int(1, 0, 0);
            default:
                return Vector3Int.zero; // Por defecto no se mueve
        }
    }
}
