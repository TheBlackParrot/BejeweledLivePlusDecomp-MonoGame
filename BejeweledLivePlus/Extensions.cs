using System;

namespace BejeweledLivePlus;

public static class MathfExtensions
{
    public static bool Approximately(this float a, float b)
    {
        return MathF.Abs(b - a) < MathF.Max(1E-06f * MathF.Max(MathF.Abs(a), MathF.Abs(b)), float.Epsilon * 8f); 
    }
}