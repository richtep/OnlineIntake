using Microsoft.EntityFrameworkCore;
using OnlineIntake.Server.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
       .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//CORS
// builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
//     p.WithOrigins("https://localhost:7258")
//      .AllowAnyHeader()
//      .AllowAnyMethod()));

var app = builder.Build();

// app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseWebAssemblyDebugging();        //ws-proxy WASM debug
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();            
app.UseStaticFiles();                     

app.MapControllers();

app.MapFallbackToFile("index.html");      

// Automatic migrations (dev/test)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
