using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ImageDownloader
{
    public class ImageDownloader
    {
        public event Action? DownloadStarted;
        public event Action? DownloadCompleted;
        public CancellationTokenSource? _cancellationTokenSource;
        public bool IsCompleted { get; private set; }
        public async Task DownloadAsync(List<string> remoteUris, CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;

            var downloadTasks = new List<Task>();
       
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
                Console.WriteLine("Error downloading files:{0} ", ex.Message);
            }
        }
        public void Cancel()
        {
           _cancellationTokenSource?.Cancel();
        }
    }
}
