using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SimpleBehaviors
{
    public static class ExpressionExtensions
    {
        public static Action ConvertMethodExpressionToAction<TSteps>( this Expression<Action<TSteps>> exp, TSteps obj )
        {
            var expressionBody = exp.Body as MethodCallExpression;
            return ( Action )Delegate.CreateDelegate( typeof( Action ), obj, expressionBody.Method.Name );
        }
    }
}
