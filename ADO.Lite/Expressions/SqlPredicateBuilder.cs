using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Lite.Expressions
{
    internal class SqlPredicateBuilder : ExpressionVisitor
    {

        private StringBuilder sb;
        private string _orderBy;
        private int? _skip;
        private int? _take;
        private string _whereClause;

        public int? Skip
        {
            get
            {
                return _skip;
            }
        }

        public int? Take
        {
            get
            {
                return _take;
            }
        }

        public string OrderBy
        {
            get
            {
                return _orderBy;
            }
        }

        public string WhereClause
        {
            get
            {
                return _whereClause;
            }
        }

        public SqlPredicateBuilder()
        {
            this._orderBy = string.Empty;
            this._skip = null;
            this._take = null;
            this._whereClause = string.Empty;
        }

        public string Translate(Expression expression)
        {
            this.sb = new StringBuilder();
            this.Visit(expression);
            return this.sb.ToString();
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where")
            {
                this.Visit(m.Arguments[0]);
                LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                this.Visit(lambda.Body);
                return m;
            }
            else if (m.Method.Name == "Take")
            {
                if (this.ParseTakeExpression(m))
                {
                    Expression nextExpression = m.Arguments[0];
                    return this.Visit(nextExpression);
                }
            }
            else if (m.Method.Name == "Skip")
            {
                if (this.ParseSkipExpression(m))
                {
                    Expression nextExpression = m.Arguments[0];
                    return this.Visit(nextExpression);
                }
            }
            else if (m.Method.Name == "OrderBy")
            {
                if (this.ParseOrderByExpression(m, "ASC"))
                {
                    Expression nextExpression = m.Arguments[0];
                    return this.Visit(nextExpression);
                }
            }
            else if (m.Method.Name == "OrderByDescending")
            {
                if (this.ParseOrderByExpression(m, "DESC"))
                {
                    Expression nextExpression = m.Arguments[0];
                    return this.Visit(nextExpression);
                }
            }

            throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    sb.Append(" NOT ");
                    this.Visit(u.Operand);
                    break;

                case ExpressionType.Convert:
                    this.Visit(u.Operand);
                    break;

                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }
            return u;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression b)
        {
            sb.Append("(");
            this.Visit(b.Left);

            switch (b.NodeType)
            {
                case ExpressionType.And:
                    sb.Append(" AND ");
                    break;

                case ExpressionType.AndAlso:
                    sb.Append(" AND ");
                    break;

                case ExpressionType.Or:
                    sb.Append(" OR ");
                    break;

                case ExpressionType.OrElse:
                    sb.Append(" OR ");
                    break;

                case ExpressionType.Equal:
                    if (IsNullConstant(b.Right))
                    {
                        sb.Append(" IS ");
                    }
                    else
                    {
                        sb.Append(" = ");
                    }
                    break;

                case ExpressionType.NotEqual:
                    if (IsNullConstant(b.Right))
                    {
                        sb.Append(" IS NOT ");
                    }
                    else
                    {
                        sb.Append(" <> ");
                    }
                    break;

                case ExpressionType.LessThan:
                    sb.Append(" < ");
                    break;

                case ExpressionType.LessThanOrEqual:
                    sb.Append(" <= ");
                    break;

                case ExpressionType.GreaterThan:
                    sb.Append(" > ");
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(" >= ");
                    break;

                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
            }

            this.Visit(b.Right);
            sb.Append(")");
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            IQueryable q = c.Value as IQueryable;

            if (q == null && c.Value == null)
            {
                sb.Append("NULL");
            }
            else if (q == null)
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        sb.Append(((bool)c.Value) ? 1 : 0);
                        break;

                    case TypeCode.String:
                        sb.Append("'");
                        sb.Append(c.Value);
                        sb.Append("'");
                        break;

                    case TypeCode.DateTime:
                        sb.Append("'");
                        sb.Append(c.Value);
                        sb.Append("'");
                        break;

                    case TypeCode.Object:

                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));

                    default:
                        sb.Append(c.Value);
                        break;
                }
            }

            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                sb.Append(m.Member.Name);
                return m;
            }

            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Constant)
            {
                object value = null;
                object container = ((ConstantExpression)m.Expression).Value;

                var member = m.Member;

                if (member is FieldInfo)
                {
                    value = ((FieldInfo)member).GetValue(container);
                }
                if (member is PropertyInfo)
                {
                    value = ((PropertyInfo)member).GetValue(container, null);
                }

                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.Boolean:
                        sb.Append(((bool)value) ? 1 : 0);
                        break;

                    case TypeCode.String:
                        sb.Append("'");
                        sb.Append(value);
                        sb.Append("'");
                        break;

                    case TypeCode.DateTime:
                        sb.Append("'");
                        sb.Append(value);
                        sb.Append("'");
                        break;
                    default:
                        sb.Append(value);
                        break;
                }

                return m;
            }

            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Convert)
            {
                sb.Append(m.Member.Name + "(" + ((UnaryExpression)m.Expression).Operand + ")");

                return m;
            }

            if (m.Expression != null && m.Expression.NodeType == ExpressionType.MemberAccess)
            {
                string operand = String.Empty;

                if (m.Expression.ToString().Contains("DateTime.Now"))
                {
                    operand = "GETDATE()";

                    sb.Append(m.Member.Name + "(" + operand + ")");

                }
                //else if (((ConstantExpression)m.Expression).Value.ToString().ToLower().StartsWith("value"))
                //{ operand = ((ConstantExpression)m.Expression).Value.ToString() ; }
                else
                {
                    operand = Expression.Lambda(m).Compile().DynamicInvoke().ToString();
                    //operand = m.Expression.ToString();

                    sb.Append(operand);
                }


                //sb.Append(m.Member.Name + "(" + operand + ")"); old
                return m;
            }

            throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
        }

        protected bool IsNullConstant(Expression exp)
        {
            return (exp.NodeType == ExpressionType.Constant && ((ConstantExpression)exp).Value == null);
        }

        private bool ParseOrderByExpression(MethodCallExpression expression, string order)
        {
            UnaryExpression unary = (UnaryExpression)expression.Arguments[1];
            LambdaExpression lambdaExpression = (LambdaExpression)unary.Operand;

            lambdaExpression = (LambdaExpression)Evaluator.PartialEval(lambdaExpression);

            MemberExpression body = lambdaExpression.Body as MemberExpression;
            if (body != null)
            {
                if (string.IsNullOrEmpty(_orderBy))
                {
                    _orderBy = string.Format("{0} {1}", body.Member.Name, order);
                }
                else
                {
                    _orderBy = string.Format("{0}, {1} {2}", _orderBy, body.Member.Name, order);
                }

                return true;
            }

            return false;
        }

        private bool ParseTakeExpression(MethodCallExpression expression)
        {
            ConstantExpression sizeExpression = (ConstantExpression)expression.Arguments[1];

            int size;
            if (int.TryParse(sizeExpression.Value.ToString(), out size))
            {
                _take = size;
                return true;
            }

            return false;
        }

        private bool ParseSkipExpression(MethodCallExpression expression)
        {
            ConstantExpression sizeExpression = (ConstantExpression)expression.Arguments[1];

            int size;
            if (int.TryParse(sizeExpression.Value.ToString(), out size))
            {
                _skip = size;
                return true;
            }

            return false;
        }
    }

    public static class Evaluator
    {
        /// <summary>
        /// Performs evaluation & replacement of independent sub-trees
        /// </summary>
        /// <param name="expression">The root of the expression tree.</param>
        /// <param name="fnCanBeEvaluated">A function that decides whether a given expression node can be part of the local function.</param>
        /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
        public static Expression PartialEval(Expression expression, Func<Expression, bool> fnCanBeEvaluated)
        {
            return new SubtreeEvaluator(new Nominator(fnCanBeEvaluated).Nominate(expression)).Eval(expression);
        }

        /// <summary>
        /// Performs evaluation & replacement of independent sub-trees
        /// </summary>
        /// <param name="expression">The root of the expression tree.</param>
        /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
        public static Expression PartialEval(Expression expression)
        {
            return PartialEval(expression, Evaluator.CanBeEvaluatedLocally);
        }

        private static bool CanBeEvaluatedLocally(Expression expression)
        {
            return expression.NodeType != ExpressionType.Parameter;
        }

        /// <summary>
        /// Evaluates & replaces sub-trees when first candidate is reached (top-down)
        /// </summary>
        public class SubtreeEvaluator : ExpressionVisitor
        {
            private HashSet<Expression> candidates;

            internal SubtreeEvaluator(HashSet<Expression> candidates)
            {
                this.candidates = candidates;
            }

            internal Expression Eval(Expression exp)
            {
                return this.Visit(exp);
            }

            public override Expression Visit(Expression node)
            {
                if (node == null)
                {
                    return null;
                }
                if (this.candidates.Contains(node))
                {
                    return this.Evaluate(node);
                }
                return base.Visit(node);
            }

            //protected  Expression Visit(Expression exp)
            //{
            //    if (exp == null)
            //    {
            //        return null;
            //    }
            //    if (this.candidates.Contains(exp))
            //    {
            //        return this.Evaluate(exp);
            //    }
            //    return base.Visit(exp);
            //}

            private Expression Evaluate(Expression e)
            {
                if (e.NodeType == ExpressionType.Constant)
                {
                    return e;
                }
                LambdaExpression lambda = Expression.Lambda(e);
                Delegate fn = lambda.Compile();
                return Expression.Constant(fn.DynamicInvoke(null), e.Type);
            }
        }

        /// <summary>
        /// Performs bottom-up analysis to determine which nodes can possibly
        /// be part of an evaluated sub-tree.
        /// </summary>
        public class Nominator : ExpressionVisitor
        {
            private Func<Expression, bool> fnCanBeEvaluated;
            private HashSet<Expression> candidates;
            private bool cannotBeEvaluated;

            internal Nominator(Func<Expression, bool> fnCanBeEvaluated)
            {
                this.fnCanBeEvaluated = fnCanBeEvaluated;
            }

            internal HashSet<Expression> Nominate(Expression expression)
            {
                this.candidates = new HashSet<Expression>();
                this.Visit(expression);
                return this.candidates;
            }

            public override Expression Visit(Expression node)
            {
                if (node != null)
                {
                    bool saveCannotBeEvaluated = this.cannotBeEvaluated;
                    this.cannotBeEvaluated = false;
                    base.Visit(node);
                    if (!this.cannotBeEvaluated)
                    {
                        if (this.fnCanBeEvaluated(node))
                        {
                            this.candidates.Add(node);
                        }
                        else
                        {
                            this.cannotBeEvaluated = true;
                        }
                    }
                    this.cannotBeEvaluated |= saveCannotBeEvaluated;
                }
                return node;
            }

            //protected  Expression Visit(Expression expression)
            //{
            //    if (expression != null)
            //    {
            //        bool saveCannotBeEvaluated = this.cannotBeEvaluated;
            //        this.cannotBeEvaluated = false;
            //        base.Visit(expression);
            //        if (!this.cannotBeEvaluated)
            //        {
            //            if (this.fnCanBeEvaluated(expression))
            //            {
            //                this.candidates.Add(expression);
            //            }
            //            else
            //            {
            //                this.cannotBeEvaluated = true;
            //            }
            //        }
            //        this.cannotBeEvaluated |= saveCannotBeEvaluated;
            //    }
            //    return expression;
            //}
        }
    }


}

