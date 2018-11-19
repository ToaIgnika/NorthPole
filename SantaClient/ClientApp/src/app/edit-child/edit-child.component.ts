import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../auth.service'

declare const google: any;

@Component({
  selector: 'app-edit-child',
  templateUrl: './edit-child.component.html',
  styleUrls: ['./edit-child.component.css']
})
export class EditChildComponent implements OnInit {
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
    lon: "",
    isNaughty: false,
    dc: ""
  }

  private data: any;
  id: any;
  constructor(private http: HttpClient,
    private route: ActivatedRoute,
    private router: Router, private auth: AuthService) {
    this.id = '' + this.route.snapshot.paramMap.get('id');
    this.load();
  }

  ngOnInit() {
  
    
  }

  update() {
    let data = {
      "Street": this.user.street,
      "City": this.user.city,
      "Province": this.user.province,
      "PostalCode": this.user.postal,
      "Country": this.user.country,
      "Latitude": this.user.lat,
      "Longitude": this.user.lon,
      "isNaughty": this.user.isNaughty
    };
    const body = JSON.stringify(data);
    var config = {
      headers: {
        "Content-Type": "application/json; charset = utf-8;",
        "Authorization": "Bearer " + this.auth.token
      }
    };
    this.http.put(this.API + '/' + this.id, data, config)
      .subscribe(
      (res) => {
        console.log('information updated.');
        this.router.navigate(['./kids/' + this.id]);
        window.location.reload();
          //this.registerSuccess = true;
        },
        err => {
          console.log(err);
          //finish loading
        }
      );
  }

  load() {
    var config = {
      headers: {
        "Content-Type": "application/json; charset = utf-8;",
        "Authorization": "Bearer " + this.auth.token
      }
    };
    this.http.get(this.API + '/' + this.id, config)
      .subscribe(
        (res) => {
          console.log(res);
          this.data = res;
          this.user.email = this.data.userName;
          this.user.fname = this.data.firstName;
          this.user.lname = this.data.lastName;
          this.user.bd = this.data.birthDate.substring(0, 10);
          this.user.street = this.data.street;
          this.user.city = this.data.city;
          this.user.province = this.data.province;
          this.user.postal = this.data.postalCode; 
          this.user.country = this.data.country;
          this.user.lat = this.data.latitude;
          this.user.lon = this.data.longitude;
          this.user.isNaughty = this.data.isNaughty;
          //this.router.navigate(['./dashboard']);
          //this.registerSuccess = true;
          this.map(this.user.lat, this.user.lon);
          this.user.dc = this.data.dateCreated;
        },
        err => {
          console.log(err);
          //finish loading
        }
      );
  }

  map(lat, lon) {
    var myLatLng = { lat: lat, lng: lon };

    let mapProp = {
      center: new google.maps.LatLng(lat, lon),
      zoom: 5,
      mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    let map = new google.maps.Map(document.getElementById("map"), mapProp);

    let marker = new google.maps.Marker({
      position: myLatLng,
      map: map,
      title: 'Home'
    });
  }
}
