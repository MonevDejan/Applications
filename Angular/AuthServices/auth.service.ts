import { Injectable } from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import { environment } from '../../environments/environment';

import { JwtHelperService } from '@auth0/angular-jwt';
import {map} from 'rxjs/operators';
import {Router} from '@angular/router';
import { RoleTypes } from './role.enum';
import { BehaviorSubject } from 'rxjs';
import * as jwt_decode from 'jwt-decode';

const httpOptions = {
    headers: new HttpHeaders({
        'Content-Type':  'application/json'
    })
};

@Injectable()
export class AuthService
{

    public isAdmin: boolean;
    public userid: string;


    constructor(private _http: HttpClient,
                private router: Router,
                public jwtHelper: JwtHelperService) {
    }

    login(email: string, password: string)
    {
        this.isAdmin = false;
        this.userid = null;

        localStorage.setItem('currentSelectedMonth', '');
        localStorage.setItem('currentSelectedSchedule', '');

        const commitUser = {
            username: email,
            password: password,
        };

        return this._http.post('/api/auth/login', JSON.stringify(commitUser), httpOptions).pipe(
            map(value => {
                const currentToken = this.jwtHelper.decodeToken(value['token']);
                //console.log("currentToken", currentToken );
                //this.setToken(

                localStorage.setItem('jwt', value['token']);


                localStorage.setItem('currentUser', JSON.stringify({
                    rol: currentToken['rol'],
                    firstname: currentToken['firstname'],
                    lastname: currentToken['lastname'],
                    photo: currentToken['photo'],
                    id: currentToken["id"]
                }));

                if (currentToken['rol'] == "Admin") {
                    this.isAdmin = true;
                }

                //this.loggedInUser.isLoggedIn = true;
                this.userid = currentToken["id"];
                //this.loggedInUser.userRole = currentToken['rol'];


                return currentToken['rol'];
            })
        );

    }



    //setToken(key: string , val: string | object) {
    //    if (typeof val === 'string') {
    //        localStorage.setItem(key, val);
    //    }
    //    localStorage.setItem(key, JSON.stringify(val));

    //}

    logout() {
        localStorage.removeItem('jwt');
        localStorage.removeItem('currentUser');
        localStorage.setItem('currentSelectedMonth', '');
        localStorage.setItem('currentSelectedSchedule', '');
        //localStorage.setItem('currentSelectedMonthSchedule', '');
        

        //console.log('logout fired');
        //this.loggedInUser.isLoggedIn = false;
        //this.loggedInUser.userRole = null;
        //this.loggedInUser.id = 0;

        this.router.navigate(['login']);
        return false;
    }


    getToken(): string {
        return localStorage.getItem("jwt");
    }



    getTokenExpirationDate(token: string): Date {
        const decoded = jwt_decode(token);

        if (decoded.exp === undefined) return null;

        const date = new Date(0);
        date.setUTCSeconds(decoded.exp);
        return date;
    }

    isTokenExpired(token?: string): boolean {
        if (!token) token = this.getToken();
        if (!token) return true;

        const date = this.getTokenExpirationDate(token);
        if (date === undefined) return false;
        return !(date.valueOf() > new Date().valueOf());
    }


}
