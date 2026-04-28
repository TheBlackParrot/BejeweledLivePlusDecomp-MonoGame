using System;

namespace BejeweledLivePlus;

public static class RemoteEvents
{
    public static void SwapGems(string[] swaps)
    {
        Board active = Board.Active;
        if (active == null)
        {
            return;
        }
        
        if (active.mInReplay || !active.CanPlay())
        {
            Console.WriteLine("Can't input move, board is not allowing it");
            return;
        }
        
        ValueTuple<int, int> gemA = (swaps[0][0] - 65, swaps[0][1] - 49);
        ValueTuple<int, int> gemB = (swaps[1][0] - 65, swaps[1][1] - 49);

        Piece piece = active.GetPieceAtRowCol(gemA.Item1, gemA.Item2);
        if (!active.IsSwapLegal(piece, gemB.Item1, gemB.Item2))
        {
            Console.WriteLine($"Can't input move {string.Join(string.Empty, swaps)}, move is invalid");
            return;
        }

        active.TrySwap(piece, gemB.Item1, gemB.Item2, false, true);
    }
}