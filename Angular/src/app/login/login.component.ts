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

  newUser: NewUser = {
    username:'',
    firstname:'',
    lastname:'',
    email:'',
    dateofbirth:'',
    permissions:1
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

  constructor(private _login: UserService) { }

  ngOnInit(): void {
  }

  login(){
    console.log("Login attempt: " + this.email);
    this._login.getUser(this.email).subscribe((data: User) => {
      console.log(data);
      this.currentUser = data;
      this.loggedIn = true;
    });
  }

  createUser() {
    if(!this.newUser.firstname || !this.newUser.lastname || !this.newUser.username ||!this.newUser.email || !this.newUser.dateofbirth)
    {
      console.log("Please fill in all data")
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
    if(!this.newUser.firstname || !this.newUser.lastname || !this.newUser.username ||!this.newUser.email || !this.newUser.dateofbirth || this.email)
    {
      console.log("Please fill in all data")
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
      this.loggedIn = false;
    }
  }

  makeUserAdmin() {
    if(this.loggedIn)
    {
      this._login.makeUserAdmin(this.currentUser.userid).subscribe(data => {
        console.log(data);
      });
      this.loggedIn = false;
    }
  }
}
