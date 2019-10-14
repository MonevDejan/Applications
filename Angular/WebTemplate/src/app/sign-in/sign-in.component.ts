import { Component, OnInit } from '@angular/core';
import {UserCredentials} from './../Models/models';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {

  hide = true;
  person: UserCredentials = {
    password: '',
    userName: ''
  };

  constructor() { }
  ngOnInit() {

  }

  sendForm() {
    console.log(`button is clicked ${this.person.userName}`);
  }
}
