import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/internal/Observable';
import { AuthService } from './auth.service';



import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AuthGuardService implements CanActivate {

  constructor(private route: Router,
    private authService: AuthService,
    public jwtHelper: JwtHelperService) {
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {

    const expectedRole: string = next.data['expectedRole'];

    const currentToken = localStorage.getItem('jwt');
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));

    console.log("can activate");

    if (currentToken === null || currentUser === null) {
      this.route.navigate(['/auth/login']);
      return false;
    }

    return true;
  }


  protected checkLogin(route?: ActivatedRouteSnapshot) {
    console.log("check login");
    const roleMatch = true;
    if (route) {
      const expectedRole = route.data.expectedRole;
    }

    return true;
  }
}
