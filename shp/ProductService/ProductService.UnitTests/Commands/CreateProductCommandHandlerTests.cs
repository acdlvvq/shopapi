using FluentValidation;
using Moq;
using ProductService.Application.ProductUseCases.Commands;
using ProductService.Application.ProductUseCases.Handlers;
using ProductService.Core.Interfaces;
using ProductService.Core.Validators;
using ProductService.Infrastructure.Jwt;
using ProductService.UnitTests.Mocks;

namespace ProductService.UnitTests.Commands;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductsRepository> _mock;

    public CreateProductCommandHandlerTests()
    {
        _mock = MockRepositories.GetProductsRepository();
    }

    [Fact]
    public async Task Invalid_Data_Throw()
    {
        var handler = new CreateProductCommandHandler(_mock.Object, new JwtReader(), new ProductValidator());

        await Assert.ThrowsAsync<ValidationException>(async () => await handler.Handle(
            new CreateProductCommand("n", "sc", Convert.ToDecimal(-213.432), -123, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJlMzkzYzgzNi0yZTA5LTRmYjMtOGI3MS00N2RjZjBkZjg5NDQiLCJzdWIiOiJhYm9iYSIsImVtYWlsIjoiYWJvYmExMjNAZ21haWwuY29tIiwibmFtZWlkIjoiODA1NDNmYTMtMWIzYi00YjM2LWIzZDEtNzA1MmViM2RhM2NmIiwicm9sZSI6IkRlZmF1bHRVc2VyIiwibmJmIjoxNzE2NzUzNDk0LCJleHAiOjE3MTY3NTM3OTQsImlhdCI6MTcxNjc1MzQ5NCwiaXNzIjoiaHR0cHM6Ly9hbC5hY2NvdW50c2VydmljZS5jb20iLCJhdWQiOiJodHRwczovL2FsLnNob3AuY29tIn0.Oupk2W82qhRo5RL91UDx599R0Xwhim_HzuikY_NPwsw"), CancellationToken.None));
    }

    [Fact]
    public async Task Valid_Data_Success()
    {
        var handler = new CreateProductCommandHandler(_mock.Object, new JwtReader(), new ProductValidator());

        var result = await handler.Handle(
            new CreateProductCommand("name", "desc", Convert.ToDecimal(213.432), 123, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJlMzkzYzgzNi0yZTA5LTRmYjMtOGI3MS00N2RjZjBkZjg5NDQiLCJzdWIiOiJhYm9iYSIsImVtYWlsIjoiYWJvYmExMjNAZ21haWwuY29tIiwibmFtZWlkIjoiODA1NDNmYTMtMWIzYi00YjM2LWIzZDEtNzA1MmViM2RhM2NmIiwicm9sZSI6IkRlZmF1bHRVc2VyIiwibmJmIjoxNzE2NzUzNDk0LCJleHAiOjE3MTY3NTM3OTQsImlhdCI6MTcxNjc1MzQ5NCwiaXNzIjoiaHR0cHM6Ly9hbC5hY2NvdW50c2VydmljZS5jb20iLCJhdWQiOiJodHRwczovL2FsLnNob3AuY29tIn0.Oupk2W82qhRo5RL91UDx599R0Xwhim_HzuikY_NPwsw"), CancellationToken.None);

        Assert.True(_mock.Object.GetAll().Any(p => p.Id == result));
    }
}
