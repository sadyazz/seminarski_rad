import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {AutentifikacijaToken} from "../../helpers/auth/autentifikacijaToken";
import { Router } from "@angular/router";

@Injectable({providedIn: 'root'})
export class MyAuthService{

  constructor(private httpClient: HttpClient, private router: Router) {
  }

  private getLocalStorageItem(key: string): string | null {
    if (typeof window !== 'undefined') {
      return window.localStorage.getItem(key) ?? null;
    }
    return null;
  }

  private setLocalStorageItem(key: string, value: string) {
    if (typeof window !== 'undefined') {
      window.localStorage.setItem(key, value);
    }
  }

  jelLogiran():boolean{
    return this.getAuthorizationToken() != null;
  }
  getAuthorizationToken(): AutentifikacijaToken | null{
    //let tokenString = window.localStorage.getItem("my-auth-token")??"";
    let tokenString = this.getLocalStorageItem("my-auth-token")??"";
    try {
      return JSON.parse(tokenString);
    }
    catch (e){
      return null;
    }
  }

  isAdmin():boolean{
    return this.getAuthorizationToken()?.korisnickiNalog.isAdmin ?? false
  }

  isUser():boolean{
    return this.getAuthorizationToken()?.korisnickiNalog.isUser ?? false
  }

  is2FActive() {
    return this.getAuthorizationToken()?.korisnickiNalog.is2FActive ?? false
  }

  returnId() {
    return this.getAuthorizationToken()?.korisnickiNalog.id;
  }

  setLogiraniKorisnik(x: AutentifikacijaToken | null) {
    if(x == null){
      //window.localStorage.setItem("my-auth-token", '');
      this.setLocalStorageItem("my-auth-token",'');
    }else{
      //window.localStorage.setItem("my-auth-token", JSON.stringify(x));
      this.setLocalStorageItem("my-auth-token", JSON.stringify(x));
    }

  }

  getUser(): any {
    const token = this.getAuthorizationToken();
    return token ? token.korisnickiNalog : null;
  }

  logout(): void {
    localStorage.removeItem('user'); // Brisanje korisničkih podataka iz localStorage-a
    this.router.navigate(['/login']); // Preusmeravanje na login stranicu
  }
  

}
