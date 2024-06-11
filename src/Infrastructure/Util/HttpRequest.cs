using System.Net;

namespace Infrastructure.Util;

public class HttpRequest
{
    public async Task<string> Send(HttpRequestMessage request)
    {
        using (HttpClient client =new HttpClient())
        {
            var response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}