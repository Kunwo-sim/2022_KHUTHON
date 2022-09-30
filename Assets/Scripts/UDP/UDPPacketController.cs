using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public enum UDPPacketType
{
    Debug,  // 디버그용
    CTS_EnterClient,    //  입장 요청
    STC_Init,   // 초기화 요청
    STC_StartVote,  // 투표 시작
    CTS_Voting,   // 투표
    STC_EndVote,    // 투표 종료
    STC_ShowNextLine,   // 다음 줄 넘어가기
    STC_StartGame,  // 게임 시작
    STC_EndGame // 게임 종료
}

public enum ProgramType
{
    Server,
    Client
}

/// <summary>
/// 게임에 사용되는 모든 UDP Packet을 전체적으로 관리
/// </summary>
/// <remarks>
/// <para>
/// - STC (Server To Client) : 서버에서 클라이언트로 보내는 패킷
/// </para>
/// <para>
/// - CTS (Client To Server) : 클라이언트에서 서버로 보내는 패킷
/// </para>
/// <para>
/// - Sender, Receiver는 다른 게임에서도 재활용하도록 별도의 클래스로 제작
/// </para>
/// <para>
/// - 게임에 종속적인 네트워크 코드는 이 클래스에서 모두 관리
/// </para>
/// </remarks>
public class UDPPacketController : Singleton<UDPPacketController>
{
    [SerializeField]
    private ProgramType programType;

    [SerializeField]
    private Sender sender;

    [SerializeField]
    private Receiver receiver;

    private Stack<UDPPacket> packetPool;

    private void Awake()
    {
        packetPool = new Stack<UDPPacket>();
    }

