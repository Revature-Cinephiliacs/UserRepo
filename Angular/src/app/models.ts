export interface NewUser {
    username: string,
    firstname: string,
    lastname: string,
    email: string,
    dateofbirth: string
  }

  export interface User {
    userid: string,
    username: string,
    firstname: string,
    lastname: string,
    email: string,
    dateofbirth: string,
    permissions: number
  }
