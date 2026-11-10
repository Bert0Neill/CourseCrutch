using CourseCrutch.API.Contracts;
using CourseCrutch.Application.Ocr;
using CourseCrutch.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

app.MapPost("/api/ocr/analyse", async Task<Results<Ok<OcrAnalysisResponse>, BadRequest<string>>> (
    OcrAnalysisRequest request,
    IOcrAnalysisService service,
    CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request.Text))
    {
        return TypedResults.BadRequest("Text is required.");
    }

    var analysis = await service.AnalyseAsync(request.Text, cancellationToken);

    return TypedResults.Ok(new OcrAnalysisResponse(
        analysis.WordCount,
        analysis.CharacterCount,
        analysis.Answer,
        analysis.Keywords));
})
.WithName("AnalyseOcrText");

app.Run();
