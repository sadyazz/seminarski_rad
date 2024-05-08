import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {AutentifikacijaToken} from "../../helpers/auth/autentifikacijaToken";


@Injectable({providedIn: 'root'})
export class MyAuthService{
  constructor(private httpClient: HttpClient) {
  }
  jelLogiran():boolean{
    return this.getAuthorizationToken() != null;
  }
  getAuthorizationToken(): AutentifikacijaToken | null{
    let tokenString = window.localStorage.getItem("my-auth-token")??"";
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

 /* returnId() {
    return this.getAuthorizationToken()?.korisnickiNalog.id;
  }*/
  setLogiraniKorisnik(x: AutentifikacijaToken | null) {
    if(x == null){
      window.localStorage.setItem("my-auth-token", '');
    }else{
      window.localStorage.setItem("my-auth-token", JSON.stringify(x));
    }

  }
}
