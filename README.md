Fork of https://github.com/bognarit80/BejeweledLivePlusDecomp-MonoGame that tweaks a few things and adds some functionality for use during intermission periods on my Twitch streams, namely a WebSocket server for live game events and remote input from chat.

# Building
To build, you need to place the game assets from the original `.xap` file into the `Contents` folder.
After that, simply build from the `.sln` file, and you should be good to go.

# Dependencies
- MonoGame.Content.Builder.Task 3.8.*
- MonoGame.Framework.DesktopGL 3.8.1.303 (but will probably work if set lower)
- .NET 6.0 / C# 10.0 (will also probably work if set lower)