import { HttpResponse, HttpRequest } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { WebRequestsService } from 'src/app/services/web-requests.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  errors: string[] = [];

  constructor(private webRequest: WebRequestsService, private router: Router) {
  }

  ngOnInit() {
  }
  
  login(email: string, password: string) {
    console.log("Email: " + email + ", password: " + password);
    this.errors = [];
    this.webRequest.post('api/users/login', { email: email, password: password }).subscribe((res: HttpResponse<any>) => {
      console.log("Success login" + res.status);
      
      var data = JSON.parse(res.body);
      localStorage.setItem("access_token", data["accessToken"]);

      this.router.navigate(['/']);
    },
    error => {
      console.log("oops", error);

      if (error.status === 400) {
        let validationErrorDictionary = JSON.parse(error.error);

        for (var e in validationErrorDictionary["errors"]) {
          console.log(validationErrorDictionary["errors"][e]);

          let array = validationErrorDictionary["errors"][e];

          for (let i = 0; i < array.length; i++) {
            console.log(array[i]);
            this.errors.push(array[i]);
          }
        }
      } else {
        this.errors.push("something went wrong!");
      }
    });
  }
}
