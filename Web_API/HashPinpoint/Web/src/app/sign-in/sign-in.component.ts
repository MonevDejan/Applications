import { Component, OnInit } from '@angular/core';
import { UserCredentials } from './../models/article.model';
import { Router } from '@angular/router';
import { AuthService } from './../auth/auth.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {

  hide = true;

  nameFormApi: any;
  isLoading: boolean;
  loginMessage: string;
  email: string;
  password: string;

  constructor(private router: Router, private _authService: AuthService) { }

  ngOnInit() {

  }

  sendForm() {
    console.log(`button is clicked`);

    this.isLoading = true;
    this._authService.login(this.email, this.password)
      .subscribe(data => {

        this.router.navigate(['/articles']);

      }, loginError => {

        this.loginMessage = "InvalidUsernameOrPassword";
        this.isLoading = false;

      }
      );
  }
}
