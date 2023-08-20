import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {PaginationModule} from 'ngx-bootstrap/pagination';
import { PagingHeaderComponent } from './Components/paging-header/paging-header.component';
import { PagerComponent } from './Components/pager/pager.component';
import { OrderTotalsComponent } from './Components/order-totals/order-totals.component';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    PagingHeaderComponent,
    PagerComponent,
    OrderTotalsComponent    
  ],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    ReactiveFormsModule
  ],
  exports:[
    PaginationModule,
    PagingHeaderComponent,
    PagerComponent,
    OrderTotalsComponent,
    ReactiveFormsModule
  ]
})
export class SharedModule { }
