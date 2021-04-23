import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { UserService } from '../user.service';
import { NewUser, User } from '../models';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  email: string = "";

  loggedIn: any = false;

  isLoginPage: boolean = true;

  movieId: string = '';
  followingMovies: string[];

  newUser: NewUser = {
    username:'',
    firstname:'',
    lastname:'',
    email:'',
    dateofbirth:''
  }

  currentUser: User = {
    userid:'',
    username:'',
    firstname:'',
    lastname:'',
    email:'',
    dateofbirth:'',
    permissions:1
  }

  userAge: number;

  constructor(private _login: UserService) { }

  ngOnInit(): void {
  }

  login() {
    console.log("Login attempt: " + this.email);
    this._login.getUser(this.email).subscribe((data: User) => {
      console.log(data);
      this.currentUser = data;
      this.newUser.username = this.currentUser.username;
      this.newUser.firstname = this.currentUser.firstname;
      this.newUser.lastname = this.currentUser.lastname;
      this.newUser.email = this.currentUser.email;
      this.newUser.dateofbirth = this.currentUser.dateofbirth;
      this.loggedIn = true;
      this.populateFollowedMovies();
      this.populateUserAge();
    });
  }

  populateFollowedMovies() {
    this.followingMovies = null;
    this._login.getFollowedMovies(this.currentUser.userid).subscribe((data: string[]) => {
      console.log(data);
      this.followingMovies = data;
    });
  }

  populateUserAge() {
    this._login.getUserAge(this.currentUser.userid).subscribe((age: number) => {
      this.userAge = age;
    });
  }

  createUser() {
    if(!this.newUser.username || !this.newUser.firstname || !this.newUser.lastname || !this.newUser.email || !this.newUser.dateofbirth)
    {
      console.log("Please fill in all data");
    }
    else
    {
      console.log("NewUser:" + JSON.stringify(this.newUser));
      this._login.createUser(this.newUser).subscribe(data => {
        console.log(data);
      });
    }
  }

  updateUser() {
    if(!this.currentUser.userid || !this.newUser.username || !this.newUser.firstname || !this.newUser.lastname || !this.newUser.email || !this.newUser.dateofbirth)
    {
      console.log("Please fill in all data");
      console.log(this.newUser.username);
      console.log(this.newUser.firstname);
      console.log(this.newUser.lastname);
      console.log(this.newUser.email);
      console.log(this.newUser.dateofbirth);
    }
    else
    {
      console.log("UpdateUser:" + JSON.stringify(this.newUser));
      this._login.updateUser(this.currentUser.userid, this.newUser).subscribe(data => {
        console.log(data);
      });
    }
  }

  deleteUser() {
    if(this.loggedIn)
    {
      this._login.deleteUser(this.currentUser.userid).subscribe(data => {
        console.log(data);
      });
      this.logOut();
    }
  }

  makeUserAdmin() {
    if(this.loggedIn)
    {
      this._login.makeUserAdmin(this.currentUser.userid).subscribe(data => {
        console.log(data);
      });
      this.logOut();
    }
  }

  followMovie() {
    this._login.followMovie(this.currentUser.userid, this.movieId).subscribe(data => {
      this.populateFollowedMovies();
    });
  }

  logOut() {
    this.loggedIn = false;
    this.followingMovies = null;
  }
}
