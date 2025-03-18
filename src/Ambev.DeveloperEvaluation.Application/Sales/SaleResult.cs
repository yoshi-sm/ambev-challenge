using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public class SaleResult
{
    public bool Success { get; set; }
    public IEnumerable<ValidationFailure>? Errors { get; set; }
    public int StatusCode { get; set; }
    public object? Data { get; set; }

    public SaleResult() { }


    public List<ValidationErrorDetail> GetErrorDetail()
    {
        if (Errors == null || !Errors.Any())
            return new();

        return Errors.Select(f => (ValidationErrorDetail)f).ToList();
    }

    public static SaleResult Ok(int statusCode, object? data)
        => new SaleResult()
        {
            Success = true,
            StatusCode = statusCode,
            Data = data
        };

    public static SaleResult Failure(IEnumerable<ValidationFailure> errors, int statusCode)
        => new SaleResult()
        {
            Success = false,
            StatusCode = statusCode,
            Errors = errors
        };
}

