
namespace Ambev.DeveloperEvaluation.Domain.ReadModels;

public class SaleDocument
{
    public virtual Guid Id { get; set; }
    public string SaleNumber { get; set; }
    public DateTime SaleDate { get; set; }
    public CustomerInfo Customer { get; set; }
    public BranchInfo Branch { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }
    public List<SaleItemDocument> Items { get; set; }
}

public class CustomerInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class BranchInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
