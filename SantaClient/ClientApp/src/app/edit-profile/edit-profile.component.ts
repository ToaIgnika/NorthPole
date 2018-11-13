import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../auth.service'

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {
  API = 'https://localhost:44321/api/list';
  private user: any = {
    email: "",
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
  private data: any;

  constructor(private http: HttpClient,
    private route: ActivatedRoute,
    private router: Router, private auth: AuthService) {
    this.load();
  }

  ngOnInit() {
  }
  load() {
    var config = {
      headers: {
        "Content-Type": "application/json; charset = utf-8;",
        "Authorization": "Bearer " + this.auth.token
      }
    };
    console.log(this.auth.uid);
    this.http.get(this.API + '/c/' + this.auth.uid, config)
      .subscribe(
        (res) => {
          console.log(res);
          this.data = res;
          this.user.email = this.data.userName;
          this.user.fname = this.data.firstName;
          this.user.lname = this.data.lastName;
          this.user.bd = this.data.birthDate;
          this.user.street = this.data.street;
          this.user.city = this.data.city;
          this.user.province = this.data.province;
          this.user.postal = this.data.postalCode;
          this.user.country = this.data.country;
          this.user.lat = this.data.latitude;
          this.user.lon = this.data.longitude;
          //this.router.navigate(['./dashboard']);
          //this.registerSuccess = true;
        },
        err => {
          console.log(err);
          //finish loading
        }
      );
  }

  update() {
    let data = {
      "FirstName": this.user.fname,
      "LastName": this.user.lname,
      "BirthDate": this.user.bd,
      "Street": this.user.street,
      "City": this.user.city,
      "Province": this.user.province,
      "PostalCode": this.user.postal,
      "Country": this.user.country,
      "Latitude": this.user.lat,
      "Longitude": this.user.lon,
    };
    const body = JSON.stringify(data);
    var config = {
      headers: {
        "Content-Type": "application/json; charset = utf-8;",
        "Authorization": "Bearer " + this.auth.token
      }
    };
    this.http.put(this.API + '/c/' + this.auth.id, data, config)
      .subscribe(
        (res) => {
          console.log('information updated.');
          window.location.reload();
          //this.registerSuccess = true;
        },
        err => {
          console.log(err);
          //finish loading
        }
      );
  }
}

