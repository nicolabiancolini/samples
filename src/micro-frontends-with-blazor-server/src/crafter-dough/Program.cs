// See the LICENSE.TXT file in the project root for full license information.

using Crafter;
using Crafter.BackOffice;
using Crafter.IngredientsSelection;
using Crafter.RecipeComposition;

var builder = WebApplication.CreateBuilder(args);

// var scanner = new AssemblyScanner(AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name!.StartsWith("Crafter")), a => a.GetName().Name!.StartsWith("Crafter"));
// var modules = scanner.Provide<IModuleStartup>();

var modules = new IModuleStartup[]
{
    new BackOfficeStartup(),
    new IngredientsSelectionStartup(),
    new RecipeCompositionStartup()
};

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

foreach (var module in modules)
{
    module.ConfigureServices(builder.Services);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();

foreach (var module in modules)
{
    module.Configure(app);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapFallbackToPage(
    page: "/IngredientsSelection/_Host",
    pattern: $"~/IngredientsSelection/{FallbackEndpointRouteBuilderExtensions.DefaultPattern}");

app.MapFallbackToPage(
    page: "/_RecipeComposition",
    pattern: $"~/recipe-composition/{FallbackEndpointRouteBuilderExtensions.DefaultPattern}");

app.Run();
