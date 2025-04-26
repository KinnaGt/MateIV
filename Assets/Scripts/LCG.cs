public class LCG
{
    private int seed;
    private int a = 1664525;
    private int c = 1013904223;
    private int m = int.MaxValue;

    public LCG(int initialSeed)
    {
        seed = initialSeed;
    }

    public int Next()
    {
        seed = (a * seed + c) % m;
        return seed;
    }

    public float NextFloat() // Para obtener valores entre 0 y 1
    {
        return Next() / (float)m;
    }

    public int GetDirection()
    {
        return Next() % 4; // Genera un número entre 0 y 3
    }
}
