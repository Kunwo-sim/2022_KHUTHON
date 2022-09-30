using System;
using System.Net;
using System.Text;
using System.Net.Sockets;

using UnityEngine;

public class UDPNetworkService : Singleton<UDPNetworkService>
{
    private const int PORT_NUMBER = 15000;
    private readonly UdpClient udp = new UdpClient(PORT_NUMBER);

    private void Start()
    {
        StartListening();
    }

    private void StartListening()
    {
        Debug.Log("Started listening");

        udp.BeginReceive(Receive, new object());
    }

    private void Receive(IAsyncResult ar)
    {
        IPEndPoint ip = new IPEndPoint(IPAddress.Any, PORT_NUMBER);
        byte[] bytes = udp.EndReceive(ar, ref ip);
        string message = Encoding.ASCII.GetString(bytes);

        Debug.Log($"From {ip.Address.ToString()} received: {message}");

        StartListening();
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
