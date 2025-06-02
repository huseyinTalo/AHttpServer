using System.Net;
using System.Net.Sockets;
using System.Text;


int port = 6851;

IPAddress localAddr = IPAddress.Parse("127.0.0.1");

TcpListener tcpListener = new TcpListener(localAddr, port);
try
{

    tcpListener.Start();


    while (true)
    {
        Console.WriteLine($"Listening on {port}");

        using TcpClient tcpClient = tcpListener.AcceptTcpClient();

        byte[] data = new byte[512];

        int offset = 0;

        int count = 512;

        tcpClient.GetStream().Read(data, offset, count);

        string message = string.Empty;

        foreach (var item in data)
        {
            message += (char)item;
        }

        string[] messageArray = message.Split(" ");

        string firstWord = messageArray[0];

        string httpResponse =
           "HTTP/1.1 200 OK\r\n" +
           "Content-Type: text/plain\r\n" +
           "Content-Length: 11\r\n" +
           "\r\n" +
           "Hello World";

        byte[] responseBytes = Encoding.UTF8.GetBytes(httpResponse);



        switch (firstWord)
        {
            case "GET":
                tcpClient.GetStream().Write(responseBytes, 0, responseBytes.Length);
                break;
            case "POST":
                tcpClient.GetStream().Write(responseBytes, 0, responseBytes.Length);
                break;
            case "PUT":
                tcpClient.GetStream().Write(responseBytes, 0, responseBytes.Length);
                break;
            case "DELETE":
                tcpClient.GetStream().Write(responseBytes, 0, responseBytes.Length);
                break;
        }

        tcpClient.Close();
    }
}
catch (Exception ex)
{
    throw;
}
finally
{
    tcpListener.Stop();
}