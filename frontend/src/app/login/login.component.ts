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
  open2Fa(targetRoute: string) {
    const dialogRef = this.dialog.open(TwofaPageComponent, {
      width: '20rem',
      height: '15rem',
      data: { targetRoute: targetRoute }  // Pass target route to the dialog
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // After 2FA verification, navigate to the target route
        this.router.navigate([result]);
      }
    });
  }

  signIn() {
    let url = MojConfig.adresa_servera + "/Autentifikacija/login";
    this.httpClient.post<AuthLoginResponse>(url, this.loginRequest).subscribe((x) => {
      if (!x.isLogiran) {
        alert("Incorrect username/password");
      } else {
        this.myAuthService.setLogiraniKorisnik(x.autentifikacijaToken);
  
        let targetRoute = x.isAdmin ? '/admin' : '/home';  // Store the target route based on the admin status
  
        if (this.myAuthService.is2FActive()) {
          // Open 2FA dialog and pass the target route as data
          this.open2Fa(targetRoute);
        } else {
          // If 2FA is not active, just redirect to the target route directly
          this.router.navigate([targetRoute]);
        }
      }
    });
  }
  


  hideAlert(): void {
    this.showSuccessAlert = false;
  }
}