    private void Start()
    {
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Alpha0))
            .Subscribe(_ =>
            {
                SendDebugPacket();
            })
            .AddTo(gameObject);

        receiver.OnProcessReceivedPacket += ProcessPacket;
    }

    private UDPPacket GetPacketContainer()
    {
        UDPPacket newPacket;

        if (packetPool.TryPop(out newPacket))
        {
            newPacket.Clear();
            return newPacket;
        }

        return new UDPPacket();
    }

    #region Send Method

    private void SendDebugPacket()  // STC, CTS
    {
        UDPPacket sendPacket = GetPacketContainer();

        sendPacket.packetType = UDPPacketType.Debug;
        sendPacket.m_IntVariable = new System.Random().Next();

        sender.Send(sendPacket);

        packetPool.Push(sendPacket);
    }

    /// <summary>CTS : 클라이언트가 서버에 입장을 요청</summary>
    public void SendEnterClientPacket() // CTS
    {
        if (programType is ProgramType.Server)
        {
            return;
        }

        UDPPacket sendPacket = GetPacketContainer();

        sendPacket.packetType = UDPPacketType.CTS_EnterClient;

        sender.Send(sendPacket);

        packetPool.Push(sendPacket);
    }

    /// <summary>STC : 서버가 클라이언트에 현재 진행 상황을 보냄</summary>
    /// <param name="nodeName">Dialogue에서 진행 중인 Node 이름을 출력</param>
    /// <param name="textID">Dialogue에서 진행 중인 Text의 고유 ID를 출력</param>
    public void SendInitPacket(string nodeName, string textID)  // STC
    {
        if (programType is ProgramType.Client)
        {
            return;
        }

        UDPPacket sendPacket = GetPacketContainer();

        sendPacket.packetType = UDPPacketType.STC_Init;
        sendPacket.m_StringlVariable1 = nodeName;
        sendPacket.m_StringlVariable2 = textID;

        sender.Send(sendPacket);

        packetPool.Push(sendPacket);
    }

    /// <summary>STC : 서버가 클라이언트에 투표 시작을 알림</summary>
    public void SendStartVotePacket()   // STC
    {
        if (programType is ProgramType.Client)
        {
            return;
        }

        UDPPacket sendPacket = GetPacketContainer();

        sendPacket.packetType = UDPPacketType.STC_StartVote;

        sender.Send(sendPacket);

        packetPool.Push(sendPacket);
    }

    /// <summary>CTS : 클라이언트가 서버에 투표 항목을 알림</summary>
    /// <param name="answerIndex">선택한 투표 항목의 인덱스</param>
    public void SendVotingPacket(int answerIndex)   // CTS
    {
        if (programType is ProgramType.Server)
        {
            return;
        }

        UDPPacket sendPacket = GetPacketContainer();

        sendPacket.packetType = UDPPacketType.CTS_Voting;
        sendPacket.m_IntVariable = answerIndex;

        sender.Send(sendPacket);

        packetPool.Push(sendPacket);
    }

    /// <summary>STC : 서버가 클라이언트에 투표 종료를 알림</summary>
    public void SendEndVotePacket() // STC
    {
        if (programType is ProgramType.Client)
        {
            return;
        }

        UDPPacket sendPacket = GetPacketContainer();

        sendPacket.packetType = UDPPacketType.STC_EndVote;

        sender.Send(sendPacket);

        packetPool.Push(sendPacket);
    }

    /// <summary>STC : 서버가 클라이언트에 다음 줄로 넘어갔음을 알림</summary>
    public void SendShowNextLinePacket()    // STC
    {
        if (programType is ProgramType.Client)
        {
            return;
        }

        UDPPacket sendPacket = GetPacketContainer();

        sendPacket.packetType = UDPPacketType.STC_ShowNextLine;

        sender.Send(sendPacket);

        packetPool.Push(sendPacket);
    }

    /// <summary>STC : 서버가 클라이언트에 게임 시작을 알림</summary>
    public void SendStartGamePacket()   // STC
    {
        if (programType is ProgramType.Client)
        {
            return;
        }

        UDPPacket sendPacket = GetPacketContainer();

        sendPacket.packetType = UDPPacketType.STC_StartGame;

        sender.Send(sendPacket);

        packetPool.Push(sendPacket);
    }

    /// <summary>STC : 서버가 클라이언트에 게임 종료를 알림</summary>
    public void SendEndGamePacket() // STC
    {
        if (programType is ProgramType.Client)
        {
            return;
        }

        UDPPacket sendPacket = GetPacketContainer();

        sendPacket.packetType = UDPPacketType.STC_EndGame;

        sender.Send(sendPacket);

        packetPool.Push(sendPacket);
    }

    #endregion

    #region Receive Method

    public void ProcessPacket(UDPPacket packet)
    {
        switch (packet.packetType)
        {
            case UDPPacketType.Debug:   // 디버그용
                ReceiveDebugPacket(packet.m_IntVariable);
                break;
            case UDPPacketType.CTS_EnterClient: //  입장 요청
                ReceiveEnterClientPacket();
                break;
            case UDPPacketType.STC_Init:    // 초기화 요청
                ReceiveInitPacket(packet.m_StringlVariable1, packet.m_StringlVariable2);
                break;
            case UDPPacketType.STC_StartVote:  // 투표 시작
                ReceiveStartVotePacket();
                break;
            case UDPPacketType.CTS_Voting:  // 투표
                ReceiveVotingPacket(packet.m_IntVariable);
                break;
            case UDPPacketType.STC_EndVote: // 투표 종료
                ReceiveEndVotePacket();
                break;
            case UDPPacketType.STC_ShowNextLine:    // 다음 줄 넘어가기
                ReceiveShowNextLinePacket();
                break;
            case UDPPacketType.STC_StartGame: // 게임 시작
                ReceiveStartGamePacket();
                break;
            case UDPPacketType.STC_EndGame: // 게임 종료
                ReceiveEndGamePacket();
                break;
        }
    }

    // 클라에서 받아서 처리하기 위한 스크립트들
    public Vote vote;

    private void ReceiveDebugPacket(int debugValue)
    {
        Debug.Log($"[KHW] UDPPacketController - ReceiveDebugPacket : debugValue => {debugValue}");
    }

    /// <summary>서버는 현재 접속한 클라이언트에게 Dialogue 정보를 보냄</summary>
    private void ReceiveEnterClientPacket() // Server에서 처리
    {
        if (programType is ProgramType.Client)
        {
            return;
        }
    }

    /// <summary>클라이언트는 서버가 보낸 Dialogue 정보로 Server 진행 상황과 동기화 진행</summary>
    private void ReceiveInitPacket(string nodeName, string textID)    // Client에서 처리
    {
        if (programType is ProgramType.Server)
        {
            return;
        }
    }

    /// <summary>클라이언트는 투표 진행을 위한 연출을 보여주고 투표 버튼을 띄움</summary>
    private void ReceiveStartVotePacket()   // Client에서 처리
    {
        if (programType is ProgramType.Server)
        {
            return;
        }

        vote?.StartVote();
    }

    /// <summary>서버는 투표 인덱스를 확인 후 값을 1 증가시킴</summary>
    private void ReceiveVotingPacket(int answerIndex)  // Server에서 처리
    {
        if (programType is ProgramType.Client)
        {
            return;
        }

        vote?.ChoiceIncrease(answerIndex);
    }

    /// <summary>클라이언트는 투표 완료 연출을 보여줌</summary>
    private void ReceiveEndVotePacket() // Client에서 처리
    {
        if (programType is ProgramType.Server)
        {
            return;
        }

        vote?.EndVote();
    }

    /// <summary>클라이언트는 다음 대사를 보여줌</summary>
    private void ReceiveShowNextLinePacket()    // Client에서 처리
    {
        if (programType is ProgramType.Server)
        {
            return;
        }
    }

    /// <summary>클라이언트는 Title에 있을 때 게임 화면으로 이동</summary>
    private void ReceiveStartGamePacket()   // Client에서 처리
    {
        if (programType is ProgramType.Server)
        {
            return;
        }

        if (SceneManager.GetActiveScene().name == "Title")
        {
            SceneManager.LoadScene("InGame");
        }
    }

    /// <summary>클라이언트는 게임 화면에 있을 때 Title로 이동</summary>
    private void ReceiveEndGamePacket() // Client에서 처리
    {
        if (programType is ProgramType.Server)
        {
            return;
        }

        if (SceneManager.GetActiveScene().name == "InGame")
        {
            SceneManager.LoadScene("Title");
        }
    }

    #endregion
}