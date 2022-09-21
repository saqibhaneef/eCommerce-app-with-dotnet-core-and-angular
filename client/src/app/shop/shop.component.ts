import { Component, OnInit } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.sass']
})
export class ShopComponent implements OnInit {
  products:IProduct[]
  brands:IBrand[]
  types:IType[]
  shopParams=new ShopParams()
  totalCount:number
  sortOptions=[
    {name:'Alphbatically', value:'name'},
    {name:'Price: Low to High', value:'priceAsc'},
    {name:'Price: High to Low', value:'priceDesc'}
  ];

  constructor(private shopService:ShopService) { }

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

getProducts(){
  this.shopService.getProducts(this.shopParams).subscribe((response)=>{
    this.products=response.data;
    this.shopParams.pageNumber=response.pageIndex
    this.shopParams.pageSize=response.pageSize
    this.totalCount=response.count

  },error=>{
    console.log(error)
  });
}

getBrands()
{
  this.shopService.getBrands().subscribe(response=>{
    this.brands=[{id:0,name:"All"},...response]
  },error=>{
    console.log(error)
  })

}

getTypes()
{
  this.shopService.getTypes().subscribe(response=>{
    this.types=[{id:0,name:"All"},...response]
  },error=>{
    console.log(error)
  })
}

onBrandSelected(brandId:number)
{
  this.shopParams.brandId=brandId;
  this.getProducts();
}

onTypeSelected(typeId:number)
{
  this.shopParams.typeId=typeId;
  this.getProducts();
}
  
onSortSelected(event)
{
  debugger
  this.shopParams.sort=event.target.value;
  this.getProducts();
}  

onPageChanged(event:any)
{
  debugger
  this.shopParams.pageNumber=event.page
  this.getProducts()
}

}
