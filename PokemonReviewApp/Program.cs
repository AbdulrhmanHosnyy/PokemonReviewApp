using PokemonReviewApp;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddTransient<Seed>();
//builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
//builder.Services.AddTransient<IPokemonRepository, PokemonRepository>();
//builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
//builder.Services.AddTransient<ICountryRepository, CountryRepository>();
//builder.Services.AddTransient<IOwnerRepository, OwnerRepository>();
//builder.Services.AddTransient<IReviewRepository, ReviewRepository>();
//builder.Services.AddTransient<IReviewerRepository, ReviewerRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(Program));


var app = builder.Build();
if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedDataContext();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
