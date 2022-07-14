string myCORS = "MyCORS";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myCORS,
                      policy =>
                      {
                          //policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                          //policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                          //policy.WithOrigins("https://localhost:44303/", "https://localhost:7061/").AllowAnyMethod().AllowAnyHeader();
                          policy.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials();
                      });
});

//services cors
//builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
//{
//    //builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
//    //builder.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials();
//}));

//builder.Services.AddCors();

// Add services to the container.
//builder.Services.AddHttpClient();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(myCORS);
//app.UseCors("corsapp");
//app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

//app.UseCors(x => x
//    .AllowAnyMethod()
//    .AllowAnyHeader()
//    .SetIsOriginAllowed(origin => true) // allow any origin 
//    .AllowCredentials());

app.UseHttpsRedirection();
//app.UseAuthorization();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
