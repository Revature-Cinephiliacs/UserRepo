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
  serverURL:string = "https://localhost:5001/";

  constructor(private http:HttpClient) { }

  createUser(newUser:NewUser){
    return this.http.post(this.serverURL + "user/", newUser);
  }

  getUser(email:string){
    return this.http.get<User>(this.serverURL +"user/" + email);
  }

  updateUser(userId: string, updatedUser: NewUser){
    return this.http.post(this.serverURL+ "user/update/" + userId, updatedUser);
  }

  deleteUser(userId: string){
    return this.http.delete(this.serverURL + "user/delete/" + userId);
  }

  makeUserAdmin(userId: string){
    return this.http.post(this.serverURL+ "user/addadmin/" + userId, null);
  }
}
