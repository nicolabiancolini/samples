// See the LICENSE.TXT file in the project root for full license information.

using Streaming.Cloudflare.WebApp.Services.Streaming;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<CloudflareStreamOptions>()
    .Bind(builder.Configuration.GetSection(CloudflareStreamOptions.SectionName));

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddHttpClient<CloudflareStream>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
