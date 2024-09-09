import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../moj-config";
import {Router} from "@angular/router";
import { MatDialogRef } from '@angular/material/dialog';
import {MyAuthService} from "../services/MyAuthService";
import { TfaGetAllResponse, TfaGetAllResponseTfa, TfasGetallEndpoint } from "../endpoints/twofa-endpoint";
import { AutentifikacijaTwoFOtkljucajEndpoint, AutentifikacijaTwoFOtkljucajRequest } from "../endpoints/tfa-otkljucaj-endpoint";

@Component({
  selector: 'twofa-page',
  templateUrl: './twofa-page.component.html',
  styleUrl: './twofa-page.component.css'
})
export class TwofaPageComponent implements OnInit{

constructor(private httpClient: HttpClient,
            private router: Router,
            public dialogRef: MatDialogRef<TwofaPageComponent>,
            private TfagetAllEndpoint: TfasGetallEndpoint,
            private myAuthService: MyAuthService,
            private AutentifikacijaTwoFOtkljucajEndpoint:AutentifikacijaTwoFOtkljucajEndpoint) {
}

  public kljuc:AutentifikacijaTwoFOtkljucajRequest = {
    kljuc:''
  }

  tfas: TfaGetAllResponseTfa[] = [];
  id: number = 0;
  verifikacijskiKod: string = '';
  uspjesnaVerifikacija: boolean = false;
  neuspjesnaVerifikacija: boolean = false;
  ngOnInit(): void {
    this.id = this.myAuthService.returnId()!;
    this.fetchTfas();
  }

  private fetchTfas() {
    console.log('Fetching TFAs from endpoint:', this.TfagetAllEndpoint.Handle()); // Log endpoint URL if possible
    this.TfagetAllEndpoint.Handle().subscribe(
      (x: TfaGetAllResponse) => {
        this.tfas = x.tfas;
      },
      error => {
        console.error('Error fetching TFAs:', error); // Log error details
      }
    );
  }

  provjeriKod() {
    console.log('Verifikacijski kod:', this.verifikacijskiKod); // Log the verification code
    const provjeriKod = this.tfas.some(tfa => tfa.twoFKey === this.verifikacijskiKod);

    if (provjeriKod) {
      this.uspjesnaVerifikacija = true;
      this.kljuc.kljuc = this.verifikacijskiKod;

      console.log('Calling AutentifikacijaTwoFOtkljucajEndpoint with:', this.kljuc); // Log request details
      this.AutentifikacijaTwoFOtkljucajEndpoint.Handle(this.kljuc!).subscribe(
        (x) => {},
        error => {
          console.error('Error in authentication request:', error); // Log error details
        }
      );

      setTimeout(() => {
        this.dialogRef.close();
        this.router.navigate(['/home']);
      }, 3000);
    } else {
      this.neuspjesnaVerifikacija = true;
      this.uspjesnaVerifikacija = false;

      setTimeout(() => {
        this.neuspjesnaVerifikacija = false;
      }, 1500);
    }
  }

  /* otkljucaj() {
     let config=MojConfig.http_opcije();
     this.httpClient.get<LoginInformacije>(MojConfig.adresa_servera+ "/api/Korisnik/Otkljucaj/" + this.code.value, config).subscribe((x:LoginInformacije)=>{
       let currentToken=AutentifikacijaHelper.getLoginInfo();
       if(currentToken.autentifikacijaToken.vrijednost==x.autentifikacijaToken.vrijednost)
       {
         x.autentifikacijaToken.gost=currentToken.autentifikacijaToken?.gost;
         x.autentifikacijaToken.admin=currentToken.autentifikacijaToken?.admin;
         x.autentifikacijaToken.vlasnik=currentToken.autentifikacijaToken?.vlasnik;
       }
       AutentifikacijaHelper.setLoginInfo(x);
       this.token=AutentifikacijaHelper.getLoginInfo();
       this.ProvjeriUlogu();
       localStorage.setItem('Working-user', JSON.stringify(this.workingUser));
       // @ts-ignore
       porukaSuccess("Uspjesno ste se logirali!");
       this.dialogRef.close();
     });
  }*/
}
