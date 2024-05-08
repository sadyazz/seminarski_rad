import {Component, OnInit} from '@angular/core';
import {AuthLoginRequest} from "./authLoginRequest";
import {MojConfig} from "../moj-config";
import {HttpClient} from "@angular/common/http";
import {AuthLoginResponse} from "./authLoginResponse";
import {Router} from "@angular/router";
import {MyAuthService} from "../services/MyAuthService";

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
  constructor(
    public httpClient: HttpClient,
    private router: Router,
    private myAuthService: MyAuthService
    ) {
  }

  ngOnInit(){

  }

  signIn(){
   let url = MojConfig.adresa_servera + "/Autentifikacija/login";
    this.httpClient.post<AuthLoginResponse>(url, this.loginRequest).subscribe((x)=>{
      if(!x.isLogiran){
        alert("pogresan username / password")
      }else{
        this.myAuthService.setLogiraniKorisnik(x.autentifikacijaToken);
        this.router.navigate(["/home"])
      }
    })
  }
}
