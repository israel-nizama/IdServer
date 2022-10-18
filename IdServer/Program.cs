using IdServer.Core.Services;
using IdServer.Infraestructure.Data.Command;
using IdServer.Infraestructure.Data.Query;
using IdServer.Infraestructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<IdServerContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("IdServer")));
builder.Services.AddAuthentication("OAuth")
    .AddJwtBearer("OAuth", config =>
    {
        var options = new SecurityOptions();
        builder.Configuration.GetSection(SecurityOptions.SectionName).Bind(options);
        var issuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.AccessTokenKeySecret));
        config.SaveToken = true;
        config.RequireHttpsMetadata = false;
        config.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = options.Issuer,
            ValidAudience = options.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = issuerSigningKey,
            ValidateIssuer = true,
            ValidateAudience = false,
        };
    });
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.Configure<SendGridKeyOptions>(builder.Configuration);
builder.Services.Configure<SecurityOptions>(builder.Configuration.GetSection(SecurityOptions.SectionName));
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<ISecurityTokenProvider, SecurityTokenProvider>();
builder.Services.AddSingleton<IdServerQueryContext>();
builder.Services.AddScoped<IIdentityQueryRepository, IdentityQueryRepository>();

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

app.MapControllers();

app.Run();
