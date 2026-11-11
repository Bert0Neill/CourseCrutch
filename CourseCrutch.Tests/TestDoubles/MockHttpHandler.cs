using System.Net;
using System.Text;
using Moq;
using Moq.Protected;

namespace CourseCrutch.Tests.TestDoubles;

public sealed class MockHttpHandler
{
    private readonly Mock<HttpMessageHandler> _handler = new();

    public HttpRequestMessage? LastRequest { get; private set; }

    public string? LastRequestBody { get; private set; }

    public MockHttpHandler Respond(HttpStatusCode statusCode, string body = "")
    {
        _handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Returns<HttpRequestMessage, CancellationToken>(async (request, cancellationToken) =>
            {
                LastRequest = request;
                LastRequestBody = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);

                return new HttpResponseMessage(statusCode)
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                };
            });

        return this;
    }

    public HttpClient CreateClient(string baseAddress) =>
        new(_handler.Object) { BaseAddress = new Uri(baseAddress) };

    public void VerifySentOnce() =>
        _handler.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
}
