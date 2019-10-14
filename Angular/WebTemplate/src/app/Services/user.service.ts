import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { User } from '../Models/models';
import { Observable } from 'rxjs';


const httpOptions = {
  headers: new HttpHeaders({
      'Content-Type': 'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  getAll(): Observable<User[]> {
    // const response: any = this.http.get<any>('api/User/Get', httpOptions);\
    return this.http.get<User[]>('/api/user/get', httpOptions);
  }
}
