using AutoMapper;
using Business.GenericRepository.BaseRep;
using Business.GenericRepository.ConcManager;
using Business.GenericRepository.ConcRep;
using Business.Mapper;
using Business.ServiceSettings;
using CoreL.ServiceClasses;
using CoreL.ServiceManager;
using DataAccess;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;  // Yeni eklenen using direktifi

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IMenuItemService, MenuItemManager>();
builder.Services.AddScoped<IAdminAuthenticationService, UserAuthenticationManager>();
builder.Services.AddTransient<IMailService, MailManager>();
builder.Services.AddScoped<IHashingService, HashingManager>();
builder.Services.AddScoped<IQRCodeService, QRCodeManager>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

// AutoMapper configuration
var mapperConfiguration = new MapperConfiguration(mc =>
{
    mc.AddMaps("Business");
});

var mapper = mapperConfiguration.CreateMapper();
builder.Services.AddSingleton(mapper);

// Repository registrations
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TableRepository>();
builder.Services.AddScoped<MenuItemRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<TableItemRepository>();
builder.Services.AddScoped<UserRoleRepository>();
builder.Services.AddDistributedMemoryCache(); // In-memory cache için

builder.Services.AddEndpointsApiExplorer();

// Deðiþtirilen kýsým: AddControllers ile JsonOptions eklendi
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "QRMenu",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

if (!Directory.Exists(Path.Combine(builder.Environment.ContentRootPath, "wwwroot")))
{
    Directory.CreateDirectory(Path.Combine(builder.Environment.ContentRootPath, "wwwroot"));
}

if (!Directory.Exists(Path.Combine(builder.Environment.ContentRootPath, "wwwroot/images")))
{
    Directory.CreateDirectory(Path.Combine(builder.Environment.ContentRootPath, "wwwroot/images"));
    Directory.CreateDirectory(Path.Combine(builder.Environment.ContentRootPath, "wwwroot/images/menu-items"));
    Directory.CreateDirectory(Path.Combine(builder.Environment.ContentRootPath, "wwwroot/images/categories"));
}

// FileService'i kaydet
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.AddDebug();
});



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = true;
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy
          .AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod();
    });
});

builder.Services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<QRMenuDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<ITableService, TableManager>();
builder.Services.AddScoped<IOrderService, OrderManager>();

// Repository registrations with interfaces
builder.Services.AddScoped<IRoleService, RoleManager>();
builder.Services.AddScoped<ITableItemService, TableItemManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();