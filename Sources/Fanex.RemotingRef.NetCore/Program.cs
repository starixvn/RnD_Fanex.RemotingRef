
using Fanex.RemotingRef.NetCore;
using MessagePack.AspNetCoreMvcFormatter;
using MessagePack.Resolvers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddMvc().AddMvcOptions(option =>
    {
        option.OutputFormatters.Clear();
        option.OutputFormatters.Add(new MessagePackOutputFormatter(ContractlessStandardResolver.Options));
        option.InputFormatters.Clear();
        option.InputFormatters.Add(new MessagePackInputFormatter1(ContractlessStandardResolver.Options));
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.AddRefDataClient(builder.Configuration);
app.UseCors("NgOrigins");
app.MapControllers();
app.MapHealthChecks("/health").AllowAnonymous();

app.Run();
