import {Component, OnInit} from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute, Router} from '@angular/router';
import {MojConfig} from "../moj-config";

@Component({
  selector: 'property',
  templateUrl: './property.component.html',
  styleUrl: './property.component.css'
})
export class PropertyComponent implements OnInit{

  propertyId!: number;
  property: any = {
    name: '',
    adress: '',
    numberOfRooms: 0,
    numberOfBathrooms: 0,
    pricePerNight: 0,
    city: { name: '' }, // Dodajte default vrednosti
    propertyType: { name: '' },
    // Dodajte i ostale propertije ako su prisutni
  };

  checkinDate = '9/16/2024';
  checkoutDate = '9/21/2024';
  guests = 1;
  pricePerNight = 528;
  nights = 5;
  serviceFee = 373;

  getTotalPrice() {
    return (this.pricePerNight * this.nights) + this.serviceFee;
  }

  constructor(public router: Router, private route: ActivatedRoute, private httpKlijent: HttpClient) {
    this.route.params.subscribe(x=>{
      this.propertyId=<number>x['id']
      this.loadPropertyDetails(this.propertyId);
    })
  }

  loadPropertyDetails(id: number): void {
    const url = `${MojConfig.adresa_servera}/GetPropertyById?id=${id}`;
    this.httpKlijent.get<any>(url).subscribe(data => {
      this.property = data;
    }, error => {
      console.error('Error fetching property data', error);
    });
  }

  bookNow(): void {
    // Implement booking logic here
    alert('Booking feature is not yet implemented.');
  }

  ngOnInit(): void {
   // this.route.paramMap.subscribe(params => {
    //  this.propertyId = +params.get('id');
    //  this.loadPropertyDetails(this.propertyId);
   // });
  }

  nazad(){
    this.router.navigate(["/home"]);
  }

}
