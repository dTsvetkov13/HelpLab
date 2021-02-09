import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class WebRequestsService {

  readonly ROOT_URL;

  constructor(private http: HttpClient) {
    this.ROOT_URL = "https://localhost:44395";
  }

  get(url: string) {
    return this.http.get(`${this.ROOT_URL}/${url}`, { observe: 'response', responseType: 'text' });
  }
  
  //Authorized post
  post(url: string, payload: object) {
    return this.http.post(`${this.ROOT_URL}/${url}`, payload, { headers: {"Content-Type": "application/json"}, observe: 'response', responseType: 'text' });
  }
}
