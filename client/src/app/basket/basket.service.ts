import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket, IBasket, IBasketItem } from '../shared/models/basket';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
baseUrl=environment.apiUrl;
private basketSource=new BehaviorSubject<IBasket>(null);
basket$=this.basketSource.asObservable();
  
constructor(private http:HttpClient) { }

getBasket(id:string)
{
  return this.http.get(this.baseUrl+'basket?id='+id)
  .pipe(
    map((basket:IBasket)=>{
      this.basketSource.next(basket);
      console.log(this.getCurrentBasketValue());
    })
  );
}

setBasket(basket:IBasket){
  debugger
  return this.http.post(this.baseUrl+'basket',basket).subscribe((response:IBasket)=>{
    debugger
    this.basketSource.next(response);
    console.log(response);
  }, error=>{
    debugger
    console.log(error);
  })
}

getCurrentBasketValue()
{
  return this.basketSource.value;
}

addItemToBasket(item:IProduct, quantity=1)
{
  console.log("Item:"+item)
  debugger
  const itemToAdd:IBasketItem=this.mapProductItemToBasketItem(item,quantity);
  const basket= this.getCurrentBasketValue() ?? this.createBasket();
  basket.basketItems=this.addOrUpdateItem(basket.basketItems,itemToAdd,quantity);
  this.setBasket(basket);

}
  private addOrUpdateItem(basketItems: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    console.log(basketItems);
    const index=basketItems.findIndex(x=>x.id===itemToAdd.id);
    if(index===-1)
    {
      itemToAdd.quantity=quantity;
      basketItems.push(itemToAdd);
    }else{
      basketItems[index].quantity +=quantity;
    }    
    return basketItems;
  }
 private createBasket(): IBasket {
    const basket=new Basket();
    localStorage.setItem('basket_id',basket.id);
    return basket;    
  }
 private mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
    return {
      id:item.id,
      productName:item.name,
      brand:item.productBrand,
      pictureUrl:item.pictureUrl,
      price:item.price,
      quantity:quantity,  
      type:item.productType
                      
    }
  }

}
