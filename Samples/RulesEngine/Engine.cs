using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace RulesEngine
{
    public class Engine<T>
    {
       
        Expression BuildSimpleExpression(Rule r, ParameterExpression e)
        {
            MemberExpression left = MemberExpression.Property(e, r.Member);
            Type t = typeof(T).GetProperty(r.Member).PropertyType;
            ExpressionType expr;
            ConstantExpression right;

            try
            {
                //operation expression
                if (ExpressionType.TryParse(r.Operation, out expr))
                {
                    //<Not> is the only unary expression supported
                    if (expr.Equals(ExpressionType.Not))
                    {
                        return Expression.MakeUnary(expr, left, left.GetType());
                    }
                    else
                    {
                        right = Expression.Constant(Convert.ChangeType(r.Target, t));
                        return Expression.MakeBinary(expr, left, right);
                    }
                }
                //method invocation expression
                else
                {
                    MethodInfo method = t.GetMethod(r.Operation, new Type[] { r.Target.GetType() });
                    Type p = method.GetParameters()[0].ParameterType;
                    right = Expression.Constant(Convert.ChangeType(r.Target, p));
                    return Expression.Call(left, method, right);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unsupported Expression", ex);
            }
        }

        Expression BuildExpression(Rule r, ParameterExpression e)
        {
            if (r.Constraint == null)
            {
                return BuildSimpleExpression(r, e);
            }
            else
            {
                Expression e0 = BuildSimpleExpression(r, e);
                Expression e1 = BuildSimpleExpression(r.Constraint, e);
                return BuildContraintExpression(e0, e1);
            }
        }

        Expression BuildContraintExpression(Expression a, Expression b)
        {
            return ConditionalExpression.Condition(a, b, Expression.Constant(true));
        }

        Func<T, bool> CompileRule(Rule r)
        {
            ParameterExpression p = Expression.Parameter(typeof(T));
            Expression expr = BuildExpression(r, p);
            return Expression.Lambda<Func<T, bool>>(expr, p).Compile();
        }

        public bool Evaluate(Rule r, T s)
        {
            Func<T, bool> compiledRule = this.CompileRule(r);
            return compiledRule(s);
        }
    }
}