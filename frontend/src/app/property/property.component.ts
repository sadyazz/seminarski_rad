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
    city: { name: '' },
    propertyType: { name: '' },
    PropertyImages: [],
  };

  checkinDate = '';
  checkoutDate = '';
  guests = 1;
  pricePerNight = 0;
  nights = 0;

  getTotalPrice() {
    return (this.pricePerNight * this.nights* this.guests);
  }

  calculateNights() {
    if (this.checkinDate && this.checkoutDate) {
      const checkin = new Date(this.checkinDate);
      const checkout = new Date(this.checkoutDate);
      const timeDiff = checkout.getTime() - checkin.getTime();
      this.nights = timeDiff / (1000 * 3600 * 24);
    } else {
      this.nights = 0;
    }
  }
  onDateChange() {
    this.calculateNights();
    this.getTotalPrice();
  }

  constructor(public router: Router, private route: ActivatedRoute, private httpKlijent: HttpClient) {
    this.route.params.subscribe(x=>{
      this.propertyId=<number>x['id']
      this.loadPropertyDetails(this.propertyId);
    })
  }

  loadPropertyDetails(id: number): void { console.log("Loading property details for ID:", id);
    const url = `${MojConfig.adresa_servera}/GetPropertyById?id=${id}`;
    this.httpKlijent.get<any>(url).subscribe(data => {
      this.property = data;
      this.property.PropertyImages = data.PropertyImages || [];console.log("Property data loaded:", this.property);
    }, error => {
      console.error('Error fetching property data', error);
    });
  }

  bookNow(): void {
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
