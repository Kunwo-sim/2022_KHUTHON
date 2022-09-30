using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
[Serializable]
public struct UDPPacket
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
    public string keyCode;

    public UDPPacketType packetType;

    [MarshalAs(UnmanagedType.Bool)]
    public bool m_BoolVariable;

    [MarshalAs(UnmanagedType.I4)]
    public int m_IntVariable;

    // 만약 배열을 사용한다면 배열을 반드시 초기화할 것 (new로 할당!)
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public int[] m_IntArray;

    [MarshalAs(UnmanagedType.R4)]
    public float m_FloatlVariable;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string m_StringlVariable1;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string m_StringlVariable2;

    public void Clear()
    {
        m_BoolVariable = false;
        m_IntVariable = 0;
        m_IntArray = null;
        m_FloatlVariable = 0f;
        m_StringlVariable1 = string.Empty;
        m_StringlVariable2 = string.Empty;
    }
}