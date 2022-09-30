using System;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

public class Receiver : MonoBehaviour
{
    private UdpClient m_Receiver;
    public int m_Port = 50001;
    public Packet m_ReceivePacket = new Packet();
    private IPEndPoint m_IpEndPoint;

    void Start()
    {
        InitReceiver();
    }

    void Update()
    {
        Receive();
    }

    void OnApplicationQuit()
    {
        CloseReceiver();
    }

    void InitReceiver()
    {
        m_Receiver = new UdpClient();

        m_IpEndPoint = new IPEndPoint(IPAddress.Any, m_Port);

        m_Receiver.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        m_Receiver.ExclusiveAddressUse = false;
        m_Receiver.Client.Bind(m_IpEndPoint);
    }

    void Receive()
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
                Debug.Log(ex.ToString());
                return;
            }

            m_ReceivePacket = ByteArrayToStruct<Packet>(packet);
            DoReceivePacket(); // 받은 값 처리
        }
    }

    void DoReceivePacket()
    {
        Debug.LogFormat($"BoolVariable = {m_ReceivePacket.m_BoolVariable} " +
              $"IntlVariable = {m_ReceivePacket.m_IntVariable} " +
              $"m_IntArray[0] = {m_ReceivePacket.m_IntArray[0]} " +
              $"m_IntArray[1] = {m_ReceivePacket.m_IntArray[1] } " +
              $"FloatlVariable = {m_ReceivePacket.m_FloatlVariable} " +
              $"StringlVariable = {m_ReceivePacket.m_StringlVariable}");
        //출력: BoolVariable = True IntlVariable = 13 m_IntArray[0] = 7 m_IntArray[1] = 47 FloatlVariable = 2020 StringlVariable = Coder Zero
    }

    void CloseReceiver()
    {
        if (m_Receiver != null)
        {
            m_Receiver.Close();
            m_Receiver = null;
        }
    }

    T ByteArrayToStruct<T>(byte[] buffer) where T : struct
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
}