import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service'

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  private user: any = {
    email: "",
    password: ""
  }

  constructor(private router: Router,
    private auth: AuthService) { }

  ngOnInit() {
  }

  loginUser() {
    console.log("loggin in...");
    this.auth.login(this.user.email, this.user.password);
  }

}
