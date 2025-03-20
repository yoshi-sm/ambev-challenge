using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Common;

public class SaleDataResult
{
    public Sale Sale { get; private set; }
    public FluentValidation.Results.ValidationResult ValidationResult { get; private set; }
    public bool IsValid => ValidationResult.IsValid;

    private SaleDataResult(Sale sale, FluentValidation.Results.ValidationResult validationResult)
    {
        Sale = sale;
        ValidationResult = validationResult;
    }

    public static SaleDataResult Success(Sale sale)
    {
        return new SaleDataResult(sale, new FluentValidation.Results.ValidationResult());
    }

    public static SaleDataResult Failure(Sale sale, FluentValidation.Results.ValidationResult validationResult)
    {
        return new SaleDataResult(sale, validationResult);
    }
}
