using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

/// <summary>
/// �ͻ��ˣ�����Unity��
/// </summary>
public class Client : MonoBehaviour
{
    public string ClientID;//�ͻ���
    private string currentPack;
    public Player player;

    Socket socket;
    byte[] buff = new byte[1024];
    Thread thread;
    float timer;

    void Start()
    {
        Application.targetFrameRate = 60;
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000));

        thread = new Thread(OnReceive);
        thread.IsBackground = true;
        thread.Start();
    }
    void Update()
    {
        //λ����Ϣ����
        string pack = "";
        pack += "002" + "_";
        pack += player.transform.position.x.ToString() + "_";
        pack += player.transform.position.y.ToString() + "_";
        pack += player.transform.position.z.ToString();
        if (ClientID=="001") Send(pack);

        //λ����Ϣͬ��
        if (currentPack != null)
        {
            string[] strings = currentPack.Split('_');
            if (strings.Length == 4)
            {
                if (strings[0] == ClientID)
                {
                    player.transform.position = new Vector3(
                                float.Parse(strings[1]),
                                float.Parse(strings[2]),
                                float.Parse(strings[3]));
                }
            }
            currentPack = null;
        }
    }

    /// <summary>
    /// �������������Ϣ
    /// </summary>
    /// <param name="pack"></param>
    void Send(string pack)
    {
        byte[] byteData = Encoding.Default.GetBytes(pack);
        socket.Send(byteData);
    }

    /// <summary>
    /// �ӷ�����������Ϣ
    /// </summary>
    void OnReceive()
    {
        while (true)
        {
            try
            {
                buff = new byte[1024];
                socket.Receive(buff);
                string strResult = Encoding.Default.GetString(buff);
                currentPack = strResult;
                Debug.Log("Receive");
            }
            catch
            {

            }
        }
    }
}