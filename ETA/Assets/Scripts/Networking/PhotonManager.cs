using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    #region private fields
    private string gameVersion = "1";
    private byte maxplayersPerRoom = 4;
    private bool isConnecting;
    private string roomName;

    // room list -use> update, print room
    List<RoomInfo> roomlist = new List<RoomInfo>();

    // current room
    private RoomInfo selectRoom;
    #endregion

    #region public fields
    public bool IsConnecting{
        set { isConnecting = value; }
        get { return isConnecting; } 
    }
    public string RoomName
    {
        set { roomName = value; }
        get {
            roomName = selectRoom.Name;
            int lastIndex = selectRoom.Name.LastIndexOf("`");
            if (lastIndex != -1)
                roomName = roomName.Substring(0, lastIndex);
            return roomName;
        }
    }

    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Connect();
    }
    void Start()
    {
    }

    #endregion


    #region public Methods
    // ��Ʈ��ũ ������ ���� Connect �Լ�
    public void Connect()
    {
        if (isConnecting)
        {
            // �̹� ����� ���
            Debug.Log("PhotonNetwork: �̹� ����Ǿ� �ֽ��ϴ�.");
        }
        else
        {
            // setting
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.NickName = "Player";

            // ���� ���� ������ kr�� �����Ͽ� �ѱ� �������� ����ǵ��� ����
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "kr";

            // start connect
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    // Make multy Rroom
    public void MakeRoom()
    {
        string roomName = "RoomName";
        string captainName = "captain";

        if (roomName.Length == 0) return;

        if (captainName == null)
            captainName = "player";

        if (captainName == null || roomName == null) return;

        RoomOptions room = new RoomOptions();
        room.MaxPlayers = maxplayersPerRoom;
        room.IsVisible = true;
        room.IsOpen = true;
        room.CleanupCacheOnLeave = false;

        // �õ� ����
        int seed = (int)System.DateTime.Now.Ticks;

        // ������ �� �̸� + ` + �õ� ��
        roomName = roomName + "`" + seed;

        // Register in lobby
        room.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "captain", captainName }, { "seed", seed } };
        room.CustomRoomPropertiesForLobby = new string[] { "captain", "seed" };
        PhotonNetwork.CreateRoom(roomName, room);
    }

    public void ClickRoom(int roomNumber)
    {
        selectRoom = roomlist[roomNumber];
    }

    public void roomEnter()
    {
        // ���߿� ����
        string nickname = "Player";//UserInfo.GetInstance().getNickName();

        if (nickname == null) return;

        if (selectRoom.MaxPlayers <= selectRoom.PlayerCount)
        {
            // �濡 ��� ����� ���� �� �̻� �� �� ����
            return;
        }

        PhotonNetwork.JoinRoom(selectRoom.Name);

        PhotonNetwork.JoinRandomRoom();
    }

    public void ExitRoom()
    {
        // ���� �ƴϸ� Ż�� �Ұ�
        if (!PhotonNetwork.InRoom) return;
        // room -> Lobby
        PhotonNetwork.LeaveRoom();
    }

    public List<RoomInfo> printList()
    {
        int idx = 0;
        foreach (RoomInfo room in roomlist)
        {
            ExitGames.Client.Photon.Hashtable has = room.CustomProperties;

            // ������ ` ���� �õ尪�� �����ϰ� ���
            string printRoomName = room.Name;
            int lastIndex = printRoomName.LastIndexOf("`");
            if (lastIndex != -1)
                printRoomName = printRoomName.Substring(0, lastIndex);
            idx++;

            //string roomInfo = "room : " + room.Value.Name + " \n" + room.Value.PlayerCount + " / " + room.Value.MaxPlayers + "\n" + "isvisible : " + room.Value.IsVisible + "\n" + "isopen : " + room.Value.IsOpen
            //    + "\n captain : " + has["captain"] + "\n" + has["ispassword"] + " / " + has["password"];
        }

        return roomlist;
    }

    public void PrintPlayer()
    {

    }
    #endregion

    #region MonoBehaviourPunCallbacks callbacks
    public override void OnConnectedToMaster()
    {
        isConnecting = true;
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo rooom in roomList)
        {
            bool change = false;
            for (int i = 0; i < roomlist.Count; i++)
            {
                if (roomlist[i].Name == rooom.Name)
                {
                    if (rooom.PlayerCount != 0)
                        roomlist[i] = rooom;    
                    // no player, no open, no multy
                    else if (rooom.PlayerCount == 0 || !rooom.IsOpen || !rooom.IsVisible)
                    {
                        roomlist.Remove(roomlist[i]);
                    }
                    change = true;
                }
            }

            if (!change)
            {
                if (rooom.PlayerCount != 0)
                    roomlist.Add(rooom);
            }
        }
        printList();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + " : " + message);
        // ��Ƽ �� á�� �� ��� �����
        //JoiningWarning.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        // ���� �÷��̾��� ĳ���͸� �����ϰ� Photon ��Ʈ��ũ�� ���
        GameObject player = PhotonNetwork.Instantiate("Prefabs/Player/Player", Vector3.zero, Quaternion.identity);
    }


    #endregion

    // ������ Ŭ���̾�Ʈ�� ����Ǿ��� �� ȣ��Ǵ� �޼ҵ�
}
