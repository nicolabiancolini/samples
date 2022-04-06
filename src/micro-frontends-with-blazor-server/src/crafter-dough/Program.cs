// See the LICENSE.TXT file in the project root for full license information.

using Crafter.RecipeComposition.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();
builder.Services.AddScoped<RecipeService>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapFallbackToAreaPage(
    area: "BackOffice",
    page: "/_IngredientsSelection",
    pattern: "/back-office/ingredients-selection");

app.MapFallbackToAreaPage(
    area: "BackOffice",
    page: "/_RecipeComposition",
    pattern: "/back-office/recipe-composition");

app.MapFallbackToPage(
    page: "/_IngredientSelection",
    pattern: "/ingredient-selection");

app.MapFallbackToPage(
    page: "/_RecipeComposition",
    pattern: "/recipe-composition");

app.MapRazorPages();

app.MapBlazorHub();

app.Run();
