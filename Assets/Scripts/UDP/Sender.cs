using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using UnityEngine;

// 참고 : https://coderzero.tistory.com/entry/%EC%9C%A0%EB%8B%88%ED%8B%B0-%EB%84%A4%ED%8A%B8%EC%9B%8C%ED%81%AC-Boardcast-Sender-Boardcast-Receiver-%EA%B5%AC%EC%A1%B0%EC%B2%B4-%EC%A0%84%EC%86%A1-UdpClient
public class Sender : MonoBehaviour
{
    [SerializeField]
    private int m_Port = 41127;

    private UdpClient m_Sender;
    private IPEndPoint m_IpEndPoint;

    private void Start()
    {
        InitSender();
    }

    private void InitSender()
    {
        m_Sender = new UdpClient();
        m_IpEndPoint = new IPEndPoint(IPAddress.Broadcast, m_Port);
        
        m_Sender.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
    }

    private void OnApplicationQuit()
    {
        CloseSender();
    }

    private void CloseSender()
    {
        if (m_Sender != null)
        {
            m_Sender.Close();
            m_Sender = null;
        }
    }

    public void Send(UDPPacket packet)
    {
        try
        {
            byte[] sendPacket = StructToByteArray(packet);
            m_Sender.Send(sendPacket, sendPacket.Length, m_IpEndPoint);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }

    private byte[] StructToByteArray(object obj)
    {
        int size = Marshal.SizeOf(obj);
        byte[] arr = new byte[size];
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.StructureToPtr(obj, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);

        return arr;
    }
}