using HELPERLAND.Models;
using HELPERLAND.Models.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
 
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<Implcontactus>();
builder.Services.AddScoped<Impluser>();
builder.Services.AddScoped<SendEmail>();
builder.Services.AddDbContext<HelperlandContext>(option =>
	option.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection"))
);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
                AddCookie(options =>
                {
                    options.LoginPath = "/Signup/Login";
                    options.LogoutPath = "/Signup/Logout";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    
                    // options.Events = new CookieAuthenticationEvents()
                    // {
                    //     OnRedirectToLogin = ctx =>
                    //     {
                    //         var redirectPath = ctx.RedirectUri;
                            
                    //         if (redirectPath.Contains("?ReturnUrl"))
                    //         {
                    //             //remove the ReturnURL
                    //             var url = redirectPath.Substring(0, redirectPath.LastIndexOf("?ReturnUrl"));

                    //             ctx.Response.Redirect(url + "?modalRequest=true");
                    //         }
                    //         return Task.CompletedTask;
                    //     },
                    // };
                });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=index}/{id?}");
app.Run();
