using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Specification.Base
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        // Nơi định nghĩa sẵn các thuộc tính dùng chung cho mọi specification khác
        // là bộ khung chuẩn để giúp các lớp con định nghĩa điều kiện 
        protected BaseSpecification(Expression<Func<T,bool>> criteria) 
        {
            Criteria = criteria;
        }
        public Expression<Func<T, bool>> Criteria { get; }


        public List<Expression<Func<T, object>>> Include { get; } =  new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>>? OrderBy { get; private set; }

        public Expression<Func<T, object>>? OrderByDescending { get;private set; }

        protected virtual void AddIncludes(Expression<Func<T, object>> includesExpression)
        {
            Include.Add(includesExpression);   
        }
        protected virtual void AddOrderBy (Expression<Func<T,object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected virtual void AddOrderByDescending (Expression<Func<T,object>> orderByDecsExpression)
        {
            OrderByDescending = orderByDecsExpression;
        }
    }
}
