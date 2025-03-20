using Ambev.DeveloperEvaluation.Application.Sales.Actions.CancelSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Factories;
using AutoMapper;
using Bogus;
using MediatR;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CancelSaleHandlerTests
{
    private readonly ISaleReadRepository _saleReadRepository = Substitute.For<ISaleReadRepository>();
    private readonly ISaleWriteRepository _saleWriteRepository = Substitute.For<ISaleWriteRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IPublisher _eventPublisher = Substitute.For<IPublisher>();
    private readonly SaleFactory _saleFactory = new();
    private readonly CancelSaleHandler _handler;

    public CancelSaleHandlerTests()
    {
        _handler = new CancelSaleHandler(_saleReadRepository, _saleWriteRepository, _mapper, _eventPublisher);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenSaleDocumentDoesNotExist()
    {
        // Arrange
        var command = new CancelSaleCommand(Guid.NewGuid());
        _saleReadRepository.GetByIdAsync(command.Id).Returns((SaleDocument)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        await _saleReadRepository.Received(1).GetByIdAsync(command.Id);
        await _saleWriteRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>());
        await _eventPublisher.DidNotReceive().Publish(Arg.Any<SaleCancelledEvent>());
    }

    [Fact]
    public async Task Handle_ShouldCancelSaleSuccessfully_WhenSaleDocumentExists()
    {
        // Arrange
        var command = new CancelSaleCommand(Guid.NewGuid());
        var saleDocument = SaleDocumentFactory.Create();
        var sale = _saleFactory.Create();
        var canceledSaleDocument = SaleDocumentFactory.Create();

        _saleReadRepository.GetByIdAsync(command.Id).Returns(saleDocument);
        _mapper.Map<Sale>(saleDocument).Returns(sale);
        _mapper.Map<SaleDocument>(sale).Returns(canceledSaleDocument);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(canceledSaleDocument, result.Data);

        await _saleReadRepository.Received(1).GetByIdAsync(command.Id);
        await _saleWriteRepository.Received(1).UpdateAsync(sale);
        await _eventPublisher.Received(1).Publish(Arg.Is<SaleCancelledEvent>(e => e.Sale == canceledSaleDocument));
    }
}