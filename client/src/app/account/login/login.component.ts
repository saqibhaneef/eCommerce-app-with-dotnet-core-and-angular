import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../account.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent{
returnUrl:string;
  loginForm=new FormGroup({
    email:new FormControl('',[Validators.required,Validators.email]),
    password:new FormControl('',Validators.required)
  })

  constructor(private accountService:AccountService,private route:Router,private activatedRoute:ActivatedRoute) { 
    this.returnUrl=activatedRoute.snapshot.queryParams['returnUrl'] || '/shop'
  }

  ngOnInit(): void {
  }

  onSubmit(){
    if(this.loginForm.valid){
      this.accountService.login(this.loginForm.value).subscribe({
        next:user=>{
          this.route.navigateByUrl(this.returnUrl)        
        }  
      });
    }
  }

}
