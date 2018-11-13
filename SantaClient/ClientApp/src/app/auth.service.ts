import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import * as jwt_decode from "jwt-decode";
import { User } from './user';


@Injectable()

export class AuthService {
  token: string;
  role: string;
  user: boolean;
  uid: string;

  API = 'https://localhost:44321/api/';
  roleId = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

  constructor(private http: HttpClient,
    private router: Router) {
    if (localStorage.getItem('token') != null) {
      this.token = localStorage.getItem('token');
      this.role = jwt_decode(this.token)[this.roleId];
      this.user = true;
      this.uid = localStorage.getItem('uid');
    }
  }

  login(email: string, password: string): Promise<void> {
    return new Promise<void>(resolve => {
      let data = {
        "Username": email,
        "Password": password
      };
      var config = {
        headers: {
          "Content-Type": "application/json; charset = utf-8;"
        }
      };
      this.http.post(this.API + 'auth/login', data, config)
        .subscribe(
          (res) => {
            localStorage.setItem("token", res['token']);
            localStorage.setItem("uid", res['id']);

            //console.log(jwt_decode(this.token));
            console.log(res);
            this.uid = res['id'];
            this.token = res['token'];
            this.role = jwt_decode(this.token)[this.roleId];
            this.user = true;
            if (this.role == "Admin") {
              this.router.navigate(['./kids']);
            } else {
              this.router.navigate(['./home']);
            }
          },
          err => {
            console.log(err);
          }
        );
    });
  }

  register(user: User) {
    return new Promise<void>(resolve => {
      let data = {
        "Email": user.email,
        "Password": user.password,
        "FirstName": user.fname,
        "LastName": user.lname,
        "BirthDate": user.bd,
        "Street": user.street,
        "City": user.city,
        "Province": user.province,
        "PostalCode": user.postal,
        "Country": user.country,
        "Latitude": user.lat,
        "Longitude": user.lon
      };
      const body = JSON.stringify(data);
      var config = {
        headers: {
          "Content-Type": "application/json; charset = utf-8;"
        }
      };
      this.http.post(this.API + 'auth/register', data, config)
        .subscribe(
        (res) => {
          console.log("user registered");
            this.login(user.email, user.password);
            //this.registerSuccess = true;
          },
          err => {
            console.log(err);
            //finish loading
          }
        );
    })
  }

}
