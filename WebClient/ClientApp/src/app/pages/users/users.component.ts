import { Component, OnInit } from '@angular/core';

import { Post } from '../../models/post';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  isAnswersButtonPressed: boolean = false;
  isPostsButtonPressed: boolean = false;
  posts: Post[] = [];

  constructor() { }

  ngOnInit() {
    this.isPostsButtonPressed = true;

    this.posts[0] = new Post("Title", "Descrip", "12.3.2002", 2, [])
    this.posts[1] = new Post("Title", "Descrip", "12.3.2002", 2, [])

    const url = window.location.href;

    const params = url.split("/");

    console.log(params[params.length - 1]);
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
