import { Component, OnInit } from '@angular/core';
import {UserService} from './../Services/user.service';
import {User} from './../Models/models';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {

  allUsers: User[];
  number: number;

  constructor(private _userService: UserService) { }

  ngOnInit() {
     this._userService.getAll().subscribe(x => {
      this.allUsers = x;
      console.log(this.allUsers)
    });
  }
}

