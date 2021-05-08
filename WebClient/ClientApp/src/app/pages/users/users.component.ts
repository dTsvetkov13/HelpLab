import { Component, OnInit } from '@angular/core';

import { Post } from '../../models/post';
import { Answer } from '../../models/answer';
import { WebRequestsService } from '../../services/web-requests.service';
import { HttpResponse } from '@angular/common/http';
import { User } from '../../models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  isAnswersButtonPressed: boolean = false;
  isPostsButtonPressed: boolean = false;
  isUserLoaded: boolean = false;
  posts: Post[] = [];
  answers: Answer[] = [];
  user: User;

  constructor(private webRequest: WebRequestsService, private router: Router) { }

  ngOnInit() {
    this.isPostsButtonPressed = true;

    const url = window.location.href;

    const params = url.split("/");

    let id = params[params.length - 1];
    let data;

    if (id == "me") {
      this.webRequest.getAuthorized("api/users").subscribe((res: HttpResponse<any>) => {
        console.log("Success " + res.status);
        data = JSON.parse(res.body);
        console.log(data);
        this.user = new User(data["id"], data["name"], data["surname"], parseInt(data["answersCount"]), parseInt(data["postsCount"]))
        this.isUserLoaded = true;

        console.log("User id: " + this.user.id);

        this.loadPosts();
      },
        error => {
          console.log("oops", error["status"]);

          if(error["status"] == 401) {
            this.router.navigateByUrl("login");
          }

          //Handle server error
        });
    }
    else {
      this.webRequest.get("api/users/" + id).subscribe((res: HttpResponse<any>) => {
        console.log("Success " + res.status);
        data = JSON.parse(res.body);
        console.log(data);
        this.user = new User(data["id"], data["name"], data["surname"], parseInt(data["answersCount"]), parseInt(data["postsCount"]))
        this.isUserLoaded = true;
      },
        error => {
          console.log("oops", error["status"]);

          //Handle bad request
          //Handle server error
        });
    }

    this.answers[0] = new Answer("", "Title", "12.23.2001", 1, null, null);
  }

  loadPosts() {
    this.webRequest.get("api/posts?UserId=" + this.user.id).subscribe((res: HttpResponse<any>) => {
      let data = JSON.parse(res.body);

      this.posts = [];
      for(let i = 0; i < data.length; i++) {
        this.posts[i] = new Post(data[i]["id"], data[i]["title"], data[i]["description"],
                                 data[i]["publishedAt"], data[i]["answersCount"], [],
                                 new User("", data[i]["authorName"], "", 0, 0))
      }

      console.log(data);
    },
      error => {
        console.log("oops", error["status"]);

        //Handle bad request
      });
  }

  loadAnswers() {
    this.webRequest.get("api/answers?userId=" + this.user.id).subscribe((res: HttpResponse<any>) => {
      console.log("Success " + res.status);
      let data = JSON.parse(res.body);
      console.log(data);
    },
      error => {
        console.log("oops", error["status"]);

        //Handle bad request
        //Handle server error
      });
  }

  showAnswers() {
    this.isPostsButtonPressed = false;
    this.isAnswersButtonPressed = true;
  }

  showPosts() {
    this.isPostsButtonPressed = true;
    this.isAnswersButtonPressed = false;
  }

  readMore(postId: string) {
    console.log(postId)
    this.router.navigateByUrl("posts/id/" + postId);
  }
}
