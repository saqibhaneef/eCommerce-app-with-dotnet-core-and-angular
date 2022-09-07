using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithBrandsAndTypesSpecification:BaseSpecification<Product>
    {
        public ProductsWithBrandsAndTypesSpecification(ProductSpecParams productsParams)
        :base(x=>(!productsParams.BrandId.HasValue || productsParams.BrandId==x.ProductBrandId) && (!productsParams.TypeId.HasValue || productsParams.TypeId==x.ProductTypeId)
        )
        {
            AddInclude(x=>x.ProductType);
            AddInclude(x=>x.ProductBrand);
            AddOrderBy(x=>x.Name);
            ApplyPaging(productsParams.PageSize*(productsParams.PageIndex-1),productsParams.PageSize);
            if(!string.IsNullOrEmpty(productsParams.Sort))
            {
                switch (productsParams.Sort)
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