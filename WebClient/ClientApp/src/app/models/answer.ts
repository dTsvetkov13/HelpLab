import { Post } from "./post";
import { User } from "./user";

export class Answer {
    
    constructor(
        public id: string,
        public text: string,
        public publishedAt: string,
        public answersCount: number,
        public post: Post,
        public author: User
      ) {  }
}