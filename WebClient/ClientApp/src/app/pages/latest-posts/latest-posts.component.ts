import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { WebRequestsService } from 'src/app/services/web-requests.service';

@Component({
  selector: 'app-latest-posts',
  templateUrl: './latest-posts.component.html',
  styleUrls: ['./latest-posts.component.css']
})
export class LatestPostsComponent implements OnInit {

  constructor(private webRequest: WebRequestsService, private router: Router) { }

  ngOnInit(): void {
  }

}
