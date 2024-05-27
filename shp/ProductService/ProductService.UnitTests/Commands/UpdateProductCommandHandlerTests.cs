using Moq;
using ProductService.Application.ProductUseCases.Commands;
using ProductService.Application.ProductUseCases.Handlers;
using ProductService.Core.Interfaces;
using ProductService.Core.Validators;
using ProductService.Infrastructure.Jwt;
using ProductService.UnitTests.Mocks;
using FluentValidation;

namespace ProductService.UnitTests.Commands;

public class UpdateProductCommandHandlerTests
{
    private readonly Mock<IProductsRepository> _mock;

    public UpdateProductCommandHandlerTests()
    {
        _mock = MockRepositories.GetProductsRepository();
    }

    [Fact]
    public async Task Invalid_ProductId_Throw()
    {
        var handler = new UpdateProductCommandHandler(_mock.Object, new JwtReader(), new ProductValidator());

        await Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(
            new UpdateProductCommand(Guid.NewGuid(), "name", "desc", Convert.ToDecimal(213.432), 123, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJiMTg4ZjEzYi03ZDcyLTRhNTYtOTNlNi1hOTlhZWRhNjRiYjYiLCJzdWIiOiJhZG1pbiIsImVtYWlsIjoiYnN2Y2guYWxAZ21haWwuY29tIiwibmFtZWlkIjoiNTZkMjdhNTAtZGZkMi00NjZhLTg0OTUtY2I1NjMxZTc3NmJjIiwicm9sZSI6IkFkbWluIiwibmJmIjoxNzE2Nzg0MDY5LCJleHAiOjE3MTY3ODQzNjksImlhdCI6MTcxNjc4NDA2OSwiaXNzIjoiaHR0cHM6Ly9hbC5hY2NvdW50c2VydmljZS5jb20iLCJhdWQiOiJodHRwczovL2FsLnNob3AuY29tIn0.INcT-Ohe-YOeJGuVhKP9ApDtJuQXuz9yCNj2SXAoPfo"), CancellationToken.None));
    }

    [Fact]
    public async Task Invalid_Account_Throw()
    {
        var handler = new UpdateProductCommandHandler(_mock.Object, new JwtReader(), new ProductValidator());

        var id = _mock.Object.Get(p => p.CreatorId != MockRepositories.ID1).FirstOrDefault()!.Id;

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(
            new UpdateProductCommand(id, "name", "desc", Convert.ToDecimal(213.432), 123, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJlMzkzYzgzNi0yZTA5LTRmYjMtOGI3MS00N2RjZjBkZjg5NDQiLCJzdWIiOiJhYm9iYSIsImVtYWlsIjoiYWJvYmExMjNAZ21haWwuY29tIiwibmFtZWlkIjoiODA1NDNmYTMtMWIzYi00YjM2LWIzZDEtNzA1MmViM2RhM2NmIiwicm9sZSI6IkRlZmF1bHRVc2VyIiwibmJmIjoxNzE2NzUzNDk0LCJleHAiOjE3MTY3NTM3OTQsImlhdCI6MTcxNjc1MzQ5NCwiaXNzIjoiaHR0cHM6Ly9hbC5hY2NvdW50c2VydmljZS5jb20iLCJhdWQiOiJodHRwczovL2FsLnNob3AuY29tIn0.Oupk2W82qhRo5RL91UDx599R0Xwhim_HzuikY_NPwsw"), CancellationToken.None));
    }

    [Fact]
    public async Task Invalid_Data_Throw()
    {
        var handler = new UpdateProductCommandHandler(_mock.Object, new JwtReader(), new ProductValidator());

        var id = _mock.Object.Get(p => p.CreatorId == MockRepositories.ID1).FirstOrDefault()!.Id;

        await Assert.ThrowsAsync<ValidationException>(async () => await handler.Handle(
            new UpdateProductCommand(id, "n", "sc", Convert.ToDecimal(-213.432), -123, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJlMzkzYzgzNi0yZTA5LTRmYjMtOGI3MS00N2RjZjBkZjg5NDQiLCJzdWIiOiJhYm9iYSIsImVtYWlsIjoiYWJvYmExMjNAZ21haWwuY29tIiwibmFtZWlkIjoiODA1NDNmYTMtMWIzYi00YjM2LWIzZDEtNzA1MmViM2RhM2NmIiwicm9sZSI6IkRlZmF1bHRVc2VyIiwibmJmIjoxNzE2NzUzNDk0LCJleHAiOjE3MTY3NTM3OTQsImlhdCI6MTcxNjc1MzQ5NCwiaXNzIjoiaHR0cHM6Ly9hbC5hY2NvdW50c2VydmljZS5jb20iLCJhdWQiOiJodHRwczovL2FsLnNob3AuY29tIn0.Oupk2W82qhRo5RL91UDx599R0Xwhim_HzuikY_NPwsw"), CancellationToken.None));
    }

    [Fact]
    public async Task Valid_AccountAndProduct_Success()
    {
        var handler = new UpdateProductCommandHandler(_mock.Object, new JwtReader(), new ProductValidator());

        var b4 = _mock.Object.Get(p => p.CreatorId == MockRepositories.ID2).FirstOrDefault()!;
        var b4Name = b4.Name;
        var b4Description = b4.Description;
        var b4Price = b4.Price;
        var b4AvailableAmount = b4.AvailableAmount;
        var id = b4.Id;

        var result = await handler.Handle(
            new UpdateProductCommand(id, "name", "desc", Convert.ToDecimal(213.432), 123, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJiMTg4ZjEzYi03ZDcyLTRhNTYtOTNlNi1hOTlhZWRhNjRiYjYiLCJzdWIiOiJhZG1pbiIsImVtYWlsIjoiYnN2Y2guYWxAZ21haWwuY29tIiwibmFtZWlkIjoiNTZkMjdhNTAtZGZkMi00NjZhLTg0OTUtY2I1NjMxZTc3NmJjIiwicm9sZSI6IkFkbWluIiwibmJmIjoxNzE2Nzg0MDY5LCJleHAiOjE3MTY3ODQzNjksImlhdCI6MTcxNjc4NDA2OSwiaXNzIjoiaHR0cHM6Ly9hbC5hY2NvdW50c2VydmljZS5jb20iLCJhdWQiOiJodHRwczovL2FsLnNob3AuY29tIn0.INcT-Ohe-YOeJGuVhKP9ApDtJuQXuz9yCNj2SXAoPfo"), CancellationToken.None);

        var after = _mock.Object.Get(p => p.CreatorId == MockRepositories.ID2).FirstOrDefault()!;

        Assert.True(b4Name != after.Name);
        Assert.True(b4Description != after.Description);
        Assert.True(b4Price != after.Price);
        Assert.True(b4AvailableAmount != after.AvailableAmount);
    }
}
