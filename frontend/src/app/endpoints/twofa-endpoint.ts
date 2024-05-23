import {BaseEndpoint} from "./base-endpoint";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {MojConfig} from "../moj-config";
import {Injectable} from "@angular/core";
@Injectable({providedIn: 'root'})
export class TfasGetallEndpoint implements BaseEndpoint<void, TfaGetAllResponse>{
  constructor(public httpClient:HttpClient) { }

  Handle(request: void): Observable<TfaGetAllResponse> {
    let url = MojConfig.adresa_servera + 'Tfa-GetAll';

    let token = window.localStorage.getItem("my-auth-token")??"";

    return this.httpClient.get<TfaGetAllResponse>(url , {
      headers:{
        "my-auth-token": token
      }
    });
  }
}
export interface TfaGetAllResponse{
  tfas : TfaGetAllResponseTfa[];
}

export interface TfaGetAllResponseTfa {
  id: number
  twoFKey: string
}
