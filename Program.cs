using System.Configuration;
using ASP12_RazorPage_EntityFramework.Models;
using ASP12_RazorPage_EntityFramework.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Add SendEmail Service
builder.Services.AddOptions();
var mailSetting = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailSetting);
builder.Services.AddSingleton<IEmailSender, SendMailService>();
builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();

// Add services to the container.
builder.Services.AddRazorPages();

//Connect to PostgresSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Đăng kí Entity Framework
builder.Services.AddDbContext<MasterDbContext>(options 
    =>  options.UseNpgsql(connectionString));


//Đăng kí Identity Framework cho ứng dụng
builder.Services.AddIdentity<User, IdentityRole>(options => // IdentityOptions
    {
        //Thiết lập về password 
        options.Password.RequireDigit = false; //Không bắt phải có số
        options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
        options.Password.RequireUppercase = false; //Không bắt phải có chữ hoa
        options.Password.RequireNonAlphanumeric = false; //Không bắt phải có kí tự đặc biệt
        options.Password.RequiredLength = 3; // Độ dài tối thiểu 3 kí tự
        options.Password.RequiredUniqueChars = 1; // Số kí tự riêng biệt
    
        // Cấu hình lockout - khóa user
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Khóa 2 phút
        options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 3 lần là khóa
        options.Lockout.AllowedForNewUsers = true;
    
        // Cấu hình về user
        options.User.AllowedUserNameCharacters = // các kí tự đặt tên user
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true; // Email là duy nhất
    
        // Cấu hình đăng nhập
        options.SignIn.RequireConfirmedEmail = true; // Yêu cầu confrim Email
        options.SignIn.RequireConfirmedPhoneNumber = false; //Yêu cầu confrim sđt
        options.SignIn.RequireConfirmedAccount = true; // Yêu cầu xác minh tài khoản
    })
    .AddEntityFrameworkStores<MasterDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login/";
    options.LogoutPath = "/logout/";
    options.AccessDeniedPath = "/khongduoctruycap.html";
});

//Đăng kí Identity UI : giao diện mặc định hệ thống tự sinh ra
// builder.Services.AddDefaultIdentity<User>()
//     .AddEntityFrameworkStores<MasterDbContext>()
//     .AddDefaultTokenProviders();


var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

/*CREATE , READ, UPDATE, DELETE ( CRUD )
 *
 * dotnet aspnet-codegenerator razorpage -m ASP12_RazorPage_EntityFramework.Models.Article -dc ASP12_RazorPage_EntityFramework.Models.MasterDbContext -outDir Pages/Blog -udl --referenceScriptLibraries
 *
 *
 * Identity :
 *      - Authentication : Xác định danh tính : Login, Logout,..
 * 
 *      - Authorization : Xác định quyền truy cập
            Role - based authorization : xác thực quyền theo vai trò
            Role (vai trò) : Admin, Editor, Manager, Member,...
            
            /Areas/Admin/Pages/Role
            Index
            Create
            Edit
            Delete
            
            [Authorize] - Controller, Action, PageModel
            

 *      - Quản lý User : Signup, User, Role
 *
 
   Identity UI: 
   /Identity/Account/Login
   /Identity/Account/Manage
   
   dotnet aspnet-codegenerator identity -dc ASP12_RazorPage_EntityFramework.Models.MasterDbContext

 */