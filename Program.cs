using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FluentFTP;

namespace FTP_Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // TODO:: Implement single tone Design Pattern
            await DoSomeThing();
            //await UploadFile();

        }

        private static FtpClient CreateFtpClient(string host = "almedadsoft.com", int port = 5000, string userName = "Sd$-Bo0K$", string password = "Sd$-Bo0K$@Medad")
        {
            return new FtpClient(host, port, userName, password);
        }

        private static async Task UploadFile()
        {
            const string filePath = @"D:\3.pdf";

            // Establish ftp 
            using var ftp = CreateFtpClient();

            await using FileStream fileStream = File.OpenRead(path: filePath);

            var memoryStream = new MemoryStream();

            await fileStream.CopyToAsync(memoryStream);

            // Read Stream
            memoryStream.Position = 0;

            await ftp.UploadAsync(fileStream: memoryStream, remotePath: Path.GetFileName(path: filePath));

        }

        public static async Task DoSomeThing()
        {
            // Establish ftp 
            using var ftp = CreateFtpClient();

            await ftp.ConnectAsync().ConfigureAwait(false);

            // Get content from ftp directory
            var listing = await ftp.GetListingAsync();

            // Get files in directory
            foreach (var ftpListItem in listing)
            {
                // Continue if item not file
                if (ftpListItem.Type != FtpFileSystemObjectType.File) continue;
                if (ftpListItem.Name != "3.pdf") continue;

                // Create stream to save data on it
                await using var memoryStream = new MemoryStream();

                // Download file to stream
                await ftp.DownloadAsync(memoryStream, ftpListItem.Name);

                // Read Stream
                memoryStream.Position = 0;

                // Create FileStream
                await using FileStream file = new FileStream(@"D:\file.pdf", FileMode.Create, System.IO.FileAccess.Write);

                await memoryStream.CopyToAsync(file);
            }
        }



    }
}
