import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class WebRequestsService {

  readonly ROOT_URL;

  constructor(private http: HttpClient) {
    //this.ROOT_URL = "https://20.52.138.14:44330";
    this.ROOT_URL = "https://localhost:44330";
  }

  get(url: string) {
    return this.http.get(`${this.ROOT_URL}/${url}`, { observe: 'response', responseType: 'text' });
  }

  getWithHeaders(url: string, headers: any) {
    return this.http.get(`${this.ROOT_URL}/${url}`, { headers: {"Content-Type": "application/json"}, observe: 'response', responseType: 'text' });
  }

  getAuthorized(url: string) {
    return this.http.get(`${this.ROOT_URL}/${url}`, { headers: { 'Authorization': 'Bearer ' + localStorage.getItem('access_token') }, observe: 'response', responseType: 'text' });
  }
  
  post(url: string, payload: object) {
    return this.http.post(`${this.ROOT_URL}/${url}`, payload, { headers: {"Content-Type": "application/json"}, observe: 'response', responseType: 'text' });
  }

  postAuthorized(url: string, payload: object) {
    return this.http.post(`${this.ROOT_URL}/${url}`, payload, { headers: {"Content-Type": "application/json", 'Authorization': 'Bearer ' + localStorage.getItem('access_token')}, observe: 'response', responseType: 'text' });
  }
}
