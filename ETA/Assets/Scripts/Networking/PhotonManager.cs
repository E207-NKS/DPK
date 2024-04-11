using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    #region private fields
    string gameVersion = "2";
    private byte maxplayersPerRoom = 4;
    bool isConnecting;

    // room list -use> update, print room
    List<RoomInfo> roomlist = new List<RoomInfo>();

    // current room
    private RoomInfo selectRoom;
    #endregion

    #region public fields

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
        isConnecting = true;
        if (PhotonNetwork.IsConnected)
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

    public void ExitRoom()
    {
        // ���� �ƴϸ� Ż�� �Ұ�
        if (!PhotonNetwork.InRoom) return;
        // room -> Lobby
        PhotonNetwork.LeaveRoom();

    }

    // Enter the dungeon portal without a room
    public void MakePersonalRoom()
    {
        Debug.Log("CreatePersonalRoom");
        //PhotonManager manager = GameObject.Find("gm").GetComponent<PhotonManager>();

        string roomName = "RoomName";
        string captainName = "Captin";

        RoomOptions room = new RoomOptions();
        room.MaxPlayers = maxplayersPerRoom;
        room.IsVisible = false;
        room.IsOpen = false;
        room.CleanupCacheOnLeave = false;

        // �õ� ����
        int seed = (int)System.DateTime.Now.Ticks;

        // set custom properties
        room.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "captain", captainName }, { "seed", seed } };
        room.CustomRoomPropertiesForLobby = new string[] { "captain", "seed" };

        PhotonNetwork.NickName = "Player";
        roomName = roomName + "`" + seed;
        PhotonNetwork.CreateRoom(roomName, room);

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

        bool ispassword = false;
        string password = "";

        // �õ� ����
        int seed = (int)System.DateTime.Now.Ticks;

        // ������ �� �̸� + ` + �õ� ��
        roomName = roomName + "`" + seed;

        // Register in lobby
        if (ispassword)
        {
            room.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "captain", captainName }, { "ispassword", ispassword }, { "password", password }, { "seed", seed } };
            room.CustomRoomPropertiesForLobby = new string[] { "captain", "ispassword", "password", "seed" };
        }
        else
        {
            room.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "captain", captainName }, { "ispassword", ispassword }, { "seed", seed } };
            room.CustomRoomPropertiesForLobby = new string[] { "captain", "ispassword", "seed" };
        }
        PhotonNetwork.CreateRoom(roomName, room);
    }

    public void ClickRoom(int roomNumber)
    {
        selectRoom = roomlist[roomNumber];
        string printRoomName = selectRoom.Name;
        int lastIndex = printRoomName.LastIndexOf("`");
        if (lastIndex != -1)
            printRoomName = printRoomName.Substring(0, lastIndex);
        if ((bool)selectRoom.CustomProperties["ispassword"])
        {
            Debug.Log(GameObject.Find("Party Joining PW Content Text"));
            GameObject.Find("Party Joining PW Content Text").GetComponent<TextMeshProUGUI>().text = printRoomName + "��Ƽ�� �����Ͻðڽ��ϱ�?";

            //password panel open
            GameObject.Find("Party Joining Window").SetActive(false);
        }
        else
            GameObject.Find("Party Joining Content Text").GetComponent<TextMeshProUGUI>().text = printRoomName + "��Ƽ�� �����Ͻðڽ��ϱ�?";
    }

    public void roomEnter()
    {
        // ���߿� ����
        string nickname = "Player";//UserInfo.GetInstance().getNickName();

        if (nickname == null) return;

        if (selectRoom.MaxPlayers <= selectRoom.PlayerCount)
        {
            return;
        }

        // no password enter
        PhotonNetwork.JoinRoom(selectRoom.Name);
    }


    public void printList()
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
    }

    public void OpenPartyWindow()
    {
        if (PhotonNetwork.InRoom)
        {
            PrintPlayer();
        }
        else
        {
        }
    }

    public void PrintPlayer()
    {
        int idx = 0;

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log(player.Key);
            Debug.Log((int)player.Value.CustomProperties["Number"]);
            //idx++;
        }
    }

    public int GetCurrentPartyMemberCount()
    {
        int idx = 0;
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.CustomProperties.ContainsKey("Number"))
            {
                idx++;
            }
        }
        return idx;
    }

    public void PrintPartyStatus()
    {
        Debug.Log("Party Status Update");

        for (int i = 0; i < 4; i++)
        {
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log(player.Value.CustomProperties);
            Debug.Log(player.Value.CustomProperties["Number"]);
        }
    }

    #endregion

    #region MonoBehaviourPunCallbacks callbacks
    public override void OnConnectedToMaster()
    {
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
    #endregion

    // ������ Ŭ���̾�Ʈ�� ����Ǿ��� �� ȣ��Ǵ� �޼ҵ�
}
