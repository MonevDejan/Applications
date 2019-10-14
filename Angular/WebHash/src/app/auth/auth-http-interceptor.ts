import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import {Observable} from 'rxjs/internal/Observable';
import { AuthService } from './auth.service';


@Injectable()
export class AuthHttpInterceptor implements HttpInterceptor {
  constructor(private _authService: AuthService, private router: Router) {}
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const jwt = localStorage.getItem('jwt');
    const authRequest = req.clone({ setHeaders: { authorization: `Bearer ${jwt}` } });

     return next.handle(authRequest).pipe();
  }
}
