import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {AuthLoginRequest} from "./authLoginRequest";
import {MojConfig} from "../moj-config";
import {HttpClient} from "@angular/common/http";
import {AuthLoginResponse} from "./authLoginResponse";
import {Router} from "@angular/router";
import {MyAuthService} from "../services/MyAuthService";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";
import {TwofaPageComponent} from "../twofa-page/twofa-page.component";
import {AuthRegisterRequest} from "./RegisterRequest";

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

  public registerRequest: AuthRegisterRequest = {
    name:  "",
    surname: "",
    username:"",
    email:  "",
    phone:"",
    password: "",
    ponoviLozinku:  "",
    dateOfBirth:  ""
  };
  constructor(
    public httpClient: HttpClient,
    private router: Router,
    private myAuthService: MyAuthService,
    public dialog:MatDialog
    ) {
  }
  hidePassword1 = true;
  hidePassword2 = true;
  showSuccessAlert = false;

  @ViewChild('tabRegister') tabRegister: ElementRef | undefined;
  @ViewChild('tabLogin') tabLogin: ElementRef | undefined;

  ngOnInit(){

  }

  register() {
    if (this.registerRequest.password !== this.registerRequest.ponoviLozinku) {
      alert("Passwords do not match!");
      return;
    }
console.log('register request -> ',this.registerRequest);
    let url = MojConfig.adresa_servera + "/Autentifikacija/register";
    this.httpClient.post(url, this.registerRequest).subscribe(() => {
      this.switchToLoginTab();
      this.showSuccessAlert = true;
      setTimeout(() => this.showSuccessAlert = false, 3000);
      this.resetRegisterForm();
    }, (error) => {
      alert("Registration failed: " + error.error.message);
    });
  }

  resetRegisterForm() {
    this.registerRequest = {
      name: "",
      surname: "",
      username: "",
      email: "",
      phone: "",
      password: "",
      ponoviLozinku: "",
      dateOfBirth: ""
    };
  }
  switchToLoginTab() {
    if (this.tabLogin && this.tabLogin.nativeElement) {
      this.tabLogin.nativeElement.click();
    }
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
          this.open2Fa();
        }else {
          this.router.navigate(["/home"])
        }
      }
    })
  }


  hideAlert(): void {
    this.showSuccessAlert = false;
  }
}
