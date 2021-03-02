import { Component, OnInit } from '@angular/core';

import { Post } from '../../models/post';
import { Answer } from '../../models/answer';
import { WebRequestsService } from '../../services/web-requests.service';
import { HttpResponse } from '@angular/common/http';
import { User } from '../../models/user';

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

  constructor(private webRequest: WebRequestsService) { }

  ngOnInit() {
    this.isPostsButtonPressed = true;

    const url = window.location.href;

    const params = url.split("/");

    let id = params[params.length - 1];
    let data;

    if (id == "me") {
      this.webRequest.getAuthorized("api/users").subscribe((res: HttpResponse<any>) => {
        console.log("Success login" + res.status);
        data = JSON.parse(res.body);
        console.log(data);
        this.user = new User(data["name"], data["surname"], parseInt(data["answersCount"]), parseInt(data["postsCount"]))
        this.isUserLoaded = true;
      },
        error => {
          console.log("oops", error);

        });
    } else {
      this.webRequest.get("api/users/" + id).subscribe((res: HttpResponse<any>) => {
        console.log("Success login" + res.status);
        data = JSON.parse(res.body);
        console.log(data);
        this.user = new User(data["name"], data["surname"], parseInt(data["answersCount"]), parseInt(data["postsCount"]))
        this.isUserLoaded = true;
      },
        error => {
          console.log("oops", error);
        });
    }

    this.posts[0] = new Post("Title", "Descrip", "12.3.2002", 2, [])
    this.posts[1] = new Post("Title", "Descrip", "12.3.2002", 2, [])

    this.answers[0] = new Answer("Title", "12.23.2001", 1, null);
  }

    
  showAnswers() {
    this.isPostsButtonPressed = false;
    this.isAnswersButtonPressed = true;
  }

  showPosts() {
    this.isPostsButtonPressed = true;
    this.isAnswersButtonPressed = false;
  }
}
