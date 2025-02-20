using WebSocketSharp;
using WebSocketSharp.Server;

public class UdpDriver
{
    private readonly WebSocketServer _server;

    public UdpDriver()
    {
        _server = new WebSocketServer("ws://localhost:8080");
        _server.AddWebSocketService<UdpService>("/udp");
        _server.Start();
        Console.WriteLine("WebSocket server started on ws://localhost:8080");
    }

    public void Send(string message)
    {
        _server.WebSocketServices["/udp"].Sessions.Broadcast(message);
    }
}

public class UdpService : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e)
    {
        Console.WriteLine($"Received: {e.Data}");
        Sessions.Broadcast(e.Data);
    }
}