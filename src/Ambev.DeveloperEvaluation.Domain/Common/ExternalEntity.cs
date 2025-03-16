using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Common;

public abstract class ExternalEntity
{
    public Guid ExternalId { get; protected set; }
    public string Name { get; protected set; }
}