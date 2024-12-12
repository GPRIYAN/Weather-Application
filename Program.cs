using WeatherApplicationServer.Models;
using WeatherApplicationServer.Services;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy to allow Blazor WebAssembly app to access API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp", builder =>
    {
        builder.WithOrigins("https://localhost:7235") // Update with your Blazor app's URL
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

// Load Supabase configuration
var supabaseSettings = builder.Configuration.GetSection("Supabase").Get<SupabaseSettings>();

// Validate Supabase settings
if (string.IsNullOrEmpty(supabaseSettings?.Url) || string.IsNullOrEmpty(supabaseSettings?.ApiKey))
{
    throw new InvalidOperationException("Supabase settings are not configured properly in appsettings.json.");
}

// Register Supabase.Client as a Singleton
builder.Services.AddSingleton(sp =>
{
    var client = new Supabase.Client(supabaseSettings.Url, supabaseSettings.ApiKey);
    client.InitializeAsync().Wait();
    return client;
});

// Register SupabaseService as Scoped
builder.Services.AddScoped<SupabaseService>();

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorApp");
app.UseAuthorization();
app.MapControllers();
app.Run();
