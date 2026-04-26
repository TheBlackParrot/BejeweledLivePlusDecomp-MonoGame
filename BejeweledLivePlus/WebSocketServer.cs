#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
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
            if (_levelPercentComplete == value)
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

        string[] swaps = e.Data.ToUpperInvariant().Split(' ');
        
        ValueTuple<int, int> gemA = ((int)(char)swaps[0][0] - 65, (int)(char)swaps[0][1] - 49);
        ValueTuple<int, int> gemB = ((int)(char)swaps[1][0] - 65, (int)(char)swaps[1][1] - 49);

        Piece piece = active.GetPieceAtRowCol(gemA.Item1, gemA.Item2);
        if (!active.IsSwapLegal(piece, gemB.Item1, gemB.Item2))
        {
            Console.WriteLine($"Can't input move {e.Data.ToUpperInvariant()}, move is invalid");
            return;
        }

        active.TrySwap(piece, gemB.Item1, gemB.Item2, false, true);
        
        base.OnMessage(e);
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
    internal static WebSocketServer? mServer;

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