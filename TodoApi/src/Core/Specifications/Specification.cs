namespace Core.Specifications;
using System;
using System.Threading.Tasks;
using System.Linq;

public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();
}
