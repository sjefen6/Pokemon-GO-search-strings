namespace SearchStringGenerator;

public class FileDownloader
{
    public static async Task DownloadFileIfChangedAsync(string url, string localPath)
    {
        var etagPath = GetEtagPath(localPath);
        using var httpClient = new HttpClient();
        await SetEtagIfFileExistsAsync(etagPath, httpClient);
        var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        if(response.StatusCode == HttpStatusCode.NotModified)
        {
            Console.WriteLine("Server returned 304 Not Modified. Delete etag file to force download.");
            return;
        }
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(
                $"Failed to download {url}. HTTP status: {response.StatusCode}. Falling back to local file.");
            return;
        }

        await DownloadFileAsync(response, localPath);
        await WriteEtagToFileAsync(response, etagPath);
    }
    
    private static async Task SetEtagIfFileExistsAsync(string etagPath, HttpClient httpClient)
    {
        if (File.Exists(etagPath))
        {
            var etag = await File.ReadAllTextAsync(etagPath);
            httpClient.DefaultRequestHeaders.IfNoneMatch.ParseAdd(etag);
        }
    }

    private static async Task DownloadFileAsync(HttpResponseMessage response, string localPath)
    {
        var oldMd5 = File.Exists(localPath) ? ComputeMd5Hash(localPath) : null;
        
        await using (var fileStream = File.Create(localPath))
        {
            await using (var httpStream = await response.Content.ReadAsStreamAsync())
            {
                Debug.WriteLine("Downloading file");
                await httpStream.CopyToAsync(fileStream);
            }
        }

        if (oldMd5 != null)
        {
            var newMd5 = ComputeMd5Hash(localPath);
            if (oldMd5 == newMd5)
            {
                Console.WriteLine("File was downloaded, but the content was unchanged.");
            }
        }
    }

    private static async Task WriteEtagToFileAsync(HttpResponseMessage response, string etagPath)
    {
        if (response.Headers.ETag != null)
        {
            var newEtag = response.Headers.ETag.ToString();
            await File.WriteAllTextAsync(etagPath, newEtag);
        }
    }

    private static string ComputeMd5Hash(string filename)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(filename);
        var hash = md5.ComputeHash(stream);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }

    public static string GetEtagPath(string file) => $"{file}.etag";
}