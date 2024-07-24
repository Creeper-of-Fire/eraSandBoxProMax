using eraSandBox.Utility.GameThing;

namespace eraSandBox.World;

/**
 * 全世界
 */
public class TotalWorld : IGameObject
{
    private TotalWorld()
    {
        this.subWorlds.Add(SubWorldUtility.makeTestSubWorld());
    }

    private readonly List<SubWorld> subWorlds = [];
    private Dictionary<Action, int> Actions { get; } = new();

    public static TotalWorld Instance { get; } = new();

    public void TakeTurn()
    {
        this.subWorlds.ForEach(world => world.TakeTurn());
        foreach (var keyValuePair in this.Actions.OrderBy(pair => pair.Value)) keyValuePair.Key.DynamicInvoke();

        this.Actions.Clear();
    }


    public static class TotalWorldUtility
    {
        // public static void AddToTop(this Action action)
        // {
        //     TotalWorld.Instance.Actions.Add(action, int.MinValue);
        // }
        //
        // public static void AddToBot(this Action action)
        // {
        //     TotalWorld.Instance.Actions.Add(action, int.MaxValue);
        // }
        //
        // public static void AddToMid(this Action action)
        // {
        //     TotalWorld.Instance.Actions.Add(action, 0);
        // }
        //
        // public static void AddAction(this Action action, int order)
        // {
        //     TotalWorld.Instance.Actions.Add(action, order);
        // }

        public static void AddToTop(params Action[] actions)
        {
            foreach (var action in actions)
                Instance.Actions.Add(action, int.MinValue);
        }

        public static void AddToBot(params Action[] actions)
        {
            foreach (var action in actions)
                Instance.Actions.Add(action, int.MaxValue);
        }

        public static void AddToMid(params Action[] actions)
        {
            foreach (var action in actions)
                Instance.Actions.Add(action, 0);
        }
    }
}