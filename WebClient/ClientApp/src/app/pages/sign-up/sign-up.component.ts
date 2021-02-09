import { HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { WebRequestsService } from 'src/app/services/web-requests.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})

export class SignUpComponent implements OnInit {
  errors: string[] = [];

  constructor(private webRequest: WebRequestsService, private router: Router) {
  }

  ngOnInit() {
  }

  signUp(name: string, surname: string,
         email: string, password: string) {
    console.log("Name: " + name + ", surname: " + surname + ", email: " + email + ", password: " + password);
    this.webRequest.post('api/users', { name: name, surname: surname,
                                        email: email, password: password }).subscribe((res: HttpResponse<any>) => {
      console.log("Success sign up" + res.status);
      
      var data = JSON.parse(res.body);

      this.router.navigate(['/login']);
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
    }
    )
  }
}
