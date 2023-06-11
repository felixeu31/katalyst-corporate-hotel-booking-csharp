using CorporateHotelBooking.Application.Bookings.Domain;
using CorporateHotelBooking.Application.Employees.Domain;
using CorporateHotelBooking.Application.Hotels.Domain;
using CorporateHotelBooking.Application.Policies.Domain;
using CorporateHotelBooking.Application.Bookings.UseCases;
using CorporateHotelBooking.Application.Employees.UseCases;
using CorporateHotelBooking.Application.Hotels.UseCases;
using CorporateHotelBooking.Application.Policies.UseCases;
using CorporateHotelBooking.Data.InMemory;
using CorporateHotelBooking.Data.InMemory.Repositories;
using CorporateHotelBooking.Employees.Infra;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IAddHotelUseCase, AddHotelUseCase>();
builder.Services.AddSingleton<ISetRoomUseCase, SetRoomUseCase>();
builder.Services.AddSingleton<IBookUseCase, BookUseCase>();
builder.Services.AddSingleton<IFindHotelUseCase, FindHotelUseCase>();
builder.Services.AddSingleton<IAddEmployeeUseCase, AddEmployeeUseCase>();
builder.Services.AddSingleton<IAddEmployeePolicyUseCase, AddEmployeePolicyUseCase>();
builder.Services.AddSingleton<IAddCompanyPolicyUseCase, AddCompanyPolicyUseCase>();
builder.Services.AddSingleton<IDeleteEmployeeUseCase, DeleteEmployeeUseCase>();
builder.Services.AddSingleton<IIsBookingAllowedUseCase, IsBookingAllowedUseCase>();

builder.Services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
builder.Services.AddSingleton<IHotelRepository, InMemoryHotelRepository>();
builder.Services.AddSingleton<IEmployeeRepository, InMemoryEmployeeRepository>();
builder.Services.AddSingleton<IPoliciesRepository, InMemoryPoliciesRepository>();
builder.Services.AddSingleton<InMemoryContext>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program
{

}