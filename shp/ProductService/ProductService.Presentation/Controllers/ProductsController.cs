using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductService.Application.ProductUseCases.Commands;
using ProductService.Application.ProductUseCases.Queries;
using ProductService.Presentation.Contracts;
using System.ComponentModel.DataAnnotations;

namespace ProductService.Presentation.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllProductsAsync()
    {
        var header = HttpContext.Request.Headers.Authorization;
        var token = header.FirstOrDefault()!.Split(" ", StringSplitOptions.RemoveEmptyEntries).Last()!;

        var products = await _mediator.Send(
            new GetAllProductsQuery(token));

        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult> CreateProductAsync(
        CreateProductRequest request)
    {
        var header = HttpContext.Request.Headers.Authorization;
        var token = header.FirstOrDefault()!.Split(" ", StringSplitOptions.RemoveEmptyEntries).Last()!;

        try
        {
            var added = await _mediator.Send(new CreateProductCommand(
                request.Name, request.Description, request.Price, request.AvailableAmount, token));

            return Ok(new CreatedProductResponse(added));
        }
        catch (SecurityTokenInvalidSignatureException stise)
        {
            return Problem(
                statusCode: 401, title: "Ivalid Token", detail: stise.Message);
        }
        catch (ValidationException ve)
        {
            return Problem(
                statusCode: 400, title: "Invalid Data", detail: ve.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateProductAsync(
        string id, UpdateProductRequest request)
    {
        var header = HttpContext.Request.Headers.Authorization;
        var token = header.FirstOrDefault()!.Split(" ", StringSplitOptions.RemoveEmptyEntries).Last()!;

        try
        {
            var updated = await _mediator.Send(new UpdateProductCommand(
                Guid.Parse(id), request.Name, request.Description, request.Price, request.AvailableAmount, token));

            return updated ? NoContent() : Problem(
                statusCode: 500, title: "Failed To Update", detail: "Something Went Wrong");
        }
        catch (SecurityTokenInvalidSignatureException stise)
        {
            return Problem(
                statusCode: 401, title: "Invalid Token", detail: stise.Message);
        }
        catch (ArgumentException ae)
        {
            return Problem(
                statusCode: 404, title: "Product Not Found", detail: ae.Message);
        }
        catch (InvalidOperationException ioe)
        {
            return Problem(
                statusCode: 403, title: "Access Denied", detail: ioe.Message);
        }
        catch (ValidationException ve)
        {
            return Problem(
                statusCode: 400, title: "Invalid Data", detail: ve.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProductAsync(Guid id)
    {
        var header = HttpContext.Request.Headers.Authorization;
        var token = header.FirstOrDefault()!.Split(" ", StringSplitOptions.RemoveEmptyEntries).Last()!;

        try
        {
            var deleted = await _mediator.Send(new DeleteProductCommand(id, token));

            return deleted ? NoContent() : Problem(
                statusCode: 500, title: "Failed To Delete", detail: "Something Went Wrong");
        }
        catch (SecurityTokenInvalidSignatureException stise) 
        {
            return Problem(
                statusCode: 401, title: "Invalid Token", detail: stise.Message);
        }
        catch (ArgumentException ae)
        {
            return Problem(
                statusCode: 404, title: "Product Not Found", detail: ae.Message);
        }
        catch (InvalidOperationException ioe)
        {
            return Problem(
                statusCode: 403, title: "Access Denied", detail: ioe.Message);
        }
    }
}
