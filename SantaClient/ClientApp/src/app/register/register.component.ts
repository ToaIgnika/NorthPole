import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service'
import { User } from '../user'

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  private user: User = {
    email: "",
    password: "",
    fname: "",
    lname: "",
    bd: "",
    street: "",
    city: "",
    province: "",
    postal: "",
    country: "",
    lat: "",
    lon: ""
  }

  constructor(private router: Router,
    private auth: AuthService) { }

  ngOnInit() {
  }

  registerUser() {
    console.log("registering user...");
    this.auth.register(this.user);
  }
}
