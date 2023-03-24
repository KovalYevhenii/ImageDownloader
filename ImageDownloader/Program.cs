using System.Net;
using System.Runtime.CompilerServices;

namespace ImageDownloader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ImageDownloader imageDownloader = new();
            CancellationTokenSource cts = new CancellationTokenSource();

            imageDownloader.DownloadStarted += () =>
                {
                    Console.WriteLine("Download started...");
                };
            imageDownloader.DownloadCompleted += () =>
            {
                Console.WriteLine("Download completed!");
            };

            var remoteUris = new List<string>
            {
                "https://effigis.com/wp-content/uploads/2015/02/Iunctus_SPOT5_5m_8bit_RGB_DRA_torngat_mountains_national_park_8bits_1.jpg",
                "https://effigis.com/wp-content/uploads/2015/02/Iunctus_SPOT5_5m_8bit_RGB_DRA_torngat_mountains_national_park_8bits_1.jpg",
                "https://effigis.com/wp-content/uploads/2015/02/Iunctus_SPOT5_5m_8bit_RGB_DRA_torngat_mountains_national_park_8bits_1.jpg",
                "https://effigis.com/wp-content/uploads/2015/02/Iunctus_SPOT5_5m_8bit_RGB_DRA_torngat_mountains_national_park_8bits_1.jpg",
                "https://effigis.com/wp-content/uploads/2015/02/Iunctus_SPOT5_5m_8bit_RGB_DRA_torngat_mountains_national_park_8bits_1.jpg",
                "https://effigis.com/wp-content/uploads/2015/02/Iunctus_SPOT5_5m_8bit_RGB_DRA_torngat_mountains_national_park_8bits_1.jpg",
                "https://effigis.com/wp-content/uploads/2015/02/Iunctus_SPOT5_5m_8bit_RGB_DRA_torngat_mountains_national_park_8bits_1.jpg",
                "https://effigis.com/wp-content/uploads/2015/02/Iunctus_SPOT5_5m_8bit_RGB_DRA_torngat_mountains_national_park_8bits_1.jpg",
                "https://effigis.com/wp-content/uploads/2015/02/Iunctus_SPOT5_5m_8bit_RGB_DRA_torngat_mountains_national_park_8bits_1.jpg",
                "https://effigis.com/wp-content/uploads/2015/02/Iunctus_SPOT5_5m_8bit_RGB_DRA_torngat_mountains_national_park_8bits_1.jpg",
            };

            Console.WriteLine("Press 'A' to exit or any other key to check the download status");

            var downloadTask = imageDownloader.DownloadAsync(remoteUris, cts.Token);
            try
            {
                if (Console.ReadKey().Key.ToString().ToUpper() == "A")
                {
                    cts.Cancel();
                    throw new OperationCanceledException();
                }
                else
                {
                    Console.WriteLine("\nImage downloading Status: {0}", imageDownloader.IsCompleted);
                    Console.ReadKey();
                    Task.WaitAll(downloadTask);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("\nOperation was cancelled");
            }

        }
    }

}