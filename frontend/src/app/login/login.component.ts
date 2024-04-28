import {Component, OnInit} from '@angular/core';
import {AuthLoginRequest} from "./authLoginRequest";
import {MojConfig} from "../moj-config";
import {HttpClient} from "@angular/common/http";
import {AuthLoginResponse} from "./authLoginResponse";
import {Router} from "@angular/router";

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit{

  public loginRequest: AuthLoginRequest = {
    lozinka : "",
    korisnickoIme:""
  };
  constructor(public httpClient:HttpClient, private router: Router) {
  }

  ngOnInit(){

  }

  signIn(){
   let url = MojConfig.adresa_servera + "/Autentifikacija/login";
    this.httpClient.post<AuthLoginResponse>(url, this.loginRequest).subscribe((x)=>{
      if(!x.isLogiran){
        alert("pogresan username / password")
      }else{
        let token=x.autentifikacijaToken.vrijednost;
        window.localStorage.setItem("my-auth-token", token);
        this.router.navigate(["/home"])
      }
    })
  }
}
