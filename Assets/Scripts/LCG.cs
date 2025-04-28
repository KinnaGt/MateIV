public class LCG
{
    private uint seed;
    private const uint a = 1664525;
    private const uint c = 1013904223;

    public LCG(int initialSeed)
    {
        seed = (uint)initialSeed;
    }

    public int Next(int max)
    {
        seed = a * seed + c; // El overflow de uint actúa como % 2^32 automáticamente
        return (int)(seed % (uint)max);
    }

    public int GetDirection()
    {
        return Next(4); // 0: up, 1: down, 2: left, 3: right
    }
}
