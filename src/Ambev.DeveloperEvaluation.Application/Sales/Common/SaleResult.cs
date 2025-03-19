using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using FluentValidation.Results;
using System.ComponentModel;

namespace Ambev.DeveloperEvaluation.Application.Sales.Common;

public class SaleResult<T>
{
    public bool Success { get; set; }
    public IEnumerable<ValidationFailure>? Errors { get; set; }
    public int StatusCode { get; set; }
    public T? Data { get; set; }
    public long TotalItems { get; set; }

    public SaleResult() { }


    public List<ValidationErrorDetail> GetErrorDetail()
    {
        if (Errors == null || !Errors.Any())
            return new();

        return Errors.Select(f => (ValidationErrorDetail)f).ToList();
    }

    public static SaleResult<T> Ok(int statusCode, T? data, long totalItems = 0)
        => new SaleResult<T>()
        {
            Success = true,
            StatusCode = statusCode,
            Data = data,
            TotalItems = totalItems
        };

    public static SaleResult<T> Failure(IEnumerable<ValidationFailure> errors, int statusCode)
        => new SaleResult<T>()
        {
            Success = false,
            StatusCode = statusCode,
            Errors = errors
        };
}

