using AwSocket;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace MyRobotZone.UWP
{
    public class NetworkStream : INetworkStream, IDisposable
    {
        StreamReader streamReader;
        StreamWriter streamWriter;

        public NetworkStream(StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Stream inStream = args.Socket.InputStream.AsStreamForRead();
            this.streamReader = new StreamReader(inStream);

            Stream outStream = args.Socket.OutputStream.AsStreamForWrite();
            this.streamWriter = new StreamWriter(outStream);
        }

        public void Dispose()
        {
            this.streamWriter.Dispose();
            this.streamReader.Dispose();
        }

        public Task<string> ReadAsync()
        {
            return this.streamReader.ReadToEndAsync();
        }

        public Task WriteAsync(string message)
        {
            return this.streamWriter.WriteLineAsync(message);
        }
    }
}
