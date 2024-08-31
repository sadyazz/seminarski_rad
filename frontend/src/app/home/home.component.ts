import {Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {MojConfig} from "../moj-config";
import {HttpClient} from "@angular/common/http";
import {MyAuthService} from "../services/MyAuthService";
import {PropertiesGetAllResponse} from "./properties-getall-response";


@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
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

  getProperties() {
    if (!this.pretraga.trim()) {
      return this.filteredProperties;
    }
    return this.filteredProperties.filter(x => x.city?.name.toLowerCase().includes(this.pretraga.toLowerCase()) || x.city?.country.name.toLowerCase().includes(this.pretraga.toLowerCase()));
  }

properties:PropertiesGetAllResponse[] = [];
filteredProperties: PropertiesGetAllResponse[] = [];
getSmjestaj() {
  let url = MojConfig.adresa_servera + '/api/Properties/GetAll';
  console.log('Fetching properties from URL:', url);

  this.httpClient.get<PropertiesGetAllResponse[]>(url).subscribe(
    (x: PropertiesGetAllResponse[]) => {
      console.log('Properties fetched successfully:', x);
      this.properties = x;
      this.filteredProperties=x;
    //  console.log('Properties fetched successfully:', this.properties);
    },
    error => {
      console.error('Error fetching properties', error);
    }
  );

}

  goToProfile(){
    this.router.navigate(["/profile"])
  }

  goToProperty(propertyId:number) {
    this.router.navigate(["/property",propertyId]).then(() => {
      window.scrollTo(0, 0);
    });
  }

  filterProperties(category: string) {
    this.filteredProperties = this.properties.filter(property => property.propertyType.name === category);
  }

  showAllProperties() {
    this.filteredProperties = this.properties;
  }

  getImageSrc(images: { id: number, path: string }[]): string {
    if (images && images.length > 0) {
      return images[0].path;
    }
    return 'https://placehold.co/600x600'; // Placeholder image
  }

}
