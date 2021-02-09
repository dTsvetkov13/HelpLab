import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Search } from '../models/search';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})

export class NavMenuComponent {
  constructor(private router: Router) { }

  showCategoriesStatement : boolean = false;
  searchObject = new Search();

  ngOnInit() {
  }

  search(query : string) {
    console.log(this.searchObject.subject);
    this.router.navigate([''], {queryParams: {s: query}});
  }

  showCategories() {
    this.showCategoriesStatement = !this.showCategoriesStatement;
  }

  redirectToHome() {
    this.router.navigateByUrl("/");
  }

  redirectToLogin() {
    this.router.navigateByUrl("/login");
  }

  redirectToSignUp() {
    this.router.navigateByUrl("/signup");
  }

  hasAccessToken() {
    console.log(localStorage.getItem("access_token"));
    
    if ((localStorage.getItem("access_token") === "undefined")
        || localStorage.getItem("access_token") === null) {
      return false;
    }
    else {
      return true;
    }
  }

  showAccessToken() {
    console.log(localStorage.getItem("access_token"));
    localStorage.removeItem("access_token");
  }
}
