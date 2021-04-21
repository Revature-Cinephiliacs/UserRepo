import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { NewUser, User } from './models'

@Injectable({
  providedIn: 'root'
})
export class UserService {

  currentUser:string ="";
  askingUser:string = "";
  connection:string ="";
  loggedIn:any;
  serverURL:string = "https://localhost:5000/";

  constructor(private http:HttpClient) { }

  createUser(newUser:NewUser){
    console.log(newUser);
    return this.http.post(this.serverURL + "user/", JSON.stringify(newUser));
  }

  getUser(email:string){
    return this.http.get<User>(this.serverURL +"user/user/" + email);
  }

  updateUser(userId: string, updatedUser: NewUser){
    console.log(updatedUser);
    return this.http.post(this.serverURL+ "user/update/" + userId, JSON.stringify(updatedUser));
  }

  deleteUser(userId: string){
    return this.http.delete(this.serverURL + "user/user/delete/" + userId);
  }

  makeUserAdmin(userId: string){
    return this.http.post(this.serverURL+ "user/user/addadmin/" + userId, null);
  }
}
