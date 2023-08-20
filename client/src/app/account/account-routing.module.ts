import { NgModule } from '@angular/core';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { RouterModule } from '@angular/router';

const routues=[
  {path:"login",component:LoginComponent},
  {path:"register",component:RegisterComponent}
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routues)
  ],
  exports:[
    RouterModule
  ]
})
export class AccountRoutingModule { }
