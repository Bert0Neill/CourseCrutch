using CourseCrutch.API.Contracts;
using CourseCrutch.Application.Ocr;
using CourseCrutch.Infrastructure;
using CourseCrutch.Logging;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFileLogging(builder.Configuration, builder.Environment.ContentRootPath);

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
    ILogger<Program> logger,
    CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(request.Text))
    {
        logger.LogWarning("OCR analysis rejected: text was empty.");
        return TypedResults.BadRequest("Text is required.");
    }

    logger.LogInformation("OCR analysis started for {CharacterCount} characters.", request.Text.Length);

    var analysis = await service.AnalyseAsync(request.Text, cancellationToken);

    logger.LogInformation("OCR analysis completed for {WordCount} words.", analysis.WordCount);

    return TypedResults.Ok(new OcrAnalysisResponse(
        analysis.WordCount,
        analysis.CharacterCount,
        analysis.Answer,
        analysis.Keywords));
})
.WithName("AnalyseOcrText");

app.Run();
