import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';
// import {Observable} from 'rxjs/Observable';
import {Observable} from 'rxjs/internal/Observable';
import {AuthService} from './auth.service';


import {RoleTypes} from './role.enum';

import {JwtHelperService} from '@auth0/angular-jwt';

@Injectable()
export class AuthGuardService implements CanActivate {

    constructor(private route: Router,
                private authService: AuthService,
                public jwtHelper: JwtHelperService) {
    }

    canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        // Get property name on security object to check
        // console.log('next', next);
        // console.log('state', state)
        const expectedRole: string = next.data['expectedRole'];

        const currentToken = localStorage.getItem('jwt');
        const currentUser = JSON.parse(localStorage.getItem('currentUser'));


        // console.log('ova e current toke', currentUser === null);
        // console.log('expectedRole', expectedRole);
        // console.log('currentToken', currentToken);


        if (currentToken === null || currentUser === null) {
            this.route.navigate(['login']);
            return false;
        }

        /*check if the roles match*/
        //if (RoleTypes[currentUser.userRole] !== expectedRole || this.jwtHelper.isTokenExpired(currentToken)) {
        //    this.route.navigate(['login']);
        //    return false;
        //}


        return true;
    }


    protected checkLogin(route?: ActivatedRouteSnapshot) {
        const roleMatch = true;
        if (route) {
            const expectedRole = route.data.expectedRole;
        }

        return true;
    }
}
