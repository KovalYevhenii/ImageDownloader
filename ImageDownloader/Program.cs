using System.Net;

namespace ImageDownloader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ImageDownloader imageDownloader = new();
            imageDownloader.DownloadStarted += () => Console.WriteLine("Download started...");
            imageDownloader.DownloadCompleted += () => Console.WriteLine("Download completed!");

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
            imageDownloader.DownloadAsync(remoteUris);

            Console.WriteLine("Press 'A' to exit or any other key to check the download status");
            if (Console.ReadKey().Key.ToString().ToUpper() == "A")
            {
                imageDownloader.Cancel();
            }
            else
            {
                Console.WriteLine("\n Image downloading Status: {0}", imageDownloader.IsCompleted);
                Console.ReadKey();
            }
        }
    }
    public class ImageDownloader
    {
        public event Action? DownloadStarted;
        public event Action? DownloadCompleted;
        private CancellationTokenSource? _cancellationTokenSource;
        public bool IsCompleted { get; private set; }
        public async Task DownloadAsync(List<string> remoteUris)
        {
               var downloadTasks = new List<Task>();
                 _cancellationTokenSource = new CancellationTokenSource();
            try
            {
                 DownloadStarted?.Invoke();
                 var i = 0;
                foreach (var remoteUri in remoteUris)
                {
                    var fileName = $"bigimage{i}.jpg";
                    i++;
                    downloadTasks.Add(Task.Run(async () =>
                    {
                        using (var myWebClient = new WebClient())
                        {
                            myWebClient.DownloadFileCompleted += (sender, e) => DownloadCompleted?.Invoke();
                            await myWebClient.DownloadFileTaskAsync(new Uri(remoteUri), fileName);
                        }
                    }, _cancellationTokenSource.Token));
                }
                await Task.WhenAll(downloadTasks);
                DownloadCompleted?.Invoke();
                IsCompleted = true;
            } 
            catch (OperationCanceledException)
            {
                Console.WriteLine("Download canceled");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error downloading files:{0} ", ex.Message) ;
            }
        }
        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
