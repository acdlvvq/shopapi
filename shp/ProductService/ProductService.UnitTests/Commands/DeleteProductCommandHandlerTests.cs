using Moq;
using ProductService.Application.ProductUseCases.Commands;
using ProductService.Application.ProductUseCases.Handlers;
using ProductService.Core.Interfaces;
using ProductService.Infrastructure.Jwt;
using ProductService.UnitTests.Mocks;

namespace ProductService.UnitTests.Commands;

public class DeleteProductCommandHandlerTests
{
    private readonly Mock<IProductsRepository> _mock;

    public DeleteProductCommandHandlerTests()
    {
        _mock = MockRepositories.GetProductsRepository();
    }

    [Fact]
    public async Task Invalid_ProductId_Throw()
    {
        var handler = new DeleteProductCommandHandler(_mock.Object, new JwtReader());

        await Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(
            new DeleteProductCommand(Guid.NewGuid(), "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJiMTg4ZjEzYi03ZDcyLTRhNTYtOTNlNi1hOTlhZWRhNjRiYjYiLCJzdWIiOiJhZG1pbiIsImVtYWlsIjoiYnN2Y2guYWxAZ21haWwuY29tIiwibmFtZWlkIjoiNTZkMjdhNTAtZGZkMi00NjZhLTg0OTUtY2I1NjMxZTc3NmJjIiwicm9sZSI6IkFkbWluIiwibmJmIjoxNzE2Nzg0MDY5LCJleHAiOjE3MTY3ODQzNjksImlhdCI6MTcxNjc4NDA2OSwiaXNzIjoiaHR0cHM6Ly9hbC5hY2NvdW50c2VydmljZS5jb20iLCJhdWQiOiJodHRwczovL2FsLnNob3AuY29tIn0.INcT-Ohe-YOeJGuVhKP9ApDtJuQXuz9yCNj2SXAoPfo"), CancellationToken.None));
    }

    [Fact]
    public async Task Invalid_Account_Throw()
    {
        var handler = new DeleteProductCommandHandler(_mock.Object, new JwtReader());

        var id = _mock.Object.Get(p => p.CreatorId != MockRepositories.ID1).FirstOrDefault()!.Id;

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(
            new DeleteProductCommand(id, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJlMzkzYzgzNi0yZTA5LTRmYjMtOGI3MS00N2RjZjBkZjg5NDQiLCJzdWIiOiJhYm9iYSIsImVtYWlsIjoiYWJvYmExMjNAZ21haWwuY29tIiwibmFtZWlkIjoiODA1NDNmYTMtMWIzYi00YjM2LWIzZDEtNzA1MmViM2RhM2NmIiwicm9sZSI6IkRlZmF1bHRVc2VyIiwibmJmIjoxNzE2NzUzNDk0LCJleHAiOjE3MTY3NTM3OTQsImlhdCI6MTcxNjc1MzQ5NCwiaXNzIjoiaHR0cHM6Ly9hbC5hY2NvdW50c2VydmljZS5jb20iLCJhdWQiOiJodHRwczovL2FsLnNob3AuY29tIn0.Oupk2W82qhRo5RL91UDx599R0Xwhim_HzuikY_NPwsw"), CancellationToken.None));
    }

    [Fact]
    public async Task Valid_AccountAndProduct_Success()
    {
        var handler = new DeleteProductCommandHandler(_mock.Object, new JwtReader());

        var id = _mock.Object.Get(p => p.CreatorId == MockRepositories.ID1).FirstOrDefault()!.Id;

        var result = await handler.Handle(
            new DeleteProductCommand(id, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJlMzkzYzgzNi0yZTA5LTRmYjMtOGI3MS00N2RjZjBkZjg5NDQiLCJzdWIiOiJhYm9iYSIsImVtYWlsIjoiYWJvYmExMjNAZ21haWwuY29tIiwibmFtZWlkIjoiODA1NDNmYTMtMWIzYi00YjM2LWIzZDEtNzA1MmViM2RhM2NmIiwicm9sZSI6IkRlZmF1bHRVc2VyIiwibmJmIjoxNzE2NzUzNDk0LCJleHAiOjE3MTY3NTM3OTQsImlhdCI6MTcxNjc1MzQ5NCwiaXNzIjoiaHR0cHM6Ly9hbC5hY2NvdW50c2VydmljZS5jb20iLCJhdWQiOiJodHRwczovL2FsLnNob3AuY29tIn0.Oupk2W82qhRo5RL91UDx599R0Xwhim_HzuikY_NPwsw"), CancellationToken.None);

        Assert.True(_mock.Object.GetAll().Count() == 2);
    }
}
