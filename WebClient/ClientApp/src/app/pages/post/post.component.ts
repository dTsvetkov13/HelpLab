import { HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Post } from 'src/app/models/post';
import { User } from 'src/app/models/user';
import { WebRequestsService } from 'src/app/services/web-requests.service';

import * as BalloonEditor from '@ckeditor/ckeditor5-build-balloon';

import { ChangeEvent } from '@ckeditor/ckeditor5-angular';
import { Answer } from 'src/app/models/answer';
import { ErrorHandler } from 'src/app/services/error-handler';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnInit { 
  isPostLoaded: boolean = false;
  hasAnswers: boolean = false;
  post: Post;
  showAddAnswerField: boolean = false;
  answers: Answer[] = [];
  tempAnswer: String;

  public Editor = BalloonEditor;

  public onChange( { editor }: ChangeEvent ) {
    const data = editor.getData();

    this.tempAnswer = data;

    console.log("Data: " + data );
  }

  constructor(private webRequest: WebRequestsService, private router: Router,
              private activatedRoute: ActivatedRoute, private errorHandler: ErrorHandler) { }

  ngOnInit() {
    /*this.post = new Post("data", "title", "description",
                              "publishedAt", 0, [],
                              new User("authorId", "authorName", "authorSurname", 0, 0))
    this.isPostLoaded = true;
    this.hasAnswers = true;*/
    this.activatedRoute.params.subscribe(routeParams => {
      console.log("Id: " + routeParams.id);
      let id = routeParams.id;
      let data;
  
      //Get post by Id
      this.webRequest.get("api/posts/" + id).subscribe((res: HttpResponse<any>) => {
        console.log("Success " + res.status);
        data = JSON.parse(res.body);
        console.log(data);
        this.post = new Post(data["id"], data["title"], data["description"],
                              this.parseBackendDate(data["publishedAt"]), data["answersCount"], [],
                              new User(data["authorId"], data["authorName"], data["authorSurname"], 0, 0))
        this.isPostLoaded = true;

        console.log("call Load Answers()");
        this.LoadAnswers();
        console.log("return Load Answers()");

        if(data["answersCount"] > 0) {
          this.hasAnswers = true;
        }
        else {
          this.hasAnswers = false;
        }
      },
      error => {
        console.log("oops", error["status"]);
        console.log("oops", error);
        this.errorHandler.handle(error["status"]);
        //Handle server error
      }); 
    })
  }

  LoadAnswers() {
    console.log("In loadAnswers");
    if(this.post) {
      console.log("Make req");
      this.webRequest.get("api/answers/postId/" + this.post.id)
      .subscribe((res: HttpResponse<any>) => {
        console.log("Its ok" + res);
        if(res.status == 200) {
          this.answers = [];
          let data = JSON.parse(res.body);

          console.log(data);

          if(data.length > 0) {
            this.hasAnswers = true;
          }
          else {
            this.hasAnswers = false;
          }

          for(let i = 0; i < data.length; i++) {
            this.answers[i] = new Answer(data[i]["id"], data[i]["text"],
                                          this.parseBackendDate(data[i]["publishedAt"]), data[i]["answersCount"], this.post,
                                          new User(data[i]["authorId"], data[i]["authorName"], " " + data[i]["authorSurname"], 0, 0))
          }

          console.log("Loaded answers: " + this.answers[0].text);
        }

        console.log(res);
      },
      error => {
        console.log("oops", error["status"]);
        console.log("oops", error);
        this.errorHandler.handle(error["status"]);
        //Handle server error
      });
    }
  }

  ShowAddAnswerField() {
    this.showAddAnswerField = !this.showAddAnswerField;
  }

  SaveAnswer() {
    console.log("Answer: " + this.tempAnswer);

    this.webRequest.postAuthorized("api/answers", { text: this.tempAnswer, postId: this.post.id })
    .subscribe((res: HttpResponse<any>) => {
      if(res.status == 200) {
        this.tempAnswer = "";
        this.showAddAnswerField = false;
        this.LoadAnswers();
      }

      console.log(res);
    },
    error => {
      console.log("oops", error["status"]);
      console.log("oops", error);
      this.errorHandler.handle(error["status"]);
      //Handle server error
    });
  }

  parseBackendDate(date: string) {
    date = date.replace('T', ' ');
    date = date.split('.')[0];
    return date;
  }
}