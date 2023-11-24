using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Contracts;
using SocialMedia.Data;
using SocialMedia.Hubs;
using SocialMedia.MappingProfiles;
using SocialMedia.Models;
using SocialMedia.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DataConnectionString")));
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddSignalR();

builder.Services.AddAutoMapper(typeof(MappingProfiles));


builder.Services.AddHttpContextAccessor();

builder.Services.AddIdentityCore<User>()//login and register
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<User>>("Auth")
    .AddEntityFrameworkStores<DataDbContext>()
    .AddDefaultTokenProviders();

//builder.Services.AddIdentity<User, IdentityRole>()
//    .AddEntityFrameworkStores<DataDbContext>()
//    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>//login and register
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});





builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//token
    .AddJwtBearer(options => options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });





//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(builder =>
//    {
//        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
//    });
//});


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors();

if (!Directory.Exists("Images"))
{
    Directory.CreateDirectory("Images");
}

app.UseStaticFiles(new StaticFileOptions
{
    //    if (!Directory.Exists("Images"))
    //{
    //    Directory.CreateDirectory("Images");
    //}
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});


app.MapControllers();
app.MapHub<ChatHub>("hubs/chat");
app.UseHttpsRedirection();
app.Run();
