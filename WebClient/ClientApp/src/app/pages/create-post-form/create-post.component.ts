import { Component, OnInit } from '@angular/core';

import { Post } from '../../models/post';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css']
})
export class CreatePostComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  model = new Post("Question", "Description")

  onSubmit() {
    console.log(this.model.question);
  }

}
