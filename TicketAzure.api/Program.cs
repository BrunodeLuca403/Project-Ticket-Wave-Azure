using TicketAzure.api.Consumers;
using TicketAzure.application.Services;
using TicketAzure.application.Services.Interface;
using TicketAzure.Core.Services;
using TicketAzure.infrastructure;
using TicketAzure.infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddHostedService<PaymentConfimerdConsumer>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("allowAny", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .SetIsOriginAllowed(_ => true)
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("allowAll");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();
