import {Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {MojConfig} from "../moj-config";
import {HttpClient} from "@angular/common/http";
import {MyAuthService} from "../services/MyAuthService";


@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{

  constructor(public router: Router, private httpClient: HttpClient, public myAuthService: MyAuthService) {
  }
  ngOnInit(): void {

  }
  showDropdown: boolean = false;


  toggleDropdown() {
    this.showDropdown = !this.showDropdown;
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
    /*window.localStorage.setItem("my-auth-token","");
    this.router.navigate(["/login"]);*/

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

}
