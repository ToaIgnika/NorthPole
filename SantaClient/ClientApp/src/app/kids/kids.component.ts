import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service'
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-kids',
  templateUrl: './kids.component.html',
  styleUrls: ['./kids.component.css']
})
export class KidsComponent implements OnInit {
  API = 'https://localhost:44321/api/list';
  items: any;

  constructor(private router: Router,
    private auth: AuthService,
    private http: HttpClient) {
    this.loadKids();
  }

  ngOnInit() {
  }
  loadKids() {
    var config = {
      headers: {
        "Content-Type": "application/json; charset = utf-8;",
        "Authorization": "Bearer " + this.auth.token
      }
    };
    this.http.get(this.API, config)
      .subscribe(
        (res) => {
          console.log(res);
          this.items = res;
          //this.router.navigate(['./dashboard']);
          //this.registerSuccess = true;
        },
        err => {
          console.log(err);
          //finish loading
        }
      );
  }

  delete(id) {
    if (confirm("Are you sure to delete contact information?")) {
      var config = {
        headers: {
          "Content-Type": "application/json; charset = utf-8;",
          "Authorization": "Bearer " + this.auth.token
        }
      };
      this.http.delete(this.API + '/' + id, config)
        .subscribe(
          (res) => {
            console.log(res);
          },
          err => {
            console.log(err);
          }
        );
    }
  }
}
