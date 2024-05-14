import {Component, OnInit} from '@angular/core';
import {MyAuthService} from "../services/MyAuthService";
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {GetUserInfo, GetUserResponse} from "./get-user-info";

@Component({
  selector: 'user-profile',
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent implements OnInit{

  constructor(private myAuthService: MyAuthService, public router :Router, private httpClient: HttpClient) {
  }


  ngOnInit() {


  }

}
