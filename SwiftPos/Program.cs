using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
using SwiftPos.Middleware;
using SwiftPos.Repositories.UserRepository;
using SwiftPos.Repositories.CategoryRepository;
using SwiftPos.Services.UserService;
using SwiftPos.Services.CategoryService;
using System.Text;
using NLog.Extensions.Logging;
using SwiftPos.AutoMapper;
using SwiftPos.Services.AuthService;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Azure.Security.KeyVault.Secrets;
using System.IO;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(LogLevel.Trace);
});

builder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();

// Register Repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IProductRepository, ProductRepository>();
//builder.Services.AddScoped<ISaleRepository, SaleRepository>();
//builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();

//Register Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IProductService, ProductService>();
//builder.Services.AddScoped<ISaleService, SaleService>();
//builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Register the DataContextEntity with the in-memory database provider.
//builder.Services.AddDbContext<DBContextEntity>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
//        sqlServerOptions => sqlServerOptions
//            .EnableRetryOnFailure(
//                maxRetryCount: 5,
//                maxRetryDelay: TimeSpan.FromSeconds(30),
//                errorNumbersToAdd: null)
//            .MigrationsAssembly("POSAPIs")));

//builder.Services.AddDbContext<DBContextEntity>(options =>
//    options.UseInMemoryDatabase("InMemoryDb"));

var keyVaultUri = builder.Configuration.GetSection("KeyVault:KeyVaultURL").Value;
var clientId = builder.Configuration.GetSection("KeyVault:ClientId").Value;
var clientSecret = builder.Configuration.GetSection("KeyVault:ClientSecret").Value;
var directoryId = builder.Configuration.GetSection("KeyVault:DirectoryId").Value;

var credential = new ClientSecretCredential(directoryId.ToString(), clientId.ToString(), clientSecret!.ToString());
var secretClient = new SecretClient(new Uri(keyVaultUri.ToString()), credential);

builder.Configuration.AddAzureKeyVault(keyVaultUri!.ToString(),clientId!.ToString(),clientSecret!.ToString(), new DefaultKeyVaultSecretManager());

var client = new SecretClient(new Uri(keyVaultUri!.ToString()), credential);

builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var endpointUrl = client.GetSecret("AccountEndpoint").Value.Value.ToString();
    var primaryKey = client.GetSecret("AccountKey").Value.Value.ToString();
    var databaseName = client.GetSecret("DatabaseName").Value.Value.ToString();

    //var endpointUrl = configuration["AccountEndpoint"]!.ToString();
    //var primaryKey = configuration["AccountEndpoint"]!.ToString();
    //var databaseName = configuration["AccountEndpoint"]!.ToString();
    return new CosmosClient(endpointUrl, primaryKey);
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();


// Use Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<AuthMiddleware>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
