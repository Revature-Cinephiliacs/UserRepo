import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { UserService } from '../user.service';
import { User } from '../models';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  @Input() currentUser: User = {
    username:'',
    firstname:'',
    lastname:'',
    email:'',
    permissions:1
  }
  @Output() currentUserChange = new EventEmitter<User>();

  userName: string = "";
  password: string = "";
  passwordNotOk: any = false;

  isLoginPage: boolean = true;

  newUser: any = {
    username:'',
    firstname:'',
    lastname:'',
    email:'',
    permissions:1
  }

  constructor(private _login: UserService) { }

  ngOnInit(): void {
  }

  login(){
    console.log("Login attempt" + this.userName);
    this._login.loginUser(this.userName).subscribe((data: User) => {
      console.log(data);
      if (data.lastname == this.password)
      {
        this.passwordNotOk = false;
        this.currentUser = data;
      }
      else {
        this.passwordNotOk = true;
        setTimeout(() => { 
          this.passwordNotOk = false;
        }, 3000);
      }
      console.log(this.currentUser.username);
      this.currentUserChange.emit(this.currentUser);
      localStorage.setItem("loggedin",JSON.stringify(this.currentUser));
      return data;
    });
  }

  createUser() {
    console.log("In Create");
    if(!this.newUser.firstname || !this.newUser.lastname || !this.newUser.username ||!this.newUser.email)
    {
      console.log("Please fill in all data")
    }
    else
    {
      console.log(JSON.stringify(this.newUser));
      this._login.createUser(this.newUser).subscribe(data => {
        console.log(data);
        this.currentUser = this.newUser;
        this.userName = this.currentUser.username;
        this.password = this.currentUser.lastname;
        this.login();
      });
    }
  }

  getUserName(){
    console.log(this.userName);
    return this.userName;
  }
  

  isPasswordRigt(pass:string){
    console.log("Checking");
    return (pass == this.password);
  }

  switchToRegister(): void {
    this.isLoginPage = false;
  }

  backToLogin(): void {
    this.isLoginPage = true;
  }
}
