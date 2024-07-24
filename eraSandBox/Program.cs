using eraSandBox.World;

namespace eraSandBox;

internal static class Program
{
    private static void Main(string[] args)
    {
        var world = TotalWorld.Instance;
        for (int i = 0; i < 10; i++) world.TakeTurn();
    }
}