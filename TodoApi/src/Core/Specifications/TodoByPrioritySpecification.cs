namespace Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;//.Expressions;

public class TodoByPrioritySpecification : Specification<TodoItem>
{
    private readonly int _priority;

    public TodoByPrioritySpecification(int priority) => _priority = priority;

    public override Expression<Func<TodoItem, bool>> ToExpression()
        => item => item.Priority == _priority;
    }
