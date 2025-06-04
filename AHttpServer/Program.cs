using System.Net;
using System.Net.Sockets;
using System.Text;


int port = 6851;

IPAddress localAddr = IPAddress.Parse("127.0.0.1");

TcpListener tcpListener = new TcpListener(localAddr, port);

try
{

    tcpListener.Start();

    Console.WriteLine($"Server listening on {localAddr}:{port}");

    while (true)
    {
        Console.WriteLine($"Waiting for connections...");
        using TcpClient tcpClient = tcpListener.AcceptTcpClient();
        Console.WriteLine("Client connected!");

        // Read the request
        NetworkStream stream = tcpClient.GetStream();
        List<byte> allData = new List<byte>();
        byte[] buffer = new byte[1024];

        // Read data until we have a complete HTTP request
        while (stream.DataAvailable || allData.Count == 0)
        {
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            if (bytesRead == 0) break;

            for (int i = 0; i < bytesRead; i++)
            {
                allData.Add(buffer[i]);
            }

            // Check if we have end of HTTP headers (\r\n\r\n)
            if (allData.Count >= 4)
            {
                byte[] lastFour = allData.GetRange(allData.Count - 4, 4).ToArray();
                if (lastFour[0] == '\r' && lastFour[1] == '\n' &&
                    lastFour[2] == '\r' && lastFour[3] == '\n')
                {
                    break;
                }
            }
        }

        // Convert to string
        string request = Encoding.UTF8.GetString(allData.ToArray());
        Console.WriteLine($"Received request:\n{request}");

        // Parse the first line to get the HTTP method
        string[] lines = request.Split('\n');
        string[] requestParts = lines[0].Split(' ');
        string method = requestParts.Length > 0 ? requestParts[0].Trim() : "UNKNOWN";

        // Create response
        string responseBody = $"Hello World! Method: {method}";
        string httpResponse =
            "HTTP/1.1 200 OK\r\n" +
            "Content-Type: text/plain\r\n" +
            $"Content-Length: {Encoding.UTF8.GetByteCount(responseBody)}\r\n" +
            "Connection: close\r\n" +
            "\r\n" +
            responseBody;

        byte[] responseBytes = Encoding.UTF8.GetBytes(httpResponse);



        switch (method)
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
    Console.WriteLine($"Error: {ex}");
    throw ex;
}
finally
{
    tcpListener.Stop();
}