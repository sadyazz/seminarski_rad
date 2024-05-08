import {Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {MojConfig} from "../moj-config";
import {HttpClient} from "@angular/common/http";
import {MyAuthService} from "../services/MyAuthService";
import {PropertiesGetAllResponse} from "./properties-getall-response";



@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{

  constructor(public router: Router, private httpClient: HttpClient, public myAuthService: MyAuthService) {
  }

  ngOnInit(): void {
    this.getSmjestaj();
  }



  brojacA =0;
  brojacC =0;
  uvecajA(){
    this.brojacA++;
  }

  smanjiA(){
    if(this.brojacA>0)
      this.brojacA--;
  }
  uvecajC(){
    this.brojacC++;
  }

  smanjiC(){
    if(this.brojacC>0)
      this.brojacC--;
  }

  logOut() {

    let token = window.localStorage.getItem("my-auth-token")??"";
    window.localStorage.setItem("my-auth-token","");

    let url=MojConfig.adresa_servera+`/Autentifikacija/logout`
    this.httpClient.post(url, {}, {
      headers:{
        "my-auth-token": token
      }
    }).subscribe(x=>{
      console.log("logout uspjesan")
    })

    this.router.navigate(["/login"])
  }

  pretraga="";

  getProperties(){
    if (!this.pretraga.trim()) {
      return this.properties;
    }
    return this.properties.filter(x => x.city?.name.toLowerCase().includes(this.pretraga.toLowerCase()) || x.city?.country.name.toLowerCase().includes(this.pretraga.toLowerCase()));
  }

properties:PropertiesGetAllResponse[] = [];
getSmjestaj() {
    let url = MojConfig.adresa_servera + '/api/Properties/GetAll';
 /*   fetch(url)
      .then(response=>{
        if(response.status != 200){
          alert("greska " + response.statusText);
          return;
        }
        response.json().then(d=>{
          this.properties = d;
        })
      })*/

  this.httpClient.get<PropertiesGetAllResponse[]>(url).subscribe((x:PropertiesGetAllResponse[])=>{
    this.properties = x;
  })

  /*
  this.propertiesGetAllResponse.Handle().subscribe((x:PropertiesGetAll)=>{
    this.properties = x.properties;
  })*/

}

  goToProfile(){
    this.router.navigate(["/profile"])
  }
}

