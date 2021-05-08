import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class ErrorHandler {

    constructor(private router: Router) {

    }

    handle(error: number) {
        if(error == 401) {
            this.router.navigateByUrl("/login");
        }
        if(error == 400) {
            
        }
    }
}