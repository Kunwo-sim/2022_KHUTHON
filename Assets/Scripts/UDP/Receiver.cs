using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using UnityEngine;

// 참고 : https://coderzero.tistory.com/entry/%EC%9C%A0%EB%8B%88%ED%8B%B0-%EB%84%A4%ED%8A%B8%EC%9B%8C%ED%81%AC-Boardcast-Sender-Boardcast-Receiver-%EA%B5%AC%EC%A1%B0%EC%B2%B4-%EC%A0%84%EC%86%A1-UdpClient
public class Receiver : MonoBehaviour
{
    [SerializeField]
    private int m_Port = 41127;

    private UdpClient m_Receiver;
    private IPEndPoint m_IpEndPoint;

    public event Action<UDPPacket> OnProcessReceivedPacket;

    private void Start()
    {
        InitReceiver();
    }

    private void InitReceiver()
    {
        m_Receiver = new UdpClient();
        m_IpEndPoint = new IPEndPoint(IPAddress.Any, m_Port);

        m_Receiver.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
    
        m_Receiver.ExclusiveAddressUse = false;
        m_Receiver.Client.Bind(m_IpEndPoint);
    }

    private void Update()
    {
        Receive();
    }

    private void Receive()
    {
        if (m_Receiver.Available != 0)
        {
            byte[] packet = new byte[1024];

            try
            {
                packet = m_Receiver.Receive(ref m_IpEndPoint);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.ToString());
                return;
            }

            // 받은 값 처리
            OnProcessReceivedPacket?.Invoke(ByteArrayToStruct<UDPPacket>(packet));
        }
    }

    private T ByteArrayToStruct<T>(byte[] buffer) where T : struct
    {
        int size = Marshal.SizeOf(typeof(T));

        if (size > buffer.Length)
        {
            throw new Exception();
        }

        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(buffer, 0, ptr, size);
        T obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
        Marshal.FreeHGlobal(ptr);

        return obj;
    }

    private void OnApplicationQuit()
    {
        CloseReceiver();
    }

    private void CloseReceiver()
    {
        if (m_Receiver != null)
        {
            m_Receiver.Close();
            m_Receiver = null;
        }
    }
}