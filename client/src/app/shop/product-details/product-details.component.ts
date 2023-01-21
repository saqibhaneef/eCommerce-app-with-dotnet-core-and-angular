import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
import { IProduct } from 'src/app/shared/models/product';
import { BreadcrumbService } from 'xng-breadcrumb';
import { Breadcrumb } from 'xng-breadcrumb/lib/types/breadcrumb';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.sass']
})
export class ProductDetailsComponent implements OnInit {
  product:IProduct
  quantity=1;

  constructor(private shopService:ShopService,private activateRoute:ActivatedRoute,private bcService:BreadcrumbService, private basketService:BasketService) { 
    this.bcService.set('@productDetails',' ')
  }

  ngOnInit(): void {
    this.loadProduct()
  }

  addItemToBasket()
  {
    this.basketService.addItemToBasket(this.product,this.quantity);
  }

  incrementProduct()
  {
    this.quantity++;
  }

  decrementProduct()
  {
    if(this.quantity>1)
    {
      this.quantity--;
    }
    
  }

  loadProduct()
  {
    this.shopService.getProduct(+this.activateRoute.snapshot.paramMap.get('id')).subscribe(res=>{
      this.product=res;
      this.bcService.set('@productDetails',this.product.name)
    },error=>{
      console.log(error)
    });
  }

}
