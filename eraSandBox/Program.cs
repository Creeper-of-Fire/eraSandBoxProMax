using System;
using eraSandBox.Coitus;

namespace eraSandBox;

internal static class Program
{
    private static void Main(string[] args)
    {
        var pawn = PawnsBuilder.MakePawn();
        Console.WriteLine(pawn);
        // // 打印生成的info对象
        // foreach (var obj in infoObjects)
        //     Console.WriteLine(
        //         $"Name: {obj.name}, LinkTo: {string.Join(",", obj.linkTo.Select(pair => pair.Key.name.ToString() + ":" + pair.Value.startPoint + ',' + pair.Value.endPoint))}");
    }
}