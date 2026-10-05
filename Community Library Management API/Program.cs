using Community_Library_Management_API.Data;
<<<<<<< HEAD
=======
using Community_Library_Management_API.Repositories;
using Community_Library_Management_API.Repositories.Interfaces;
using Community_Library_Management_API.Services;
using Community_Library_Management_API.Services.Interfaces;
>>>>>>> master
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
<<<<<<< HEAD
=======


builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();


builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ILoanService, LoanService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

>>>>>>> master
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();