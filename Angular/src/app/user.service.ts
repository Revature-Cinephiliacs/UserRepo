import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from './models'

@Injectable({
  providedIn: 'root'
})
export class UserService {

  currentUser:string ="";
  askingUser:string = "";
  connection:string ="";
  loggedIn:any;
  baseURL:string = " ";

  constructor(private http:HttpClient) { }

  createUser(newUser:string){
    console.log(newUser);
    return this.http.post(this.baseURL+ "user/",newUser);
  }

  loginUser(userName:string){

    this.connection =  this.baseURL +"user/" + userName;
    console.log(this.connection);
    return this.http.get<User>(this.connection);
  }


  
}
