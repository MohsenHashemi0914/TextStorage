using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TextStorage.Web;
using TextStorage.Web.Features.PasteText;
using TextStorage.Web.Features.Reading;
using TextStorage.Web.Persistence;
using TextStorage.Web.Workers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Services.AddStackExchangeRedisCache(options =>
{
    var redisConnection = builder.Configuration[nameof(AppSettings.RedisConnection)];
    options.Configuration = redisConnection; 
});

builder.Services.AddSingleton<LoadBalancer>(sp =>
{
    var connections = sp.GetRequiredService<IOptions<AppSettings>>().Value.ConnectionStrings;
    return new([connections.Master1, connections.Master2, connections.Master3]);
});

builder.Services.AddDbContext<TextStorageDbContext>(options =>
{
    options.UseSqlServer();
});

builder.Services.AddDbContext<ReadOnlyTextStorageDbContext>(options =>
{
    options.UseSqlServer().UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddHostedService<TextStorageCleanupBacgroundService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapCreateTextEndpouint();
app.MapReadingEndpouint();
app.Run();