using WalletApi.Data;
using Microsoft.EntityFrameworkCore;
using Bogus;
using WalletApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionStrings"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(); // Added this line to register controller services

var app = builder.Build();

// Configure the HTTP request pipeline ( Sequence is important)
if (app.Environment.IsDevelopment())
{
    //fake data generate
    //using (var scope = app.Services.CreateScope())
    //{
    //    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //    SeedUserData(db); // Call the seed method here
    //}

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();



////fake data
//static void SeedUserData(ApplicationDbContext db)
//{
//    var uniqueUsernames = new HashSet<string>();
//    var userFaker = new Faker<Users>()
//        // Ensure unique UserName by using a retry mechanism
//        .RuleFor(u => u.UserName, (f, u) => {
//            string username;
//            do
//            {
//                username = f.Internet.UserName();
//            } while (!uniqueUsernames.Add(username)); // Keep trying until a new unique username is generated
//            return username;
//        })
//        .RuleFor(u => u.Password, f => f.Internet.Password(8))
//        .RuleFor(u => u.Email, f => f.Internet.Email())
//        .RuleFor(u => u.UserStatus, f => f.PickRandom(new[] { "active", "inactive" })   )
//        .RuleFor(u => u.CreatedAt, f => f.Date.PastOffset(1))
//        .RuleFor(u => u.UpdatedAt, f => f.Date.PastOffset(1)) 
//        .RuleFor(u => u.SignInStatus, f => f.PickRandom(new[] { "in", "out" }));

//    // Adjust the number of users to generate if needed
//    var users = userFaker.GenerateLazy(15000); // Use GenerateLazy for better performance with large numbers

//    db.Users.AddRange(users);
//    db.SaveChanges();
//}


