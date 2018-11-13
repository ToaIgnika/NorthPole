import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service'

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  constructor(private router: Router,
    private auth: AuthService) {
  }


  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    localStorage.removeItem('token');
    this.auth.user = false;
    this.router.navigate(['./home']);
  }
}
