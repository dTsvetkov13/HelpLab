import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from '../pages/home/home.component';
import { PostComponent } from '../pages/post/post.component';
import { LatestPostsComponent } from '../pages/latest-posts/latest-posts.component';
import { LoginComponent } from '../pages/login/login.component';
import { CreatePostComponent } from '../pages/create-post-form/create-post.component';
import { SignUpComponent } from '../pages/sign-up/sign-up.component';
import { UsersComponent } from '../pages/users/users.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'home', component: HomeComponent, pathMatch: 'full' },
  { path: 'posts/id/:id', component: PostComponent, pathMatch: 'full' },
  { path: 'posts/latest', component: LatestPostsComponent, pathMatch: 'full' },
  { path: 'login', component: LoginComponent, pathMatch: 'full' },
  { path: 'posts/create', component: CreatePostComponent, pathMatch: 'full' },
  { path: 'signup', component: SignUpComponent, pathMatch: 'full' },
  { path: "users/:id", component: UsersComponent, pathMatch: "full" }
  ];


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })
  ],
  exports: [
    RouterModule
  ]
})
export class RoutingModule { }
