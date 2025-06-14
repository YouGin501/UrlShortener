using Api.Middleware;
using Application;
using Domain.Exceptions;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// -------------------- Services --------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("auth", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey
	});
});
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// -------------------- Identity --------------------
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
	.AddEntityFrameworkStores<DataContext>()
	.AddDefaultTokenProviders();

// -------------------- JWT Authentication --------------------
var jwtConfig = builder.Configuration.GetSection("JwtOptions");
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidIssuer = jwtConfig["Issuer"],
		ValidAudience = jwtConfig["Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]!)),
	};

	options.Events = new JwtBearerEvents
	{
		OnChallenge = context =>
		{
			context.HandleResponse();
			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			context.Response.ContentType = "application/json";

			var result = JsonSerializer.Serialize(new { errors = new[] { "Unauthorized access - authentication failed" } });
			return context.Response.WriteAsync(result);
		},

		OnForbidden = context =>
		{
			context.Response.StatusCode = StatusCodes.Status403Forbidden;
			context.Response.ContentType = "application/json";

			var result = JsonSerializer.Serialize(new { errors = new[] { "Forbidden access - insufficient permissions" } });
			return context.Response.WriteAsync(result);
		}
	};
});

// -------------------- Authorization --------------------
builder.Services.AddAuthorization();

// -------------------- Repositories & Services --------------------
builder.Services
	.AddApplication()
	.AddInfrastructure(builder.Configuration);

// -------------------- Cors --------------------
builder.Services.AddCors();

var app = builder.Build();

// -------------------- Middleware --------------------
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles(); 
app.UseCors(options =>
{
	options.AllowAnyHeader();
	options.AllowAnyMethod();
	options.AllowAnyOrigin();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();