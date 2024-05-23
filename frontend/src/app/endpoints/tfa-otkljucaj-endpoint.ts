import {Injectable} from "@angular/core";
import {BaseEndpoint} from "./base-endpoint";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {MojConfig} from "../moj-config";

@Injectable({providedIn: 'root'})
export class AutentifikacijaTwoFOtkljucajEndpoint implements  BaseEndpoint<AutentifikacijaTwoFOtkljucajRequest, void>{
  constructor(public httpClient:HttpClient) { }
  Handle(request: AutentifikacijaTwoFOtkljucajRequest): Observable<void> {
    let url=MojConfig.adresa_servera + `Autentifikacija/2f-otkljucaj`;

    return this.httpClient.post<void>(url, request);
  }
}
export interface AutentifikacijaTwoFOtkljucajRequest {
  kljuc:string
}
