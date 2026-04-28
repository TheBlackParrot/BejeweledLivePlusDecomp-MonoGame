#nullable enable
using System;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace BejeweledLivePlus;

public static class GameState
{
    private static int _level;
    public static int Level
    {
        set
        {
            if (_level == value)
            {
                return;
            }
            
            GameWebSocket.Send("level", value);
            _level = value;
        }
    }

    private static float _levelPercentComplete;
    public static float LevelPercentComplete
    {
        set
        {
            if (_levelPercentComplete.Approximately(value))
            {
                return;
            }
            
            GameWebSocket.Send("levelCompletion", value);
            _levelPercentComplete = value;
        }
    }
    
    private static int _score;
    public static int Score
    {
        set
        {
            if (_score == value)
            {
                return;
            }
            
            _score = value;
            GameWebSocket.Send("score", _score);
        }
    }

    private static ValueTuple<int, int> _pointsNeededToClear;

    public static ValueTuple<int, int> PointsNeededToClear
    {
        set
        {
            if (_pointsNeededToClear == value)
            {
                return;
            }
            
            GameWebSocket.Send("pointsNeededToClear", value);
            _pointsNeededToClear = value;
        }
    }

    public static void BroadcastUpdates()
    {
        GameWebSocket.Send("score", _score);
        GameWebSocket.Send("levelCompletion", _levelPercentComplete);
        GameWebSocket.Send("level", _level);
        GameWebSocket.Send("pointsNeededToClear", _pointsNeededToClear);
    }
}

public class WebSocketHandler : WebSocketBehavior
{
    protected override void OnOpen()
    {
        Console.WriteLine("WebSocket connection opened");
        GameState.BroadcastUpdates();
        base.OnOpen();
    }

    protected override void OnClose(CloseEventArgs e)
    {
        Console.WriteLine("WebSocket connection closed");
        base.OnClose(e);
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        base.OnMessage(e);
        
        string[] commandParts = e.Data.Split('\t');
        if (commandParts.Length < 1)
        {
            return;
        }
        
        switch (commandParts[0])
        {
            case "swap":
                RemoteEvents.SwapGems(commandParts[1], commandParts[2]);
                break;
            
            case "forceswap":
                RemoteEvents.SwapGems(commandParts[1], commandParts[2], true);
                break;
            
            case "diffuse":
                GameWebSocket.Send("powerupDiffused", RemoteEvents.DiffusePowerup(commandParts[1]));
                break;
            
            case "close":
                GlobalMembers.gApp.WantExit = true;
                break;
        }
    }
}

[JsonObject(MemberSerialization.OptIn)]
internal class WebSocketMessage
{
    [JsonProperty(PropertyName = "time")]
    private static long Timestamp => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    
    [JsonProperty(PropertyName = "event")]
    private string _eventType;
    
    [JsonProperty(PropertyName = "data")]
    private object? _data;

    public WebSocketMessage(string eventType, object? obj = null)
    {
        _eventType = eventType;
        _data = obj;
    }
}

public static class GameWebSocket
{
    private static WebSocketServer? mServer;

    public static void Send(string evnt, object? message = null)
    {
        if (!mServer?.IsListening ?? true)
        {
            Console.WriteLine("WebSocket server has not started yet");
            return;
        }
        
        mServer.WebSocketServices.Broadcast(JsonConvert.SerializeObject(new WebSocketMessage(evnt, message)));
    }
    
    public static void Start()
    {
        if (!mServer?.IsListening ?? false)
        {
            Console.WriteLine("WebSocket server already running");
            return;
        }
        
        mServer = new WebSocketServer("ws://127.0.0.1:6677");

        mServer.AddWebSocketService<WebSocketHandler>("/");
        mServer.Start();
        
        Console.WriteLine($"WebSocket server running on ws://{mServer.Address}:{mServer.Port}");
    }
}