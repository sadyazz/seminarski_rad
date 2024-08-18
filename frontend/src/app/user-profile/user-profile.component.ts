import {Component, OnInit} from '@angular/core';
import {MyAuthService} from "../services/MyAuthService";
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../moj-config";
import {Observable} from "rxjs";

@Component({
  selector: 'user-profile',
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent implements OnInit{

  constructor(public myAuthService: MyAuthService, public router :Router, private httpClient: HttpClient) {
  }

  user: any;
  isEditing: boolean = false;

  toggleEditMode() {
    this.isEditing = !this.isEditing;
  }
  ngOnInit(): void {
    const userId = this.myAuthService.returnId();
    if (userId) {
      this.getUserData(userId).subscribe(data => {
        this.user = data;
        this.tempUser = { ...data };
        console.log(this.user);
      }, error => {
        console.error('Error fetching user data', error);
      });
    } else {
      console.error('User ID not found');
    }
    }

  getUserData(id: number): Observable<any> {
    const userId = this.myAuthService.returnId();
    let url = MojConfig.adresa_servera+`/GetUserById?id=${userId}`;
    if (userId) {
      return this.httpClient.get<any>(url);
    }
    throw new Error('User ID not found');
  }

  logOut() {

    let token = window.localStorage.getItem("my-auth-token")??"";
    window.localStorage.setItem("my-auth-token","");

    let url=MojConfig.adresa_servera+`/Autentifikacija/logout`
    this.httpClient.post(url, {}, {
      headers:{
        "my-auth-token": token
      }
    }).subscribe(x=>{
      console.log("logout uspjesan")
    })

    this.router.navigate(["/login"])
  }
  nazad(){
    this.router.navigate(["/home"]);
  }
  tempUser: any;
  saveChanges() {
    const userId = this.myAuthService.returnId();
    console.log("USER ID: ", userId);
    const url = MojConfig.adresa_servera + `/EditUser?id=${userId}`;

    this.httpClient.put(url, this.user).subscribe(
      (response: any) => {
        console.log('User updated successfully');
      },
      (error) => {
        console.error('Error updating user data', error);
      }
    );
  }
}
