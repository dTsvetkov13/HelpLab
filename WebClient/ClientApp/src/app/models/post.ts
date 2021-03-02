export class Post {

    constructor(
      public id: string,
      public title: string,
      public description: string,
      public publishedAt: string,
      public answersCount: number,
      public categories: string[]
    ) {  }
  
  }