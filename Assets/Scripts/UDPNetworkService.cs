using System;
using System.Net;
using System.Text;
using System.Net.Sockets;

using UnityEngine;

public class UDPNetworkService : Singleton<UDPNetworkService>
{
    private const int PORT_NUMBER = 61127;
    private readonly UdpClient udp = new UdpClient(PORT_NUMBER);
    private Byte[] buffer;

    private void Start()
    {
        StartListening();
    }

    private void StartListening()
    {
        Debug.Log("Started listening");

        // udp.BeginReceive(Receive, new object());

        // --------

        int port = PORT_NUMBER; // 'netstat -an' shows this is initially unused
        int bufferSize = 1024;
        buffer = new Byte[bufferSize];

        IPAddress ip = IPAddress.Any; // IPAddress.Parse( "127.0.0.1" );
        IPEndPoint ep = new IPEndPoint(ip, port);
        Socket sock = new Socket(ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

        sock.Bind(ep); // 'SocketException: Address already in use'

        sock.BeginReceive(buffer, 0, 1024, SocketFlags.None, new AsyncCallback(this.OnReceive), sock);
    }

    // private void Receive(IAsyncResult ar)
    // {
    //     IPEndPoint ip = new IPEndPoint(IPAddress.Any, PORT_NUMBER);
    //     byte[] bytes = udp.EndReceive(ar, ref ip);
    //     string message = Encoding.ASCII.GetString(bytes);

    //     Debug.Log($"From {ip.Address.ToString()} received: {message}");

    //     StartListening();
    // }

    private void OnReceive(IAsyncResult ar)
    {
        Socket s1 = (Socket)ar.AsyncState;
        int x = s1.EndReceive(ar);
        string message = Encoding.ASCII.GetString(buffer, 0, x);

        Debug.Log(message);

        s1.BeginReceive(buffer, 0, 1024, SocketFlags.None, new AsyncCallback(this.OnReceive), s1);
    }

    public void Send(string message)
    {
        UdpClient client = new UdpClient();
        IPEndPoint ip = new IPEndPoint(IPAddress.Parse("255.255.255.255"), PORT_NUMBER);
        byte[] bytes = Encoding.ASCII.GetBytes(message);

        client.Send(bytes, bytes.Length, ip);
        client.Close();

        Debug.Log($"Sent: {message}");
    }

    public void Stop()
    {
        try
        {
            Debug.Log("Stopped listening");

            udp.Close();
        }
        catch { /* don't care */ }
    }
}
