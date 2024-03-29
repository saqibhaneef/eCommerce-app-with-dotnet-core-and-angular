import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NotFoundError } from 'rxjs';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './core/guards/auth.guard';

const routes: Routes = [
  {path:'',component:HomeComponent,data:{breadcrumb:'Home'}},
  {path:'test-error',component:TestErrorComponent,data:{breadcrumb:'Test Errors'}},
  {path:'not-found',component:NotFoundError,data:{breadcrumb:'Not Found'}},
  {path:'server-error',component:ServerErrorComponent,data:{breadcrumb:'Server error'}},
  {path:'shop',loadChildren:()=>import('./shop/shop.module').then(mod=>mod.ShopModule),data:{breadcrumb:'Shop'}},
  {
    path:'basket',
    canActivate:[AuthGuard],
    loadChildren:()=>import('./basket/basket.module').then(mod=>mod.BasketModule),data:{breadcrumb:'Basket'}
  },
  {path:'checkout',loadChildren:()=>import('./checkout/checkout.module').then(mod=>mod.CheckoutModule),data:{breadcrumb:'Checkout'}},
  {path:'account',loadChildren:()=>import('./account/account.module').then(mod=>mod.AccountModule),data:{breadcrumb:'Account'}},
  {path:'**',redirectTo:'',pathMatch:'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
