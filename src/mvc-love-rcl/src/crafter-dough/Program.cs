// See the LICENSE.TXT file in the project root for full license information.

using Crafter.BackOffice;
using Crafter.IngredientsSelection;
using Crafter.RecipeComposition;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddServerSideBlazor();

var backOfficeConfigurator = new BackOfficeBoundedContextConfigurator(builder.Environment, builder.Services);
var ingredientsSelectionConfigurator = new IngredientsSelectionBoundedContextConfigurator(builder.Environment, builder.Services);
var recipeCompositionConfigurator = new RecipeCompositionBoundedContextConfigurator(builder.Environment, builder.Services);

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

backOfficeConfigurator.Configure(app);
ingredientsSelectionConfigurator.Configure(app);
recipeCompositionConfigurator.Configure(app);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapBlazorHub();

app.Run();
