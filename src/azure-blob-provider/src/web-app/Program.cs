// See the LICENSE.TXT file in the project root for full license information.

using WebApp.Infrastructure.AzureBlobProvider;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDirectoryBrowser();

builder.Services.AddAzureBlobProvider(options =>
{
    options.ConnectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=https;BlobEndpoint=https://127.0.0.1:10000/devstoreaccount1;";
    options.ContainerName = "ugidotnet";
    options.ContainerRoot = "Content";
});

var app = builder.Build();

var blobProvider = app.Services.GetRequiredService<BlobProvider>();

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = blobProvider,
    RequestPath = PathString.Empty
});

app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = blobProvider,
    RequestPath = PathString.Empty
});

app.Run();
