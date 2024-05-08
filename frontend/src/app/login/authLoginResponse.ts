import {AutentifikacijaToken} from "../../helpers/auth/autentifikacijaToken";

export interface AuthLoginResponse{
  autentifikacijaToken: AutentifikacijaToken
  isLogiran: boolean
}



