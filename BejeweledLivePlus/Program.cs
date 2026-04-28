using System;
using System.IO;
using System.Runtime.InteropServices;
using BejeweledLivePlus;

AllocConsole();

/*if (!Directory.Exists("./logs"))
{
    Directory.CreateDirectory("./logs");
}
StreamWriter consoleStream = new StreamWriter(File.Create($"./logs/{DateTimeOffset.Now.ToUnixTimeMilliseconds()}.txt"));
consoleStream.AutoFlush = true;
Console.SetOut(consoleStream);
Console.SetError(consoleStream);*/

GameWebSocket.Start();

GameMain game = new GameMain();
game.Activated += GameOnActivated;
game.Deactivated += GameOnDeactivated;

try
{
    game.Run();
}
catch (Exception e)
{
    Console.Error.Write(e);
    throw;
}

return;

void GameOnActivated(object sender, EventArgs e)
{
    Console.WriteLine("Game activated");
    game._paused = false;
}
void GameOnDeactivated(object sender, EventArgs e)
{
    Console.WriteLine("Game deactivated");
    game._paused = true;
}

[DllImport("kernel32")]
static extern bool AllocConsole();