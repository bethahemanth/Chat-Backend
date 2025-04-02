using ChatAppBackend.Hubs;
using ChatApplication;
using ChatApplication.Data;
using ChatApplication.Data.ChatAppBackend.Data;
using ChatApplication.Data.Database1;
using ChatApplication.Repositories;
using ChatApplication.Repositories.Service_Contracts;
using ChatApplication.Services;
using ChatApplication.Services.Service_Contracts;
using Microsoft.OpenApi.Models;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials()
                       .SetIsOriginAllowed(origin => true)); // Allow all origins
        });

        // Register Swagger services
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ChatApp API",
                Version = "v1",
                Description = "API for Chat Application"
            });

            // ✅ Support File Uploads in Swagger
            options.OperationFilter<FileUploadOperationFilter>();
        });

        // Register application services
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IGroupService, GroupService>();
        builder.Services.AddScoped<IMessageService, MessageService>();
        builder.Services.AddScoped<IChatService, ChatService>();
        builder.Services.AddScoped<IMessageStatus, MessageStatusService>();
        builder.Services.AddScoped<IRepo, Repo>();
        builder.Services.AddScoped<IDatabaseContext, DatabaseContext>();
        builder.Services.AddScoped<IGroupMembers, GroupMembersService>();
        builder.Services.AddScoped<IDatabaseContext1, DatabaseContext1>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(); // Enable Swagger middleware
            app.UseSwaggerUI(); // Enable Swagger UI middleware
        }

        app.UseHttpsRedirection();
        app.UseCors("CorsPolicy");
        app.UseRouting();
        app.UseAuthorization();
        app.UseStaticFiles();
        app.MapControllers(); // Map API controllers
        app.Run(); // Run the application
    }
}