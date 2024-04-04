using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

public class HttpClientHelper
{
	private HttpClient _httpClient = new();
	private string EndPoint = Constants.ServerEndPoint + Constants.button;
	
	public async Task<T> GetRequest<T>(int index = 0)
	{
		using var response = await _httpClient.GetAsync(index == 0 ? EndPoint : $"{EndPoint}/{index}");

		if (!response.IsSuccessStatusCode)
		{
			throw new HttpRequestException("RequestMetaData response statusCode: " +
			                               (int) response.StatusCode + ": " + response.StatusCode);
		}

		var json = await response.Content.ReadAsStringAsync();
		var data =  JsonConvert.DeserializeObject<T>(json);
		return data;
	}

	public async Task PostRequest()
	{
		using var response = await _httpClient.PostAsync(EndPoint, null);
		
		if (!response.IsSuccessStatusCode)
		{
			throw new HttpRequestException("RequestMetaData response statusCode: " +
			                               (int) response.StatusCode + ": " + response.StatusCode);
		}
	}
	
	public async Task DeleteRequest(int index)
	{
		using var response = await _httpClient.DeleteAsync(EndPoint + $"/{index}");
		
		if (!response.IsSuccessStatusCode)
		{
			throw new HttpRequestException("RequestMetaData response statusCode: " +
			                               (int) response.StatusCode + ": " + response.StatusCode);
		}
	}
	
	public async Task PutRequest(int index)
	{
		var str = JsonConvert.SerializeObject(new ButtonModel
			{ID = index.ToString(), Name = "Andrew", Color = "#567985", AnimationType = false});
		HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");
		using var response = await _httpClient.PutAsync(EndPoint + $"/{index}", content);
		
		if (!response.IsSuccessStatusCode)
		{
			throw new HttpRequestException("RequestMetaData response statusCode: " +
			                               (int) response.StatusCode + ": " + response.StatusCode);
		}
	}
}