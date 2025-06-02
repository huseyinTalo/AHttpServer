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
        int offset = 0;

        int dataChunk = 128;

        Console.WriteLine($"Listening on {port}");

        using TcpClient tcpClient = tcpListener.AcceptTcpClient();

        byte[] data = new byte[dataChunk];

        data[dataChunk - 1] = 7;
        
        List<byte[]> datas = new List<byte[]>();

        int bytesRead;

        do
        {
            bytesRead = tcpClient.GetStream().Read(data, offset, dataChunk);


            byte[] bufferCopy = new byte[bytesRead];

            Array.Copy(data, 0, bufferCopy, 0, bytesRead);

            datas.Add(bufferCopy);

            if(bytesRead != data.Length)
            {
                break;
            }
        }
        while (data[dataChunk - 1] != 0);

        string message = string.Empty;

        foreach (var item in datas)
        {
            foreach (var byteItem in data)
            {
                message += byteItem;
            }
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
            default:
                tcpClient.GetStream().Write(responseBytes, 0, responseBytes.Length);
                break;
        }

        tcpClient.Close();

    }
}
catch (Exception ex)
{
    throw ex;
}
finally
{
    tcpListener.Stop();
}