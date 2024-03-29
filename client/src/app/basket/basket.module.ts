import { NgModule } from '@angular/core';
import { BasketComponent } from './basket.component';
import { BasketRoutingModule } from './basket-routing.module';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [
    BasketComponent,
  ],
  imports: [
    BasketRoutingModule,
    CommonModule,
    SharedModule
  ]
})
export class BasketModule { }
