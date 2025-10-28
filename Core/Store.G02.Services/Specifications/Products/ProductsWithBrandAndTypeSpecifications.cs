using Store.G02.Domain.Entities;
using Store.G02.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Specifications.Products
{
    public class ProductsWithBrandAndTypeSpecifications : BaseSpecifications<int, Product>
    {
        public ProductsWithBrandAndTypeSpecifications(int id ) : base(P => P.Id == id)
        {
            ApplyInclude();
        }
        public ProductsWithBrandAndTypeSpecifications(int? brandId, int? typeId, string? sort) : base
            (
                P => 
                (!brandId.HasValue ||  P.BrandId == brandId)  
                && 
                (!typeId.HasValue  ||   P.TypeId == typeId)
            )
        {
            ApplySorting(sort);

            ApplyInclude();
        }

        public void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "priceasc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }

        }

        private void ApplyInclude()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Type);
        }
    }
}
