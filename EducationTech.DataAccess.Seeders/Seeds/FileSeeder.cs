using EducationTech.Business.Business.Interfaces;
using EducationTech.Business.Shared.DTOs.Business.Auth;
using EducationTech.DataAccess.Core.Contexts;
using EducationTech.Storage;
using System.Net.Http.Json;

namespace EducationTech.DataAccess.Seeders.Seeds;

public class FileSeeder : Seeder
{
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;
    public FileSeeder(EducationTechContext context, IAuthService authService) : base(context)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5013/api/v1") };
        _authService = authService;
    }

    public override void Seed()
    {
        var token = Login().GetAwaiter().GetResult();

        var videoPath = Path.Combine(GlobalReference.Instance.StaticFilesPath, "video_sample.mp4");
        var imagePath = Path.Combine(GlobalReference.Instance.StaticFilesPath, "image_sample.png");

        UploadImageAsync(imagePath, token).GetAwaiter().GetResult();
        UploadVideoAsync(videoPath, token).GetAwaiter().GetResult();
    }


    public async Task<string> Login()
    {
        var tokens = await _authService.Login(new LoginDto
        {
            Username = "admin",
            Password = "12345678"
        });

        return tokens.AccessToken;
    }

    public async Task UploadVideoAsync(string filePath, string token)
    {
        var fileInfo = new FileInfo(filePath);
        var fileName = fileInfo.Name;
        var fileSize = fileInfo.Length;

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Step 1: Prepare session
        var prepareResponse = await _httpClient.PostAsJsonAsync("/File/Session", new
        {
            fileName,
            fileSize
        });

        prepareResponse.EnsureSuccessStatusCode();
        var jsonString = await prepareResponse.Content.ReadAsStringAsync();
        var prepareData = System.Text.Json.JsonSerializer.Deserialize<dynamic>(jsonString);
        string sessionId = prepareData.sessionId;
        int chunkSize = int.Parse(prepareData.chunkSize.ToString());
        int totalChunks = int.Parse(prepareData.totalChunks.ToString());

        // Step 2: Split file into chunks
        var chunks = new List<(long Start, long End, int Index)>();
        for (int i = 0; i < totalChunks; i++)
        {
            long start = i * chunkSize;
            long end = Math.Min(start + chunkSize, fileSize) - 1;
            chunks.Add((start, end, i));
        }

        // Step 3: Upload chunks
        var uploadTasks = new List<Task<HttpResponseMessage>>();
        foreach (var chunk in chunks)
        {
            using var chunkStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var buffer = new byte[chunk.End - chunk.Start + 1];
            chunkStream.Seek(chunk.Start, SeekOrigin.Begin);
            await chunkStream.ReadAsync(buffer, 0, buffer.Length);

            using var content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(buffer), "chunkFormFile", fileName);
            content.Add(new StringContent(sessionId), "sessionId");
            content.Add(new StringContent(chunk.Index.ToString()), "index");

            uploadTasks.Add(_httpClient.PostAsync("/File/Session/Chunk", content));
        }

        // Step 4: Wait for all chunks to complete
        var responses = await Task.WhenAll(uploadTasks);
        foreach (var response in responses)
        {
            response.EnsureSuccessStatusCode();
        }

        Console.WriteLine("File upload completed.");
    }

    public async Task UploadImageAsync(string filePath, string token)
    {
        // Tạo form data
        using var formData = new MultipartFormDataContent();
        using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var fileName = Path.GetFileName(filePath);

        formData.Add(new StreamContent(fileStream), "file", fileName);

        // Thêm tiêu đề Authorization
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Gửi yêu cầu POST
        var response = await _httpClient.PostAsync("/File/Upload", formData);
        response.EnsureSuccessStatusCode();

        Console.WriteLine("File uploaded successfully.");
    }


}
