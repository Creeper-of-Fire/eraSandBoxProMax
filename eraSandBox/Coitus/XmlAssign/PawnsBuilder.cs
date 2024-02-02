namespace eraSandBox.Coitus;

public class PawnsBuilder
{
    private PawnsBuilder()
    {
    }

    public static PawnsBuilder Instance { get; } = new();

    public static TestPawn MakePawn(int heightMillimeter = 1700, string template = "人类")
    {
        var pawn = new TestPawn(heightMillimeter, template);
        pawn.parts.totalParts = PartsBuilder.MakeParts(pawn, pawn.template);
        pawn.parts.Initialize();
        pawn.parts.UpdateRoutesTotally();
        return pawn;
    }
}