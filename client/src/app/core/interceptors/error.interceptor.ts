import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private route:Router,private toastr:ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler,): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error=>{
        if(error){
          if(error.status===400)
          {
            this.toastr.error(error.error.message,error.error.statusCode)
          }
          if(error.status===401)
          {
            this.toastr.error(error.error.message,error.error.statusCode)
          }
          if(error.status===404)
          {
            this.route.navigateByUrl('/not-found');
          }
          if(error.status===500){
            this.route.navigateByUrl('/server-error')
          }
        }
        return throwError(error)
      })
    );
  }
}
