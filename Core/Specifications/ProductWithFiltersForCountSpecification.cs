using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithFiltersForCountSpecification:BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecParams productsParams)
        :base(x=>(string.IsNullOrEmpty(productsParams.Search) || x.Name.ToLower().Contains(productsParams.Search)) && (!productsParams.BrandId.HasValue || productsParams.BrandId==x.ProductBrandId) && (!productsParams.TypeId.HasValue || productsParams.TypeId==x.ProductTypeId)
        )
        {
            
        }
    }
}