import {Component, OnInit} from '@angular/core';
import {MyAuthService} from "../services/MyAuthService";
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../moj-config";

@Component({
  selector: 'user-profile',
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent implements OnInit{

  constructor(public myAuthService: MyAuthService, public router :Router, private httpClient: HttpClient) {
  }

  ngOnInit(): void {

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


}
