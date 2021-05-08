import { HttpResponse } from '@angular/common/http';
import { Component, OnInit, Inject } from '@angular/core';
import { WebRequestsService } from 'src/app/services/web-requests.service';
import { DialogMessageComponent } from 'src/app/pages/dialog-message/dialog-message.component';
import {MatDialog} from '@angular/material/dialog';

import { Post } from '../../models/post';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';

import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import * as BalloonEditor from '@ckeditor/ckeditor5-build-balloon';

import { ChangeEvent } from '@ckeditor/ckeditor5-angular';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.css']
})
export class CreatePostComponent implements OnInit {

  public Editor = BalloonEditor;

  public onChange( { editor }: ChangeEvent ) {
    const data = editor.getData();

    console.log( data );
  }

  constructor(private webRequest: WebRequestsService,
              private router: Router, public dialog: MatDialog) { }

  ngOnInit() {
  }

  model = new Post("", "Question", "Description", "", 0, [], null)

  onSubmit() {
    console.log(this.model.title + " " + this.model.description);
    let data;

    if(this.model.title == '' || this.model.title == 'Title'
       || this.model.description == '' || this.model.description == 'Description') {
      this.openDialog();
    }

    /*this.webRequest.postAuthorized("api/posts", {"Title": this.model.title, "Description": this.model.description})
    .subscribe((res: HttpResponse<any>) => {
      console.log("Success " + res.status);
      Swal.fire("Successfully created");
      this.router.navigateByUrl("/");
    },
    error => {
      console.log("oops", error["status"]);

      //Handle server error
    });*/
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(DialogMessageComponent, {
      width: '250px',
      data: {message: "Error"}
    });
  }

}