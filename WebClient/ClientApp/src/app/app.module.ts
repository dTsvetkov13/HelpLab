import { BrowserModule } from '@angular/platform-browser';
import { LOCALE_ID, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './pages/home/home.component';
import { PostComponent } from './pages/post/post.component';
import { CreatePostComponent } from './pages/create-post-form/create-post.component';
import { LoginComponent } from './pages/login/login.component';
import { SignUpComponent } from './pages/sign-up/sign-up.component';
import { UsersComponent } from './pages/users/users.component';
import { MatInputModule } from '@angular/material/input';

import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { FlexLayoutModule } from '@angular/flex-layout';
import { NgbDropdown } from '@ng-bootstrap/ng-bootstrap';
import { LatestPostsComponent } from './pages/latest-posts/latest-posts.component';
import Swal from 'sweetalert2';
import { RoutingModule } from './routing/routing.module';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    PostComponent,
    CreatePostComponent,
    LoginComponent,
    SignUpComponent,
    UsersComponent,
    LatestPostsComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RoutingModule,
    MatButtonModule,
    NgbModule,
    RoutingModule,
    MatSelectModule,
    BrowserAnimationsModule,
    CKEditorModule,
    MatDialogModule,
    MatDividerModule,
    MatInputModule,
  ],
  providers: [ { provide: LOCALE_ID, useValue: ['bg', "en"] }],
  bootstrap: [AppComponent]
})
export class AppModule { }
