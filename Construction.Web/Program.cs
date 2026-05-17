using Construction.Business.Abstract;
using Construction.Business.Concrete;
using Construction.Business.ValidatonRules;
using Construction.DataAccess.Abstract;
using Construction.DataAccess.Concrete;
using Construction.DataAccess.Context;
using Construction.Entity.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ConstructionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
})
    .AddEntityFrameworkStores<ConstructionDbContext>();

builder.Services.AddScoped<IAIRecommendationService, AIRecommendationManager>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Admin/Auth/Login";
    options.LogoutPath = "/Admin/Auth/Logout";
    options.AccessDeniedPath = "/Admin/Auth/AccessDenied";

    options.ExpireTimeSpan = TimeSpan.FromHours(8);   // Oturum süresi
    options.SlidingExpiration = true;                     // Her istekte süreyi uzat

    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.Name = "ConstructionAdmin.Auth";
});


builder.Services.AddValidatorsFromAssemblyContaining<CategoryValidator>();


builder.Services.AddControllersWithViews();

//DataAccess ve Business katmanlarý için baðýmlýlýklar
builder.Services.AddScoped<IProjectDal, EFProjectDal>();
builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();

builder.Services.AddScoped<IProjectImageDal, EfProjectImageDal>();
builder.Services.AddScoped<IProjectImageService, ProjectImageManager>();

builder.Services.AddScoped<IProjectService, ProjectManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();

builder.Services.AddScoped<ITestimonialDal, EfTestimonialDal>();
builder.Services.AddScoped<ITestimonialService, TestimonialManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/ErrorPage/Error404");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath, "Admin", "sneat-bootstrap-html-admin-template-free", "assets")),
    RequestPath = "/admin-assets"
});
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
