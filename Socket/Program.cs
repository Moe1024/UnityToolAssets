using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Serialization;

/// <summary>
/// 服务器端代码，放在服务器的文件里
/// </summary>
public class Program
{
    static Socket socket;
    public static List<Client> clients = new List<Client>();

    static void Main(string[] args)
    {
        Init();//开启服务器
    }
    //开启服务器
    static void Init()
    {
        socket=new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipaddress = IPAddress.Parse("127.0.0.1");
        EndPoint endPoint = new IPEndPoint(ipaddress, 5000);
        socket.Bind(endPoint);
        socket.Listen(100);
        Console.WriteLine(string.Format("开启服务端{0}...", socket.LocalEndPoint));
        while (true)
        {
            Socket clientSocket = socket.Accept();
            Client client = new Client(clientSocket);
            clients.Add(client);
            Console.WriteLine(string.Format("新加入的客户端{0}...", clientSocket.LocalEndPoint));
        }
    }

    /// <summary>
    /// 发送信息给所有的客户端
    /// </summary>
    /// <param name="message"></param>
    public static void SendAllClients(byte[] pack)
    {
        foreach (var o in clients)
        {
            if (o != null)
            {
                if (pack == null) return;
                o.Send(pack);
            }
            else
                clients.Remove(o);
        }
    }

}
public class Client
{
    public Socket socket;
    public byte[] buff = new byte[1024];
    public Thread thread;

    public string ClientID;

    /// <summary>
    /// 初始化生成客户端
    /// </summary>
    /// <param name="socket"></param>
    public Client(Socket socket)
    {
        this.socket = socket;

        thread = new Thread(OnReceive);
        thread.IsBackground = true;//把线程放到后台执行
        thread.Start();
    }

    /// <summary>
    /// 向客户端单独发送信息
    /// </summary>
    /// <param name="pack"></param>
    public void Send(byte[] pack)
    {
        socket.Send(pack);
    }

    /// <summary>
    /// 接收并回复信息
    /// </summary>
    void OnReceive()
    {
        while (true)
        {
            try
            {
                buff = new byte[1024];
                socket.Receive(buff);
                //string strResult = Encoding.Default.GetString(buff);
                //Pack_Trans result = Message.DeserializeFromXmlString<Pack_Trans>(strResult);
                //Console.WriteLine(result);
                Program.SendAllClients(buff);
            }
            catch
            {
            }
        }
    }


}