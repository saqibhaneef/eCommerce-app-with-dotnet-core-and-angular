using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithBrandsAndTypesSpecification:BaseSpecification<Product>
    {
        public ProductsWithBrandsAndTypesSpecification(string? sort,int? brandId, int? typeId)
        :base(x=>(!brandId.HasValue || brandId==x.ProductBrandId) && (!typeId.HasValue || typeId==x.ProductTypeId)
        )
        {
            AddInclude(x=>x.ProductType);
            AddInclude(x=>x.ProductBrand);
            AddOrderBy(x=>x.Name);
            if(!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "priceAsc":
                        AddOrderBy(x=>x.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(x=>x.Price);
                        break;                        
                    default:
                        AddOrderBy(x=>x.Name);
                        break;
                }
            }
        }

        public ProductsWithBrandsAndTypesSpecification(int id) : base(x=>x.Id==id)
        {
            AddInclude(x=>x.ProductType);
            AddInclude(x=>x.ProductBrand);
        }
    }
}