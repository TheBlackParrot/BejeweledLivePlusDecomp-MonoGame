using System;
using System.IO;
using System.Runtime.InteropServices;
using BejeweledLivePlus;

AllocConsole();
/*
if (!Directory.Exists("./logs"))
{
    Directory.CreateDirectory("./logs");
}
StreamWriter consoleStream = new StreamWriter(File.Create($"./logs/{DateTimeOffset.Now.ToUnixTimeMilliseconds()}.txt"));
consoleStream.AutoFlush = true;
Console.SetOut(consoleStream);
Console.SetError(consoleStream);*/

GameWebSocket.Start();

try
{
    using GameMain game = new GameMain();

    game.Activated += (sender, eventArgs) =>
    {
        Console.WriteLine("Game activated");
        game._paused = false;
    };
    game.Deactivated += (sender, eventArgs) =>
    {
        Console.WriteLine("Game deactivated");
        game._paused = true;
    };

    game.Run();
}
catch (Exception e)
{
    Console.Error.Write(e);
    throw;
}

return;

[DllImport("kernel32")]
static extern bool AllocConsole();