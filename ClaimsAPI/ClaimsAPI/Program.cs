using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;
var services = builder.Services;
// Add services to the container.

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddAuthentication(options =>
{
    options.DefaultScheme = AzureADDefaults.AuthenticationScheme;

}).AddJwtBearer("AzureAD", options =>
{
    options.Audience = Configuration.GetValue<string>("AzureAd:ClientId");
    options.Authority = Configuration.GetValue<string>("AzureAd:Instance")
    +Configuration.GetValue<string>("AzureAd:TenantId");

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidIssuer = Configuration.GetValue<string>("AzureAd:Issuer"),
        ValidAudience = Configuration.GetValue<string>("AzureAd:ClientId")
    };
});
services.AddMvc();
services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
    });
});
//any dependency

//services.AddSingleton<IHomeServices, HomeServices>();

//
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
