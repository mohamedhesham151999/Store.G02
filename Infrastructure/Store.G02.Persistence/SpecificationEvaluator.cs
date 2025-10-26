using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Persistence
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TKey,TEntity> (IQueryable<TEntity> inputQuery ,ISpecifications<TKey,TEntity> spec) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;

            if(spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }

            query = spec.Includes.Aggregate(query,(query, IncludeExpression) => query.Include(IncludeExpression) );


            return query;
        }

    }
    }
