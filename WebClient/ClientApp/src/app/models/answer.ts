import { Post } from "./post";

export class Answer {
    
    constructor(
        public text: string,
        public publishedAt: string,
        public answersCount: number,
        public post: Post
      ) {  }
}