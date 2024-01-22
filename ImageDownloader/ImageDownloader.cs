using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ImageDownloader
{
    public class ImageDownloader
    {
        public event Action? DownloadStarted;
        public event Action? DownloadCompleted;
        public bool IsCompleted { get; private set; }
        public async Task DownloadAsync(List<string> remoteUris, CancellationToken cancellationToken)
        {
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
                    }, cancellationToken));
                }
                await Task.WhenAll(downloadTasks);
                IsCompleted = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error downloading files:{0} ", ex.Message);
            }
        }
    }
}
