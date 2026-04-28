using System;

namespace BejeweledLivePlus;

public static class RemoteEvents
{
    public static void SwapGems(string swapA, string swapB)
    {
        swapA = swapA.ToUpper();
        swapB = swapB.ToUpper();
        
        Board active = Board.Active;
        if (active == null)
        {
            return;
        }
        
        Console.WriteLine($"Attempting swap input {swapA}{swapB}");
        
        if (active.mInReplay || !active.CanPlay())
        {
            Console.WriteLine("Can't input move, board is not allowing it");
            return;
        }
        
        ValueTuple<int, int> gemA = (swapA[0] - 65, swapA[1] - 49);
        ValueTuple<int, int> gemB = (swapB[0] - 65, swapB[1] - 49);
        
#if DEBUG
        Console.WriteLine($"Parsed first gem at position {gemA}");
        Console.WriteLine($"Parsed second gem at position {gemB}");
#endif

        Piece piece = active.GetPieceAtRowCol(gemA.Item1, gemA.Item2);
        if (!active.IsSwapLegal(piece, gemB.Item1, gemB.Item2))
        {
            Console.WriteLine($"Can't input move {swapA}{swapB}, move is invalid");
            return;
        }

        active.TrySwap(piece, gemB.Item1, gemB.Item2, false, true);
    }
}