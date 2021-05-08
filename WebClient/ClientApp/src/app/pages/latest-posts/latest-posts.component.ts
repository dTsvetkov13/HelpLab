import { HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Post } from 'src/app/models/post';
import { User } from 'src/app/models/user';
import { WebRequestsService } from 'src/app/services/web-requests.service';

@Component({
  selector: 'app-latest-posts',
  templateUrl: './latest-posts.component.html',
  styleUrls: ['./latest-posts.component.css']
})
export class LatestPostsComponent implements OnInit {
  arePostsLoaded: boolean = false;
  posts: Post[];

  constructor(private webRequest: WebRequestsService, private router: Router) { }

  ngOnInit(): void {
    this.posts = []
    /*for(let i = 0; i < 10; i++) {
      this.posts[i] = new Post("id", "title", "description",
                                "publishedAt", 0, [],
                                new User("authorId", "authorName", "", 0, 0))
    }*/
    //this.arePostsLoaded = true;
    
    let data;

    this.webRequest.get("api/posts/latest").subscribe((res: HttpResponse<any>) => {
      console.log("Success " + res.status);
      data = JSON.parse(res.body);
      console.log(data);
      this.posts = [];
      for(let i = 0; i < data.length; i++) {
        this.posts[i] = new Post(data[i]["id"], data[i]["title"], data[i]["description"],
                                 data[i]["publishedAt"], data[i]["answersCount"], [],
                                 new User(data[i]["authorId"], data[i]["authorName"], "", 0, 0))
      }

      this.arePostsLoaded = true;
    },
    error => {
      console.log("oops", error["status"]);

      //Handle server error
    }); 
  }

  readMore(postId: string) {
    console.log(postId)
    this.router.navigateByUrl("posts/id/" + postId);
  }
}
