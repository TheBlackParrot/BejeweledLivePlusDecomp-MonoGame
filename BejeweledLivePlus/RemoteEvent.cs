using System;

namespace BejeweledLivePlus;

public static class RemoteEvents
{
    public static void SwapGems(string swapA, string swapB, bool allowIllegal = false)
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
        if (!active.IsSwapLegal(piece, gemB.Item1, gemB.Item2) && !allowIllegal)
        {
            Console.WriteLine($"Can't input move {swapA}{swapB}, move is invalid");
            return;
        }

        active.TrySwap(piece, gemB.Item1, gemB.Item2, allowIllegal, true);
    }

    public static bool DiffusePowerup(string rawPosition)
    {
        rawPosition = rawPosition.ToUpper();
        
        Board active = Board.Active;
        if (active == null)
        {
            return false;
        }
        
        Console.WriteLine($"Attempting to diffuse {rawPosition}");
        
        if (active.mInReplay || !active.CanPlay())
        {
            Console.WriteLine("Can't input move, board is not allowing it");
            return false;
        }
        
        ValueTuple<int, int> position = (rawPosition[0] - 65, rawPosition[1] - 49);
        
        Piece piece = active.GetPieceAtRowCol(position.Item1, position.Item2);
#if DEBUG
        Console.WriteLine($"Gem {position} has flag value {piece.mFlags}");
#endif

        // laser (4u), hypercube (2u), flame (1u)
        uint[] flagChecks = { 4u, 1u };
        foreach (uint flag in flagChecks)
        {
            if (piece.IsFlagSet(flag))
            {
                piece.ClearFlag(flag);
                piece.ClearBoundEffects();
                return true;
            }
        }

        return false;
    }
}