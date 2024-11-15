using milad_speechforcsharp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Register our custom services
builder.Services.AddSingleton<ISpeechRecognitionService, VoskSpeechService>();
builder.Services.AddSingleton<ISentimentAnalysisService, VaderSentimentService>();
builder.Services.AddSingleton<ITranscriptService, TranscriptService>();

// Disable HTTPS requirement for development
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpsRedirection(options =>
    {
        options.HttpsPort = null;
    });
}

var app = builder.Build();

// Ensure directories exist
Directory.CreateDirectory(Path.Combine(app.Environment.WebRootPath, "models"));
Directory.CreateDirectory(Path.Combine(app.Environment.WebRootPath, "transcripts"));

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapControllers();

app.Run();
