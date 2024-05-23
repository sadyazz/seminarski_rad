import {Component, OnInit} from '@angular/core';
import {AuthLoginRequest} from "./authLoginRequest";
import {MojConfig} from "../moj-config";
import {HttpClient} from "@angular/common/http";
import {AuthLoginResponse} from "./authLoginResponse";
import {Router} from "@angular/router";
import {MyAuthService} from "../services/MyAuthService";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";
import {TwofaPageComponent} from "../twofa-page/twofa-page.component";

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
    private myAuthService: MyAuthService,
    public dialog:MatDialog
    ) {
  }

  ngOnInit(){

  }

  open2Fa(){
    const dialogRef = this.dialog.open(TwofaPageComponent,{width:'20rem',height:'15rem'});
  }

  signIn(){
   let url = MojConfig.adresa_servera + "/Autentifikacija/login";
    this.httpClient.post<AuthLoginResponse>(url, this.loginRequest).subscribe((x)=>{
      if(!x.isLogiran){
        alert("pogresan username / password")
      }else{
        this.myAuthService.setLogiraniKorisnik(x.autentifikacijaToken);

        if(this.myAuthService.is2FActive()){
          //this.router.navigate(["/twofaPage"])
          this.open2Fa();
        }else {
          this.router.navigate(["/home"])
        }


      }
    })
  }
}
