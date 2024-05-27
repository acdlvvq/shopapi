using Moq;
using ProductService.Application.ProductUseCases.Handlers;
using ProductService.Application.ProductUseCases.Queries;
using ProductService.Core.Interfaces;
using ProductService.Infrastructure.Jwt;
using ProductService.UnitTests.Mocks;

namespace ProductService.UnitTests.Queries;

public class GetAllProductsQueryHandlerTests
{
    private readonly Mock<IProductsRepository> _mock;

    public GetAllProductsQueryHandlerTests()
    {
        _mock = MockRepositories.GetProductsRepository();
    }

    [Fact]
    public async Task User_Products_NotAll()
    {
        var handler = new GetAllProductsQueryHandler(
            _mock.Object, new JwtReader());

        var result = await handler.Handle(new GetAllProductsQuery("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJlMzkzYzgzNi0yZTA5LTRmYjMtOGI3MS00N2RjZjBkZjg5NDQiLCJzdWIiOiJhYm9iYSIsImVtYWlsIjoiYWJvYmExMjNAZ21haWwuY29tIiwibmFtZWlkIjoiODA1NDNmYTMtMWIzYi00YjM2LWIzZDEtNzA1MmViM2RhM2NmIiwicm9sZSI6IkRlZmF1bHRVc2VyIiwibmJmIjoxNzE2NzUzNDk0LCJleHAiOjE3MTY3NTM3OTQsImlhdCI6MTcxNjc1MzQ5NCwiaXNzIjoiaHR0cHM6Ly9hbC5hY2NvdW50c2VydmljZS5jb20iLCJhdWQiOiJodHRwczovL2FsLnNob3AuY29tIn0.Oupk2W82qhRo5RL91UDx599R0Xwhim_HzuikY_NPwsw"), CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.True(_mock.Object.GetAll().Count() > result.Count());
    }

    [Fact]
    public async Task Admin_Products_All()
    {
        var handler = new GetAllProductsQueryHandler(
            _mock.Object, new JwtReader());

        var result = await handler.Handle(new GetAllProductsQuery("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJiMTg4ZjEzYi03ZDcyLTRhNTYtOTNlNi1hOTlhZWRhNjRiYjYiLCJzdWIiOiJhZG1pbiIsImVtYWlsIjoiYnN2Y2guYWxAZ21haWwuY29tIiwibmFtZWlkIjoiNTZkMjdhNTAtZGZkMi00NjZhLTg0OTUtY2I1NjMxZTc3NmJjIiwicm9sZSI6IkFkbWluIiwibmJmIjoxNzE2Nzg0MDY5LCJleHAiOjE3MTY3ODQzNjksImlhdCI6MTcxNjc4NDA2OSwiaXNzIjoiaHR0cHM6Ly9hbC5hY2NvdW50c2VydmljZS5jb20iLCJhdWQiOiJodHRwczovL2FsLnNob3AuY29tIn0.INcT-Ohe-YOeJGuVhKP9ApDtJuQXuz9yCNj2SXAoPfo"), CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(_mock.Object.GetAll().Count() == result.Count());
    }
}
