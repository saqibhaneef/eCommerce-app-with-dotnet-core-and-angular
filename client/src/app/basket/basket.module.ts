import { NgModule } from '@angular/core';
import { BasketComponent } from './basket.component';
import { BasketRoutingModule } from './basket-routing.module';
import { CommonModule } from '@angular/common';



@NgModule({
  declarations: [
    BasketComponent,
  ],
  imports: [
    BasketRoutingModule,
    CommonModule
  ]
})
export class BasketModule { }
